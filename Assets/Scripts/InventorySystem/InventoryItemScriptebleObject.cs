using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InventoryItem;

public class InventoryItemScriptebleObject : ScriptableObject
{
    public string itemName;
    public float price;
    public int amount;
    public ItemsType type;
    public ItemsSubtype subType;
    public GameObject dropedObjectPrefab;
    public float additionalValue;
}
