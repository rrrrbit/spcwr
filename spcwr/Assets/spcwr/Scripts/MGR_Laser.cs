using RBitUtils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class MGR_Laser : MonoBehaviour
{
    public bool active;

    [SerializeField] float laserTimer;
    [SerializeField] float trajLineAlphaMult;
    [SerializeField] AnimationCurve trajLineFlashLengthOverTime;

    public Ship owner;
    public Scene scene;
    public PhysicsScene2D pScene;
    public List<List<Vector2>> positions;
    public List<TrajectoryLine> trajectoryLines;
    public GameObject tracer;
    public Rigidbody2D tracerRb;

    public LaserSeg edgeColliderPrefab;
    public TrajectoryLine trajectoryLinePrefab;

    bool endInMiddle;
    public float wrapCountMinimumDistance;

    [SerializeField] float trajLineFlashAnim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tracerRb = tracer.GetComponent<Rigidbody2D>();
        scene = SceneManager.CreateScene("trajectoryScene", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
        pScene = scene.GetPhysicsScene2D();
        SceneManager.MoveGameObjectToScene(tracer, scene);

        positions = new List<List<Vector2>>();
        trajectoryLines = new List<TrajectoryLine>();
        for (int i = 0; i < MGR.game.settings.laserMaxWrap + 2; i++)
        {
            trajectoryLines.Add(Instantiate(trajectoryLinePrefab));
        }
    }

    public void Fire()
    {
        laserTimer = -1;
        trajLineFlashAnim = 0;
        
        MGR.vfx.Shake(4);
        MGR.vfx.DirectionalImpactFrame(owner.transform.position, -owner.transform.right);

        for (int i = 0; i < positions.Count; i++)
        {
            var thisLaserSeg = Instantiate(edgeColliderPrefab);
            thisLaserSeg.maxLifespan = 1f;

            thisLaserSeg.Init();

            thisLaserSeg.col.enabled = thisLaserSeg.col.SetPoints(positions[i]);
            thisLaserSeg.line.enabled = thisLaserSeg.col.enabled;

            thisLaserSeg.col.edgeRadius = MGR.game.settings.laserWidth / 2;

            thisLaserSeg.line.positionCount = positions[i].Count;
            thisLaserSeg.line.SetPositions(positions[i].Select(x => x.xy()).ToArray());
            bool isStart = i == 0;
            bool isEnd = i == MGR.game.settings.laserMaxWrap - 1;

            thisLaserSeg.isStart = isStart;
            bool extendStart = !isStart;
            bool extendEnd = !isEnd || !endInMiddle;

            if (extendStart && positions[i].Count > 1)
            {
                Vector2 extendedStart = positions[i][0] + (positions[i][0] - positions[i][1]) * 20;
                thisLaserSeg.line.SetPosition(0, extendedStart);
            }
            if (extendEnd && positions[i].Count > 1)
            {
                Vector2 extendedEnd = positions[i][^1] + (positions[i][^1] - positions[i][^2]) * 20;
                thisLaserSeg.line.SetPosition(thisLaserSeg.line.positionCount - 1, extendedEnd);
            }

            var overlapList = new List<Collider2D>();
            thisLaserSeg.col.Overlap(overlapList);

            foreach (Collider2D col in overlapList)
            {
                if (col.TryGetComponent(out Ship ship) && ship != owner)
                {
                    ship.Die();
                }
            }

            thisLaserSeg.PlayPtcl();
        }
        owner.GetComponent<Rigidbody2D>().AddForce(-owner.transform.right * MGR.game.settings.laserRecoil, ForceMode2D.Impulse);
    }

    void UpdateTrajectoryLines()
    {
        for (int i = 0; i < trajectoryLines.Count; i++)
        {
            if (i >= positions.Count)
            {
                trajectoryLines[i].gameObject.SetActive(false);
                continue;
            }
            trajectoryLines[i].gameObject.SetActive(true);
            trajectoryLines[i].line.positionCount = positions[i].Count;
            trajectoryLines[i].line.SetPositions(positions[i].Select(x => x.xy()).ToArray());

            bool isStart = i == 0;
            bool isEnd = i == MGR.game.settings.laserMaxWrap - 1;

            bool extendStart = !isStart;
            bool extendEnd = !isEnd || !endInMiddle;

            if (extendStart && positions[i].Count > 1)
            {
                Vector2 extendedStart = positions[i][0] + (positions[i][0] - positions[i][1]) * 20;
                trajectoryLines[i].line.SetPosition(0, extendedStart);
            }
            if (extendEnd && positions[i].Count > 1)
            {
                Vector2 extendedEnd = positions[i][^1] + (positions[i][^1] - positions[i][^2]) * 20;
                trajectoryLines[i].line.SetPosition(trajectoryLines[i].line.positionCount - 1, extendedEnd);
            }
        }
    }

    public void StartLaser()
    {
        laserTimer = MGR.game.settings.laserChargeTime;
    }

    void UpdateTrajectory()
    {
        tracer.transform.SetPositionAndRotation(owner.transform.position, owner.shootOrigin.rotation);
        tracerRb.linearVelocity = owner.shootOrigin.right * MGR.game.settings.laserStartVel;

        int currentWraps = 0;
        float accmLength = 0;
        float thisSegmentAccmLength = 0;
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

            bool afterShootOffset = accmLength * accmLength > (owner.transform.position - owner.shootOrigin.position).sqrMagnitude;

            if (afterShootOffset) // ignore everything before the shoot offset
            {
                thisSegment.Add(tracer.transform.position);
                
            }

            if (tracer.GetComponent<Wrap>().WrapPos())
            {
                positions.Add(thisSegment.ToList());
                if (afterShootOffset && thisSegmentAccmLength > wrapCountMinimumDistance) currentWraps++;
                thisSegment.Clear();
                thisSegmentAccmLength = 0;
            }
            else
            {
                accmLength += (tracer.transform.position - oldPos).magnitude;
                thisSegmentAccmLength += (tracer.transform.position - oldPos).magnitude;
            }
        }
        endInMiddle = currentWraps < MGR.game.settings.laserMaxWrap;
        if (endInMiddle)
        {
            positions.Add(thisSegment.ToList());
        }

        positions.Last().Remove(positions.Last().Last());

        UpdateTrajectoryLines();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var line in trajectoryLines)
        {
            line.line.enabled = laserTimer > 0;
        }

        if (owner == null) laserTimer = -1;

        if (laserTimer == -1) return;

        laserTimer -= Time.deltaTime;
        trajLineFlashAnim += Time.deltaTime / trajLineFlashLengthOverTime.Evaluate(1 - laserTimer / MGR.game.settings.laserChargeTime);

        foreach (var line in trajectoryLines)
        {
            line.line.startColor = new(1, 1, 1, (trajLineFlashAnim % 1 <= .5f) ? trajLineAlphaMult : 0);
            line.line.endColor = new(1, 1, 1, (trajLineFlashAnim % 1 <= .5f) ? trajLineAlphaMult : 0);

        }

        UpdateTrajectory();


        if (laserTimer <= 0) Fire();

    }
}
