using UnityEngine;

public class MGR_Pause : MonoBehaviour
{
    public bool paused;

    public GameObject pauseOverlay;

    public SVC_Input input;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = SVC.Get<SVC_Input>();
    }

    // Update is called once per frame
    void Update()
    {
        if (input.actionsMenu.MainPause.WasPressedThisFrame())
        {
            paused = !paused;
        }

        pauseOverlay.SetActive(paused);
        Time.timeScale = paused ? 0 : 1;
    }
}
