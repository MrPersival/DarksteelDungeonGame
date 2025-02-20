using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerTutorial : MonoBehaviour
{
    public TextMeshProUGUI moveText;
    public TextMeshProUGUI dodgeText;
    public TextMeshProUGUI sprintText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI heavyAttackText;
    public TextMeshProUGUI inventoryText;

    public Color complitetTutorialTextColor;

    List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();

    private void Start()
    {
        texts.Add(moveText);
        texts.Add(dodgeText);
        texts.Add(sprintText);
        texts.Add(attackText);
        texts.Add(heavyAttackText);
        texts.Add(inventoryText);
    }
    public void playerMoved()
    {
        compliteTutorialStep(moveText);
    }

    public void playerDodged()
    {
        compliteTutorialStep(dodgeText);
    }
    public void playerSprintet()
    {
        compliteTutorialStep(sprintText);
    }
    public void playerAttacked()
    {
        compliteTutorialStep(attackText);
    }

    public void playerHeavyAttacked()
    {
        compliteTutorialStep(heavyAttackText);
    }

    public void playerOpenedInventory()
    {
        compliteTutorialStep(inventoryText);
    }

    void compliteTutorialStep(TextMeshProUGUI step)
    {
        if (step.color != complitetTutorialTextColor)
        {
            texts.Remove(step);
            step.color = complitetTutorialTextColor;
            if(texts.Count <= 0) gameObject.SetActive(false);
        }
    }
}
