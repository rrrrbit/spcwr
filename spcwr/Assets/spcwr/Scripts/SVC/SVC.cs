using System;
using System.Collections.Generic;
using UnityEngine;

public class SVC : MonoBehaviour
{
    public static SVC Instance { get; private set; }
    Dictionary<Type, object> services = new();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance != null) Debug.LogWarning("Replacing service manager");
        Instance = this;

        RegisterService(GetComponent<SVC_Data>());
        
    }

    void RegisterService<T>(T impl)
    {
        services[typeof(T)] = impl;
    }

    public static T Get<T>()
    {
        if (Instance.services.TryGetValue(typeof(T), out var impl))
        {
            return (T)impl;
        }
        else
        {
            throw new Exception($"Manager of type {typeof(T)} not found");
        }
    }
}