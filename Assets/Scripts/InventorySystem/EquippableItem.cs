using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InventoryItem;

public class EquippableItem : InventoryItemScriptebleObject
{
    public bool isEquiped;
    public Mesh mesh;
    public Material[] materials;
    public List<EquipebleItemStat> itemStats = new List<EquipebleItemStat>();
    public void equipItem() //Player going to always have some kind of equipment on him, and start it is going to be "started equipment". 
    {

    }
}
