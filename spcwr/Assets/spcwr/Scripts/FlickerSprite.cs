using TMPro;
using UnityEngine;

public class FlickerSprite : Flicker
{
    protected override void Update()
    {
        base.Update();

        var c = GetComponent<SpriteRenderer>().color;
        c.a = alpha;
        GetComponent<SpriteRenderer>().color = c;
    }
}