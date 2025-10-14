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
    }

    private void Awake()
    {
        var motor = GetComponent<RigidbodyForceMotor>();
        var mover = GetComponent<CharacterMover>();
        var input = GetComponent<PlayerMoveInput>();

        // Settings injizieren
        var field = typeof(RigidbodyForceMotor).GetField("settings",
                     System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field?.SetValue(motor, settings);

        // Orientation setzen
        var oField = typeof(RigidbodyForceMotor).GetField("orientation",
                     System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        oField?.SetValue(motor, orientation);

        // Input verdrahten
        var iField = typeof(CharacterMover).GetField("inputSourceBehaviour",
                     System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        iField?.SetValue(mover, input);

        var mField = typeof(CharacterMover).GetField("motor",
                     System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        mField?.SetValue(mover, motor);

        var orientField = typeof(CharacterMover).GetField("orientation",
                     System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        orientField?.SetValue(mover, orientation);
    }
}
