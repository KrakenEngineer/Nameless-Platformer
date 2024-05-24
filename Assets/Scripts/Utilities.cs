using UnityEngine;
using UnityEngine.Tilemaps;

public static class Utilities
{
    public static bool IsPositionInCircle(Vector2 center, Vector2 position, float radius)
    {
        Vector2 deltaPos = center - position;
        return deltaPos.sqrMagnitude > radius;
    }

    public static Vector2 Direction4ToVector(Direction4 direction)
    {
        switch (direction)
        {
            case Direction4.Up:
                return new Vector2(0, 1);
            case Direction4.Right:
                return new Vector2(1, 0);
            case Direction4.Down:
                return new Vector2(0, -1);
            case Direction4.Left:
                return new Vector2(-1, 0);
            default:
                throw new System.Exception("Invalid direction");
        }
    }

    public static Vector2Int Floor(Vector2 vector)
    {
        int x = Mathf.FloorToInt(vector.x);
        int y = Mathf.FloorToInt(vector.y);
        return new Vector2Int(x, y);
    }

    public static Vector3Int Floor(Vector3 vector)
    {
        int x = Mathf.FloorToInt(vector.x);
        int y = Mathf.FloorToInt(vector.y);
        int z = Mathf.FloorToInt(vector.z);
        return new Vector3Int(x, y, z);
    }

    public static Vector2Int Ceil(Vector2 vector)
    {
        int x = Mathf.CeilToInt(vector.x);
        int y = Mathf.CeilToInt(vector.y);
        return new Vector2Int(x, y);
    }

    public static int TilesCountInRange(Tilemap tilemap, TileBase tile, Vector2 position, float radius)
    {
        Vector2Int start = Floor(position - Vector2.one * radius);
        Vector2Int end = Ceil(position + Vector2.one * radius);
        var output = 0;

        for (int x = start.x; x < end.x; x++)
        {
            for (int y = start.y; y < end.y; y++)
            {
                if (!IsPositionInCircle(position, new Vector2(x, y), radius))
                    continue;
                TileBase t = tilemap.GetTile(new Vector3Int(x, y, 0));
                if (t == tile)
                    output++;
            }
        }
        
        return output;
    }
}

[System.Serializable]
public sealed class Event<T> where T : Object
{
    [SerializeField] private bool _cancel;
    [SerializeField] private Vector3 _position;
    [SerializeField] private T _prefab;

    public Event(bool cancel, Vector3 position, T prefab)
    {
        _cancel = cancel;
        _position = position;
        _prefab = prefab;
    }

    public bool Cancel => _cancel;
    public Vector3 Position => _position;
    public T Prefab => _prefab;
}

public enum Direction4
{
    Up,
    Right,
    Down,
    Left
}