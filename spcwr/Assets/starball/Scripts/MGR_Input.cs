using UnityEngine;
using UnityEngine.Windows;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MGR_Input : MonoBehaviour
{
    public Input input;
    Input.PlayerAActions actionsPlayerA;
    Input.PlayerBActions actionsPlayerB;

    public ShipInput playerA;
    public ShipInput playerB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = new Input();
        input.Enable();
        actionsPlayerA = input.PlayerA;
        actionsPlayerA.Enable();
        actionsPlayerB = input.PlayerB;
        actionsPlayerB.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        playerA.turn = actionsPlayerA.turn.ReadValue<float>();
        playerA.thrust = actionsPlayerA.thrust.ReadValue<float>();
        playerA.shoot = actionsPlayerA.shoot.ReadValue<float>() == 1;

        playerB.turn = actionsPlayerB.turn.ReadValue<float>();
        playerB.thrust = actionsPlayerB.thrust.ReadValue<float>();
        playerB.shoot = actionsPlayerB.shoot.ReadValue<float>() == 1;
    }
}

public struct ShipInput
{
    public float turn;
    public float thrust;
    public bool shoot;
}