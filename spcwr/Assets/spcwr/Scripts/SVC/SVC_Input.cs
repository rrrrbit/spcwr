
using UnityEngine;
using UnityEngine.InputSystem;

public class SVC_Input : MonoBehaviour
{
    public Input input;
    public Input.PlayerAActions actionsPlayerA;
    public Input.PlayerBActions actionsPlayerB;
    public Input.MenuActions actionsMenu;

    void Awake()
    {
        input = new Input();

        input.bindingMask = new InputBinding { groups = "Keyboard" };
        input.Enable();
        actionsPlayerA = input.PlayerA;
        actionsPlayerA.Enable();
        actionsPlayerB = input.PlayerB;
        actionsPlayerB.Enable();
        actionsMenu = input.Menu;
        actionsMenu.Enable();
    }

    public void DebugSetArcadeCtrls()
    {
        input.bindingMask = new InputBinding { groups = "Arcade" };
    }

    public void DebugSetKeyboardtrls()
    {
        input.bindingMask = new InputBinding { groups = "Keyboard" };
    }
}

