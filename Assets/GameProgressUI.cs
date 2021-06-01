using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameProgressUI : MonoBehaviour {

    [SerializeField]
    Image SpaceProgress;
    [SerializeField]
    Image AirProgress;
    [SerializeField]
    Image WaterProgress;
	
	void Update ()
    {
        foreach (GameManager.SpawnerState spawnerState in GameManager.instance.spawners)
        {
            switch (spawnerState.level)
            {
                case GameManager.Level.Space:
                    SpaceProgress.fillAmount = spawnerState.currentLife;
                    break;
                case GameManager.Level.Air:
                    AirProgress.fillAmount = spawnerState.currentLife;
                    break;
                case GameManager.Level.Water:
                    WaterProgress.fillAmount = spawnerState.currentLife;
                    break;
                default:
                    break;
            }
        }
	}
}
