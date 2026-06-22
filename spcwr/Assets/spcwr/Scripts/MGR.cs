using UnityEngine;

public class MGR : MonoBehaviour
{
    public static MGR instance;
    public static MGR_Game game;
    public static MGR_Input input;
    public static MGR_Vfx vfx;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        game = GetComponent<MGR_Game>();
        input = GetComponent<MGR_Input>();
        vfx = GetComponent<MGR_Vfx>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
