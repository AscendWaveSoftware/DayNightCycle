using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private MovementSettings settings;
    [SerializeField] private Transform orientation;

    private void Reset()
    {
        if (!TryGetComponent<Rigidbody>(out _)) gameObject.AddComponent<Rigidbody>();
        if (!TryGetComponent<RigidbodyForceMotor>(out _)) gameObject.AddComponent<RigidbodyForceMotor>();
        if (!TryGetComponent<PlayerMoveInput>(out _)) gameObject.AddComponent<PlayerMoveInput>();
        if (!TryGetComponent<CharacterMover>(out _)) gameObject.AddComponent<CharacterMover>();
        if (!TryGetComponent<PlayerLookInput>(out _)) gameObject.AddComponent<PlayerLookInput>();
        if (!TryGetComponent<CameraLook>(out _)) gameObject.AddComponent<CameraLook>();
    }

    private void Awake()
    {
        var motor = GetComponent<RigidbodyForceMotor>();
        var mover = GetComponent<CharacterMover>();

        if (motor)
        {
            if (!settings)
                settings = ScriptableObject.CreateInstance<MovementSettings>();
        }
    }
}
