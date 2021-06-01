using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpaceShip))]
public class EnemyController : MonoBehaviour
{

    [SerializeField]
    Transform target;
    [SerializeField]
    GameObject _laserPrefab;
    [SerializeField]
    Transform shootSpot;
    [SerializeField]
    GameObject colliderBody;
    [SerializeField]
    bool formation = false;
    [SerializeField]
    SpaceShip leaderSpaceShip;
    [SerializeField]
    bool log = false;
    public GameManager.Team _team;


    float acelerationInputLeader;
    float horizontalInputLeader;
    float veritcalInputLeader;
    float rollInputLeader;
    float boostInputLeader;
    float yawInputLeader;
    float pitchInputLeader;


    SpaceShip ship;

    LayerMask layerMaskEnemies = 0;

    Vector3 directionToGo;
    SteeringState currentState;
    float _tmpRate;
    bool isAvoiding = false;

    enum SteeringState
    {
        wander,
        seek,
        flee,
        parking
    }

    private void Awake()
    {
        ship = GetComponent<SpaceShip>();
        colliderBody.layer = (int)_team;
        layerMaskEnemies = GameManager.instance.SetLayers(_team);
        StartCoroutine(CheckTarget());
        StartCoroutine(FormationCorrection());
    }

    void Update()
    {
        UpdateInputs();
    }

    private void FixedUpdate()
    {
        LeaderEngineInputs();
        AIAvoid();
        CorrectRotation();
        Aim();
    }

    void CorrectRotation()
    {
        if (!formation)
        {
            if (target == null) return;

            float anglesToGoUp = Quaternion.LookRotation(transform.forward, target.up).eulerAngles.z;
            anglesToGoUp = anglesToGoUp - transform.localRotation.eulerAngles.z;
            anglesToGoUp = NormalizedAngle(anglesToGoUp);
            if (Mathf.Abs(anglesToGoUp) > 1f)
            {
                ship.Engine.rollInput.Value = anglesToGoUp;
            }
        }
    }

    void Aim()
    {
        GameObject enemyToShoot = null;
        Vector3 targetDir = Vector3.zero;
        RaycastHit hit = new RaycastHit();
        float closestEnemy = ship.weapon.range;
        if (Time.time > _tmpRate)
        {
            _tmpRate = Time.time + ship.weapon.rate;
            foreach (Collider col in Physics.OverlapSphere(transform.position, ship.weapon.range, layerMaskEnemies))
            {
                float distanceToTarget = Vector3.Distance(col.transform.position, transform.position);
                if (distanceToTarget < closestEnemy)
                {
                    targetDir = col.transform.position - transform.position;
                    float angleToTarget = Vector3.Angle(targetDir, transform.forward);
                    if (angleToTarget < 30f)
                    {
                        Ray ray = new Ray(transform.position, targetDir.normalized);
                        if (Physics.Raycast(ray, out hit, ship.weapon.range, layerMaskEnemies))
                        {
                            if (hit.collider == col)
                            {
                                enemyToShoot = col.gameObject;
                                closestEnemy = distanceToTarget;
                            }
                        }
                    }
                }
            }

            if (enemyToShoot != null)
            {
                ship.weapon.Shoot(hit);
            }
        }
    }

    void LeaderEngineInputs()
    {

        if (currentState == SteeringState.parking)
        {

            ship.Engine.acelerationInput.Value = leaderSpaceShip.Engine.acelerationInput.Value;
            ship.Engine.horizontalInput.Value = leaderSpaceShip.Engine.horizontalInput.Value;
            ship.Engine.veritcalInput.Value = leaderSpaceShip.Engine.veritcalInput.Value;
            ship.Engine.rollInput.Value = leaderSpaceShip.Engine.rollInput.Value;
            ship.Engine.boostInput.Value = leaderSpaceShip.Engine.boostInput.Value;
            ship.Engine.yawInput.Value = leaderSpaceShip.Engine.yawInput.Value;
            ship.Engine.pitchInput.Value = leaderSpaceShip.Engine.pitchInput.Value;

        }
    }

    float NormalizedAngle(float angle)
    {
        float result = angle % 360;
        result = result > 180 ? result -= 360 : result;
        result = result < -180 ? result += 360 : result;
        return result;
    }

    void AIAvoid()
    {
        RaycastHit hit;


        if (Physics.SphereCast(transform.position, 5f, transform.forward, out hit, 100f))
        {
            isAvoiding = true;
            Vector3 targetToAvoid = transform.InverseTransformPoint(hit.point);

            ship.Engine.horizontalInput.Value = -targetToAvoid.x;
            ship.Engine.yawInput.Value = -targetToAvoid.x;
            ship.Engine.veritcalInput.Value = -targetToAvoid.y;
            ship.Engine.pitchInput.Value = -targetToAvoid.y;

        }
        else
        {
            isAvoiding = false;
        }
    }


    void UpdateInputs()
    {
        directionToGo = GetDirection();

    }

    IEnumerator CheckTarget()
    {
        while (true)
        {

            if (!formation)
            {

                foreach (Collider c in Physics.OverlapSphere(transform.position, 2000f, layerMaskEnemies))
                {
                    float distance = Vector3.Distance(c.transform.position, transform.position);

                    float angle = Vector3.Angle(transform.forward, (c.transform.position - transform.position).normalized);
                    if (angle < 30 || angle > 150)
                    {
                        distance *= 0.5f;
                    }
                    if (target == null)
                    {
                        target = c.transform;
                    }
                    else if (distance < Vector3.Distance(transform.position, target.position))
                    {
                        target = c.transform;
                    }
                }
            }

            yield return new WaitForSeconds(.5f);
        }
    }

