using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trails : MonoBehaviour {


    Rigidbody _rb;

    TrailRenderer[] trails;

	// Use this for initialization
	void Awake () {
        _rb = GetComponentInParent<Rigidbody>();
        trails = GetComponentsInChildren<TrailRenderer>();
      

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        foreach (TrailRenderer trail in trails)
        {
            float actualSpeed = transform.InverseTransformVector(_rb.velocity).z;

               Color _color = trail.material.GetColor("_TintColor");
                _color.a = Mathf.Clamp01(actualSpeed * actualSpeed * .0001f);
                trail.material.SetColor("_TintColor", _color);

        }
	}
}
