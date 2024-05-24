using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class Player : Magnet
{
    private Tilemap _tilemap;
    private Rigidbody2D _rigidbody;

    [SerializeField, Range(0, float.MaxValue)] private float _horizontalForce;
    [SerializeField, Range(0, float.MaxValue)] private float _verticalForce;
    [SerializeField] private TileBase _magnetTile;

    public void Initialize()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _tilemap = FindAnyObjectByType<Level>().Tilemap;
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyUp(KeyCode.R))
            Level.Restart();

        if (!IsActive(out IEnumerable<Object> objectsInField))
            return;

        Vector2 force = GetForce();
        Push(force);
        if (objectsInField == null)
            return;

        if (objectsInField is IEnumerable<Magnet> magnets)
        {
            foreach (var magnet in magnets)
            {
                if (magnet == this) continue;
                Vector3 relativePosition = magnet.transform.position - transform.position;
                magnet.Push(relativePosition.normalized / Mathf.Pow(relativePosition.magnitude, 2));
            }
        }
    }

    public override void Push(Vector2 force)
    {
        float x = force.x * _horizontalForce;
        float y = force.y * _verticalForce;
        force = new Vector2(x, y);
        _rigidbody.AddForce(force);
    }

    public override bool IsActive() => IsActive(out IEnumerable<Object> objectsInField);

    private Vector2 GetForce()
    {
        var force = new Vector2();

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))
            force += Vector2.up;
        if (Input.GetKey(KeyCode.S))
            force += Vector2.down;
        if (Input.GetKey(KeyCode.A))
            force += Vector2.left;
        if (Input.GetKey(KeyCode.D))
            force += Vector2.right;

        return force;
    }

    private bool IsActive(out IEnumerable<Object> objectsInField)
    {
        if (Input.GetKey(KeyCode.Q))
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(
                transform.position, _interactionRadius, Vector2.zero);
            IEnumerable<Magnet> magnets = hits.Select(hit => hit.collider.gameObject.GetComponent<Magnet>()).
                Where(magnet => magnet != null && !(magnet is Player)).Distinct();

            objectsInField = magnets;
            return magnets.Count() > 0;
        }

        else
        {
            int tiles = Utilities.TilesCountInRange
                (_tilemap, _magnetTile, transform.position, _interactionRadius);
            objectsInField = null;
            return tiles > 0;
        }
    }

    public float VerticalForce => _verticalForce;
    public float HorizontalForce => _horizontalForce;
    public float InteractionRadius => _interactionRadius;
}