using JetBrains.Annotations;
using RBitUtils;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ship : MonoBehaviour
{
    public bool isPlayerA;
    float shootTimer;

    public Transform shootOrigin;

    public LayerMask killLayers;
    public ParticleSystem exhaustParticle;
    public bool died;
    
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

        Color shipColor = isPlayerA ? MGR.game.colorShipA : MGR.game.colorShipB;
        foreach (GameObject i in GetComponent<Wrap>().clones)
        {
            i.GetComponent<SpriteRenderer>().color = shipColor;
        }
        var main = exhaustParticle.main;
        main.startColor = shipColor;
    }

    private void FixedUpdate()
    {
        float turnVel = MGR.game.laser.owner == this && MGR.game.laser.laserTimer != -1 ? MGR.game.settings.laserChargeShipTurnVel : MGR.game.settings.shipTurnVel;
        rb.angularVelocity = turnVel * shipInput.turn;
        rb.AddForce(shipInput.thrust * MGR.game.settings.shipThrust * transform.right);

        Vector2 totalGrav = Vector2.zero;
        foreach (GameObject obj in MGR.game.star.GetComponent<Wrap>().clones)
        {
            Vector2 d = obj.transform.position - transform.position;
            totalGrav += d.WithMag(1 / d.sqrMagnitude);
        }
        rb.AddForce(totalGrav * MGR.game.settings.starGravity);
    }

    void Shoot()
    {
        GameObject thisPellet = Instantiate(MGR.game.pelletPrefab, shootOrigin.position, shootOrigin.rotation);
        thisPellet.GetComponent<Rigidbody2D>().linearVelocity = transform.right * MGR.game.settings.pelletSpeed + rb.linearVelocity.xy() * MGR.game.settings.pelletInheritVel;
        thisPellet.GetComponent<Wrap>().bounds = MGR.game.bounds;
        thisPellet.GetComponent<Pellet>().lifespan = MGR.game.settings.pelletLifespan;
        rb.AddForce(-transform.right * MGR.game.settings.shipRecoil, ForceMode2D.Impulse);
    }

    public void Die()
    {
        MGR.vfx.PtclBurst(transform.position, Vector3.right, 360, 250, 50, 3, GetComponent<SpriteRenderer>().color);
        died = true;
        MGR.game.shipDied = true;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out LaserPickup pickup))
        {
            MGR.game.laser.owner = this;
            MGR.game.laser.StartLaser();
        }
        
        if (killLayers.Contains(collision.gameObject))
        {
            
            MGR.vfx.Shake(4);
            
            if (LayerMask.LayerToName(collision.gameObject.layer) == "star")
            {
                //MGR.vfx.RadialImpactFrame(transform.position);
            }

            Die();
        }
    }
}
