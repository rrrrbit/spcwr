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
    public List<LineRenderer> trajectoryLines;
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
    }

    public void Fire()
    {
        MGR.vfx.Shake(4);
        MGR.vfx.ImpactFrame(transform.position);

        for (int i = 0; i < MGR.game.settings.laserMaxWrap; i++)
        {
            var thisLaserSeg = Instantiate(edgeColliderPrefab);
            thisLaserSeg.maxLifespan = 1f;

            thisLaserSeg.Init();

            thisLaserSeg.col.enabled = thisLaserSeg.col.SetPoints(positions[i]);
            thisLaserSeg.line.enabled = thisLaserSeg.col.enabled;

            thisLaserSeg.line.positionCount = positions[i].Count;
            thisLaserSeg.line.SetPositions(positions[i].Select(x => x.xy()).ToArray());
            bool isStart = i == 0;
            bool isEnd = i == MGR.game.settings.laserMaxWrap - 1;

            thisLaserSeg.isStart = isStart;
            bool extendStart = !isStart;
            bool extendEnd = !isEnd || !endInMiddle;

            if (extendStart)
            {
                Vector2 extendedStart = positions[i][0] + (positions[i][0] - positions[i][1]) * 20;
                thisLaserSeg.line.SetPosition(0, extendedStart);
            }
            if (extendEnd)
            {
                Vector2 extendedEnd = positions[i][^1] + (positions[i][^1] - positions[i][^2]) * 20;
                thisLaserSeg.line.SetPosition(thisLaserSeg.line.positionCount - 1, extendedEnd);
            }

            thisLaserSeg.PlayPtcl();
        }
        owner.GetComponent<Rigidbody2D>().AddForce(-owner.transform.right * MGR.game.settings.laserRecoil, ForceMode2D.Impulse);
    }

    void UpdateTrajectoryLines()
    {
        for (int i = 0; i < MGR.game.settings.laserMaxWrap; i++)
        {
            trajectoryLines[i].positionCount = positions[i].Count;
            trajectoryLines[i].SetPositions(positions[i].Select(x => x.xy()).ToArray());

            bool isStart = i == 0;
            bool isEnd = i == MGR.game.settings.laserMaxWrap - 1;

            bool extendStart = !isStart;
            bool extendEnd = !isEnd || !endInMiddle;

            if (extendStart)
            {
                Vector2 extendedStart = positions[i][0] + (positions[i][0] - positions[i][1]) * 20;
                trajectoryLines[i].SetPosition(0, extendedStart);
            }
            if (extendEnd)
            {
                Vector2 extendedEnd = positions[i][^1] + (positions[i][^1] - positions[i][^2]) * 20;
                trajectoryLines[i].SetPosition(trajectoryLines[i].positionCount - 1, extendedEnd);
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
    }

    // Update is called once per frame
    void Update()
    {
        if (!active) return;
        UpdateTrajectory();
    }
}