    IEnumerator FormationCorrection()
    {
        while (true)
        {
            if (currentState == SteeringState.parking && !isAvoiding)
            {
                ship.transform.rotation = leaderSpaceShip.transform.rotation;
            }
            yield return new WaitForSeconds(.5f);
        }

    }

    Vector3 GetDirection()
    {
        Vector3 distanceToTarget = Vector3.zero;

        if (target != null)
        {
            distanceToTarget = (target.position) - transform.position;

            if (formation)
            {
                if (distanceToTarget.magnitude < 70f)
                {
                    currentState = SteeringState.parking;
                }
                else
                {
                    currentState = SteeringState.seek;
                }
            }
            else if (distanceToTarget.magnitude > 200f)
            {

                currentState = SteeringState.seek;
            }
            else if (Vector3.Angle(target.forward, transform.position - target.position) < 30f || distanceToTarget.magnitude < 50f)
            {
                currentState = SteeringState.flee;
            }
            else
            {
                currentState = SteeringState.seek;
            }
        }
        else
        {
            currentState = SteeringState.wander;
        }
        if (log)
        {
            Debug.Log(currentState);
        }

        Vector3 _directionToGo = transform.forward;
        switch (currentState)
        {
            case SteeringState.wander:
                _directionToGo = transform.forward;
                ship.Engine.acelerationInput.Value = 1;
                break;
            case SteeringState.seek:
                _directionToGo = transform.InverseTransformDirection(distanceToTarget.normalized);
                ship.Engine.acelerationInput.Value = 1;
                ship.Engine.yawInput.Value = _directionToGo.x;
                ship.Engine.pitchInput.Value = _directionToGo.y;
                if (log)
                {
                    Debug.Log(ship.Engine.yawInput.Value);
                     Debug.Log(ship.Engine.pitchInput.Value);
                }
                break;
            case SteeringState.flee:
                _directionToGo = -transform.InverseTransformDirection(distanceToTarget.normalized);
                ship.Engine.acelerationInput.Value = 1;
                ship.Engine.yawInput.Value = _directionToGo.x;
                ship.Engine.pitchInput.Value = _directionToGo.y;
                break;
            case SteeringState.parking:
                _directionToGo = transform.InverseTransformVector(distanceToTarget.normalized);
                // if (leaderSpaceShip)
                // {
                //     ship.Engine.acelerationInput.Value = leaderSpaceShip.Engine.acelerationInput.Value;
                //     ship.Engine.horizontalInput.Value = leaderSpaceShip.Engine.horizontalInput.Value;
                //     ship.Engine.veritcalInput.Value = leaderSpaceShip.Engine.veritcalInput.Value;
                //     ship.Engine.rollInput.Value = leaderSpaceShip.Engine.rollInput.Value;
                //     ship.Engine.boostInput.Value = leaderSpaceShip.Engine.boostInput.Value;
                //     ship.Engine.yawInput.Value = leaderSpaceShip.Engine.yawInput.Value;
                //     ship.Engine.pitchInput.Value = leaderSpaceShip.Engine.pitchInput.Value;
                //
                // }

                //  ship.Engine.horizontalInput.Value = _directionToGo.x;
                //  ship.Engine.veritcalInput.Value = _directionToGo.y;
                //  ship.Engine.acelerationInput.Value = _directionToGo.z;
                //
                //
                //
                //  float anglesToGoForwardY = target.rotation.eulerAngles.y;// Quaternion.LookRotation(target.forward, target.up).eulerAngles.y;
                //  anglesToGoForwardY -= transform.localRotation.eulerAngles.y;
                //  anglesToGoForwardY = NormalizedAngle(anglesToGoForwardY);
                //
                //  if (log)
                //  {
                //     // Debug.Log(transform.localRotation.eulerAngles.y);
                //  }
                //
                //  if (Mathf.Abs(anglesToGoForwardY) > 1f)
                //  {
                //      ship.Engine.yawInput.Value = -anglesToGoForwardY;
                //  }
                //
                //
                //
                //  float anglesToGoForwardX = target.rotation.eulerAngles.x;// Quaternion.LookRotation(target.forward, target.up).eulerAngles.y;
                //  anglesToGoForwardX -= transform.localRotation.eulerAngles.x;
                //  anglesToGoForwardX = NormalizedAngle(anglesToGoForwardX);
                //
                //  if (log)
                //  {
                //      // Debug.Log(transform.localRotation.eulerAngles.y);
                //  }
                //
                //  if (Mathf.Abs(anglesToGoForwardX) > 1f)
                //  {
                //      ship.Engine.pitchInput.Value = -anglesToGoForwardX;
                //  }

                break;
            default:
                _directionToGo = transform.forward;
                ship.Engine.acelerationInput.Value = 1;
                currentState = SteeringState.seek;
                break;
        }

        //Limites del mapa... a repasar
        //  if (new Vector2(transform.position.x, transform.position.z).magnitude > GameManager.instance.maxRadius)
        //  {
        //      _directionToGo = (new Vector3(0, transform.position.y, 0) - transform.position).normalized;
        //  }
        //  else if (transform.position.y > GameManager.instance.maxHeight || transform.position.y < GameManager.instance.minHeight)
        //  {
        //      _directionToGo = (new Vector3(transform.position.x, 0, transform.position.z) - transform.position).normalized;
        //  }

        return _directionToGo;
    }



    void OnDrawGizmos()
    {

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.TransformDirection(directionToGo) * 100);
    }

}






