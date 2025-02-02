using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SettingsMenuElementSaver : MonoBehaviour
{
    void Start()
    {
        if(TryGetComponent<TMP_Dropdown>(out TMP_Dropdown dropdown))
        {
            int value = PlayerPrefs.GetInt(gameObject.name, -1);
            if(value != -1) dropdown.value = value;
        }
        else if(TryGetComponent<Slider>(out Slider slider))
        {
            float value = PlayerPrefs.GetFloat(gameObject.name, -1);
            if(value != -1) slider.value = value; 
        }
        else
        {
            Debug.LogWarning("Unknown settings element " + gameObject.name + ". Could not get value from saved settings");
        }
    }

    private void OnDisable()
    {
        if (TryGetComponent<TMP_Dropdown>(out TMP_Dropdown dropdown))
        {
            PlayerPrefs.SetInt(gameObject.name, dropdown.value);
        }
        else if (TryGetComponent<Slider>(out Slider slider))
        {
            PlayerPrefs.SetFloat(gameObject.name, slider.value);

        }
        else
        {
            Debug.LogWarning("Unknown settings element " + gameObject.name + ". Could not set value to saved settings");
        }
        PlayerPrefs.Save();
    }
}
