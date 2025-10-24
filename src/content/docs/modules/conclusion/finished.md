---
title: Finished
description: You've succesfully completed the workshop.
---

Thank you for completing this workshop! You've built a functional coding agent with Semantic Kernel and learned the fundamentals of agent development. You now have a solid foundation to continue building and improving your agent.

## Ideas for Expanding Your Agent

Your agent is functional, but there are many ways to make it even more powerful and useful. Here are some ideas to explore:

### Custom Prompts

Add support for custom prompts to give users more control over how the agent behaves:

- Allow users to define custom system prompts for specific tasks
- Create prompt templates that users can select from
- Enable users to override the default agent behavior with their own instructions

This gives your agent more flexibility and allows it to adapt to different use cases and preferences.

### Glob-Based Custom Instructions

Expand your custom instructions feature to support file-specific guidance:

- Add support for glob patterns (e.g., `*.test.cs`, `src/api/**/*.cs`)
- Let users define different instructions for different parts of their codebase
- Allow inheritance of instructions from broader to more specific patterns

For example, you could have one set of instructions for all C# files, and more specific instructions for test files or API controllers.

### Tool Permission System

Improve the security and control of your agent by implementing a permission system:

- Ask for user confirmation before executing potentially dangerous operations
- Create a whitelist/blacklist system for allowed commands
- Implement different permission levels for different types of tools
- Add the ability to review and approve changes before they're applied

This is especially important for operations that modify files, execute system commands, or interact with external services.

## Continue Building

These are just starting points—the possibilities for expanding your agent are endless. Consider what would make your agent most useful for your specific workflow and start experimenting!

Ready to learn more? Check out the [Next Steps](/modules/conclusion/next-steps) page for additional resources and learning materials.
