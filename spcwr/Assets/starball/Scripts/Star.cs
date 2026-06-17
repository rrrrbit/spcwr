using RBitUtils;
using UnityEngine;

public class Star : MonoBehaviour
{
    public float gravity;
    public Collider2D col;
    public Rigidbody2D rb;
    public LayerMask knockLayers;

    public float knockStrength;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print(collision.gameObject.name);
        if (knockLayers.Contains(collision.gameObject))
        {
            Vector2 force = (transform.position - collision.transform.position).normalized * knockStrength;
            rb.AddForce(force, ForceMode2D.Impulse);
        }
    }
}
