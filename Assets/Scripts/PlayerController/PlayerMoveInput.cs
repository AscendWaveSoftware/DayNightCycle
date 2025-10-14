using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMoveInput : MonoBehaviour, IMoveInputSource
{
    private PlayerInputManager input;
    private InputAction move;

    private void Awake()
    {
        input = new PlayerInputManager();
        input.Enable();
        move = input.Player.Move;
    }

    public Vector2 ReadMove() => move.ReadValue<Vector2>();

    private void OnDisable() => input.Disable();
}
