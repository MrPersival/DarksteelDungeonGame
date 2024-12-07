using System.Collections;
using System.Collections.Generic;
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
    public enum ItemsType
    {
        Weapon,
        Potion,
        ArmorAndAccessories,
        Other
    }

    public enum ItemsSubtype
    {
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
}
