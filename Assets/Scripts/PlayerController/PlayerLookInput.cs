using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerLookInput : MonoBehaviour, ILookInputSource
{
    private PlayerInputManager input;
    private InputAction look;

    private void OnEnable()
    {
        if (input == null)
            input = new PlayerInputManager();
        input.Enable();
        look = input.Player.Look;
    }

    public Vector2 ReadLook() => look.ReadValue<Vector2>();

    private void OnDisable() => input.Disable();
}
