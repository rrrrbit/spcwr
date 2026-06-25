using UnityEngine;

public class MGR_pause : MonoBehaviour
{
    public bool paused;

    public GameObject pauseOverlay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MGR.input.actionsMenu.MainPause.WasPressedThisFrame())
        {
            paused = !paused;
        }

        pauseOverlay.SetActive(paused);
        Time.timeScale = paused ? 0 : 1;
    }
}
