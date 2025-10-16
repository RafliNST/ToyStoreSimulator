using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : Singleton<GameInput>
{
    InputSystem inputSystem;

    [HideInInspector]
    public InputAction onPlayerMove,
        onPlayerInteract;

    public override void OnInitialize()
    {
        Debug.Log("Object Initialize: " + this.gameObject.name);
    }

    private void OnEnable()
    {
        inputSystem = new InputSystem();

        inputSystem.Player.Enable();
        onPlayerMove = inputSystem.Player.Move;
        onPlayerInteract = inputSystem.Player.Interact;

        onPlayerMove.Enable();
        onPlayerInteract.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Player.Disable();
        onPlayerMove.Disable();
        onPlayerInteract.Disable();
    }

    public Vector3 Move()
    {
        return onPlayerMove.ReadValue<Vector2>();
    }
}
