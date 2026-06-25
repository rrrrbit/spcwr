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
    public RectTransform bounds;

    public Sprite spriteShipA;
    public Sprite spriteShipB;

    public GameObject pelletPrefab;
    public GameObject laserPickupPrefab;

    public float laserPickupEjectForce;
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

    public void SpawnLaserPickup()
    {
        var thisPickup = Instantiate(laserPickupPrefab, star.transform.position, Quaternion.identity);
        thisPickup.GetComponent<Wrap>().bounds = bounds;
        thisPickup.GetComponent<Rigidbody2D>().AddForce(UnityEngine.Random.insideUnitCircle.normalized * laserPickupEjectForce, ForceMode2D.Impulse);
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
    public float laserPickupPelletKnockback;
    public float laserWidth;
    public float laserStartVel;
    public float laserMaxLength;
    public float laserMaxWrap;
    public float laserChargeTime;
    public float laserRecoil;
}