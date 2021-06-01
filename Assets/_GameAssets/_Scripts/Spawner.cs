using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthEntity))]
public class Spawner : MonoBehaviour
{
    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    GameObject shipPrefab;
    [SerializeField]
    GameManager.Team team;
    [SerializeField]
    float spawnRate = 1f;
    [SerializeField]
    float waveRate = 1f;
    [SerializeField]
    int waveSpawns = 20;
    public GameManager.Level scene;

    [HideInInspector]
    public HealthEntity Armor;
    GameManager.SpawnerState spawnerState;

    private void Awake()
    {
        Armor = GetComponent<HealthEntity>();
    }


    private void Start()
    {
        foreach (GameManager.SpawnerState spawner in GameManager.instance.spawners)
        {
            if (spawner.level == scene)
            {
                if (!spawner.isDestroyed)
                {
                    spawner.spawner = this;
                }
                else
                {
                    Destroy(gameObject);
                }

                spawnerState = spawner;
            }
        }

        StartCoroutine(Spawn());
    }

    private void Update()
    {
        if (spawnerState != null)
        {
            spawnerState.currentLife = Armor.Life.Actual / Armor.Life.Max;
        }
    }

    private void OnDestroy()
    {
        if (spawnerState != null)
        {
            spawnerState.isDestroyed = true;
        }
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            for (int i = 0; i < waveSpawns; i++)
            {
                EnemyController ai = Instantiate(shipPrefab,
                               spawnPoint.position +
                               spawnPoint.InverseTransformVector(Vector3.up * Random.Range(-20, 20)) +
                               spawnPoint.InverseTransformVector(Vector3.right * Random.Range(-20, 20)),
                             Quaternion.LookRotation(spawnPoint.forward)).GetComponent<EnemyController>();

                ai._team = team;

                yield return new WaitForSeconds(spawnRate);
            }
            yield return new WaitForSeconds(waveRate);
        }
    }
}
