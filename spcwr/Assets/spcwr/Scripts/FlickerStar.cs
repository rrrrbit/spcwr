using TMPro;
using UnityEngine;

public class FlickerStar : Flicker
{
    protected override void Update()
    {
        base.Update();

        var c = GetComponent<SpriteRenderer>().color;
        c.a = alpha;
        GetComponent<SpriteRenderer>().color = c;

        foreach (ParticleSystem ptcl in GetComponentsInChildren<ParticleSystem>())
        {
            var m = ptcl.emission;
            m.enabled = (alpha > 0.5f);
        }
    }
}