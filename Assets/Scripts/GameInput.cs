using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : Singleton<GameInput>
{
    InputSystem inputSystem;

    [HideInInspector]
    public InputAction onPlayerMove,
        onPlayerInteract, onPlayerChangeMenu;

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
        onPlayerChangeMenu = inputSystem.Player.ChangeMenu;

        onPlayerMove.Enable();
        onPlayerInteract.Enable();
        onPlayerChangeMenu.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Player.Disable();
        onPlayerMove.Disable();
        onPlayerInteract.Disable();
        onPlayerChangeMenu.Disable();
    }

    public Vector3 Move()
    {
        return onPlayerMove.ReadValue<Vector2>();
    }

    public Vector3 ChangeMenu()
    {
        return onPlayerChangeMenu.ReadValue<Vector2>();
    }
}
