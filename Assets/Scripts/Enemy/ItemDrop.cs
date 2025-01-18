using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemDrop : MonoBehaviour
{
    public SpawnTable dropSpawnTable;
    public void dropItem()
    {
        if (Random.Range(0, 100) * GameObject.FindGameObjectWithTag("Player").GetComponent<AttributesSystem>().playerLuckCoef < dropSpawnTable.chanseToSpawn) Instantiate(dropSpawnTable.getItemToSpawn().itemToSpawn,
            transform.position + new Vector3(0, 1, 0), Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
    }

   
}




