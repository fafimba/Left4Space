using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIAim : MonoBehaviour
{
    public float distanceAim;
    public float angleAim;
    Transform weapon;


    void Update()
    {
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject enemyToShoot = null;
        float closestEnemy = distanceAim;
        for (int i = 0; i < enemies.Length; i++)
        {
            float distanceToTarget = Vector3.Distance(enemies[i].transform.position, transform.position);
            if (distanceToTarget < distanceAim ) //check distance in this way to improve performances
            {
                if (distanceToTarget < closestEnemy)
                {
                    Vector3 targetDir = enemies[i].transform.position - transform.position;
                    float angleToTarget = Vector3.Angle(targetDir, transform.forward);
                    if (angleToTarget < angleAim)
                    {
                        if (!Physics.Linecast(transform.position, enemies[i].transform.position))
                        {
                            enemyToShoot = enemies[i];
                            closestEnemy = distanceToTarget;
                        }
                    }
                }
            }
        }

        if (enemyToShoot != null)
        {
            weapon.LookAt(enemyToShoot.transform);
        }
        else
        {
            weapon.rotation = transform.parent.rotation;
        }
    }
}
