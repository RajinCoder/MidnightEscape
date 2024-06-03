using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public bool drainOverTime = true;
    public float maxBrightness = 1;
    public float minBrightness = 0.1f;
    public float drainRate = 2;
    public AudioClip flashSFX;

    Light light;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
       
    }

    // Update is called once per frame
    void Update()
    {
        // light dims over time 
        // instead light goes off when battery runs out <------
        light.intensity = Mathf.Clamp(light.intensity, minBrightness, maxBrightness);
        if (drainOverTime && light.enabled)
        {
            if (light.intensity > minBrightness)
            {
                light.intensity -= Time.deltaTime * (drainRate / 1000);
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            AudioSource.PlayClipAtPoint(flashSFX, transform.position);
            light.enabled = !light.enabled;
        }
    }

    public void ReplaceBattery(float amount)
    {
        light.intensity += amount;
    }
}
