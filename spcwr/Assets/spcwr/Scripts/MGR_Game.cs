using System;
using System.Collections;
using System.Collections.Generic;
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

    public Transform startTransformA;
    public Transform startTransformB;
    public Transform startTransformStar;

    public Sprite spriteShipA;
    public Sprite spriteShipB;

    public GameObject pelletPrefab;
    public GameObject laserPickupPrefab;

    public List<GameObject> tempObjs;

    public float laserPickupEjectForce;

    public bool shipDied;
    bool finishGame;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        laser = GetComponent<MGR_Laser>();
        tempObjs = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(finishGame) FinishGame();
        if (shipDied) finishGame = true;
    }

    void FinishGame()
    {
        finishGame = false;
        if (shipA.died && shipB.died)
        {
            print("DRAW");
        }
        else if (shipA.died)
        {
            print("SHIP B WIN");
        }
        else if (shipB.died)
        {
            print("SHIP A WIN");
        }

        shipDied = false;
        StartCoroutine(FinishGameCoroutine());
    }

    IEnumerator FinishGameCoroutine()
    {
        yield return new WaitForSeconds(3);

        foreach (GameObject obj in tempObjs)
        {
            Destroy(obj);
        }

        shipA.gameObject.SetActive(true);
        shipB.gameObject.SetActive(true);

        shipA.transform.SetPositionAndRotation(startTransformA.position, startTransformA.rotation);
        shipB.transform.SetPositionAndRotation(startTransformB.position, startTransformB.rotation);
        star.transform.SetPositionAndRotation(startTransformStar.position, startTransformStar.rotation);

        shipA.died = false;
        shipB.died = false;


        shipA.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        shipB.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        star.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;


        yield return null;

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