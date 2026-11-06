
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

## 3. Unity Development Rules
- Use Unity 6 compatible APIs.
- Prefer the new Unity Input System.
- Use Rigidbody2D or Rigidbody according to the gameplay requirements.
- Follow Unity component lifecycle conventions.
- Handle object activation and deactivation safely.
- Consider performance when managing large numbers of objects.
- Do not introduce deprecated APIs without a clear reason.

## 4. AI Agent Workflow
Follow this workflow for every development task:

1. Analyze the requested task.
2. Inspect related scripts and dependencies.
3. Explain the current implementation.
4. Propose an implementation plan.
5. Wait for approval before making major changes.
6. Implement changes in small, reviewable steps.
7. Check compilation errors and available tests.
8. Report the results.

## 5. Safety Rules
- Never delete project files without approval.
- Never modify ProjectSettings or Packages without approval.
- Never overwrite scenes or prefabs without approval.
- Never change project architecture without approval.
- Never commit or push changes without approval.
- Never claim that tests passed unless they were actually executed.
- Do not modify unrelated files.

## 6. Code Modification Rules
- Prefer modifying existing scripts over creating new scripts.
- Preserve existing naming conventions.
- Explain why a new class or script is necessary.
- Include complete code when providing replacement scripts.
- Clearly identify modified sections.
- Explain Inspector setup requirements when adding serialized fields.

## 7. Reporting
After completing a task, report:

- Files inspected
- Files created or modified
- Reason for each change
- Important implementation details
- Tests executed
- Test results
- Known issues or limitations
