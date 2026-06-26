using UnityEngine;
using UnityEngine.SceneManagement;

public class MGR_MainMenu : MonoBehaviour
{
    SVC_Input input;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = SVC.Get<SVC_Input>();
    }

    // Update is called once per frame
    void Update()
    {
        if (input.actionsMenu.Any.WasCompletedThisFrame())
        {
            SceneManager.LoadScene("settingsMenu");
        }
    }
}
