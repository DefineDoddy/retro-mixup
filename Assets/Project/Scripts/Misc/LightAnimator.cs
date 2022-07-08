using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightAnimator : MonoBehaviour
{
    public float returnSpeed = 10;
    
    private Light2D _light;
    private float _initialIntensity;

    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponent<Light2D>();
        _initialIntensity = _light.intensity;
        _light.intensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _light.intensity = Mathf.Lerp(_light.intensity, 0, Time.deltaTime * returnSpeed);
    }

    public void Flash()
    {
        _light.intensity = _initialIntensity;
    }
}
