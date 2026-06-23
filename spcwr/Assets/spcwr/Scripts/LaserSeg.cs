using UnityEngine;

public class LaserSeg : MonoBehaviour
{
    public EdgeCollider2D col;
    public LineRenderer line;

    public bool isStart;

    public AnimationCurve startWidth;
    public AnimationCurve middleWidth;

    public AnimationCurve widthAnim;

    public Gradient startColor;
    public Gradient middleColor;
    public Gradient endColor;

    public float maxLifespan;
    public float lifespan;

    ParticleSystem ptcl;

    public void Init()
    {
        lifespan = maxLifespan;
        col = GetComponent<EdgeCollider2D>();
        line = GetComponent<LineRenderer>();
        ptcl = GetComponentInChildren<ParticleSystem>();

        
    }

    public void PlayPtcl()
    {
        Mesh lineMesh = new();
        line.BakeMesh(lineMesh);
        var shape = ptcl.shape;
        shape.mesh = lineMesh;
        ptcl.Play(lineMesh);
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            line.widthCurve = startWidth;
            line.colorGradient = startColor;
        }
        else
        {
            line.widthCurve = middleWidth;
            line.colorGradient = endColor;
        }

        lifespan -= Time.deltaTime;
        line.widthMultiplier = 5*widthAnim.Evaluate(1- lifespan / maxLifespan);


        if (lifespan < 0)
        {
            transform.DetachChildren();
            Destroy(gameObject);
        }

        
    }
}
