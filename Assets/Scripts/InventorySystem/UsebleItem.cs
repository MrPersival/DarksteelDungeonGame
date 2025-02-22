using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static InventoryItem;

public class UsebleItem : InventoryItemScriptebleObject
{
    PlayerController playerControllerScript;
    public List<UsableItemEffect> itemEffects = new List<UsableItemEffect>();

    public void useItem()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        foreach(UsableItemEffect effect in itemEffects)
        {
            //Health, and, eventually, mana potions has diffrent logic and therefore needs to be applied diffrently
            if (effect.itemEffect == ItemEffect.ChangeHp)
            {
                playerControllerScript = player.GetComponent<PlayerController>();
                AudioSource audioSource = playerControllerScript.audioSource;
                if (player.GetComponent<PlayerHitPoints>().restoreHP(additionalValue))
                {
                    audioSource.volume = 0.25f;
                    audioSource.PlayOneShot(playerControllerScript.drinkPotionSound);
                    audioSource.volume = 1f;
                    player.GetComponent<PlayerInventory>().removeItem(this);
                }
            }
            else
            {
                player.GetComponent<PlayerEffectsHolder>().addEffect(effect);
                player.GetComponent<PlayerInventory>().removeItem(this);
            }
        }

    }
}
