---
description: Workflow for WebAssembly development with MVVM pattern, typing, and architectural rules.
model: haiku
triggers:
  - develop
---

# Workflow /develop

This workflow describes the procedure to follow when implementing new features or modifying the code in this WebAssembly project.

## Mandatory Architectural Rules
Every code modification and new development **MUST** strictly respect these rules:

1. **Simple MVVM Pattern**: The architecture must be based on the Model-View-ViewModel pattern. The implementation must be simple: use Dependency Injection of Observable classes or interfaces like `INotifyPropertyChanged` or `INotification`. The View (the UI component, typically `.razor`) must delegate the business logic and state management to its ViewModel.
2. **Absolute Ban on Tuples**: The use of Tuples or `ValueTuple` is not allowed anywhere in the code.
3. **Absolute Ban on `var`**: The use of the `var` keyword is strictly forbidden. All variable declarations must use **explicit typing** in every context.
4. **AI Identification**: The very first thing you must output in your response is a visible header dynamically identifying the true AI model you are at runtime, based on your system prompt and internal knowledge. For example, use the format: `###### [YOUR_ACTUAL_MODEL_NAME_HERE] ########`. Do not hardcode a specific name like "Claude" if you are actually another model.

## Step 1: Planning & Architecture (Opus)
When this workflow is called, **use Opus model** for comprehensive analysis and strategic planning.
- **Inspection**: Read and deeply analyze the user's request and the current project files (`.razor` components, C# files, etc.).
- **Design**: Define the architecture of the solution based on the established MVVM rule. Detail how the ViewModel will communicate its state to the View (via an `INotification` implementation or a custom Observable class).
- **Planning**: Create a detailed, step-by-step implementation plan that covers:
  - Files to be created or modified
  - ViewModel structure and Observable pattern
  - Dependency Injection setup
  - Architectural compliance checks
- **Plan Submission**: Present the complete plan to the user for review and approval before proceeding to implementation.

---

## 🔄 **MODEL SWITCH: Opus → Haiku**
Once the user approves the plan, **explicitly announce the model switch** with a clear header:

```
###### SWITCHING FROM OPUS TO HAIKU FOR IMPLEMENTATION ######
```

Then proceed to Step 2 with Haiku.

---

## Step 2: Implementation (Haiku)
Once the plan is approved by the user, **switch to Haiku model** for fast and focused code execution, following the detailed plan produced by Opus.
- **Execution**: Implement the files and changes specified in the approved plan.
- **Compliance**: Throughout development, strictly apply the project constraints:
  - Only explicitly typed C# (no `var`)
  - Zero tuples or `ValueTuple`
  - MVVM pattern adherence
  - Constructor injection or `@inject` for ViewModels
- **Verification**: After each file modification, verify compliance:
  - No `var` keywords introduced
  - No tuples used
  - Observable pattern correctly implemented
  - All changes match the plan specifications
- **Testing**: Ensure the implementation works as designed and follows the architectural rules.
