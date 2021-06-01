using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipsCountUI : MonoBehaviour {

    [SerializeField]
    Text blue;
    [SerializeField]
    Text red;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        blue.text = GameManager.instance.blueCount.ToString();
        red.text = GameManager.instance.redCount.ToString();
    }
}
