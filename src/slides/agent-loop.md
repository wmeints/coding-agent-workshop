---
marp: true
theme: default
paginate: true
---

# The Agent Loop

Building the core of your coding agent

---

## What We'll Learn

- Creating an agent application from scratch
- Understanding the agent loop pattern
- Connecting a terminal interface
- Testing your agent

---

## Agent Architecture

![Agent Architecture](/images/agent-architecture-overview.png)

---

## The Agent Loop Pattern

```mermaid
flowchart LR
    S@{ shape: start }
    P[User input]
    L[LLM]
    E@{ shape: stop, label: "End" }
    T[Process tool calls]

    S --> P
    P --> L 
    L .-> T
    L .-> E
    T --> L
```

---

## What Makes a Good Agent Loop?

1. **Clear state management** - Track what the agent is doing
2. **Tool execution** - Allow the agent to take actions
3. **Error handling** - Gracefully handle failures
4. **User feedback** - Keep the user informed

---

## Let's Build It!

In this module we will:

1. Create the agent application structure
2. Implement the core agent loop
3. Connect a terminal interface
4. Test our implementation
