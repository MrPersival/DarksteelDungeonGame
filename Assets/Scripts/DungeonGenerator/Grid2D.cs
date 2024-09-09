using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid2D<T> {
    T[] data;

    public Vector2Int Size { get; private set; }
    public Vector2Int Offset { get; set; }

    public Grid2D(Vector2Int size, Vector2Int offset) {
        Size = size;
        Offset = offset;
        data = new T[size.x * size.y];
    }

    public int GetIndex(Vector2Int pos) {
        return pos.x + (Size.x * pos.y);
    }

    public bool InBounds(Vector2Int pos) {
        return new RectInt(Vector2Int.zero, Size).Contains(pos + Offset);
    }

    public T this[int x, int y] {
        get {
            return this[new Vector2Int(x, y)];
        }
        set {
            this[new Vector2Int(x, y)] = value;
        }
    }

    public T this[Vector2Int pos] {
        get {
            pos += Offset;
            return data[GetIndex(pos)];
        }
        set {
            pos += Offset;
            data[GetIndex(pos)] = value;
        }
    }

    public T getObjOnCell(Vector2Int pos)
    {
        if (pos.x >= 0 && pos.y >= 0 && pos.x < Size.x && pos.y < Size.y)
        {
            //Debug.Log(pos + " " + this[pos] + " " + default(T));
            return this[pos];
        }
        else return default(T);
    }

    public Dictionary<Vector2Int, T> getAdjestedTiles(Vector2Int pos, int radiusToCheck = 1)
    {
        Dictionary<Vector2Int, T> cellsWithCoords = new Dictionary<Vector2Int, T>();
        for (int i = 1; i <= radiusToCheck; i++)
        {
            cellsWithCoords.Add(pos - new Vector2Int(-i, 0), getObjOnCell(pos - new Vector2Int(-i, 0)));
            cellsWithCoords.Add(pos - new Vector2Int(0, -i), getObjOnCell(pos - new Vector2Int(0, -i)));
            cellsWithCoords.Add(pos - new Vector2Int(i, 0), getObjOnCell(pos - new Vector2Int(i, 0)));
            cellsWithCoords.Add(pos - new Vector2Int(0, i), getObjOnCell(pos - new Vector2Int(0, i)));
        }
        return cellsWithCoords;
    }

}
