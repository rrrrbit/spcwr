using RBitUtils;
using UnityEngine;

public class Star : MonoBehaviour
{
    public float gravity;
    public Collider2D col;
    public Rigidbody2D rb;
    public LayerMask knockLayers;

    public Material warp;

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
        var wrap = GetComponent<Wrap>();
        
        warp.SetVector("_warpPos0", transform.position);
        warp.SetVector("_warpPos1", wrap.clones[0].transform.position);
        warp.SetVector("_warpPos2", wrap.clones[1].transform.position);
        warp.SetVector("_warpPos3", wrap.clones[2].transform.position);
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
