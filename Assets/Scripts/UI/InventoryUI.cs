using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static InventoryItem;

public class InventoryUI : MonoBehaviour
{
    public GameObject itemUiPrefab;

    public TMP_Dropdown selectionDropdown;
    public TMP_Dropdown armorAndAccSubselection;
    public TMP_Dropdown weaponsSubselection;
    public TMP_Dropdown potionsSubselections;

    public Transform itemsContainer;

    List<InventoryItemScriptebleObject> items = new List<InventoryItemScriptebleObject>();
    List<TMP_Dropdown> subSelectionDropdowns = new List<TMP_Dropdown>();

    private void Start()
    {
        subSelectionDropdowns.Add(armorAndAccSubselection);
        subSelectionDropdowns.Add(weaponsSubselection);
        subSelectionDropdowns.Add(potionsSubselections);
    }
    public void upadeteInvenoty(List<InventoryItemScriptebleObject> items)
    {
        this.items = items;
        selectionChanged();
    }

    public void selectionChanged()
    {
        foreach (Transform child in itemsContainer)
        {
            Destroy(child.gameObject);
        }
        foreach(TMP_Dropdown dropdown in subSelectionDropdowns)
        {
            dropdown.gameObject.SetActive(false);
        }

        switch (selectionDropdown.value)
        {
            case 0:
                foreach (InventoryItemScriptebleObject item in items)
                {
                    displayItemInContainer(item);
                }
                return;
            case 1:
                weaponsSubselection.gameObject.SetActive(true);
                break;
            case 2:
                armorAndAccSubselection.gameObject.SetActive(true);
                break;
            case 3:
                potionsSubselections.gameObject.SetActive(true);
                break;
            default:
                Debug.LogError("Unnkown value in selection dropdown: " + Convert.ToString(selectionDropdown.value));
                break;
        }

        subSelectionChanged();
    }

    public void subSelectionChanged()
    {
        foreach (Transform child in itemsContainer)
        {
            Destroy(child.gameObject);
        }

        if (armorAndAccSubselection.gameObject.activeSelf)
        {
            switch (armorAndAccSubselection.value)
            {
                case 0:
                    foreach(InventoryItemScriptebleObject item in items)
                    {
                        if (item.type == InventoryItem.ItemsType.ArmorAndAccessories) displayItemInContainer(item);
                    }
                    break;
                case 1:
                    foreach(InventoryItemScriptebleObject item in items)
                    {
                        if (item.subType == InventoryItem.ItemsSubtype.Chestwear) displayItemInContainer(item);
                    }
                    break;
                case 2:
                    foreach (InventoryItemScriptebleObject item in items)
                    {
                        if (item.subType == InventoryItem.ItemsSubtype.Legwear) displayItemInContainer(item);
                    }
                    break;
                case 3:
                    foreach (InventoryItemScriptebleObject item in items)
                    {
                        if (item.subType == InventoryItem.ItemsSubtype.Helmet) displayItemInContainer(item);
                    }
                    break;
                default:
                    Debug.LogError("Unnkown value in selection dropdown: " + Convert.ToString(selectionDropdown.value));
                    break;
            }
        }

        if (weaponsSubselection.gameObject.activeSelf)
        {
            switch (weaponsSubselection.value)
            {
                case 0:
                    foreach (InventoryItemScriptebleObject item in items)
                    {
                        if (item.type == InventoryItem.ItemsType.Weapon) displayItemInContainer(item);
                    }
                    break;
                case 1:
                    foreach (InventoryItemScriptebleObject item in items)
                    {
                        if (item.subType == InventoryItem.ItemsSubtype.Axe) displayItemInContainer(item);
                    }
                    break;
                case 2:
                    foreach (InventoryItemScriptebleObject item in items)
                    {
                        if (item.subType == InventoryItem.ItemsSubtype.Sword) displayItemInContainer(item);
                    }
                    break;
                case 3:
                    foreach (InventoryItemScriptebleObject item in items)
                    {
                        if (item.subType == InventoryItem.ItemsSubtype.Spear) displayItemInContainer(item);
                    }
                    break;
                default:
                    Debug.LogError("Unnkown value in selection dropdown: " + Convert.ToString(selectionDropdown.value));
                    break;
            }
        }

        if (potionsSubselections.gameObject.activeSelf)
        {
            switch (potionsSubselections.value)
            {
                case 0:
                    foreach (InventoryItemScriptebleObject item in items)
                    {
                        if (item.type == InventoryItem.ItemsType.Potion) displayItemInContainer(item);
                    }
                    break;
                case 1:
                    foreach (InventoryItemScriptebleObject item in items)
                    {
                        if (item.subType == InventoryItem.ItemsSubtype.HealthPotion) displayItemInContainer(item);
                    }
                    break;
                case 2:
                    foreach (InventoryItemScriptebleObject item in items)
                    {
                        if (item.subType == InventoryItem.ItemsSubtype.ManaPotion) displayItemInContainer(item);
                    }
                    break;
                case 3:
                    foreach (InventoryItemScriptebleObject item in items)
                    {
                        if (item.subType == InventoryItem.ItemsSubtype.OtherPotions) displayItemInContainer(item);
                    }
                    break;
                default:
                    Debug.LogError("Unnkown value in selection dropdown: " + Convert.ToString(selectionDropdown.value));
                    break;
            }
        }
    }

