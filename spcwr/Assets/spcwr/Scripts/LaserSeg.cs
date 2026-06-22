using UnityEngine;

public class LaserSeg : MonoBehaviour
{
    public EdgeCollider2D col;
    public LineRenderer line;

    public bool isStart;

    public AnimationCurve startWidth;
    public AnimationCurve middleWidth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<EdgeCollider2D>();
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            line.widthCurve = startWidth;
        }
        else
        {
            line.widthCurve = middleWidth;
        }
    }
}
