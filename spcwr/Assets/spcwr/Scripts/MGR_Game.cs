using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MGR_Game : MonoBehaviour
{
    public Star star;
    public MGR_Laser laser;
    public Ship shipA;
    public Ship shipB;
    public GameSettings settings;

    public Sprite spriteShipA;
    public Sprite spriteShipB;

    public GameObject pelletPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        laser = GetComponent<MGR_Laser>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
    [Header("Laser")]
    public float laserWidth;
    public float laserStartVel;
    public float laserMaxLength;
    public float laserMaxWrap;
    public float laserChargeTime;
    public float laserRecoil;
}