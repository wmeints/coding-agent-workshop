# Coding Agent Workshop Instructions

This project provides a one-day workshop to attendees of Bitbash to learn about
agents by building a personal coding agent in C# with Semantic Kernel.

Please find information about the learning goals for the workshop in
the [README file](./README.md).

## Structure of the workshop

- `src/content/docs/modules` - contains the content for the modules in the workshop
- `src/content/docs/index.mdx` - contains the homepage for the workshop
- `labs/{module-name}/{lab-name}/start` - contains the files for the attendee to start a lab
- `labs/{module-name}/{lab-name}/start` - contains the final solution for the attendee to catch up on a lab

## How are modules structured

Each module has the following structure:

1. Overview - Provides an overview of what we'll learn in the module
2. Labs - One or more pages containing a lab for the user to work on

### Overview page in a module

The overview page of the module always has this structure:

1. A short introduction paragraph explaining what the module is about.
2. A list of learning goals for the attendee.
3. One or more sections providing an introduction of the topic and concepts
   in the module
4. A section "What we'll build" at the end of the page with a list of labs 
   included in the module. Link the items in the list to the individual labs.

### Lab structure

**Important:** Attendees are encouraged to explore the content so we don't
provide copy-and-paste pieces of code to use in the application. The only
exception is code that's too complicated to get right in 5 - 10 minutes.

A lab should have the following structure:

1. Start with an introduction section that explains the goal of the lab.
2. Provide a link to the start and solution of the lab (you can find these in
   labs/{lab-name}/starter and labs/{lab-name}/solution)
3. Create sections that focus on building a single component or concept.
4. Within each section use the [Steps component][STEPS_COMPONENT] to provide instructions.
5. End with a summary of the lab, and include a link to the solution for the lab.

[STEPS_COMPONENT]: https://starlight.astro.build/components/steps/

Lab sections can refer to product documentation to remind the attendee how to
create certain patterns or components using Semantic Kernel.