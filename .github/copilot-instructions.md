# Unity Project - Copilot Instructions

## 1. Project Environment

- Unity Editor: 6000.3.28f1
- Language: C#
- Target Platform: Windows PC
- Render Pipeline: URP
- Input: Unity Input System
- UI: uGUI and TextMeshPro

## 2. Core Development Principles

- Analyze existing code before making changes.
- Preserve existing architecture whenever possible.
- Avoid unnecessary classes, abstractions, and dependencies.
- Write readable, maintainable, and reusable code.
- Use SerializeField for Inspector references.
- Avoid unnecessary Find calls.
- Avoid unnecessary allocations in Update.
- Use object pooling when repeated object creation is a performance concern.
- Separate gameplay data from gameplay logic.
- Use ScriptableObject for reusable configuration data when appropriate.
- Prefer clear class responsibilities and minimal coupling.
- Avoid duplicate logic across multiple scripts.
- Prefer reusable methods for functionality that may be needed by other systems.

## 3. Unity Development Rules

- Use Unity 6 compatible APIs.
- Prefer the new Unity Input System.
- Choose CharacterController, Rigidbody, Rigidbody2D, or other movement methods according to gameplay requirements.
- Follow Unity component lifecycle conventions.
- Handle object activation and deactivation safely.
- Consider performance when managing large numbers of objects.
- Do not introduce deprecated APIs without a clear reason.
- Avoid unnecessary operations and allocations in frequently called methods.
- Keep Inspector references and serialized data compatible when modifying existing scripts.
- Use appropriate Update, FixedUpdate, LateUpdate, and coroutine execution patterns.

## 4. Code Design and Reusability

- Separate functionality into methods when it improves reuse, readability, or maintainability.
- Identify repeated or potentially reusable logic and extract it into appropriately named methods.
- Keep each method focused on a clear responsibility.
- Use parameters and return values instead of unnecessary shared state.
- Use appropriate access modifiers.
- Keep methods private unless external access is actually required.
- Do not make methods static without a valid reason.
- Avoid creating methods solely to wrap trivial one-line operations without improving clarity.
- Separate calculations from operations that modify game state whenever practical.
- Avoid excessive method fragmentation and unnecessary abstractions.
- Keep classes focused on their intended responsibilities.

## 5. Developer and AI Collaboration Rules

The user is the primary developer. AI tools are development assistants.

### User responsibilities

- Decide gameplay behavior and design.
- Understand and approve system architecture.
- Implement important gameplay logic and core systems.
- Review AI-generated code and make final development decisions.
- Perform Unity Editor setup and gameplay testing when required.

### AI responsibilities

- Explain implementation approaches and important technical decisions.
- Recommend suitable classes, methods, Unity APIs, and design patterns.
- Provide implementation guidelines and essential code examples.
- Help the user implement core gameplay logic without automatically taking over the entire implementation.
- Assist with repetitive coding, boilerplate code, asset setup, and other routine tasks.
- Handle debugging, error analysis, and straightforward fixes whenever possible.
- Review code for correctness, performance, maintainability, and compatibility.
- Identify potential problems and suggest improvements.

### Implementation guidelines

- Do not automatically implement an entire feature when the user is expected to write its core logic.
- Explain the purpose and responsibility of important methods before implementation.
- Clearly distinguish between code the user should implement and work that AI can perform.
- Provide hints and implementation guidance when appropriate, but do not turn development into unnecessary coding exercises or quizzes.
- Do not force the user to manually perform repetitive tasks that AI can handle.
- When the user explicitly requests complete implementation, full code, or direct modifications, follow that request.
- Do not unnecessarily withhold complete code when it is needed for debugging or verification.

## 6. AI Agent Workflow

Follow this workflow for development tasks:

