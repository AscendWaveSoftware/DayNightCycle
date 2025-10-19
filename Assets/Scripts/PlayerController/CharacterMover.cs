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
        // Auto-Wire falls nicht gesetzt
        if (!motor)
            motor = GetComponent<RigidbodyForceMotor>();
        if (!inputSourceBehaviour)
        {
            inputSourceBehaviour = GetComponent<MonoBehaviour>();
            foreach(var mb in GetComponents<MonoBehaviour>())
            {
                if(mb is IMoveInputSource)
                {
                    inputSourceBehaviour = mb;
                    break;
                }
            }
        }

        if (!orientation)
            orientation = transform;

        inputSource = inputSourceBehaviour as IMoveInputSource;
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
