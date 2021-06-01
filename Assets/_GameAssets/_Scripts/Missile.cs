using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipEngine))]
public class Missile : Ammunition
{
    public Transform target;

    Rigidbody _rb;
    ShipEngine engine;

    void Awake()
    {
        engine = GetComponent<ShipEngine>();
        _rb = GetComponent<Rigidbody>();
    }
    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        UpdateInputs();
    }

    void UpdateInputs()
    {

        Vector3 directionToGo = GetDirection();

       engine.acelerationInput.Value = 1;
       engine.yawInput.Value = directionToGo.x;
       engine.pitchInput.Value = directionToGo.y;
    }

    Vector3 GetDirection()
    {
        Vector3 _directionToGo = Vector3.forward;
        
        if (target != null)
        {
            _directionToGo = transform.InverseTransformDirection((target.position - transform.position).normalized);
        }
        return _directionToGo;
    }
}
