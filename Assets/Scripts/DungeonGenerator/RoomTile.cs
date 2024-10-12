using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTile : MonoBehaviour
{
    public GameObject WallPosX;
    public GameObject WallPosZ;
    public GameObject WallNegX;
    public GameObject WallNegZ;
    List<GameObject> walls = new List<GameObject>();
    List<GameObject> solidWalls = new List<GameObject>();

    public Mesh doorwayWall;
    public bool isHallway = false;
    //TODO: Some interior, loot and so on generation will be here.
    private void Start()
    {
        startGeneration();
    }

    void startGeneration()
    {
        //if (isHallway) return;
        Collider[] collidersInRange = Physics.OverlapSphere(transform.position, 5);
        foreach (Collider collider in collidersInRange) if (collider.gameObject.tag == "Doorway") return; //Otherways can potentially block doorway

        if (WallPosZ.activeSelf) walls.Add(WallPosZ);
        if (WallNegZ.activeSelf) walls.Add(WallNegZ);
        if (WallPosX.activeSelf) walls.Add(WallPosX);
        if (WallNegX.activeSelf) walls.Add(WallNegX);
        foreach (GameObject wall in walls)
        {
            if (wall.GetComponent<MeshFilter>().mesh != doorwayWall) solidWalls.Add(wall);
        }

        if (solidWalls.Count > 0)
        {
            solidWalls[Random.Range(0, solidWalls.Count - 1)].GetComponent<DecorationPresetGenerator>().Generate(isHallway);
        }
        else if(walls.Count <= 0) gameObject.GetComponent<DecorationPresetGenerator>().Generate(isHallway);
    }
}
