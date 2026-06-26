using UnityEngine;

public class MGR_Game : MonoBehaviour
{
    public static MGR_Game instance;
    public static MGR_GameMain game;
    public static MGR_GameInput input;
    public static MGR_Vfx vfx;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        game = GetComponent<MGR_GameMain>();
        input = GetComponent<MGR_GameInput>();
        vfx = GetComponent<MGR_Vfx>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
