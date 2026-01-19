---
marp: true
theme: default
paginate: true
---

# Context Engineering

Improving agent performance through better context

---

## What We'll Learn

- What context engineering is and why it matters
- Implementing custom instructions with AGENTS.md
- Building task memory with TODO tracking
- Testing context engineering components

---

## What is Context Engineering?

The practice of providing structured information to improve LLM decision-making

- **Tools** extend what an agent *can* do
- **Context** improves *how well* it does those things

---

## Two Types of Context

### Custom Instructions
Static guidance that shapes behavior
- Coding styles and conventions
- Project-specific guidelines
- Domain knowledge

### Task Memory
Dynamic state for tracking progress
- TODO lists for complex tasks
- Completion status tracking
- Focus maintenance

---

## Custom Instructions with AGENTS.md

A file-based approach to personalization:

```markdown
# Project Guidelines

- Use TypeScript for all new code
- Follow functional programming patterns
- Write tests for all new features
```

---

## Task Memory Benefits

Keeps agents focused on multi-step plans:

1. Breaks complex tasks into discrete items
2. Tracks completion status
3. Reduces forgotten or skipped steps
4. Provides clear progress visibility

---

## Let's Build It!

In this module we will:

1. Implement custom instructions with AGENTS.md
2. Build the TaskMemoryPlugin
3. Test our implementations
