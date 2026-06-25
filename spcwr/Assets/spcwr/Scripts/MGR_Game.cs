using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MGR_Game : MonoBehaviour
{
    public float restartTime;
    public float fastRestartTime;
    public float fastRestartThreshold;
    public int scoreA;
    public int scoreB;

    public float gameTimer;
    
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

    public Color colorShipA;
    public Color colorShipB;

    public GameObject pelletPrefab;
    public GameObject laserPickupPrefab;

    public List<GameObject> tempObjs;

    public float laserPickupEjectForce;

    public bool shipDied;
    public bool downtime;

    public TMP_Text winText;
    public TMP_Text scoreText;

    public GameObject debugPanel;
    public GameObject pauseOverlay;
    public MGR_pause pause;

    public bool paused;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        laser = GetComponent<MGR_Laser>();
        tempObjs = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (downtime) return;
        gameTimer += Time.deltaTime;
        if (shipDied) FinishGame();

        scoreText.text = "session score:\n" +
            "<#" + colorShipA.ToHexString() + ">" + scoreA.ToString().PadLeft(3, '0') +
            "</color> - " +
            "<#" + colorShipB.ToHexString() + ">" + scoreB.ToString().PadLeft(3, '0') + 
            "</color>\n\nround time:\n" +
            Mathf.RoundToInt(gameTimer).ToString().PadLeft(4, '0');

        if (MGR.input.actionsMenu.Pause.WasPressedThisFrame())
        {
            debugPanel.SetActive(!debugPanel.activeSelf);
        }
        if (MGR.input.actionsMenu.MainPause.WasPressedThisFrame())
        {
            paused = !paused;
        }

    }

    void FinishGame()
    {
        downtime = true;
        
        if (shipA.died && shipB.died)
        {
            winText.text = "DRAW";
            winText.color = Color.white;
            winText.GetComponent<Flicker>().InInstant();
        }
        else if (shipA.died)
        {
            winText.text = "WIN";
            winText.color = colorShipB;
            winText.GetComponent<Flicker>().InInstant();
            scoreB++;
        }
        else if (shipB.died)
        {
            winText.text = "WIN";
            winText.color = colorShipA;
            winText.GetComponent<Flicker>().InInstant();
            scoreA++;
        }

        
        StartCoroutine(FinishGameCoroutine());
    }

    IEnumerator FinishGameCoroutine()
    {
        float thisRestartTime = gameTimer > fastRestartThreshold ? restartTime : fastRestartTime;
        yield return new WaitForSeconds(thisRestartTime);
        shipDied = false;
        downtime = false;

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

        star.GetComponent<Flicker>().In();
        shipA.GetComponent<Flicker>().In();
        shipB.GetComponent<Flicker>().In();

        shipA.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        shipB.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        star.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        star.hitCount = 0;

        winText.GetComponent<Flicker>().Out();
        gameTimer = 0;
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
    public float laserChargeShipTurnVel;
    public float laserPickupPelletKnockback;
    public float laserWidth;
    public float laserStartVel;
    public float laserMaxLength;
    public float laserMaxWrap;
    public float laserChargeTime;
    public float laserRecoil;
}