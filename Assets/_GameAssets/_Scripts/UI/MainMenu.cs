using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenu : MonoBehaviour {


    void Update () {

        if (Input.anyKeyDown)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
            else
            {
               // GameManager.instance.StartGame();
                GameManager.instance.LoadScene((int)GameManager.Level.Space);
            }
        }
	}
}
