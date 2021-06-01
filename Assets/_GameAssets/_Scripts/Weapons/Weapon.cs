using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{

    public float rate;
    public float range;
    [SerializeField]
    Transform _spawnPoint;
    [SerializeField]
    Ammunition _laserPrefab;
    [SerializeField]
    Ammunition _missilePrefab;
    [SerializeField]
    float ShootSpeed = 100f;

#if UNITY_EDITOR
    [SerializeField]
    bool _debugMode;
#endif
    public void Shoot(Vector3 impactPoint)
    {
        InstatiateShoot(impactPoint);
    }

    public void Shoot(RaycastHit hit)
    {
        Vector3 shootTarget;

        Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
        if (rb)
        {
            shootTarget = FindInterceptVector(transform.position, ShootSpeed, hit.transform.position, rb.velocity);
        }
        else
        {
            shootTarget = hit.point;
        }

        InstatiateShoot(shootTarget);
    }

    void InstatiateShoot(Vector3 shootTarget)
    {
        Rigidbody rbAmmunition = Instantiate(_laserPrefab, transform.position, Quaternion.LookRotation(shootTarget - transform.position)).GetComponent<Rigidbody>();
        rbAmmunition.AddRelativeForce(Vector3.forward * ShootSpeed, ForceMode.Impulse);
    }

    public void ShootMissile(Transform missileTarget = null)
    {
      Missile missile =  Instantiate(_missilePrefab, transform.position + (transform.forward*20), transform.rotation).GetComponent<Missile>();
        missile.target = missileTarget;
    }

    public Vector3 GetSpawnPoint()
    {
        return _spawnPoint.position;
    }

    void OnDrawGizmos()
    {
     //   if (!_debugMode) return;
     //
     //   Gizmos.color = Color.red;
     //   Gizmos.DrawRay(_spawnPoint.position, transform.forward * 100);
    }

    private Vector3 FindInterceptVector(Vector3 shotOrigin, float shotSpeed, Vector3 targetOrigin, Vector3 targetVel)
    {

        Vector3 dirToTarget = Vector3.Normalize(targetOrigin - shotOrigin);

        Vector3 targetVelOrth = Vector3.Dot(targetVel, dirToTarget) * dirToTarget;
        Vector3 shotVelTang = targetVel - targetVelOrth;

        float shotVelSpeed = shotVelTang.magnitude;

        if (shotVelSpeed > shotSpeed)
        {
            return targetVel.normalized * shotSpeed;
        }
        else
        {
            float shotSpeedOrth =
            Mathf.Sqrt(shotSpeed * shotSpeed - shotVelSpeed * shotVelSpeed);
            Vector3 shotVelOrth = dirToTarget * shotSpeedOrth;

            float timeToCollision = ((shotOrigin - targetOrigin).magnitude) / (shotVelOrth.magnitude - targetVelOrth.magnitude);

            Vector3 shotVel = shotVelOrth + shotVelTang;
            Vector3 shotCollisionPoint = shotOrigin + shotVel * timeToCollision;

            return shotCollisionPoint;
        }
    }
}
