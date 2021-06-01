using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldedHealthEntity : HealthEntity
{
    [SerializeField]
    int _maxShield = 10;
    [SerializeField]
    float _shieldRegenerationRate = .5f;
    [SerializeField]
    float _hitTimeRecovery = 5f;

    public Stat Shield;

    bool hitted;

    protected override void Awake()
    {
        base.Awake();
        Shield.Max = _maxShield;
        Shield.Actual = _maxShield;
        Shield.RegenerationRate = _shieldRegenerationRate;
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(RegenerateShield());
    }

    public override void GetDamage(int damage)
    {
        Life.Actual -= damage - Shield.Actual;
        Shield.Actual -= damage;

        hitted = true;

        if (Life.Actual == 0)
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }


    IEnumerator RegenerateShield()
    {
        while (true)
        {
            if (!hitted)
            {
                Shield.Actual++;
            }
            else
            {
                yield return new WaitForSeconds(_hitTimeRecovery);
                hitted = false;
            }

            yield return new WaitForSeconds(Shield.RegenerationRate);
        }
    }
}

