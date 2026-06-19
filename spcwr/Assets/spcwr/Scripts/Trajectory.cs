using RBitUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trajectory : MonoBehaviour
{
    public Star star;
    public Ship ship;
    public Transform start;
    public Scene scene;
    public PhysicsScene2D pScene;
    public int maxFrameSim;
    public Vector2[] positions;
    public float startVel;
    public Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scene = SceneManager.CreateScene("trajectoryScene", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
        pScene = scene.GetPhysicsScene2D();
        SceneManager.MoveGameObjectToScene(gameObject, scene);

        positions = new Vector2[maxFrameSim];
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = start.position;
        transform.rotation = start.rotation;
        rb.linearVelocity = transform.right * startVel;
        for (int i = 0; i < maxFrameSim; i++)
        {
            Vector2 totalGrav = Vector2.zero;
            Vector2 d = star.transform.position - transform.position;
            totalGrav += d.WithMag(1 / d.sqrMagnitude);
            foreach (GameObject obj in star.GetComponent<Wrap>().clones)
            {
                d = obj.transform.position - transform.position;
                totalGrav += d.WithMag(1 / d.sqrMagnitude);
            }
            rb.AddForce(totalGrav * star.gravity);

            var oldPos = transform.position;
            pScene.Simulate(Time.fixedDeltaTime);
            positions[i] = transform.position;
            Debug.DrawLine(oldPos, transform.position, Color.green);
        }
    }
}
