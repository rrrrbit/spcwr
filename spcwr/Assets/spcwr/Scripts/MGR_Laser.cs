using RBitUtils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class MGR_Laser : MonoBehaviour
{
    public bool active;

    public Ship owner;
    public Scene scene;
    public PhysicsScene2D pScene;
    public List<List<Vector2>> positions;
    public List<LaserSeg> edges;
    public GameObject tracer;
    public Rigidbody2D tracerRb;

    public LaserSeg edgeColliderPrefab;

    bool endInMiddle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tracerRb = tracer.GetComponent<Rigidbody2D>();
        scene = SceneManager.CreateScene("trajectoryScene", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
        pScene = scene.GetPhysicsScene2D();
        SceneManager.MoveGameObjectToScene(tracer, scene);

        positions = new List<List<Vector2>>();
        edges = new List<LaserSeg>();

        for (int i = 0; i < MGR.game.settings.laserMaxWrap; i++)
        {
            edges.Add(Instantiate(edgeColliderPrefab));
        }
    }

    void UpdateEdgeColliders()
    {
        for (int i = 0; i < MGR.game.settings.laserMaxWrap; i++)
        {
            edges[i].col.enabled = edges[i].col.SetPoints(positions[i]);
        }
    }

    void UpdateLineRenderers()
    {
        for (int i = 0; i < MGR.game.settings.laserMaxWrap; i++)
        {
            edges[i].line.enabled = edges[i].col.enabled;

            edges[i].line.positionCount = positions[i].Count;
            edges[i].line.SetPositions(positions[i].Select(x => x.xy()).ToArray());

            bool isStart = i == 0;
            bool isEnd = i == MGR.game.settings.laserMaxWrap - 1;

            edges[i].isStart = isStart;
            bool extendStart = !isStart;
            bool extendEnd = !isEnd || !endInMiddle;

            if (extendStart)
            {
                Vector2 extendedStart = positions[i][0] + (positions[i][0] - positions[i][1]) * 20;
                edges[i].line.SetPosition(0, extendedStart);
            }
            if (extendEnd)
            {
                Vector2 extendedEnd = positions[i][^1] + (positions[i][^1] - positions[i][^2]) * 20;
                edges[i].line.SetPosition(edges[i].line.positionCount - 1, extendedEnd);
            }
        }
    }

    void UpdateTrajectory()
    {
        tracer.transform.SetPositionAndRotation(owner.shootOrigin.position, owner.shootOrigin.rotation);
        tracerRb.linearVelocity = owner.shootOrigin.right * MGR.game.settings.laserStartVel;

        int currentWraps = 0;
        float accmLength = 0;
        bool ignoreForces = false;

        positions.Clear();
        List<Vector2> thisSegment = new List<Vector2>();
        while (accmLength < MGR.game.settings.laserMaxLength && currentWraps < MGR.game.settings.laserMaxWrap)
        {
            if (!ignoreForces)
            {
                Vector2 totalGrav = Vector2.zero;
                foreach (GameObject obj in MGR.game.star.GetComponent<Wrap>().clones)
                {
                    Vector2 d = obj.transform.position - tracer.transform.position;
                    totalGrav += d.WithMag(1 / d.sqrMagnitude);
                }
                tracerRb.AddForce(totalGrav * MGR.game.settings.starGravity);
            }

            var oldPos = tracer.transform.position;
            pScene.Simulate(Time.fixedDeltaTime);


            if (tracerRb.linearVelocity.sqrMagnitude > MGR.game.settings.laserStartVel * MGR.game.settings.laserStartVel * 3) ignoreForces = true;
            
            thisSegment.Add(tracer.transform.position);
            if (tracer.GetComponent<Wrap>().WrapPos())
            {
                positions.Add(thisSegment.ToList());
                thisSegment.Clear();
                currentWraps++;
            }
            else
            {
                accmLength += (tracer.transform.position - oldPos).magnitude;
            }
        }
        endInMiddle = currentWraps < MGR.game.settings.laserMaxWrap;
        if (endInMiddle)
        {
            positions.Add(thisSegment.ToList());
        }

        print(positions.Count);

        positions.Last().Remove(positions.Last().Last());

        UpdateEdgeColliders();
        UpdateLineRenderers();

    }

    // Update is called once per frame
    void Update()
    {
        if (!active) return;
        UpdateTrajectory();
    }
}
