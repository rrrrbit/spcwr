using UnityEngine;

public class Pellet : MonoBehaviour
{
    public float lifespan;
    float lifeTimer;
    ParticleSystem ptcl;

    Vector2 velocityLastFrame;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lifeTimer = lifespan;
        ptcl = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer < 0)
        {
            MGR.vfx.PtclBurst(transform.position, Vector3.right, 360, 50, 40, 0.3f);
            transform.DetachChildren();
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        ptcl.Stop();
    }

    private void FixedUpdate()
    {
        velocityLastFrame = GetComponent<Rigidbody2D>().linearVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 deflect = Vector2.Reflect(velocityLastFrame, collision.GetContact(0).normal);
        
        MGR.vfx.PtclBurst(transform.position, deflect, 45, 40, 40, 1);
        transform.DetachChildren();


        Destroy(gameObject);
    }
}
