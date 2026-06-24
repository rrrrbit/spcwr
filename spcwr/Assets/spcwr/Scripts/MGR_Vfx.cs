using System.Collections;
using UnityEngine;

public class MGR_Vfx : MonoBehaviour
{
    public GameCam cam;

    public float starShakeMultiplier;

    public ParticleSystem ptclBurstPrefab;

    public Material radialImpactFrameMaterial;

    public Material directionalImpactFrameMaterial;

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

    public void RadialImpactFrame(Vector3 center)
    {
        
        StartCoroutine(RadIFCoroutine(impactFrameLength, cam.cam.WorldToViewportPoint(center)-Vector3.one/2));
    }
    public IEnumerator RadIFCoroutine(float length, Vector2 center)
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

    public void DirectionalImpactFrame(Vector3 center, Vector3 direction)
    {

        StartCoroutine(DirectionalIFCoroutine(impactFrameLength, cam.cam.WorldToViewportPoint(center) - Vector3.one / 2, direction));
    }

    public void DEBUGLineImpactFrame(Vector3 center, Vector3 direction)
    {
        directionalImpactFrameMaterial.SetVector("_offset", cam.cam.WorldToViewportPoint(center) - Vector3.one / 2);
        directionalImpactFrameMaterial.SetVector("_direction", direction);
    }

    public IEnumerator DirectionalIFCoroutine(float length, Vector2 center, Vector3 dir)
    {
        directionalImpactFrameMaterial.SetVector("_offset", center);
        directionalImpactFrameMaterial.SetVector("_direction", dir);
        directionalImpactFrameMaterial.SetInt("_on", 1);

        directionalImpactFrameMaterial.SetInt("_threshold", 1);
        directionalImpactFrameMaterial.SetInt("_invert", 1);

        yield return new WaitForSeconds(length);

        directionalImpactFrameMaterial.SetInt("_invert", 0);

        yield return new WaitForSeconds(length);

        directionalImpactFrameMaterial.SetInt("_threshold", 0);
        directionalImpactFrameMaterial.SetInt("_invert", 1);

        yield return new WaitForSeconds(length);

        directionalImpactFrameMaterial.SetInt("_on", 0);
    }

}
