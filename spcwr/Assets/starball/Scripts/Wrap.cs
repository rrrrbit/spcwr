using UnityEngine;

public class Wrap : MonoBehaviour
{
    public RectTransform bounds;
    public Camera cam;
    Bounds wrapBoundsWorld;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3[] corner = new Vector3[4];
        bounds.GetWorldCorners(corner);
        wrapBoundsWorld.min = corner[0];
        wrapBoundsWorld.max = corner[2];

        RBitUtils.DebugPlus.DrawBounds(wrapBoundsWorld, Color.yellow);

        Vector2 clampPos = new(
            RBitUtils.MathPlus.Wrap(transform.position.x, wrapBoundsWorld.min.x, wrapBoundsWorld.max.x),
            RBitUtils.MathPlus.Wrap(transform.position.y, wrapBoundsWorld.min.y, wrapBoundsWorld.max.y)
            );

        transform.position = clampPos;
    }
}
