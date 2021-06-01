using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActiveSceneSwitch : MonoBehaviour
{

    [SerializeField]
    string _scene;
    [SerializeField]
    GameObject _lights;



    bool isActive;
    void Update()
    {
        if (transform.InverseTransformPoint(GameManager.instance.CurrentCamera.transform.position).y > 0)
        {
            if (!isActive)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(_scene));
                _lights.SetActive(true);
                isActive = true;
            }
        }
        else
        {
            if (isActive)
            {
             
                isActive = false;
                _lights.SetActive(false);
            }
        }

    }
}
