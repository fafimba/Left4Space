using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ShieldedHealthEntity))]
[RequireComponent(typeof(ShipEngine))]
public class SpaceShip : MonoBehaviour
{


    [HideInInspector] public ShieldedHealthEntity armor;
    [HideInInspector] public ShipEngine Engine;
    [SerializeField]  public Weapon weapon;

    Rigidbody _rb;

    void Awake()
    {
        armor = GetComponent<ShieldedHealthEntity>();
        Engine = GetComponent<ShipEngine>();
        _rb = GetComponent<Rigidbody>();

        DontDestroyOnLoad(transform.gameObject);
    }

   void OnCollisionEnter(Collision hit)
    {
        Rigidbody rbHit = hit.gameObject.GetComponent<Rigidbody>();
         float hitForce;
        hitForce = hit.relativeVelocity.magnitude;
        _rb.AddExplosionForce(hitForce * 20, hit.contacts[0].point, 1000f);
         armor.GetDamage((int)(hitForce * .5f));
    }
}
