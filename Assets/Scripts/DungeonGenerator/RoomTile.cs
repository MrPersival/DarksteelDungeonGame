using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using static UnityEditor.Progress;

public class RoomTile : MonoBehaviour
{
    public GameObject WallPosX;
    public GameObject WallPosZ;
    public GameObject WallNegX;
    public GameObject WallNegZ;
    public bool isLastRoom;
    public SpawnTable generationPresets;
    public float enterPointDistance = 40f;

    List<GameObject> walls = new List<GameObject>();
    List<GameObject> solidWalls = new List<GameObject>();

    public Mesh doorwayWall;
    public bool isHallway = false;
    //TODO: Some interior, loot and so on generation will be here.
    private void Start()
    {
        if (isLastRoom)
        {
            gameObject.GetComponent<NavMeshSurface>().enabled = true;
            gameObject.GetComponent<NavMeshSurface>().BuildNavMesh();
        }
        startGeneration();
    }

    void startGeneration()
    {
        float randomNumber = Random.Range(0, 100);
        if(randomNumber < generationPresets.chanseToSpawn)
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

            GameObject spawnedObject = null;
            SpawnTableItem chosedObjToSpawn = new();
            //TODO: Find a better way to do this.
            List<SpawnTableItem> legitItemsToSpawn = new List<SpawnTableItem>(generationPresets.tableElements);
            if (isHallway) for (int i = legitItemsToSpawn.Count - 1; i >= 0; i--) if (!legitItemsToSpawn[i].isHallwayFrendly) legitItemsToSpawn.RemoveAt(i); 
            if (Vector3.Distance(GameObject.FindGameObjectWithTag("EnterPoint").transform.position, transform.position) <= enterPointDistance)
                for (int i = legitItemsToSpawn.Count - 1; i >= 0; i--) if (!legitItemsToSpawn[i].isEnterRoomFrendly) legitItemsToSpawn.RemoveAt(i);
            for(int i = legitItemsToSpawn.Count - 1; i >= 0; i--)
            {
                Collider[] colidersInSphere = Physics.OverlapSphere(transform.position, legitItemsToSpawn[i].minDistanceToSameObj);
                foreach (Collider col in colidersInSphere)
                {
                    //Debug.Log("Checking" + col.name + " and " + legitItemsToSpawn[i].itemToSpawn.name);
                    if (col.gameObject.name == legitItemsToSpawn[i].itemToSpawn.name + "(Clone)")
                    {
                        legitItemsToSpawn.RemoveAt(i);
                        break;
                    }

                }
            }



            if (solidWalls.Count > 0)
            {
                for (int i = legitItemsToSpawn.Count - 1; i >= 0; i--) if (!legitItemsToSpawn[i].isRequiesWall) legitItemsToSpawn.RemoveAt(i);
                GameObject chosedWall = solidWalls[Random.Range(0, solidWalls.Count - 1)];
                chosedObjToSpawn = generationPresets.getItemToSpawn(legitItemsToSpawn);
                if (chosedObjToSpawn.itemToSpawn == null) return;
                spawnedObject = Instantiate(chosedObjToSpawn.itemToSpawn, chosedWall.transform.position, chosedWall.transform.rotation, chosedWall.transform);
                spawnedObject.transform.Rotate(new Vector3(0, chosedObjToSpawn.rotationOffset, 0));
                spawnedObject.transform.position = spawnedObject.transform.position + spawnedObject.transform.forward * chosedObjToSpawn.wallOfset.x;
                spawnedObject.transform.localPosition = spawnedObject.transform.localPosition + spawnedObject.transform.up * chosedObjToSpawn.wallOfset.y;
                spawnedObject.transform.position = spawnedObject.transform.position + spawnedObject.transform.right * chosedObjToSpawn.wallOfset.z;

            }
            else if (walls.Count <= 0)
            {
                for (int i = legitItemsToSpawn.Count - 1; i >= 0; i--) if (legitItemsToSpawn[i].isRequiesWall) legitItemsToSpawn.RemoveAt(i);
                if (chosedObjToSpawn.itemToSpawn == null) return;
                chosedObjToSpawn = generationPresets.getItemToSpawn(legitItemsToSpawn);
                spawnedObject = Instantiate(chosedObjToSpawn.itemToSpawn, transform.position, transform.rotation, transform);
                spawnedObject.transform.Rotate(new Vector3(0, chosedObjToSpawn.rotationOffset, 0));
            }
        }
    }

}
