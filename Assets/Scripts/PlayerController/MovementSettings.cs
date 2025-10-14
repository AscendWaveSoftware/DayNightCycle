using UnityEngine;

[CreateAssetMenu(fileName = "MovementSettings", menuName = "Config/ Movement Settings")]
public class MovementSettings : ScriptableObject
{
    [Header("Speeds")]
    [Min(0f)] public float maxSpeed = 6f;
    [Min(0f)] public float accel = 30f;
    [Min(0f)] public float airAccel = 10f;

    [Header("Damping")]
    [Range(0f, 1f)] public float groundDamping = 0.12f;
    [Range(0f, 1f)] public float airDamping = 0.02f;

    [Header("Misc")]
    public float orientationLerp = 12f;
    public LayerMask groundMask = ~0;
    public float groundCheckRadius = 0.2f;
    public Vector3 groundCheckOffset = new Vector3(0, -0.9f, 0);
}
