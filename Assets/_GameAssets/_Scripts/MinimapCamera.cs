using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour {

    [SerializeField]Transform _targetFollow;
    Camera _camera;

	// Use this for initialization
	void Awake () {
        _camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 targetPosition = _targetFollow.position;
        targetPosition.y = transform.position.y;

        transform.position = targetPosition;
	}
}
