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
    [Header("Fill only if is equipeble, otherwise does nothing")]
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
        public StatType attributeToChange;
        public float value;
        public bool isProcent;
        [Tooltip("If checked, value going to be set as absolute value for a stats when equiped")] //If two or more items have absolute value on stats biggest value going to be used.
        public bool isAbsolute;
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

}
