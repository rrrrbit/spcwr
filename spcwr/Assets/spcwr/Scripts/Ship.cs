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
        shipInput = isPlayerA ? MGR_Game.input.playerA : MGR_Game.input.playerB;
        var ptclEmission = exhaustParticle.emission;
        shootTimer = Mathf.Max(0f, shootTimer - Time.deltaTime);
        if (shootTimer <= 0 && shipInput.shoot)
        {
            Shoot();
            shootTimer = MGR_Game.game.settings.reloadTime;
        }

        ptclEmission.enabled = shipInput.thrust == 1;

        Color shipColor = isPlayerA ? MGR_Game.game.colorShipA : MGR_Game.game.colorShipB;
        foreach (GameObject i in GetComponent<Wrap>().clones)
        {
            i.GetComponent<SpriteRenderer>().color = shipColor;
        }
        var main = exhaustParticle.main;
        main.startColor = shipColor;
    }

    private void FixedUpdate()
    {
        float turnVel = MGR_Game.game.laser.owner == this && MGR_Game.game.laser.laserTimer != -1 ? MGR_Game.game.settings.laserChargeShipSpeed : MGR_Game.game.settings.turnSpeed;
        rb.angularVelocity = turnVel * shipInput.turn;
        rb.AddForce(shipInput.thrust * MGR_Game.game.settings.thrust * transform.right);

        Vector2 totalGrav = Vector2.zero;
        foreach (GameObject obj in MGR_Game.game.star.GetComponent<Wrap>().clones)
        {
            Vector2 d = obj.transform.position - transform.position;
            totalGrav += d.WithMag(1 / d.sqrMagnitude);
        }
        rb.AddForce(totalGrav * MGR_Game.game.settings.gravity);
    }

    void Shoot()
    {
        GameObject thisPellet = Instantiate(MGR_Game.game.pelletPrefab, shootOrigin.position, shootOrigin.rotation);
        thisPellet.GetComponent<Rigidbody2D>().linearVelocity = transform.right * MGR_Game.game.settings.bulletVel + rb.linearVelocity.xy() * MGR_Game.game.settings.bulletInheritShipVel;
        thisPellet.GetComponent<Wrap>().bounds = MGR_Game.game.bounds;
        thisPellet.GetComponent<Pellet>().lifespan = MGR_Game.game.settings.bulletLifespan;
        rb.AddForce(-transform.right * MGR_Game.game.settings.bulletRecoil, ForceMode2D.Impulse);
    }

    public void Die()
    {
        MGR_Game.vfx.PtclBurst(transform.position, Vector3.right, 360, 250, 50, 3, GetComponent<SpriteRenderer>().color);
        died = true;
        MGR_Game.game.shipDied = true;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out LaserPickup pickup))
        {
            MGR_Game.game.laser.owner = this;
            MGR_Game.game.laser.StartLaser();
        }
        
        if (killLayers.Contains(collision.gameObject))
        {
            
            MGR_Game.vfx.Shake(4);
            
            if (LayerMask.LayerToName(collision.gameObject.layer) == "star")
            {
                //MGR.vfx.RadialImpactFrame(transform.position);
            }

            Die();
        }
    }
}
