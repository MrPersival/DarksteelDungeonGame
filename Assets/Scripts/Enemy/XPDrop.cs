using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPDrop : MonoBehaviour
{
    [SerializeField]
    float maxXPToGive = 25f;
    [SerializeField]
    float minXPToGive = 15f;

    public void giveXP()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<XPSystem>().addXP(Random.Range(minXPToGive, maxXPToGive));
    }
}
