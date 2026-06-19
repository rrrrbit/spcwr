using RBitUtils;
using UnityEngine;

public class Wrap : MonoBehaviour
{
    public RectTransform bounds;
    public GameObject clonePrefab;
    Bounds wrapBoundsWorld;
    public GameObject[] clones;
    Vector3 clonePos;

    private void Start()
    {
        clones = new GameObject[4];
        clones[0] = gameObject;
        for (int i = 1; i < 4; i++)
        {
            clones[i] = Instantiate(clonePrefab, transform);
        }
        SetBounds();
    }

    // Update is called once per frame
    void Update()
    {
        WrapPos();
        UpdateClones();
    }

    public void SetBounds()
    {
        Vector3[] corner = new Vector3[4];
        bounds.GetWorldCorners(corner); // get the screen (technically not the whole screen but a square in the ui that i can resize how i want)
        wrapBoundsWorld.min = corner[0]; // bottom left corner
        wrapBoundsWorld.max = corner[2]; // top right corner
    }

    public bool WrapPos()
    {
        
        
        


        Vector2 clampPos = new(
            MathPlus.Wrap(transform.position.x, wrapBoundsWorld.min.x, wrapBoundsWorld.max.x), // wrap x between minimum and maximum
            MathPlus.Wrap(transform.position.y, wrapBoundsWorld.min.y, wrapBoundsWorld.max.y) // same for y
            );

        bool wrapped = transform.position.xy() != clampPos;
        transform.position = clampPos;

        return wrapped;
    }

    private void OnDestroy()
    {
        foreach (GameObject obj in clones)
        {
            Destroy(obj);
        }
    }

    void UpdateClones()
    {
        foreach (GameObject obj in clones[1..4])
        {
            obj.transform.rotation = transform.rotation;
            obj.transform.localScale = Vector3.one;
        }

        clonePos = transform.position + wrapBoundsWorld.size.Scaled(new Vector3(
            transform.position.x > wrapBoundsWorld.center.x ? -1 : 1,
            transform.position.y > wrapBoundsWorld.center.y ? -1 : 1,
            1
            ));

        clones[1].transform.position = new Vector3(clonePos.x, transform.position.y);
        clones[2].transform.position = new Vector3(transform.position.x, clonePos.y);
        clones[3].transform.position = clonePos;
    }
}

