using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public sealed class PlayerMovement : MonoBehaviour
{
    [SerializeField] private ArenaInputReader inputReader;
    [SerializeField] private Camera viewCamera;
    [SerializeField, Min(0f)] private float moveSpeed = 6f;
    [SerializeField, Min(0f)] private float gravity = 20f;

    private CharacterController characterController;
    private float verticalVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (inputReader == null)
        {
            Debug.LogError("PlayerMovement requires an ArenaInputReader reference.", this);
        }

        if (viewCamera == null)
        {
            Debug.LogError("PlayerMovement requires a view Camera reference.", this);
        }
    }

    private void Update()
    {
        if (inputReader == null || viewCamera == null || characterController == null)
        {
            return;
        }

        Vector3 moveDirection = CalculateMoveDirection(inputReader.MoveInput);
        UpdateVerticalVelocity();
        MoveCharacter(moveDirection);
    }

    private Vector3 CalculateMoveDirection(Vector2 input)
    {
        // Project camera axes onto the arena plane so WASD follows the view orientation.
        Vector3 screenRight = Vector3.ProjectOnPlane(viewCamera.transform.right, Vector3.up).normalized;
        Vector3 screenForward = Vector3.ProjectOnPlane(viewCamera.transform.forward, Vector3.up).normalized;
        Vector3 moveDirection = screenRight * input.x + screenForward * input.y;

        return moveDirection.sqrMagnitude > 1f
            ? moveDirection.normalized
            : moveDirection;
    }

    private void UpdateVerticalVelocity()
    {
        // Keep a small downward velocity while grounded so the controller stays in contact.
        verticalVelocity = characterController.isGrounded
            ? -2f
            : verticalVelocity - gravity * Time.deltaTime;
    }

    private void MoveCharacter(Vector3 moveDirection)
    {
        Vector3 velocity = moveDirection * moveSpeed + Vector3.up * verticalVelocity;
        characterController.Move(velocity * Time.deltaTime);
    }
}
