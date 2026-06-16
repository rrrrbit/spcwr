using UnityEngine;

public class Wrap : MonoBehaviour
{
    public RectTransform bounds;
    Bounds wrapBoundsWorld;

    // Update is called once per frame
    void Update()
    {
        Vector3[] corner = new Vector3[4];
        bounds.GetWorldCorners(corner); // get the screen (technically not the whole screen but a square in the ui that i can resize how i want)
        wrapBoundsWorld.min = corner[0]; // bottom left corner
        wrapBoundsWorld.max = corner[2]; // top right corner


        Vector2 clampPos = new(
            RBitUtils.MathPlus.Wrap(transform.position.x, wrapBoundsWorld.min.x, wrapBoundsWorld.max.x), // wrap x between minimum and maximum
            RBitUtils.MathPlus.Wrap(transform.position.y, wrapBoundsWorld.min.y, wrapBoundsWorld.max.y) // same for y
            );

        transform.position = clampPos;
    }
}

