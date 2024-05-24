using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Magnet : MonoBehaviour
{
    [SerializeField, Range(1, float.MaxValue)] protected float _interactionRadius;

    public void Push(Direction4 direction, float magnitude) =>
        Push(Utilities.Direction4ToVector(direction) * magnitude);

    public abstract void Push(Vector2 force);

    public abstract bool IsActive();
}

[RequireComponent(typeof(SpriteRenderer))]
public abstract class FourDirectional : MonoBehaviour
{
    private SpriteRenderer _renderer;

    [SerializeField] private TileBase _surface;
    [SerializeField] private Sprite _floating;
    [SerializeField] private Sprite _onSurface;

    [SerializeField] private Direction4 _direction;

    protected virtual void Start()
    {
        if (_surface == null || _onSurface == null || _floating == null)
            throw new System.Exception("Set sprites and surface tile");
        _renderer = GetComponent<SpriteRenderer>();
        InitializeTransform();
        Vector3Int surfacePosition = Utilities.Floor(transform.position - Vector3.one / 2 - transform.up);
        InitializeRenderer(surfacePosition);
    }

    private void InitializeTransform()
    {
        transform.localScale = Vector3.one;
        transform.position = Utilities.Floor(transform.position) + Vector3.one * 0.5f;
        transform.eulerAngles = new Vector3(0, 0, -(int)_direction * 90);
    }

    private void InitializeRenderer(Vector3Int surfacePosition)
    {
        var terrain = FindAnyObjectByType<Tilemap>();
        TileBase surface = terrain.GetTile(surfacePosition);
        _renderer.sprite = _surface == surface ? _onSurface : _floating;
        _renderer.color = Color.white;
    }

    public Direction4 Direction => _direction;
}

public abstract class Sensor : FourDirectional
{
    [SerializeField] private float _signalStrength = 0;
    [SerializeField] private int _eventHandlerIndex;

    protected float _eventSignal;
    protected float _cancelSignal;
    protected float _maxSignal;
    protected float _minSignal;

    private EventHandler _eventHandler;

    private void OnValidate()
    {
        if (_minSignal >= _maxSignal)
            _maxSignal = _minSignal + float.Epsilon;
    }

    public void Initialize(EventHandler eventHandler)
    {
        _eventHandler = eventHandler;
    }

    protected void IncreaseSignal(float value)
    {
        if (value <= 0)
            throw new System.Exception("Invalid value for increasing signal");

        float newSignal = Mathf.Clamp(_signalStrength + value, _minSignal, _maxSignal);
        if (_signalStrength < _eventSignal && newSignal >= _eventSignal)
            _eventHandler.TryExecuteEvents();
        _signalStrength = newSignal;
    }

    protected void DecreaseSignal(float value)
    {
        if (value <= 0)
            throw new System.Exception("Invalid value for decreasing signal");

        float newSignal = Mathf.Clamp(_signalStrength - value, _minSignal, _maxSignal);
        if (_signalStrength > _cancelSignal && newSignal <= _cancelSignal)
            _eventHandler.TryCancelEvents();
        _signalStrength = newSignal;
    }

    public int EventHandlerIndex => _eventHandlerIndex;
}