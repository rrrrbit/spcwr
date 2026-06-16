using UnityEngine;
using UnityEngine.Windows;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MGR_Input : MonoBehaviour
{
    public Input input;
    Input.PlayerAActions actionsPlayerA;
    Input.PlayerAActions actionsPlayerB;

    public ShipInput playerA;
    public ShipInput playerB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = new Input();
        input.Enable();
        actionsPlayerA = input.PlayerA;
        actionsPlayerA.Enable();
        actionsPlayerB = input.PlayerA;
        actionsPlayerB.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        playerA.turn = actionsPlayerA.turn.ReadValue<float>();
        playerA.thrust = actionsPlayerA.thrust.ReadValue<float>();

        playerB.turn = actionsPlayerB.turn.ReadValue<float>();
        playerB.thrust = actionsPlayerB.thrust.ReadValue<float>();
    }
}

public struct ShipInput
{
    public float turn;
    public float thrust;
}