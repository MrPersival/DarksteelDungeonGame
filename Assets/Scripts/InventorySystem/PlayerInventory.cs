using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventoryUI inventoryUi;

    public EquippableItem equippedHelmet;
    public EquippableItem equippedCharm;
    public EquippableItem equippedChestwear;
    public EquippableItem equippedLegwear;

    public EquippableItem equippedWeapon;

    public List<InventoryItem> startingEquipment = new List<InventoryItem>();

    List<InventoryItemScriptebleObject> items = new List<InventoryItemScriptebleObject>();


    private void Start()
    {
        foreach (var item in startingEquipment) addItem(item);
        foreach (var item in items)
        {
            switch (item.type)
            {
                case InventoryItem.ItemsType.Weapon:
                    EquippableItem weapon = item as EquippableItem;
                    weapon.isEquiped = true;
                    equippedWeapon = weapon;
                    break;
                case InventoryItem.ItemsType.ArmorAndAccessories:
                    EquippableItem equipment = item as EquippableItem;
                    equipment.isEquiped = true;
                    switch (item.subType)
                    {
                        case InventoryItem.ItemsSubtype.Charm:
                            equippedCharm = item as EquippableItem;
                            break;
                        case InventoryItem.ItemsSubtype.Chestwear:
                            equippedChestwear = item as EquippableItem;
                            break;
                        case InventoryItem.ItemsSubtype.Legwear:
                            equippedLegwear = item as EquippableItem;
                            break;
                        case InventoryItem.ItemsSubtype.Helmet:
                            equippedHelmet = item as EquippableItem;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        updateInventoryUI();
        GetComponent<AttributesSystem>().updateAttributes();
    }

    public void addItem(InventoryItem item) //Not sure that this is the best method do this.

    {
        InventoryItemScriptebleObject itemToAdd = ScriptableObject.CreateInstance<InventoryItemScriptebleObject>();
        if (item.isOneTimeUse) itemToAdd = ScriptableObject.CreateInstance<UsebleItem>();
        if (item.isEquipeble)
        {
            itemToAdd = ScriptableObject.CreateInstance<EquippableItem>();
            EquippableItem equipebleItem = itemToAdd as EquippableItem;
            equipebleItem.itemStats = item.itemStats;
            equipebleItem.mesh = item.dropedObjectPrefab.GetComponent<MeshFilter>().sharedMesh;
            equipebleItem.materials = item.dropedObjectPrefab.GetComponent<MeshRenderer>().sharedMaterials;
        } 


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

    public List<EquippableItem> getEquippedItems()
    {
        List<EquippableItem> items = new List<EquippableItem>();
        items.Add(equippedHelmet);
        items.Add(equippedCharm);
        items.Add(equippedChestwear);
        items.Add(equippedLegwear);
        items.Add(equippedWeapon);
        return items;
    }
}
