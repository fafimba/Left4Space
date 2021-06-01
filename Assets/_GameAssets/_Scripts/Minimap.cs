using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour {


    void Update()
    {
        Vector3 euler = transform.root.eulerAngles;
        transform.rotation = Quaternion.Euler(90, euler.y,0);
    }

}