    private void displayItemInContainer(InventoryItemScriptebleObject item)
    {
        GameObject itemPrefab = Instantiate(itemUiPrefab, itemsContainer);
        itemPrefab.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = item.itemName;
        itemPrefab.transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = Convert.ToString(item.amount);
        if(item is EquippableItem)
        {
            EquippableItem equippableItem = item as EquippableItem;
            if (equippableItem.isEquiped) itemPrefab.transform.Find("IsEquiped").gameObject.SetActive(true);
            itemPrefab.GetComponent<Button>().onClick.AddListener(() => {
                equipebleItemClicked(equippableItem);
                upadeteInvenoty(items);
            });
            itemPrefab.transform.Find("MainStat").GetComponent<TextMeshProUGUI>().text = equippableItem.itemStats[0].attributeToChange.ToString() + ": " + equippableItem.itemStats[0].value.ToString();
            string toolTipText = "";
            foreach (EquipebleItemStat stat in equippableItem.itemStats)
            {
                if(stat.isProcent) toolTipText += $"{stat.attributeToChange.ToString()}: {stat.value}%";
                else if(stat.isAbsolute) toolTipText += $"{stat.attributeToChange.ToString()}: {stat.value} absolute change";
                else toolTipText += $"{stat.attributeToChange.ToString()}: {stat.value}";
                toolTipText += "\n";
            }
            itemPrefab.GetComponent<ItemTooltipOnHoover>().textToDisplay = toolTipText;
        }
        else if(item is UsebleItem)
        {
            UsebleItem usebleItem = item as UsebleItem;
            itemPrefab.GetComponent<Button>().onClick.AddListener(() => { usebleItem.useItem(); });
            itemPrefab.transform.Find("MainStat").GetComponent<TextMeshProUGUI>().text = "HP: " + usebleItem.additionalValue.ToString(); //Make it support all potions
        }
    }

    private void equipebleItemClicked(EquippableItem equippableItem)
    {
        if (equippableItem.isEquiped) return;
        //TODO: Potentially change to slot system with list of structs for each slot
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerInventory inv = player.GetComponent<PlayerInventory>();
        equippableItem.isEquiped = true;
        if (equippableItem.type == InventoryItem.ItemsType.Weapon)
        {
            if(inv.equippedWeapon != null) inv.equippedWeapon.isEquiped = false;
            inv.equippedWeapon = equippableItem;
            //controller.attackDamage = equippableItem.additionalValue;
        }
        else
        {
            switch (equippableItem.subType)
            { //Actual changes to armor will be implimentet later, when working stats and armor system will be added.
                case InventoryItem.ItemsSubtype.Chestwear:
                    if (inv.equippedChestwear != null) inv.equippedChestwear.isEquiped = false;
                    inv.equippedChestwear = equippableItem;
                    break;
                case InventoryItem.ItemsSubtype.Legwear:
                    if (inv.equippedLegwear != null) inv.equippedLegwear.isEquiped = false;
                    inv.equippedLegwear = equippableItem;
                    break;
                case InventoryItem.ItemsSubtype.Helmet:
                    if (inv.equippedHelmet != null) inv.equippedHelmet.isEquiped = false;
                    inv.equippedHelmet = equippableItem;
                    break;
                case InventoryItem.ItemsSubtype.Charm:
                    if(inv.equippedCharm != null) inv.equippedCharm.isEquiped = false;
                    inv.equippedCharm = equippableItem;
                    break;
                default:
                    Debug.LogWarning($"Something gone wrong, item {equippableItem.name} has unknown subtype {equippableItem.subType}");
                    break;
            }
        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<AttributesSystem>().updateAttributes();
    }
}
