using System.Collections;
using UnityEngine;

public class MGR_Vfx : MonoBehaviour
{
    public GameCam cam;

    public float starShakeMultiplier;

    public ParticleSystem ptclBurstPrefab;

    public Material radialImpactFrameMaterial;

    public float impactFrameLength;
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

    public void PtclBurst(Vector3 position, Vector3 angle, float arcDegrees, int number, float speed, float lifetime)
    {
        var thisBurst = Instantiate(ptclBurstPrefab, position, Quaternion.identity);
        thisBurst.transform.right = angle;
        var main = thisBurst.main;
        var emission = thisBurst.emission;
        var shape = thisBurst.shape;
        
        main.startSpeed = new(0, speed);
        main.startLifetime = new(lifetime / 10, lifetime);

        shape.arc = arcDegrees;
        shape.rotation = new(0, 0, -arcDegrees / 2);
        var burst = emission.GetBurst(0);

        burst.count = number;

        emission.SetBurst(0, burst);

        thisBurst.Play();
    }

    public void ImpactFrame(Vector3 center)
    {
        
        StartCoroutine(ImpactFrameCoroutine(impactFrameLength, cam.cam.WorldToViewportPoint(center)-Vector3.one/2));
    }

    public IEnumerator ImpactFrameCoroutine(float length, Vector2 center )
    {
        radialImpactFrameMaterial.SetVector("_offset", center);
        radialImpactFrameMaterial.SetInt("_on", 1);

        radialImpactFrameMaterial.SetInt("_threshold", 1);
        radialImpactFrameMaterial.SetInt("_invert", 1);

        yield return new WaitForSeconds(length);

        radialImpactFrameMaterial.SetInt("_invert", 0);

        yield return new WaitForSeconds(length);

        radialImpactFrameMaterial.SetInt("_threshold", 0);
        radialImpactFrameMaterial.SetInt("_invert", 1);

        yield return new WaitForSeconds(length);

        radialImpactFrameMaterial.SetInt("_on", 0);
    }
}
