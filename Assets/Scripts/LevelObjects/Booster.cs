using UnityEngine;

public sealed class Booster : FourDirectional
{
    [SerializeField] private float _force;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == null) return;
        if (!collision.gameObject.TryGetComponent(out Magnet magnet)) return;

        magnet.Push(Direction, _force);
    }
}