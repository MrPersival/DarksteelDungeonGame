using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using Graphs;
using static UnityEditor.FilePathAttribute;
using System.Linq;
using Newtonsoft.Json.Linq;
using Unity.AI.Navigation;
using System;

public class Generator2D : MonoBehaviour {
    enum CellType {
        None,
        Room,
        Hallway,
        Doorway
    }

    class Room {
        public RectInt bounds;

        public Room(Vector2Int location, Vector2Int size) {
            bounds = new RectInt(location, size);
        }

        public static bool Intersect(Room a, Room b) {
            return !((a.bounds.position.x >= (b.bounds.position.x + b.bounds.size.x)) || ((a.bounds.position.x + a.bounds.size.x) <= b.bounds.position.x)
                || (a.bounds.position.y >= (b.bounds.position.y + b.bounds.size.y)) || ((a.bounds.position.y + a.bounds.size.y) <= b.bounds.position.y));
        }
    }

    public int bossLevelNumber = 3;

    [SerializeField]
    Vector2Int size;
    [SerializeField]
    int roomCount;
    [SerializeField]
    Vector2Int roomMaxSize;
    [SerializeField]
    GameObject roomTilePrefab;
    [SerializeField]
    GameObject enterPointTilePrefab;
    [SerializeField]
    GameObject exitPointPrefab;
    [SerializeField]
    float tileSizeCoef;
    [SerializeField]
    Material redMaterial;
    [SerializeField]
    Material blueMaterial;
    [SerializeField]
    Material greenMaterial;
    [SerializeField]
    GameObject stairsDownTile;
    [SerializeField]
    float levelHeightOffsets;
    [SerializeField]
    GameObject stairsUpTile;
    [SerializeField]
    DungeonController dungeonController;
    [SerializeField]
    GameObject bossLevel;

    Random random;
    Grid2D<CellType> grid;
    List<Room> rooms;
    Delaunay2D delaunay;
    HashSet<Prim.Edge> selectedEdges;
    int currentLevel = 1;

    void Start() {

        for (int i = 1; i < bossLevelNumber; i++)
        {
            Debug.Log("Generated dungeon, level: " + Convert.ToString(currentLevel));
            Generate();
            GenerateDungeonFromGrid();
            currentLevel++;
        }
        //TODO: Generate boss level
        GameObject spawnedBossLevel = Instantiate(bossLevel, new Vector3(0, 0, currentLevel * levelHeightOffsets), Quaternion.identity);
        dungeonController.levels.Add(spawnedBossLevel);
        spawnedBossLevel.SetActive(false);
        dungeonController.generationDone();
    }

    void Generate() {
        random = new Random();
        grid = new Grid2D<CellType>(size, Vector2Int.zero);
        rooms = new List<Room>();

        PlaceRooms();
        Triangulate();
        CreateHallways();
        PathfindHallways();
    }

    void PlaceRooms() {
        for (int i = 0; i < roomCount; i++) {
            Vector2Int location = new Vector2Int(
                random.Next(0, size.x),
                random.Next(0, size.y)
            );

            Vector2Int roomSize = new Vector2Int(
                random.Next(1, roomMaxSize.x + 1),
                random.Next(1, roomMaxSize.y + 1)
            );

            bool add = true;
            Room newRoom = new Room(location, roomSize);
            Room buffer = new Room(location + new Vector2Int(-1, -1), roomSize + new Vector2Int(2, 2));

            foreach (var room in rooms) {
                if (Room.Intersect(room, buffer)) {
                    add = false;
                    break;
                }
            }

            if (newRoom.bounds.xMin < 0 || newRoom.bounds.xMax >= size.x
                || newRoom.bounds.yMin < 0 || newRoom.bounds.yMax >= size.y) {
                add = false;
            }

            if (add) {
                rooms.Add(newRoom);
                //PlaceRoom(newRoom.bounds.position, newRoom.bounds.size);
                foreach (var pos in newRoom.bounds.allPositionsWithin) {
                    grid[pos] = CellType.Room;
                }
            }
        }
    }

    void Triangulate() {
        List<Vertex> vertices = new List<Vertex>();

        foreach (var room in rooms) {
            vertices.Add(new Vertex<Room>((Vector2)room.bounds.position + ((Vector2)room.bounds.size) / 2, room));
        }

        delaunay = Delaunay2D.Triangulate(vertices);
    }

    void CreateHallways() {
        List<Prim.Edge> edges = new List<Prim.Edge>();

        foreach (var edge in delaunay.Edges) {
            edges.Add(new Prim.Edge(edge.U, edge.V));
        }

        List<Prim.Edge> mst = Prim.MinimumSpanningTree(edges, edges[0].U);

        selectedEdges = new HashSet<Prim.Edge>(mst);
        var remainingEdges = new HashSet<Prim.Edge>(edges);
        remainingEdges.ExceptWith(selectedEdges);

        foreach (var edge in remainingEdges) {
            if (random.NextDouble() < 0.125) {
                selectedEdges.Add(edge);
            }
        }
    }

