using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderSmoothnes : MonoBehaviour
{
    public float smoothTime = 3f;
    public float smoothVelocity = 0;
    public float target;
    Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        if(slider.value != target) slider.value = Mathf.SmoothDamp(slider.value, target, ref smoothVelocity, smoothTime); 
    }
}
