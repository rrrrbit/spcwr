
using UnityEngine;
using UnityEngine.InputSystem;

public class MGR_GameInput : MonoBehaviour
{
    public ShipInput playerA;
    public ShipInput playerB;

    public SVC_Input input;

    private void Start()
    {
        input = SVC.Get<SVC_Input>();
    }

    // Update is called once per frame
    void Update()
    {
        Input.PlayerAActions actionsA = input.actionsPlayerA;
        Input.PlayerBActions actionsB = input.actionsPlayerB;

        playerA.turn = actionsA.turn.ReadValue<float>();
        playerA.thrust = actionsA.thrust.ReadValue<float>();
        playerA.shoot = actionsA.shoot.ReadValue<float>() == 1;

        playerB.turn = actionsB.turn.ReadValue<float>();
        playerB.thrust = actionsB.thrust.ReadValue<float>();
        playerB.shoot = actionsB.shoot.ReadValue<float>() == 1;
    }
}
public struct ShipInput
{
    public float turn;
    public float thrust;
    public bool shoot;
}