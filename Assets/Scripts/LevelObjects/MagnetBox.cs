using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public sealed class MagnetBox : Magnet
{
    private Rigidbody2D _rigidbody;

    [SerializeField] private float _repelForce;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public override void Push(Vector2 position)
    {
        _rigidbody.AddForce(position * _repelForce);
    }

    public override bool IsActive()
    {
        IEnumerable<RaycastHit2D> hits =
            Physics2D.CircleCastAll(transform.position, _interactionRadius, Vector2.zero);
        hits = hits.Where(hit => hit.collider.gameObject.TryGetComponent(out Magnet magnet));
        return hits.Count() > 1;
    }
}