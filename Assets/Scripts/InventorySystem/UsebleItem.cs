using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UsebleItem : InventoryItemScriptebleObject
{
    PlayerController playerControllerScript;


   
    public void useItem()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        //Later, if need arises, can be implimented as diffrent child scripteble objects with own funcitons. Not required right now, I guess, and a little bit more painfull to do.
        if (itemName == "Medium health potion")
        {
            playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
            AudioSource audioSource = playerControllerScript.audioSource;
            if (player.GetComponent<PlayerHitPoints>().restoreHP(additionalValue))
            {
                player.GetComponent<PlayerInventory>().removeItem(this);
                audioSource.volume = 0.25f;
                audioSource.PlayOneShot(playerControllerScript.drinkPotionSound);
                audioSource.volume = 1f;
            }
        }
      


    }
}
