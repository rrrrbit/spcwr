using UnityEngine;

public class MGR_Vfx : MonoBehaviour
{
    public GameCam cam;

    public float starShakeMultiplier;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shake(float strength)
    {
        cam.shakeMagnitudeEased.y += strength;
    }
}
