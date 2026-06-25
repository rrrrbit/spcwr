using RBitUtils;
using UnityEngine;

public class GameCam : MonoBehaviour
{
    public Camera cam;
    public float shakeMagnitude;
    public float shakeFreq;
    public RBitUtils.ResponseTypes.Spring shakeMagnitudeEased;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GetComponent<Camera>();
        shakeMagnitudeEased = new(0, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        shakeMagnitude = shakeMagnitudeEased.Update(Time.unscaledDeltaTime, 0);

        Vector2 shake = new Vector2(Mathf.PerlinNoise1D(Time.time * shakeFreq), Mathf.PerlinNoise1D(Time.time * shakeFreq + 500)) * 2 - Vector2.one;

        transform.position = (shake * shakeMagnitude).xy(-10);
    }
}