    void PathfindHallways() {
        DungeonPathfinder2D aStar = new DungeonPathfinder2D(size);

        foreach (var edge in selectedEdges) {
            var startRoom = (edge.U as Vertex<Room>).Item;
            var endRoom = (edge.V as Vertex<Room>).Item;

            var startPosf = startRoom.bounds.center;
            var endPosf = endRoom.bounds.center;
            var startPos = new Vector2Int((int)startPosf.x, (int)startPosf.y);
            var endPos = new Vector2Int((int)endPosf.x, (int)endPosf.y);

            var path = aStar.FindPath(startPos, endPos, (DungeonPathfinder2D.Node a, DungeonPathfinder2D.Node b) => {
                var pathCost = new DungeonPathfinder2D.PathCost();
                
                pathCost.cost = Vector2Int.Distance(b.Position, endPos);    //heuristic

                if (grid[b.Position] == CellType.Room) {
                    pathCost.cost += 10;
                } else if (grid[b.Position] == CellType.None) {
                    pathCost.cost += 5;
                } else if (grid[b.Position] == CellType.Hallway) {
                    pathCost.cost += 1;
                }

                pathCost.traversable = true;

                return pathCost;
            });

            if (path != null) {
                for (int i = 0; i < path.Count; i++) {
                    var current = path[i];

                    if (grid[current] == CellType.None) {
                        grid[current] = CellType.Hallway;
                    }

                    if (i > 0) {
                        var prev = path[i - 1];

                        var delta = current - prev;
                    }
                }

                foreach (var pos in path) {
                    if (grid[pos] == CellType.Hallway) {
                        //PlaceHallway(pos);
                    }
                }
            }
        }
    }

