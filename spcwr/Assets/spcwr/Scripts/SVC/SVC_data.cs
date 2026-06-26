using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SVC_Data : MonoBehaviour
{
    public List<GameParameter> gameParameters = new();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public float GetParameter(string name)
    {
        foreach (var parameter in gameParameters)
        {
            if (parameter.name == name)
            {
                return parameter.value;
            }
        }
        return 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class GameParameter
{
    public string name;
    public float value;
}

[Serializable]
public struct GameSettings
{
    [Header("Star")]
    public float gravity;
    public float starBulletKnockback;

    [Header("Ship")]
    public float turnSpeed;
    public float thrust;
    public float bulletRecoil;
    public float laserRecoil;

    [Header("Bullet")]
    public float reloadTime;
    public float bulletVel;
    public float bulletLifespan;
    public float bulletInheritShipVel;

    [Header("LaserPickup")]
    public float laserPickupBulletKnockback;
    public int laserPickupSpawnStarHits;


    [Header("Laser")]
    public float laserChargeTime;
    public float laserChargeShipSpeed;
    public float laserWidth;
    public float laserStartVel;
    public float laserMaxLength;
    public float laserMaxWrap;
}