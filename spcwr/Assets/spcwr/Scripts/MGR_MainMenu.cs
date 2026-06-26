using UnityEngine;
using UnityEngine.SceneManagement;

public class MGR_MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MGR.input.actionsMenu.Any.WasCompletedThisFrame())
        {
            SceneManager.LoadScene("settingsMenu");
        }
    }
}
