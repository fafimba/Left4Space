using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammunition : MonoBehaviour {

    [SerializeField]
    float lifeTime = 4f;
    [SerializeField]
    GameObject shootExplotionPrefab;
    [SerializeField] int _damage;

    protected virtual void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    protected void FixedUpdate()
    {
        Ray ray = new Ray(transform.position,transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray,5f, out hit, 10f))
        {
            HealthEntity armor = hit.transform.gameObject.GetComponentInParent<HealthEntity>();
       
            if (armor)
            {
                armor.GetDamage(_damage);
            }
            Instantiate(shootExplotionPrefab, hit.point, Quaternion.identity);
            Destroy(gameObject);
        }
    }    
}
