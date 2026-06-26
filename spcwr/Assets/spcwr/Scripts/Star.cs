using RBitUtils;
using UnityEngine;

public class Star : MonoBehaviour
{
    public Collider2D col;
    public Rigidbody2D rb;
    [SerializeField] LayerMask knockLayers;

    public Renderer bg;

    public int hitCount;
    public int laserPickupSpawnHits;

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
        
        bg.material.SetVector("_warpPos0", transform.position);
        bg.material.SetVector("_warpPos1", wrap.clones[1].transform.position);
        bg.material.SetVector("_warpPos2", wrap.clones[2].transform.position);
        bg.material.SetVector("_warpPos3", wrap.clones[3].transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (knockLayers.Contains(collision.gameObject))
        {
            if (LayerMask.LayerToName(collision.gameObject.layer) == "pellet")
            {
                hitCount++;
                if (hitCount == laserPickupSpawnHits)
                {
                    hitCount = 0;
                    MGR_Game.game.SpawnLaserPickup();
                }
            }
            
            Vector3 d = (transform.position - collision.transform.position).normalized;
            Vector2 force = Vector3.Project(collision.relativeVelocity, d) * MGR_Game.game.settings.starPelletKnockback;
            MGR_Game.vfx.Shake(force.magnitude * MGR_Game.vfx.starShakeMultiplier);
            rb.AddForce(force, ForceMode2D.Impulse);
        }
    }
}
