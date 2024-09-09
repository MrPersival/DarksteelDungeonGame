using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTile : MonoBehaviour
{
    public GameObject WallPosX;
    public GameObject WallPosZ;
    public GameObject WallNegX;
    public GameObject WallNegZ;

    public Mesh doorwayWall;
    public bool isHallway = false;
    //TODO: Some interior, loot and so on generation will be here.
}
