// @ts-check
import { defineConfig } from "astro/config";
import starlight from "@astrojs/starlight";

// https://astro.build/config
export default defineConfig({
	integrations: [
		starlight({
			title: "Build a coding agent with Semantic Kernel in a day",
			customCss: [
				"./src/styles/global.css",
			],
			social: [
				{
					icon: "github",
					label: "GitHub",
					href: "https://github.com/wmeints/coding-agent-workshop",
				},
			],
			sidebar: [
				{
					label: "Start Here",
					items: [
						{ label: "Getting started", slug: "modules/introduction/getting-started" },
						{ label: "Workshop overview", slug: "modules/introduction/workshop-overview" },
						{ label: "Provide feedback", slug: "modules/introduction/provide-feedback" },
						
					],
				},
				{
					label: "The Foundation",
					items: [
						{ label: "Introduction to LLMs", slug: "modules/foundation/introduction-to-llms" },
						{ label: "Introduction to Semantic Kernel", slug: "modules/foundation/introduction-to-semantic-kernel" },
					]
				},
				{
					label: "Building the agent loop",
					items: [
						{ label: "Overview", slug: "modules/agent-loop/overview" },
						{ label: "Setting up the project", slug: "modules/agent-loop/create-agent-application" },
						{ label: "Implement the agent loop", slug: "modules/agent-loop/implement-agent-loop" },
						{ label: "Connect the terminal interface", slug: "modules/agent-loop/connect-terminal-interface" },
						{ label: "Testing the agent", slug: "modules/agent-loop/testing" },
					]
				},
				{
					label: "Adding Coding Tools",
					items: [
						{ label: "Overview", slug: "modules/tools/overview" },
						{ label: "Building the Shell Plugin", slug: "modules/tools/building-the-shell-plugin" },
						{ label: "Building the Text Editor Plugin", slug: "modules/tools/building-the-text-editor-plugin" },
						{ label: "Integrating Plugins into the Agent", slug: "modules/tools/integrating-plugins-into-agent" },
						{ label: "Testing the agent with tools", slug: "modules/tools/testing-agent-with-tools" },
					]
				},
				{
					label: "Task Memory",
					items: [
						{ label: "Overview", slug: "modules/task-memory/overview" },
						{ label: "Implement custom instructions", slug: "modules/task-memory/custom-instructions" },
						{ label: "Build the task memory plugin", slug: "modules/task-memory/task-memory-plugin" },
						{ label: "Testing", slug: "modules/task-memory/testing" },
					]
				}
			],
		}),
	],
});
