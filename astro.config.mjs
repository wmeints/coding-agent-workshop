// @ts-check
import { defineConfig } from "astro/config";
import starlight from "@astrojs/starlight";
import { readFileSync } from 'node:fs';

// https://astro.build/config
export default defineConfig({
	vite: {
		plugins: [
			{
				name: 'line-range-import',
				transform(code, id) {
					const rangeMatch = id.match(/\?raw&lines=([0-9,-]+)/)
					if (rangeMatch) {
						const [, rangeString] = rangeMatch
						const cleanId = id.split('?')[0]
						const content = readFileSync(cleanId, 'utf-8')
						const allLines = content.split('\n')

						// Parse ranges like "10-20,30-35,50-60"
						const ranges = rangeString.split(',').map(range => {
							const [start, end] = range.split('-').map(n => parseInt(n))
							return { start, end: end || start } // Support single line like "10"
						})

						// Extract lines for each range
						const extractedLines = ranges.map(({ start, end }) => {
							return allLines.slice(start - 1, end).join('\n')
						})

						// Join ranges the ranges together into one continuous block.
						const result = extractedLines.join('\n')

						return `export default ${JSON.stringify(result)}`
					}
				}
			}
		]
	},
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
						{ label: "Introduction to Agents", slug: "modules/foundation/introduction-to-agents" },
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
						{ label: "Adding context awareness", slug: "modules/tools/context-awareness" },
						{ label: "Building the Shell Plugin", slug: "modules/tools/building-the-shell-plugin" },
						{ label: "Building the Text Editor Plugin", slug: "modules/tools/building-the-text-editor-plugin" }
					]
				},
				{
					label: "Context Engineering",
					items: [
						{ label: "Overview", slug: "modules/context-engineering/overview" },
						{ label: "Implement custom instructions", slug: "modules/context-engineering/custom-instructions" },
						{ label: "Build the task memory plugin", slug: "modules/context-engineering/task-memory-plugin" }
					]
				},
				{
					label: "MCP Servers",
					items: [
						{ label: "Overview", slug: "modules/mcp-servers/overview" },
						{ label: "Extending with MCP server support", slug: "modules/mcp-servers/extending-with-mcp-servers" },
						{ label: "Integrating MCP servers in the agent loop", slug: "modules/mcp-servers/integrating-mcp-servers" },
					]
				},
				{
					label: "Conclusion",
					items: [
						{ label: "Finished!", slug: "modules/conclusion/finished" },
						{ label: "Next Steps", slug: "modules/conclusion/next-steps" },
					]
				}
			],
		}),
	],
});
