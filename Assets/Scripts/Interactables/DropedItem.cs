using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropedItem : Interactable
{
    public InventoryItem inventoryItem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // this function is where we will design our interaction using code
    protected override void Interact()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Log(inventoryItem);
        player.GetComponent<PlayerInventory>().addItem(GetComponent<InventoryItem>());
        Debug.Log("Interacted with " + gameObject.name);
        Destroy(gameObject);
    }
}
