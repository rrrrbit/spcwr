using UnityEngine;

public class Pellet : MonoBehaviour
{
    public float lifespan;
    float lifeTimer;
    ParticleSystem ptcl;
    
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
            transform.DetachChildren();
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        ptcl.Stop();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.DetachChildren();
        Destroy(gameObject);
    }
}
