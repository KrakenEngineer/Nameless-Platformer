using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public sealed class LaserSensor : Sensor
{
    private List<Collider2D> _collidersInTrigger = new List<Collider2D>();

    protected override void Start()
    {
        base.Start();
        _eventSignal = 1;
        _cancelSignal = 0;
        _minSignal = 0;
        _maxSignal = float.MaxValue;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _collidersInTrigger.Add(collision);
        List<Collider2D> nullDeleted = _collidersInTrigger.Where(c => c != null).ToList();
        IncreaseSignal(1);
        if (_collidersInTrigger.Count > nullDeleted.Count)
            DecreaseSignal(_collidersInTrigger.Count - nullDeleted.Count);
        _collidersInTrigger = nullDeleted;
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {
        _collidersInTrigger.Remove(collision);
        DecreaseSignal(1);
    }
}