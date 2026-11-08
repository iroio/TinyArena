
using UnityEngine;

[RequireComponent(typeof(Camera))]
public sealed class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 16f, -16f);
    [SerializeField, Min(0.01f)] private float smoothTime = 0.25f;

    private Vector3 followVelocity;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("CameraFollow requires a target Transform.", this);
            enabled = false;
            return;
        }

        transform.position = target.position + cameraOffset;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = target.position + cameraOffset;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref followVelocity,
            smoothTime);
    }
}
