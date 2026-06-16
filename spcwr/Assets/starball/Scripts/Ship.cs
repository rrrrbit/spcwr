using RBitUtils;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public float rotSpeed;
    public float thrust;
    public Transform star;
    public float gravity;

    Rigidbody2D rb;
    BoxCollider2D bc;
    MGR_Input input;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.angularVelocity = rotSpeed * input.playerA.turn;
        rb.AddForce(transform.right * thrust * input.playerA.thrust);
        Debug.DrawRay(transform.position, transform.right * thrust * input.playerA.thrust, Color.blue);

        Vector2 d = star.position - transform.position;
        rb.AddForce(d.WithMag(1/d.sqrMagnitude) * gravity);
        Debug.DrawRay(transform.position, d.WithMag(1 / d.sqrMagnitude) * gravity, Color.red);
    }
}
