using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator : MonoBehaviour
{

//   [System.Serializable]
//   public struct sWaveInfo
//   {
//       public int countEnemies;
//       public int enemyLife;
//
//       public int durationSpawn;
//       public int spawnRadius;
//       public int enemyVelocity;
//
//       public Transform spawnPoint;
//       public EnemyShip prefab;
//   }
//
//   public sWaveInfo[] info;
//
//   private PlayerShip _playerShip;
//   private int _actualWaveIndex;
//   private int _actualCountShip;
//
//   // Use this for initialization
//   void Awake()
//   {
//       _playerShip = FindObjectOfType<PlayerShip>();
//   }
//
//   void Start()
//   {
//       NextSpawn();
//   }
//
//   public void DecrementEnemy()
//   {
//       _actualCountShip--;
//       if (_actualCountShip == 0)
//       {
//           NextSpawn();
//       }
//   }
//
//   public void NextSpawn()
//   {
//       StartCoroutine(NextSpawnCoroutine());
//   }
//
//   IEnumerator NextSpawnCoroutine()
//   {
//
//       sWaveInfo i = info[_actualWaveIndex];
//       WaitForSeconds w = new WaitForSeconds(i.countEnemies / i.durationSpawn);
//
//       for (int j = 0; j < i.countEnemies; j++)
//       {
//           EnemyShip enemy = Instantiate(i.prefab, i.spawnPoint.position, Quaternion.identity);
//           enemy.Inizialize(_playerShip,this,i.enemyVelocity);
//           yield return w;
//       }
//
//       _actualCountShip = i.countEnemies;
//       if (++_actualWaveIndex >=  info.Length)
//       {
//           //TODO: fin del juego
//
//       }
//   }

}
