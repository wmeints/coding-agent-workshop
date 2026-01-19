---
marp: true
theme: default
paginate: true
---

# Agent Tools

Extending your agent's capabilities with plugins

---

## What We'll Learn

- What tools a coding agent needs
- How to implement plugins in Semantic Kernel
- Building shell and text editor plugins
- Testing custom plugins

---

## Why Tools Matter

LLMs can only generate text by default

Tools allow agents to:
- **Execute commands** in the shell
- **Read and write files** on disk
- **Interact with external services**
- **Take real actions** in the world

---

## Tools in Semantic Kernel

Tools are grouped into **plugins**

```csharp
public class MyPlugin
{
    [KernelFunction("greet_user")]
    [Description("Greets a user")]
    public string GreetUser(string userName)
    {
        return $"Hello, {userName}!";
    }
}
```

---

## Best Practices for Tools

- Use `snake_case` for tool names
- Keep descriptions short and precise
- Return useful error messages
- Test tools thoroughly

---

## What Tools Does a Coding Agent Need?

Essential tools for coding:

1. **Shell Plugin** - Run commands and scripts
2. **Text Editor Plugin** - Read and write code files

These two tools cover most coding tasks!

---

## Let's Build It!

In this module we will:

1. Build the `ShellPlugin`
2. Build the `TextEditorPlugin`
3. Write unit tests for both
