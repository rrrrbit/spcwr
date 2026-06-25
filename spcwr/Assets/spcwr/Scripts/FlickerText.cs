using TMPro;
using UnityEngine;

public class FlickerText : Flicker
{
    protected override void Update()
    {
        base.Update();

        var c = GetComponent<TMP_Text>().color;
        c.a = alpha;
        GetComponent<TMP_Text>().color = c;
    }
}