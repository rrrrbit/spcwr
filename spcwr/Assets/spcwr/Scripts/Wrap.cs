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
        wrapBoundsWorld.SetMinMax(bounds.GetWorldRect().min, bounds.GetWorldRect().max);
    }

    // Update is called once per frame
    void Update()
    {
        WrapPos();
        UpdateClones();
    }

    /// <summary>
    /// Wrap a position within the boundary.
    /// </summary>
    /// <returns>Whether or not the position was changed i.e. wrapped around</returns>
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

