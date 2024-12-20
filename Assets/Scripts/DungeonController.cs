using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonController : MonoBehaviour
{
    public List<GameObject> levels = new List<GameObject>();
    public UIController uiController;
    int activeLevel = 1;

    //TODO: Add step sounds, pause and black screen before teleportation
    public void generationDone()
    {
        levels[0].SetActive(true);
        activeLevel = 1;
    }

    public void enteredOnStairs(bool isStairsUp)
    {
        if(!isStairsUp)
        {
            if(activeLevel == levels.Count)
            {
                uiController.gameWon();
            }
            else
            {
                levels[activeLevel - 1].SetActive(false);
                activeLevel++;
                levels[activeLevel - 1].SetActive(true);
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().teleport(levels[activeLevel - 1].transform.Find("EnterPoint(Clone)").transform.position);
            }
        }
        else
        {
            if(activeLevel == 1)
            {
                uiController.changeTooltipText("It was hard enough to get here. I don't want to go back.");
            }
            else
            {
                levels[activeLevel - 1].SetActive(false);
                activeLevel--;
                levels[activeLevel - 1].SetActive(true);
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().teleport(levels[activeLevel - 1].transform.Find("ExitPoint(Clone)").transform.position);
            }
        }
    }

    public void exitedFromStairs()
    {
        uiController.changeTooltipText("");
    }
}
