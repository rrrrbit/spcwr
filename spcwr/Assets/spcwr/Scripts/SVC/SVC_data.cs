using System;
using UnityEngine;

public class SVC_data : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public struct GameSettings
{
    [Header("Star")]
    public float starGravity;
    public float starPelletKnockback;
    [Header("Ship Movement")]
    public float shipTurnVel;
    public float shipThrust;
    [Header("Ship Shooting")]
    public float shipReloadTime;
    public float shipRecoil;
    public float pelletSpeed;
    public float pelletLifespan;
    public float pelletInheritVel;
    [Header("Laser")]
    public float laserChargeShipTurnVel;
    public float laserPickupPelletKnockback;
    public float laserWidth;
    public float laserStartVel;
    public float laserMaxLength;
    public float laserMaxWrap;
    public float laserChargeTime;
    public float laserRecoil;
}