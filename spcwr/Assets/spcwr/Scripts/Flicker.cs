using TMPro;
using UnityEngine;

public class Flicker : MonoBehaviour
{
    [SerializeField] protected float startDelay;
    [SerializeField] protected float time;
    [SerializeField] protected float speed;
    [SerializeField] protected bool shown = true;
    [SerializeField] protected bool startShown = false;
    [SerializeField] protected float timer;
    protected float alpha;
    protected virtual void Start()
    {
        timer = startShown ? 0 : time + startDelay;
    }
    protected virtual void Update()
    {
        timer = shown ? Mathf.Max(0, timer - Time.unscaledDeltaTime) : Mathf.Min(time + startDelay, timer + Time.unscaledDeltaTime);
        if (shown)
        {
            alpha = timer > time ? 0 : timer > 0 ? Mathf.Floor(timer * speed % 2) : 1;
        }
        else
        {
            alpha = timer < startDelay ? 1 : timer < startDelay + time ? Mathf.Floor(timer * speed % 2) : 0;
        }
    }

    public virtual void In()
    {
        timer = time + startDelay;
        shown = true;
    }

    public virtual void InInstant()
    {
        timer = 0;
        shown = true;
    }

    public virtual void Out()
    {
        timer = 0;
        shown = false;
    }
}