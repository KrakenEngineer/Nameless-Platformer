using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public sealed class EventHandler
{
    [SerializeField] private bool _enabled = false;
    [SerializeField] private List<Event<Object>> _objects;
    [SerializeField] private List<Event<TileBase>> _tiles;

    private Tilemap _tilemap;
    private Dictionary<Vector3Int, Event<TileBase>> _startTiles;
    private List<GameObject> _temporalObjects = new List<GameObject>();

    public void Initialize(Level level)
    {
        _tilemap = level.Tilemap;

        _startTiles = new Dictionary<Vector3Int, Event<TileBase>>();
        foreach (var tile in _tiles)
        {
            Vector3Int position = Utilities.Floor(tile.Position);
            var newTile = new Event<TileBase>(tile.Cancel, tile.Position, _tilemap.GetTile<TileBase>(position));
            _startTiles.Add(position, newTile);
        }
    }

    public bool TryExecuteEvents()
    {
        if (_enabled) return false;

        foreach (var tile in _tiles)
            _tilemap.SetTile(Utilities.Floor(tile.Position), tile.Prefab);

        foreach (var m_object in _objects)
        {
            var newObject = Object.Instantiate(m_object.Prefab, m_object.Position, Quaternion.identity) as GameObject;
            if (m_object.Cancel)
                _temporalObjects.Add(newObject);
        }

        _enabled = true;
        return true;
    }

    public bool TryCancelEvents()
    {
        if (!_enabled) return false;

        foreach (var tile in _startTiles)
            if (tile.Value.Cancel)
                _tilemap.SetTile(tile.Key, tile.Value.Prefab);

        foreach (var obj in _temporalObjects)
            Object.Destroy(obj);
        _temporalObjects.Clear();

        _enabled = false;
        return true;
    }
}