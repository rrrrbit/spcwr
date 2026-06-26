using RBitUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MGR_GameMain : MonoBehaviour
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

    public List<GameObject> tempObjs = new();

    public float laserPickupEjectForce;

    public bool shipDied;
    public bool downtime;

    public TMP_Text winText;
    public TMP_Text scoreText;

    public GameObject debugPanel;

    public bool paused;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        laser = GetComponent<MGR_Laser>();
    }

    // Update is called once per frame
    void Update()
    {
        if (downtime) return;
        gameTimer += Time.deltaTime;
        if (shipDied) FinishGame();

        scoreText.text = "session score:\n" +
            scoreA.ToString().PadLeft(3, '0').Color(colorShipA) + " - " + scoreB.ToString().PadLeft(3, '0').Color(colorShipB) +
            "\n\nround time:\n" +
            Mathf.RoundToInt(gameTimer).ToString().PadLeft(4, '0');
    }

    void FinishGame()
    {
        downtime = true;
        laser.owner = null;
        laser.laserTimer = -1;
        
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