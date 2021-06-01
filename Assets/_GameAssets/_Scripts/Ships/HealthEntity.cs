using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEntity : MonoBehaviour
{

    [SerializeField]
    protected GameObject _explosionPrefab;

    [SerializeField]
    int _maxLife = 10;
    [SerializeField]
    float _lifeRegenerationRate = 5f;

    public Stat Life;

    virtual protected void Awake()
    {
        Life.Max = _maxLife;
        Life.Actual = _maxLife;
        Life.RegenerationRate = _lifeRegenerationRate;
    }

    virtual protected void Start()
    {
        StartCoroutine(RegenerateLife());
    }

    public virtual void GetDamage(int damage)
    {
        Life.Actual -= damage;
        if (Life.Actual == 0)
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    IEnumerator RegenerateLife()
    {
        while (true)
        {
            Life.Actual++;
            yield return new WaitForSeconds(Life.RegenerationRate);
        }
    }
}

