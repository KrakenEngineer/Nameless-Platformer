using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
[RequireComponent(typeof(TilemapRenderer))]
[RequireComponent(typeof(TilemapCollider2D))]
public sealed class Level : MonoBehaviour
{
    [SerializeField] private EventHandler[] _eventHandlers;

    public Tilemap Tilemap { get; private set; }

    private void Awake() => OnLevelWasLoaded();

    private void OnLevelWasLoaded()
    {
        Tilemap = GetComponent<Tilemap>();
        foreach (var handler in _eventHandlers)
            handler.Initialize(this);
        foreach (var sensor in FindObjectsByType<Sensor>(FindObjectsSortMode.None))
            sensor.Initialize(_eventHandlers[sensor.EventHandlerIndex]);
        FindAnyObjectByType<Player>().Initialize();
    }

    public static void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}