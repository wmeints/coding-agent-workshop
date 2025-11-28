using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Moq;

namespace CodingAgent.Tests;

public class AgentTests
{
    [Fact]
    public async Task Test_PromptCausingToolCall_InvokesTool()
    {
        var agentCallbacks = new Mock<IAgentCallbacks>();
        var chatCompletionService = new Mock<IChatCompletionService>();
        var kernel = TestObjectFactory.CreateTestKernel(chatCompletionService.Object);
        var agent = new Agent(kernel, AgentContext.Create(), new AgentInstructions());

        kernel.Plugins.AddFromFunctions("TestPlugin", [
            KernelFunctionFactory.CreateFromMethod(() => { }, "test_function")
        ]);

        chatCompletionService.SetupSequence(x => x.GetChatMessageContentsAsync(
                It.IsAny<ChatHistory>(),
                It.IsAny<PromptExecutionSettings>(),
                It.IsAny<Kernel>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([
                TestObjectFactory.CreateFakeToolCall("TestPlugin", "test_function")
            ])
            .ReturnsAsync([
                TestObjectFactory.CreateFakeAgentResponse("All done!")
            ]);

        await agent.InvokeAsync("Test prompt", agentCallbacks.Object);

        agentCallbacks.Verify(x => x.ReportFunctionCallAsync(
            It.Is<string>(toolName => toolName == "test_function"),
            It.IsAny<KernelArguments?>(), It.IsAny<object?>()));
    }

    [Fact]
    public async Task Test_PromptWithoutToolCall_ReturnsResponse()
    {
        var agentCallbacks = new Mock<IAgentCallbacks>();
        var chatCompletionService = new Mock<IChatCompletionService>();
        var kernel = TestObjectFactory.CreateTestKernel(chatCompletionService.Object);
        var agent = new Agent(kernel, AgentContext.Create(), new AgentInstructions());

        chatCompletionService.Setup(x => x.GetChatMessageContentsAsync(
                It.IsAny<ChatHistory>(),
                It.IsAny<PromptExecutionSettings>(),
                It.IsAny<Kernel>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([
                TestObjectFactory.CreateFakeAgentResponse("All done!")
            ]);

        await agent.InvokeAsync("Test prompt", agentCallbacks.Object);

        agentCallbacks.Verify(x => x.ReportAgentResponseAsync(
            It.Is<string>(content => content == "All done!")));

        agentCallbacks.Verify(x => x.ReportFunctionCallAsync(
            It.IsAny<string>(), It.IsAny<KernelArguments?>(), It.IsAny<object>()), Times.Never());
    }

    [Fact]
    public async Task Test_PromptWithToolsAndContent_CallsToolAndReturnsResponse()
    {
        var agentCallbacks = new Mock<IAgentCallbacks>();
        var chatCompletionService = new Mock<IChatCompletionService>();
        var kernel = TestObjectFactory.CreateTestKernel(chatCompletionService.Object);
        var agent = new Agent(kernel, AgentContext.Create(), new AgentInstructions());

        kernel.Plugins.AddFromFunctions("TestPlugin", [
            KernelFunctionFactory.CreateFromMethod(() => { }, "test_function")
        ]);

        chatCompletionService.SetupSequence(x => x.GetChatMessageContentsAsync(
                It.IsAny<ChatHistory>(),
                It.IsAny<PromptExecutionSettings>(),
                It.IsAny<Kernel>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([
                TestObjectFactory.CreateFakeToolCall(
                    "TestPlugin",
                    "test_function",
                    "Invoking the test function")
            ])
            .ReturnsAsync([
                TestObjectFactory.CreateFakeAgentResponse("All done!")
            ]);

        await agent.InvokeAsync("Test prompt", agentCallbacks.Object);

        agentCallbacks.Verify(x => x.ReportAgentResponseAsync(
            It.IsAny<string>()), Times.Exactly(2));

        agentCallbacks.Verify(x => x.ReportFunctionCallAsync(
            It.IsAny<string>(), It.IsAny<KernelArguments?>(), It.IsAny<object>()), Times.Once());
    }
}