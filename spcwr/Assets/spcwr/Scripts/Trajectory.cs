using RBitUtils;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trajectory : MonoBehaviour
{
    public bool active;
    
    public Transform start;
    public Scene scene;
    public PhysicsScene2D pScene;
    public List<Vector2> positions;
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
        tracerRb.linearVelocity = transform.right * MGR_Game.game.settings.laserStartVel;

        int currentWraps = 0;
        float accmLength = 0;
        bool ignoreForces = false;
        
        positions.Clear();
        while (accmLength < MGR_Game.game.settings.laserMaxLength && currentWraps < MGR_Game.game.settings.laserMaxWrap)
        {
            if (!ignoreForces)
            {
                Vector2 totalGrav = Vector2.zero;
                foreach (GameObject obj in MGR_Game.game.star.GetComponent<Wrap>().clones)
                {
                    Vector2 d = obj.transform.position - tracer.transform.position;
                    totalGrav += d.WithMag(1 / d.sqrMagnitude);
                }
                tracerRb.AddForce(totalGrav * MGR_Game.game.settings.starGravity);
            }

            var oldPos = tracer.transform.position;
            pScene.Simulate(Time.fixedDeltaTime);


            if (tracerRb.linearVelocity.sqrMagnitude > MGR_Game.game.settings.laserStartVel*MGR_Game.game.settings.laserStartVel *3) ignoreForces = true;
            //positions[i] = transform.position;
            accmLength+= (tracer.transform.position - oldPos).magnitude;
            if (tracer.GetComponent<Wrap>().WrapPos()) currentWraps++;
            else Debug.DrawLine(oldPos, tracer.transform.position, Color.green);      
            positions.Add(tracer.transform.position);
        }

        positions.RemoveAt(positions.Count-1);
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
