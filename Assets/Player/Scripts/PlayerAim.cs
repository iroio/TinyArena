using UnityEngine;

public sealed class PlayerAim : MonoBehaviour
{
    [SerializeField] private ArenaInputReader inputReader;
    [SerializeField] private Camera viewCamera;
    [SerializeField] private Transform visualRoot;
    [SerializeField] private float groundHeight;

    private void Awake()
    {
        if (inputReader == null)
        {
            Debug.LogError("PlayerAim requires an ArenaInputReader reference.", this);
        }

        if (viewCamera == null)
        {
            Debug.LogError("PlayerAim requires a view Camera reference.", this);
        }

        if (visualRoot == null)
        {
            Debug.LogError("PlayerAim requires a visual root to rotate.", this);
        }
    }

    private void Update()
    {
        if (inputReader == null || viewCamera == null || visualRoot == null || !inputReader.HasPointAction)
        {
            return;
        }

        if (TryGetAimDirection(inputReader.PointerPosition, out Vector3 aimDirection))
        {
            ApplyAimDirection(aimDirection);
        }
    }

    private bool TryGetAimDirection(Vector2 pointerPosition, out Vector3 aimDirection)
    {
        aimDirection = Vector3.zero;
        Ray aimRay = viewCamera.ScreenPointToRay(pointerPosition);
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, groundHeight, 0f));

        // Intersect the pointer ray with the arena plane rather than depending on scene colliders.
        if (!groundPlane.Raycast(aimRay, out float distance))
        {
            return false;
        }

        aimDirection = groundPlane.ClosestPointOnPlane(aimRay.GetPoint(distance)) - transform.position;
        aimDirection = Vector3.ProjectOnPlane(aimDirection, Vector3.up);
        return aimDirection.sqrMagnitude > 0.0001f;
    }

    private void ApplyAimDirection(Vector3 aimDirection)
    {
        visualRoot.rotation = Quaternion.LookRotation(aimDirection, Vector3.up);
    }
}
