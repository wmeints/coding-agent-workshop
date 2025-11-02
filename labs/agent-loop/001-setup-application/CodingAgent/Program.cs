using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;


var configuration = new ConfigurationBuilder()
    .AddUserSecrets(typeof(Program).Assembly)
    .AddEnvironmentVariables()
    .Build();

var kernelBuilder = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(
        configuration["LanguageModel:DeploymentName"]!,
        configuration["LanguageModel:Endpoint"]!,
        configuration["LanguageModel:ApiKey"]!
    );

var kernel = kernelBuilder.Build();

Console.WriteLine("You bettah check yoself, you're about to build an agent.");
