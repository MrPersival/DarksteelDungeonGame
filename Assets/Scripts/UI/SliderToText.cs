using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SliderToText : MonoBehaviour
{
    TextMeshProUGUI textToUpdate;
    private void Awake()
    {
        textToUpdate = GetComponent<TextMeshProUGUI>();
    }
    public void sliderUpdate(float sliderValue)
    {
        if(textToUpdate != null) textToUpdate.text = sliderValue.ToString();
    }
}
