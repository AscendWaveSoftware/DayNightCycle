using UnityEngine;

[DisallowMultipleComponent]
public class CharacterMover : MonoBehaviour
{
    [SerializeField] private RigidbodyForceMotor motor;
    [SerializeField] private MonoBehaviour inputSourceBehaviour;
    [SerializeField] private Transform orientation;

    private IMoveInputSource inputSource;

    private void Awake()
    {
        inputSource = inputSourceBehaviour as IMoveInputSource;
        if (motor == null)
            motor = GetComponent<RigidbodyForceMotor>();
        if(motor != null && orientation != null)
        {
            var field = typeof(RigidbodyForceMotor).GetField("orientation",
                            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(motor, orientation);
        }
    }

    private void FixedUpdate()
    {
        if (motor == null || inputSource == null)
            return;

        Vector2 move = inputSource.ReadMove();
        if (move.sqrMagnitude < 0.01f)
            move = Vector2.zero;
        Vector3 worldMove = motor.ToWorldFromOrientation(move);
        motor.SetDesiredMove(worldMove);
    }
}