    private void GenerateDungeonFromGrid()
    {
        bool isEnterPointGenerated = false;
        Transform lastSpawnedRoom = null;
        //Debug.Log("Entered method");
        Transform parentObject = new GameObject("Level " + Convert.ToString(currentLevel)).transform;
        for (int x = 0; x < grid.Size.x; x++)
        {
            for (int y = 0; y < grid.Size.y; y++)
            {
                Vector2Int gridCoords  = new Vector2Int(x, y);
                Vector2Int spawnCoord = new Vector2Int(x * 5, y * 5);
                if (grid[gridCoords] != CellType.None)
                {
                    RoomTile spawnedRoom = Instantiate(roomTilePrefab, new Vector3(spawnCoord.x, (currentLevel - 1) * levelHeightOffsets, spawnCoord.y), Quaternion.identity).GetComponent<RoomTile>();
                    spawnedRoom.transform.SetParent(parentObject);
                    Dictionary<Vector2Int, CellType> adjestedTiles = grid.getAdjestedTiles(gridCoords);
                    if (grid.getObjOnCell(gridCoords + Vector2Int.left) == CellType.None) spawnedRoom.WallNegX.SetActive(true);
                    if (grid.getObjOnCell(gridCoords + Vector2Int.up) == CellType.None) spawnedRoom.WallPosZ.SetActive(true);
                    if (grid.getObjOnCell(gridCoords + Vector2Int.right) == CellType.None) spawnedRoom.WallPosX.SetActive(true);
                    if (grid.getObjOnCell(gridCoords + Vector2Int.down) == CellType.None) spawnedRoom.WallNegZ.SetActive(true);
                    if (grid[gridCoords] == CellType.Hallway)
                    {
                        spawnedRoom.isHallway = true;
                        spawnedRoom.transform.Find("Floor").GetComponent<MeshRenderer>().material = blueMaterial; //Only for debbuging purposes

                        //This part of code requied to build "beliveble" transitions between hallways and rooms. Sometimes will create double doors.
                        int adjestedRooms = 0;
                        int adjestedHallways = 0;
                        foreach (CellType cell in adjestedTiles.Values)
                        {
                            if (cell == CellType.Room) adjestedRooms++;
                            if (cell == CellType.Hallway || cell == CellType.Doorway) adjestedHallways++; //In our case doorway is a hallway
                        }

                        int adjestedDoorwaysIn3Tiles = 0;
                        foreach (CellType cell in grid.getAdjestedTiles(gridCoords, 3).Values) if (cell == CellType.Doorway) adjestedDoorwaysIn3Tiles++;

                        if (adjestedRooms < 3 && adjestedRooms > 0)
                        {
                            /*Vector2Int roomCoords = adjestedTiles.FirstOrDefault(x => x.Value == CellType.Room).Key;
                            List<Vector2Int> roomsCoords = adjestedTiles
                                .Where(pair => pair.Value == CellType.Room)
                                .Select(pair => pair.Key)
                                .ToList();*/

                            foreach (Vector2Int coords in adjestedTiles.Keys)
                            {
                                if (adjestedTiles[coords] == CellType.Room)
                                {
                                    if (coords == gridCoords + Vector2Int.left) spawnedRoom.WallNegX.SetActive(true);
                                    if (coords == gridCoords + Vector2Int.up) spawnedRoom.WallPosZ.SetActive(true);
                                    if (coords == gridCoords + Vector2Int.right) spawnedRoom.WallPosX.SetActive(true);
                                    if (coords == gridCoords + Vector2Int.down) spawnedRoom.WallNegZ.SetActive(true);
                                }
                            }

                            if (adjestedDoorwaysIn3Tiles == 0 || adjestedHallways == 1)
                            {
                                foreach (Vector2Int coords in adjestedTiles.Keys)
                                {
                                    if (adjestedTiles[coords] == CellType.Room)
                                    {
                                        if (coords == gridCoords + Vector2Int.left)
                                        {
                                            spawnedRoom.WallNegX.GetComponent<MeshFilter>().mesh = spawnedRoom.doorwayWall;
                                            spawnedRoom.WallNegX.GetComponent<MeshCollider>().sharedMesh = spawnedRoom.doorwayWall;
                                        }
                                        if (coords == gridCoords + Vector2Int.up)
                                        {
                                            spawnedRoom.WallPosZ.GetComponent<MeshFilter>().mesh = spawnedRoom.doorwayWall;
                                            spawnedRoom.WallPosZ.GetComponent<MeshCollider>().sharedMesh = spawnedRoom.doorwayWall;
                                        }
                                        if (coords == gridCoords + Vector2Int.right)
                                        {
                                            spawnedRoom.WallPosX.GetComponent<MeshFilter>().mesh = spawnedRoom.doorwayWall;
                                            spawnedRoom.WallPosX.GetComponent<MeshCollider>().sharedMesh = spawnedRoom.doorwayWall;
                                        }
                                        if (coords == gridCoords + Vector2Int.down)
                                        {
                                            spawnedRoom.WallNegZ.GetComponent<MeshFilter>().mesh = spawnedRoom.doorwayWall;
                                            spawnedRoom.WallNegZ.GetComponent<MeshCollider>().sharedMesh = spawnedRoom.doorwayWall;
                                        }
                                    }
                                }
                                grid[gridCoords] = CellType.Doorway;
                                spawnedRoom.tag = "Doorway";
                                spawnedRoom.transform.Find("Floor").GetComponent<MeshRenderer>().material = greenMaterial;
                            }
                        }
                    }
                    if (grid[gridCoords] == CellType.Room)
                    {
                        spawnedRoom.tag = "Room";
                        lastSpawnedRoom = spawnedRoom.transform;
                        if (!isEnterPointGenerated)
                        {
                            Instantiate(enterPointTilePrefab, new Vector3(spawnCoord.x, lastSpawnedRoom.transform.position.y + 2, spawnCoord.y), Quaternion.identity).
                                transform.SetParent(parentObject);
                            lastSpawnedRoom.GetComponent<RoomTile>().WallNegX.SetActive(false);
                            lastSpawnedRoom.GetComponent<RoomTile>().isHallway = true;
                            Instantiate(stairsUpTile, lastSpawnedRoom.position + new Vector3(-tileSizeCoef - 3.25f, 2, -1), Quaternion.Euler(0, -270, 0)).
                                transform.SetParent(parentObject);
                            isEnterPointGenerated = true;
                        }
                    }
                }
            }

        }

        if(lastSpawnedRoom != null)
        {
            lastSpawnedRoom.GetComponent<RoomTile>().WallPosX.SetActive(false);
            lastSpawnedRoom.GetComponent<RoomTile>().isHallway = true;
            Instantiate(stairsDownTile, lastSpawnedRoom.position + new Vector3(1, 0, -1.5f), Quaternion.Euler(0, 90, 0)).
                transform.SetParent(parentObject);
            Instantiate(exitPointPrefab, lastSpawnedRoom.position + new Vector3(0, 2, 0), Quaternion.identity).
                transform.SetParent(parentObject);
            lastSpawnedRoom.gameObject.GetComponent<NavMeshSurface>().enabled = true;
            lastSpawnedRoom.gameObject.GetComponent<NavMeshSurface>().BuildNavMesh();
        }

        dungeonController.levels.Add(parentObject.gameObject);
        parentObject.gameObject.SetActive(false);
    }
    /*
     Requied for testing puproses

    void PlaceCube(Vector2Int location, Vector2Int size, Material material) {
        GameObject go = Instantiate(cubePrefab, new Vector3(location.x, 0, location.y), Quaternion.identity);
        go.GetComponent<Transform>().localScale = new Vector3(size.x, 1, size.y);
        go.GetComponent<MeshRenderer>().material = material;
    }

    void PlaceRoom(Vector2Int location, Vector2Int size) {
        Vector2Int startLocation = location - size / 2;
        for (int x = 0; x <= size.x; x++)
        {
            for (int y = 0; y <= 0; y++)
            {

            }
        }
    }

    void PlaceHallway(Vector2Int location) {
        RoomTile spawnedRoomTile = Instantiate(roomTilePrefab, new Vector3(location.x, 0, location.y), Quaternion.identity).GetComponent<RoomTile>();
    } */
}
