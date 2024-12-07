using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventoryUI inventoryUi;

    public EquippableItem equippedHelmet;
    public EquippableItem equippedChestwear;
    public EquippableItem equippedLegwear;

    public EquippableItem equippedWeapon;


    List<InventoryItemScriptebleObject> items = new List<InventoryItemScriptebleObject>();

    public void addItem(InventoryItem item) //Not sure that this is the best method do this.

    {
        InventoryItemScriptebleObject itemToAdd = ScriptableObject.CreateInstance<InventoryItemScriptebleObject>();
        if (item.isOneTimeUse) itemToAdd = ScriptableObject.CreateInstance<UsebleItem>();
        if (item.isEquipeble) itemToAdd = ScriptableObject.CreateInstance<EquippableItem>();


        itemToAdd.itemName = item.itemName;
        itemToAdd.price= item.price;
        itemToAdd.amount = item.amount;
        itemToAdd.type = item.type;
        itemToAdd.subType = item.subType;
        itemToAdd.dropedObjectPrefab = item.dropedObjectPrefab;
        itemToAdd.name = item.name;
        itemToAdd.additionalValue = item.additionalValue;

        if (items.Find(findedItem => findedItem.itemName == itemToAdd.itemName)) items.Find(findedItem => findedItem.itemName == itemToAdd.itemName).amount += 1;
        else items.Add(itemToAdd);

        updateInventoryUI();
    }
    public void removeItem(InventoryItemScriptebleObject item)
    {
        if (items.Contains(item))
        {
            if (item.amount == 1) items.Remove(item);
            else item.amount--;
        }

        updateInventoryUI();
    }

    void updateInventoryUI()
    {
        inventoryUi.upadeteInvenoty(items);
    }
}
