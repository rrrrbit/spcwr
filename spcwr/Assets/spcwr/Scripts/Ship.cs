using JetBrains.Annotations;
using RBitUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ship : MonoBehaviour
{
    public bool isPlayerA;
    float shootTimer;

    public Transform shootOrigin;

    public LayerMask killLayers;
    public ParticleSystem exhaustParticle;
    
    Rigidbody2D rb;
    ShipInput shipInput;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        shipInput = isPlayerA ? MGR.input.playerA : MGR.input.playerB;
        var ptclEmission = exhaustParticle.emission;
        shootTimer = Mathf.Max(0f, shootTimer - Time.deltaTime);
        if (shootTimer <= 0 && shipInput.shoot)
        {
            Shoot();
            shootTimer = MGR.game.settings.shipReloadTime;
        }

        ptclEmission.enabled = shipInput.thrust == 1;

        foreach (GameObject i in GetComponent<Wrap>().clones)
        {
            i.GetComponent<SpriteRenderer>().sprite = isPlayerA ? MGR.game.spriteShipA : MGR.game.spriteShipB;
        }
    }

    private void FixedUpdate()
    {
        Star star = MGR.game.star;

        rb.angularVelocity = MGR.game.settings.shipTurnVel * shipInput.turn;
        rb.AddForce(shipInput.thrust * MGR.game.settings.shipThrust * transform.right);

        Vector2 totalGrav = Vector2.zero;
        foreach (GameObject obj in star.GetComponent<Wrap>().clones)
        {
            Vector2 d = obj.transform.position - transform.position;
            totalGrav += d.WithMag(1 / d.sqrMagnitude);
        }
        rb.AddForce(totalGrav * MGR.game.settings.starGravity);
    }

    void Shoot()
    {
        GameObject thisPellet = Instantiate(MGR.game.pelletPrefab);
        thisPellet.transform.position = shootOrigin.position;
        thisPellet.GetComponent<Rigidbody2D>().linearVelocity = transform.right * MGR.game.settings.pelletSpeed;
        thisPellet.GetComponent<Wrap>().bounds = GetComponent<Wrap>().bounds;
        rb.AddForce(-transform.right * MGR.game.settings.shipRecoil, ForceMode2D.Impulse);
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
