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
    float currentHitPoints;

    void Awake()
    {
        currentHitPoints = maxHitPoints;
        updateHPonUI();
    }

    public void TakeDamage(float damage)
    {
        currentHitPoints -= damage;
        updateHPonUI();
        if (currentHitPoints <= 0) Death();
    }

    private void updateHPonUI()
    {
        if (maxHitPoints >= currentHitPoints) hpSlider.maxValue = maxHitPoints;
        else hpSlider.maxValue = currentHitPoints;
        hpSlider.value = currentHitPoints;
        hpText.text = currentHitPoints + "/" + maxHitPoints;
    }

    void Death()
    {
        //TODO: Play death animation
        //TODO: Record stats
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0f;
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
        updateHPonUI();
        return true;
    }
}
