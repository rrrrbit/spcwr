using RBitUtils;
using Unity.VisualScripting;
using UnityEngine;

public class LaserPickup : MonoBehaviour
{
    public float iTime;
    public Collider2D col;
    public Rigidbody2D rb;
    [SerializeField] LayerMask knockLayers;

    Vector2 velocityLastFrame;
    [SerializeField] GameObject[] clearChildren;
    [SerializeField] ParticleSystem ptcl;

    private void Update()
    {
        iTime = Mathf.Max(iTime - Time.deltaTime, 0);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        MGR_Game.game.tempObjs.Add(gameObject);

    }

    private void OnDestroy()
    {
        ptcl.Stop();
    }

    private void FixedUpdate()
    {
        velocityLastFrame = GetComponent<Rigidbody2D>().linearVelocity;
        if (iTime > 0) return;

        Vector2 totalGrav = Vector2.zero;
        foreach (GameObject obj in MGR_Game.game.star.GetComponent<Wrap>().clones)
        {
            Vector2 d = obj.transform.position - transform.position;
            totalGrav += d.WithMag(1 / d.sqrMagnitude);
        }
        rb.AddForce(totalGrav * MGR_Game.game.settings.gravity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        

        if (knockLayers.Contains(collision.gameObject))
        {
            print("a");
            Vector3 d = (transform.position - collision.transform.position).normalized;
            Vector2 force = Vector3.Project(collision.relativeVelocity, d) * MGR_Game.game.settings.laserPickupBulletKnockback;
            rb.AddForce(force, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Ship ship) && iTime <= 0)
        {
            MGR_Game.vfx.PtclBurst(transform.position, Vector3.right, 360, 100, 40, 0.5f);
            foreach (GameObject obj in clearChildren)
            {
                Destroy(obj);
            }
            transform.DetachChildren();
            Destroy(gameObject);
        }

        if (LayerMask.LayerToName(collision.gameObject.layer) == "star" && iTime <= 0)
        {
            Vector2 deflect = Vector2.Reflect(velocityLastFrame, (transform.position.xy() - collision.ClosestPoint(transform.position)));

            MGR_Game.vfx.PtclBurst(transform.position, deflect, 60, 60, 50, 1);

            foreach (GameObject obj in clearChildren)
            {
                Destroy(obj);
            }
            transform.DetachChildren();
            Destroy(gameObject);
        }
    }
}
