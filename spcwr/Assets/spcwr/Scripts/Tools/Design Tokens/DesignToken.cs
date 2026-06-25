using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public abstract class DesignToken<T> : ScriptableObject, IDesignToken
{
    public T value;
    public static implicit operator T(DesignToken<T> token)
    {
        return token.value;
    }
    public event System.Action Reload;
    public void InvokeReload()
    {
        Reload?.Invoke();
    }
}

public interface IDesignToken
{
    public void InvokeReload();
}
