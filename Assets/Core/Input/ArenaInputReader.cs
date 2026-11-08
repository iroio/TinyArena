using UnityEngine;
using UnityEngine.InputSystem;

public sealed class ArenaInputReader : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    private InputAction moveAction;
    private InputAction pointAction;
    private InputAction fireAction;

    public Vector2 MoveInput =>
        moveAction != null && moveAction.enabled
            ? moveAction.ReadValue<Vector2>()
            : Vector2.zero;

    public Vector2 PointerPosition =>
        pointAction != null && pointAction.enabled
            ? pointAction.ReadValue<Vector2>()
            : Vector2.zero;

    public bool IsFirePressed =>
        fireAction != null && fireAction.enabled && fireAction.IsPressed();

    public bool HasMoveAction => moveAction != null && moveAction.enabled;
    public bool HasPointAction => pointAction != null && pointAction.enabled;
    public bool HasFireAction => fireAction != null && fireAction.enabled;

    private void Awake()
    {
        if (inputActions == null)
        {
            Debug.LogError("ArenaInputReader requires an Input Actions asset.", this);
            return;
        }

        moveAction = inputActions.FindAction("Player/Move");
        pointAction = inputActions.FindAction("UI/Point");
        fireAction = inputActions.FindAction("Player/Fire");

        if (moveAction == null)
        {
            Debug.LogError("The Player/Move action was not found.", this);
        }

        if (pointAction == null)
        {
            Debug.LogError("The UI/Point action was not found.", this);
        }

        if (fireAction == null)
        {
            Debug.LogError("The Player/Fire action was not found.", this);
        }
    }

    private void OnEnable()
    {
        // Enable only the actions used by this prototype.
        if (moveAction != null && !moveAction.enabled)
        {
            moveAction.Enable();
        }

        if (pointAction != null && !pointAction.enabled)
        {
            pointAction.Enable();
        }

        if (fireAction != null && !fireAction.enabled)
        {
            fireAction.Enable();
        }
    }

    private void OnDisable()
    {
        if (moveAction != null && moveAction.enabled)
        {
            moveAction.Disable();
        }

        if (pointAction != null && pointAction.enabled)
        {
            pointAction.Disable();
        }

        if (fireAction != null && fireAction.enabled)
        {
            fireAction.Disable();
        }
    }
}
