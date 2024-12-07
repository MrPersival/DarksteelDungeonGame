using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemDrop : MonoBehaviour
{
    public List<ItemsDrop> items = new List<ItemsDrop>();
    public float chanseToDrop = 10f;

    public void dropItem()
    {
        List<float> weights = new List<float>();
        foreach (ItemsDrop item in items) weights.Add(item.chanseToDrop);
        int chosedItem = GetRandomWeightedIndex(weights.ToArray());

        if (Random.Range(0, 100) < chanseToDrop) Instantiate(items[chosedItem].itemToDrop,
            transform.position + new Vector3(0, 1, 0), Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
    }

    public int GetRandomWeightedIndex(float[] weights)
    {
        if (weights == null || weights.Length == 0) return -1;

        float w;
        float t = 0;
        int i;
        for (i = 0; i < weights.Length; i++)
        {
            w = weights[i];

            if (float.IsPositiveInfinity(w))
            {
                return i;
            }
            else if (w >= 0f && !float.IsNaN(w))
            {
                t += weights[i];
            }
        }

        float r = Random.value;
        float s = 0f;

        for (i = 0; i < weights.Length; i++)
        {
            w = weights[i];
            if (float.IsNaN(w) || w <= 0f) continue;

            s += w / t;
            if (s >= r) return i;
        }

        return -1;
    }
}


[System.Serializable]
public struct ItemsDrop
{
    public float chanseToDrop;
    public GameObject itemToDrop;
}


