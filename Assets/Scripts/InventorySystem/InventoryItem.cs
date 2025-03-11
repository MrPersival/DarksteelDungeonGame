using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public string itemName;
    public float price;
    public int amount;
    public ItemsType type;
    public ItemsSubtype subType;
    public GameObject dropedObjectPrefab;
    public bool isOneTimeUse;
    public bool isEquipeble;
    public float additionalValue;
    [Header("Used only if isOneTimeUse == true")]
    public List<UsableItemEffect> itemEffects = new List<UsableItemEffect>();
    [Header("Used only if isEquipeble == true")]
    public List<EquipebleItemStat> itemStats = new List<EquipebleItemStat>();



    public enum ItemsType
    {
        Weapon,
        Potion,
        ArmorAndAccessories,
        Other
    }

    public enum ItemsSubtype
    {
        Charm,
        Chestwear,
        Legwear,
        Helmet,
        Axe,
        Sword,
        Spear,
        Bows,
        HealthPotion,
        ManaPotion,
        OtherPotions
    }

    [System.Serializable]
    public struct EquipebleItemStat
    {
        public string statName;
        public StatType attributeToChange;
        public float value;
        public bool isProcent;
        [Tooltip("If checked, value going to be set as absolute value for a stats when equiped")] //If two or more items have absolute value on stats biggest value going to be used.
        public bool isAbsolute;
    }

    [System.Serializable]
    public class UsableItemEffect
    {
        [Header("If empty, will use effectName or statName:")]
        public string effectName;
        public ItemEffect itemEffect;
        [Header("If itemEffect = StatChange, specify stat change:")]
        public StatType statChange;
        public float value;
        public bool hasTimeOfEffect;
        [ConditionalField("hasTimeOfEffect")]
        public float timeOfEffectInSeconds;
        public string getInfoInString()
        {
            string displayName = effectName;
            string timeLeft = "(" + TimeSpan.FromSeconds(timeOfEffectInSeconds).Minutes + ":" + TimeSpan.FromSeconds(timeOfEffectInSeconds).Seconds + ") ";
            if (timeOfEffectInSeconds <= 0) timeLeft = "";
            string symbolInFrontOfValue = "";
            if (value > 0) symbolInFrontOfValue = "+";
            if(displayName == "")
            {
                if (itemEffect == ItemEffect.StatChange) displayName = statChange.ToString();
                else displayName = effectName.ToString();
            }
            string info = "";
            info = $"{displayName}: {symbolInFrontOfValue + value.ToString()} {timeLeft}<br>";
            return info;
        }
    }

    public enum StatType
    {
        Strength,
        Dexterity,
        Charisma,
        Intelligence,
        Luck,
        Damage,
        Armor,
        AttackSpeed,
        MovementSpeed,
        MaxHP
    }

    public enum ItemEffect
    {
        ChangeHp,
        StatChange
    }
}
