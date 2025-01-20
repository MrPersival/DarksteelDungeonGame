using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPSystem : MonoBehaviour
{
    [SerializeField]
    float xpToLevelUp = 100f;
    [SerializeField]
    int attributesPointsPerLevel = 1;
    [SerializeField]
    TextMeshProUGUI uiValue;
    [SerializeField]
    TextMeshProUGUI unspendXPPoints;
    [SerializeField]
    Slider xpSlider;

    PlayerController playerControllerScript;

    public int attributesPoints = 0;
    public int playerLevel = 1;

    float xp = 0f;

    private void Start()
    {
        updateUI();
        playerControllerScript = GetComponent<PlayerController>();
    }

    public void addXP(float xpToAdd)
    {
        xp += xpToAdd * GetComponent<AttributesSystem>().playerXPGainCoef;
        if(xp >= xpToLevelUp)
        {
            xp -= xpToLevelUp;
            attributesPoints += attributesPointsPerLevel;
            playerLevel += 1;
            GetComponent<AttributesSystem>().updateAttributes();
            playerControllerScript.audioSource.PlayOneShot(playerControllerScript.levelUpSound);
        }
        updateUI();
    }

    private void updateUI()
    {
        uiValue.text = MathF.Round(xp, 0) + "/" + xpToLevelUp;
        if (attributesPoints == 0) unspendXPPoints.gameObject.SetActive(false);
        else
        {
            unspendXPPoints.gameObject.SetActive(true);
            unspendXPPoints.text = Convert.ToString(attributesPoints);
        }
        xpSlider.maxValue = xpToLevelUp;
        xpSlider.value = xp;
    }

    public bool changeAPValue(int value)
    {
        if (value < 0) return false;
        else attributesPoints = value;
        updateUI();
        return true;
    }
}
