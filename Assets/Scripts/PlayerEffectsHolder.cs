using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static InventoryItem;

public class PlayerEffectsHolder : MonoBehaviour
{
    public TextMeshProUGUI hudText;

    List<UsableItemEffect> currentEffects = new List<UsableItemEffect>();
    AttributesSystem attributesSystem;
    PlayerHitPoints playerHitPoints;

    private void Start()
    {
        attributesSystem = GetComponent<AttributesSystem>();
        playerHitPoints = GetComponent<PlayerHitPoints>();
    }

    private void FixedUpdate()
    {
        if (hudText.text != "") hudText.text = ""; 
        if(currentEffects.Count > 0)
        {
            List<UsableItemEffect> expiredEffects = new List<UsableItemEffect>();
            for (int i = 0; i < currentEffects.Count; i++)
            {
                UsableItemEffect effect = currentEffects[i]; //If moddified directly throws out CS1612
                effect.timeOfEffectInSeconds -= Time.deltaTime;
                if (effect.timeOfEffectInSeconds < 0)
                {
                    expiredEffects.Add(currentEffects[i]);
                }
                else currentEffects[i] = effect;
            }

            foreach (UsableItemEffect effect in expiredEffects)
            {
                currentEffects.Remove(effect);
                UsableItemEffect negativeEffect = effect;
                negativeEffect.value = negativeEffect.value * -1;
                applyEffect(negativeEffect); //To remove effect, we apply negative version of it.
            }


            foreach (UsableItemEffect currentEffect in currentEffects)
            {
                hudText.text += currentEffect.getInfoInString();
            }
        }
    }

    public void addEffect(UsableItemEffect effect)
    {
        if (effect.hasTimeOfEffect) currentEffects.Add(effect);
        applyEffect(effect);
    }

    void applyEffect(UsableItemEffect effect)
    {
        switch (effect.itemEffect)
        {
            case ItemEffect.ChangeHp:
                playerHitPoints.restoreHP(effect.value);
                break;
            case ItemEffect.StatChange:
                switch (effect.statChange)
                {
                    case StatType.Strength:
                        attributesSystem.strength.baseValue += effect.value;
                        break;
                    case StatType.Dexterity:
                        attributesSystem.dexterity.baseValue += effect.value;
                        break;
                    case StatType.Charisma:
                        attributesSystem.charisma.baseValue += effect.value;
                        break;
                    case StatType.Intelligence:
                        attributesSystem.intelligence.baseValue += effect.value;
                        break;
                    case StatType.Luck:
                        attributesSystem.luck.baseValue += effect.value;
                        break;
                    case StatType.Damage:
                        attributesSystem.attackDamage.baseValue += effect.value;
                        break;
                    case StatType.Armor:
                        attributesSystem.armor.baseValue += effect.value;
                        break;
                    case StatType.AttackSpeed:
                        attributesSystem.attackSpeed.baseValue += effect.value;
                        break;
                    case StatType.MovementSpeed:
                        attributesSystem.moveSpeed.baseValue += effect.value;
                        break;
                    case StatType.MaxHP:
                        attributesSystem.maxHp.baseValue += effect.value;
                        break;
                    default:
                        break;
                }
                break;
        }
        attributesSystem.updateAttributes();
    }

}
