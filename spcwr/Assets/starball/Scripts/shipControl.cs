using RBitUtils;
using UnityEngine;

public class shipControl : MonoBehaviour
{
    public float rotSpeed;
    public float thrust;
    public Transform star;
    public float gravity;

    Rigidbody2D rb;
    BoxCollider2D bc;
    Input input;
    Input.PlayerAActions playerA;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = new Input();
        input.Enable();
        playerA = input.PlayerA;
        playerA.Enable();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        print(playerA.turn.ReadValue<float>());
    }

    private void FixedUpdate()
    {
        rb.angularVelocity = rotSpeed * playerA.turn.ReadValue<float>();
        rb.AddForce(transform.right * thrust * playerA.thrust.ReadValue<float>());
        Vector2 d = star.position - transform.position;

        rb.AddForce(d.WithMag(1/d.sqrMagnitude) * gravity);
    }
}
