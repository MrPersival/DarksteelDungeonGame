using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerHitPoints : MonoBehaviour
{
    //Basic version, will be changed after abilities is implimented.
    public float maxHitPoints = 100f;
    public UnityEngine.UI.Slider hpSlider;
    public TMP_Text hpText;
    public GameObject gameOverScreen;
    public bool isDead = false;
    float currentHitPoints;
    [SerializeField]
    SliderSmoothnes sliderSmoothnes;

    PlayerController playerControllerScript;

    void Awake()
    {
        currentHitPoints = maxHitPoints;
        updateHitPointsUI();
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public void TakeDamage(float damage)
    {
        currentHitPoints -= damage - (damage * (GetComponent<AttributesSystem>().playerDamageResist / 100));
        playerControllerScript.DifferentHurtSounds();
        updateHitPointsUI();
        if (currentHitPoints <= 0) Death();
    }
    public void changeMaxHp(float newMaxHP)
    {
        if (maxHitPoints == currentHitPoints) currentHitPoints = newMaxHP;
        maxHitPoints = newMaxHP;
        updateHitPointsUI();
    }

    private void updateHitPointsUI()
    {
        if (maxHitPoints >= currentHitPoints) hpSlider.maxValue = maxHitPoints;
        else hpSlider.maxValue = currentHitPoints;
        sliderSmoothnes.target = currentHitPoints;
        hpText.text = MathF.Round(currentHitPoints, 0) + "/" + MathF.Round(maxHitPoints, 0);
    }

    void Death()
    {
        //TODO: Play death animation
        //TODO: Record stats
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0f;
        isDead = true;
        gameOverScreen.SetActive(true);
        
    }

    public bool restoreHP(float hpToRestore, bool isGoingOverMaxHp = false)
    {
        if(maxHitPoints > currentHitPoints + hpToRestore || isGoingOverMaxHp)
        {
            currentHitPoints += hpToRestore;
        }
        else
        {
            if(currentHitPoints >= maxHitPoints) return false;
            currentHitPoints = maxHitPoints;
        }
        updateHitPointsUI();
        return true;
    }
}