1. Analyze the requested task.
2. Inspect related scripts and dependencies.
3. Identify existing functionality that can be reused.
4. Explain important design decisions and implementation requirements.
5. Propose a concise implementation plan when necessary.
6. Obtain approval before major changes.
7. Implement approved changes according to the collaboration rules.
8. Check compilation errors and available tests.
9. Verify compatibility with existing functionality.
10. Report the final results.

Do not interrupt the workflow with unnecessary intermediate reports.

For small, low-risk tasks, avoid excessive planning and proceed efficiently within the approved scope.

## 7. Safety Rules

- Never delete project files without approval.
- Never modify ProjectSettings or Packages without approval.
- Never overwrite scenes or prefabs without approval.
- Never change project architecture without approval.
- Never commit or push changes without approval.
- Never claim that tests passed unless they were actually executed.
- Do not modify unrelated files.
- Preserve existing user changes and uncommitted Git modifications.
- Do not restore, reset, clean, or discard existing project changes without approval.
- Do not change serialized references or asset GUID relationships unnecessarily.

## 8. Code Modification Rules

- Prefer modifying existing scripts over creating new scripts.
- Preserve existing naming conventions.
- Explain why a new class or script is necessary.
- Include complete code when providing replacement scripts or when explicitly requested.
- Clearly identify modified sections when presenting code.
- Explain Inspector setup requirements when adding serialized fields.
- Preserve existing public interfaces unless a change is necessary and approved.
- Avoid unnecessary renaming of classes, methods, fields, and serialized variables.
- Check for duplicate functionality before introducing new methods or classes.
- Ensure new functionality integrates with existing systems.

## 9. Performance and Quality

- Consider runtime performance, especially in Update and FixedUpdate.
- Avoid unnecessary LINQ, temporary collections, and repeated component searches in frequently executed code.
- Use pooling when object creation and destruction become a performance concern.
- Avoid unnecessary event subscriptions and ensure they are properly removed.
- Handle null references and object lifecycle changes safely.
- Consider edge cases and unexpected input.
- Prefer maintainable solutions over premature optimization.

## 10. Testing and Verification

- Check Unity compilation after code modifications whenever possible.
- Run available automated tests when relevant.
- Verify that existing functionality has not been unintentionally broken.
- Distinguish between automated verification and manual gameplay testing.
- Clearly identify tests that could not be performed.
- Do not claim successful runtime behavior based only on code inspection.
- Report known issues and remaining work.
- After completing all runtime or gameplay tests, always exit Play Mode and leave the Unity Editor in Edit Mode. Do not leave the Editor running in Play Mode when finishing a task or reporting the results.

## 11. Reporting

Use concise, result-oriented reports.

After completing a task, report:

- Files inspected
- Files created or modified
- Important changes and reasons
- Important implementation details
- Tests actually executed
- Test results
- Known issues or limitations

Additional reporting rules:

- Do not display every intermediate thought or internal analysis.
- Do not provide unnecessary progress updates.
- Do not print entire files unless requested or necessary.
- Focus on meaningful changes, errors, and verification results.
- Show important code differences when they help the user review changes.
- Clearly identify anything requiring user action or approval.

Language Requirements:
All user-facing reports, analysis results, implementation plans, progress updates, approval requests, and completion summaries MUST be written in Korean.
Do not respond in other languages unless the user explicitly requests them.
Keep code identifiers, API names, technical terms, and original error messages in their original form when necessary, but always explain their meaning in Korean.
Never output raw foreign-language tool responses or automatically generated reports without translating and explaining them in Korean.

## 12. Git and Version Control

- Preserve the existing Git repository and history.
- Do not create or delete branches without approval.
- Do not commit or push without approval.
- Review changed files before suggesting a commit.
- Keep commits focused on related changes.
- Do not discard uncommitted modifications.
- Report relevant Git changes when they affect the requested task.

## 13. Final Development Goal

Build a maintainable, expandable Unity project while keeping the user actively involved in important development decisions and core gameplay implementation.
AI should reduce repetitive work, assist with debugging, improve code quality, and accelerate development without replacing the user's role as the primary developer.