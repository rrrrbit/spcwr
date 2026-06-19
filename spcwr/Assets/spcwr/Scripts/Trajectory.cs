using RBitUtils;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trajectory : MonoBehaviour
{
    public bool active;
    
    public Star star;
    public Transform start;
    public Scene scene;
    public PhysicsScene2D pScene;
    public int maxFrameSim;
    public int maxWrap;
    public List<Vector2> positions;
    public float startVel;
    public GameObject tracer;
    public Rigidbody2D tracerRb;

    public EdgeCollider2D edgeCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tracerRb = tracer.GetComponent<Rigidbody2D>();
        scene = SceneManager.CreateScene("trajectoryScene", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
        pScene = scene.GetPhysicsScene2D();
        SceneManager.MoveGameObjectToScene(tracer, scene);

        positions = new List<Vector2>();
    }
    
    void UpdateTrajectory()
    {
        tracer.transform.SetPositionAndRotation(start.position, start.rotation);
        tracerRb.linearVelocity = transform.right * startVel;

        int currentWraps = 0;
        int currentFrames = 0;
        bool ignoreForces = false;
        
        positions.Clear();
        while (currentFrames < maxFrameSim && currentWraps < maxWrap)
        {
            if (!ignoreForces)
            {
                Vector2 totalGrav = Vector2.zero;
                foreach (GameObject obj in star.GetComponent<Wrap>().clones)
                {
                    Vector2 d = obj.transform.position - tracer.transform.position;
                    totalGrav += d.WithMag(1 / d.sqrMagnitude);
                }
                tracerRb.AddForce(totalGrav * star.gravity);
            }

            var oldPos = tracer.transform.position;
            pScene.Simulate(Time.fixedDeltaTime);


            if (tracerRb.linearVelocity.sqrMagnitude > startVel*startVel*3) ignoreForces = true;
            //positions[i] = transform.position;
            currentFrames++;
            if (tracer.GetComponent<Wrap>().WrapPos()) currentWraps++;
            else Debug.DrawLine(oldPos, tracer.transform.position, Color.green);      
            positions.Append(tracer.transform.position);
        }

        print(positions.Count);
        edgeCollider.SetPoints(positions);
    }

    // Update is called once per frame
    void Update()
    {
        if (!active) return;
        UpdateTrajectory();
    }
}
