using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spawn Table", menuName = "Spawn Table")]
public class SpawnTable : ScriptableObject
{
    public List<SpawnTableItem> tableElements;
    public float chanseToSpawn = 10f;
    public SpawnTableItem getItemToSpawn(List<SpawnTableItem> items = null)  //Sometimes custom list of items will be given, like, for example, in dungeon generation
    {
        if (items == null || items == new List<SpawnTableItem>()) items = tableElements;
        List<float> weights = new List<float>();
        foreach (SpawnTableItem item in items) weights.Add(item.chanseToSpawn);
        int chosedItem = GetRandomWeightedIndex(weights.ToArray());
        if (chosedItem == -1) return new SpawnTableItem();
        return items[chosedItem];

    }

    int GetRandomWeightedIndex(float[] weights) //Code was taken from Stack Overwflow, black box for me for now
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
public struct SpawnTableItem
{
    public float chanseToSpawn;
    [SerializeField]
    public GameObject itemToSpawn;
    public bool isDungeonGenerationPreset;
    [ConditionalField("isDungeonGenerationPreset")]
    public float minDistanceToSameObj;
    [ConditionalField("isDungeonGenerationPreset")]
    public bool isHallwayFrendly;
    [ConditionalField("isDungeonGenerationPreset")]
    public bool isEnterRoomFrendly;
    [ConditionalField("isDungeonGenerationPreset")]
    public float rotationOffset;
    [ConditionalField("isDungeonGenerationPreset")]
    public bool isRequiesWall;
    [ConditionalField("isDungeonGenerationPreset")]
    public Vector3 offset;

}