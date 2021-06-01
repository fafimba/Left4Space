using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitch : MonoBehaviour
{
    [SerializeField]
    string[] sceneToLoad;
    [SerializeField]
    string[] sceneToUnload;
    [SerializeField]
    Material fogMaterial;

    private int num;
    Transform target;
    bool isInside;

    private void Start()
    {
        target = GameManager.instance.CurrentCamera.transform;

        StartCoroutine(SwitchLevel());

    }


    IEnumerator SwitchLevel()
    {
        while (true)
        {
            float yDistance = transform.InverseTransformDirection(target.position - transform.position).y;

            if (fogMaterial != null)
            {
                if (Mathf.Abs(yDistance) <= 300f)
                {
                    fogMaterial.SetFloat("_Density", 1f / (yDistance * 0.01f));
                }
                else
                {
                    fogMaterial.SetFloat("_Density", 0f);
                }

            }


            if (yDistance <= 0)
            {

                for (int i = 0; i < sceneToLoad.Length; i++)
                {
                
                    yield return SceneManager.LoadSceneAsync(sceneToLoad[i], LoadSceneMode.Additive);
                }

                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad[0]));

                for (int j = 0; j < sceneToUnload.Length; j++)
                {
                    yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneToUnload[j]));
                }
                break;
            }
            yield return new WaitForEndOfFrame();
        }

    }
}



