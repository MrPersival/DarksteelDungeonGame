using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonStairsContoller : MonoBehaviour
{
    [SerializeField]
    bool isStairsUp = false;

    DungeonController dungeonController;

    private void Start()
    {
        dungeonController = (DungeonController)GameObject.FindObjectOfType(typeof(DungeonController));
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger entered. Up " + isStairsUp);
        if (!other.CompareTag("Player")) return;
        dungeonController.enteredOnStairs(isStairsUp);
    }

    private void OnTriggerExit(Collider other)
    {
        dungeonController.exitedFromStairs();
    }

}
