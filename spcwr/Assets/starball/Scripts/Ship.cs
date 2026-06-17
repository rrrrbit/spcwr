using RBitUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ship : MonoBehaviour
{
    public MGR_Input input;
    public bool isPlayerA;
    public float shootInterval;
    float shootTimer;

    public float rotSpeed;
    public float thrust;
    public Star star;
    public GameObject pellet;
    public float shootVel;
    public float shootOffset;

    public LayerMask killLayers;
    public ParticleSystem exhaustParticle;
    
    Rigidbody2D rb;
    Collider2D bc;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ShipInput shipInput = isPlayerA ? input.playerA : input.playerB;

        shootTimer = Mathf.Max(0f, shootTimer - Time.deltaTime);
        if (shootTimer <= 0 && shipInput.shoot)
        {
            Shoot();
            shootTimer = shootInterval;
        }

        if (shipInput.thrust == 1)
        {
            exhaustParticle.Play();
        }
        else
        {
            if (exhaustParticle.isPlaying)
            {
                exhaustParticle.Stop();
            }
        }
    }

    private void FixedUpdate()
    {
        ShipInput shipInput = isPlayerA ? input.playerA : input.playerB;
        
        rb.angularVelocity = rotSpeed * shipInput.turn;
        rb.AddForce(transform.right * thrust * shipInput.thrust);
        Debug.DrawRay(transform.position, transform.right * thrust * shipInput.thrust, Color.blue);

        Vector2 d = star.transform.position - transform.position;
        rb.AddForce(d.WithMag(1/d.sqrMagnitude) * star.gravity);
        Debug.DrawRay(transform.position, d.WithMag(1 / d.sqrMagnitude) * star.gravity, Color.red);
    }

    void Shoot()
    {
        GameObject thisPellet = Instantiate(pellet);
        thisPellet.transform.position = transform.position + transform.right * shootOffset;
        thisPellet.GetComponent<Rigidbody2D>().linearVelocity = transform.right * shootVel;
        thisPellet.GetComponent<Wrap>().bounds = GetComponent<Wrap>().bounds;
    }

    void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (killLayers.Contains(collision.gameObject))
        {
            Die();
        }
    }
}
