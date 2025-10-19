using UnityEngine;

[CreateAssetMenu(fileName = "LookSettings", menuName = "Config/ Look Settings")]
public class LookSettings : ScriptableObject
{
    [Header("Sensitivity")]
    [Min(0.01f)] public float mouseSensitivity = 0.12f;
    [Min(0.01f)] public float stickSensitivity = 120f;

    [Header("Smoothing")]
    [Range(0f, 1f)] public float yawSmoothing = 0.1f;
    [Range(0f, 1f)] public float pitchSmoothing = 0.1f;

    [Header("Pitch Clamp")]
    [Range(-89f, 0f)] public float minPitch = -75f;
    [Range(0f, 89f)] public float maxPitch = 75f;

    [Header("Options")]
    public bool invertY = false;
    public bool lockCursor = true;
}
