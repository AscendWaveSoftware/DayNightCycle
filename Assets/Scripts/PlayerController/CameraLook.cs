using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Dreht die Orientierung (Yaw) und einen Pitch-Pivot (CameraHolder).
/// Keine Physik, läuft in Update. Produktionsreif: Clamping, optionales Smoothing, Cursor-Lock.
/// </summary>
[DisallowMultipleComponent]
public class CameraLook : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private LookSettings settings;
    [SerializeField] private Transform orientation; 
    [SerializeField] private Transform cameraPivot;   

    [Header("Input")]
    [SerializeField] private MonoBehaviour inputSourceBehaviour; 
    private ILookInputSource inputSource;

    private float yaw;    
    private float pitch;  
    private float yawVel;     
    private float pitchVel;   

    void Awake()
    {
        if (!orientation) orientation = transform;

        if (!cameraPivot)
        {
            if (transform.childCount > 0) cameraPivot = transform.GetChild(0);
        }

        if (!inputSourceBehaviour)
        {
            foreach (var mb in GetComponents<MonoBehaviour>())
            {
                if (mb is ILookInputSource) { inputSourceBehaviour = mb; break; }
            }
        }
        inputSource = inputSourceBehaviour as ILookInputSource;

        if (settings && settings.lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        yaw = orientation ? orientation.rotation.eulerAngles.y : transform.rotation.eulerAngles.y;
        pitch = cameraPivot ? cameraPivot.localRotation.eulerAngles.x : 0f;
        if (pitch > 180f) pitch -= 360f;
        ClampPitch();
        ApplyInstant();
    }

    void OnEnable()
    {
        if (settings && settings.lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        if (inputSource == null || settings == null) return;

        Vector2 raw = inputSource.ReadLook();

        bool likelyMouse = Mouse.current != null && (Mouse.current.delta.ReadValue() == raw);

        float dx = raw.x;
        float dy = raw.y * (settings.invertY ? 1f : -1f);

        if (likelyMouse)
        {
            yaw += dx * settings.mouseSensitivity;
            pitch += dy * settings.mouseSensitivity;
        }
        else
        {
            yaw += dx * settings.stickSensitivity * Time.deltaTime;
            pitch += dy * settings.stickSensitivity * Time.deltaTime;
        }

        ClampPitch();

        // Glätten
        if (settings.yawSmoothing > 0f || settings.pitchSmoothing > 0f)
        {
            // Kritisch-dämpfendes Approx (SmoothDamp in Grad)
            float smY = Mathf.SmoothDampAngle(GetYawFrom(orientation), yaw, ref yawVel, settings.yawSmoothing);
            float smP = Mathf.SmoothDampAngle(GetPitchFrom(cameraPivot), pitch, ref pitchVel, settings.pitchSmoothing);

            if (orientation) orientation.rotation = Quaternion.Euler(0f, smY, 0f);
            if (cameraPivot) cameraPivot.localRotation = Quaternion.Euler(smP, 0f, 0f);
        }
        else
        {
            ApplyInstant();
        }
    }

    private void ApplyInstant()
    {
        if (orientation) orientation.rotation = Quaternion.Euler(0f, yaw, 0f);
        if (cameraPivot) cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private void ClampPitch()
    {
        if (!settings) return;
        pitch = Mathf.Clamp(pitch, settings.minPitch, settings.maxPitch);
    }

    private static float GetYawFrom(Transform t) => t ? t.rotation.eulerAngles.y : 0f;
    private static float GetPitchFrom(Transform t)
    {
        if (!t) return 0f;
        float p = t.localEulerAngles.x;
        return (p > 180f) ? p - 360f : p;
    }

    public void AddYawPitch(float addYaw, float addPitch)
    {
        yaw += addYaw;
        pitch += addPitch * (settings != null && settings.invertY ? 1f : -1f);
        ClampPitch();
    }
}
