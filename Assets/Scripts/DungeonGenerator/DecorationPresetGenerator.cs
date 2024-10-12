using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DecorationPresetGenerator : MonoBehaviour
{
    [SerializeField]
    List<GenerationPreset> presets = new List<GenerationPreset>();
    [SerializeField]
    float chanceToGenerateSomething;

    public void Generate(bool isHallway = false)
    {
        float randomResult = Random.Range(0, 1000);
        //Debug.Log(randomResult);
        if (randomResult <= chanceToGenerateSomething * 10)
        {
            //Debug.Log("Generating on: " + gameObject);
            for (; ; ) //TODO: Find better way
            {
                if(presets.Count <= 0) break;
                GenerationPreset chosedPreset = presets[Random.Range(0, presets.Count - 1)];
                if (isHallway && !chosedPreset.isHallwayFrendly) break;
                Collider[] collidersInRange = Physics.OverlapSphere(transform.position, chosedPreset.minDistanceToSameObj);
                bool isPresetFindedInRadius = false;
                foreach(Collider collider in collidersInRange)
                {
                    if(collider.gameObject.name == chosedPreset.presetObj.name)
                    {
                        isPresetFindedInRadius = true;
                        break;
                    }
                }
                if (!isPresetFindedInRadius)
                {
                    //Debug.Log("Spawned: " + chosedPreset.presetObj);
                    chosedPreset.presetObj.SetActive(true);
                    break;
                }
                else presets.Remove(chosedPreset);
            }
        }
    }

    [System.Serializable]
    public struct GenerationPreset
    {
        public GameObject presetObj;
        public float minDistanceToSameObj;
        public bool isHallwayFrendly;
    }
}
