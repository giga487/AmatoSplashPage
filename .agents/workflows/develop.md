---
description: Workflow for WebAssembly development with MVVM pattern, typing, and architectural rules.
---

# Workflow /develop

This workflow describes the procedure to follow when implementing new features or modifying the code in this WebAssembly project.

## Mandatory Architectural Rules
Every code modification and new development **MUST** strictly respect these rules:

1. **Simple MVVM Pattern**: The architecture must be based on the Model-View-ViewModel pattern. The implementation must be simple: use Dependency Injection of Observable classes or interfaces like `INotifyPropertyChanged` or `INotification`. The View (the UI component, typically `.razor`) must delegate the business logic and state management to its ViewModel.
2. **Absolute Ban on Tuples**: The use of Tuples or `ValueTuple` is not allowed anywhere in the code.
3. **Absolute Ban on `var`**: The use of the `var` keyword is strictly forbidden. All variable declarations must use **explicit typing** in every context.
4. **AI Identification**: The very first thing you must output in your response is a visible header dynamically identifying the true AI model you are at runtime, based on your system prompt and internal knowledge. For example, use the format: `###### [YOUR_ACTUAL_MODEL_NAME_HERE] ########`. Do not hardcode a specific name like "Claude" if you are actually another model.

## Step 1: Code Analysis (Advanced AI)
When this workflow is called, begin by approaching the problem with the strategic mindset of an advanced AI.
- **Phase Declaration**: You MUST start your response for this phase with a visible header indicating your analytical persona, for example: `###### [YOUR_MODEL] - ANALYSIS PHASE ########`.
- **Inspection**: Read and deeply analyze the user's request and the current project files (`.razor` components, C# files, etc.).
- **Design**: Define the architecture of the solution based on the established MVVM rule. Define how the ViewModel will communicate its state to the View (via an `INotification` implementation or a custom Observable class).
- **Planning**: Internally outline the exact steps needed to implement or modify the code **without** writing it immediately. Ensure all steps adhere to the architectural rules.

## Step 2: Implementation (Executive AI)
Once the plan is approved or the architecture clarified, shift your mindset to that of a direct, code-executing AI. 
- **Phase Declaration**: You MUST clearly declare this persona shift with a new visible header before writing the code, for example: `###### [YOUR_MODEL] - IMPLEMENTATION PHASE ########`.
- **Execution**: Modify or create the files specified in the plan.
- **Compliance**: Throughout iterative development, constantly apply the project constraints (write only explicitly typed C#, zero tuples or `var`).
- **Verification**: At the end of each modification, ensure, file by file, that you haven't inadvertently inserted tuples or `var`. Use constructor injection or `@inject` within Razor pages to wire up the Observable classes/ViewModels.
