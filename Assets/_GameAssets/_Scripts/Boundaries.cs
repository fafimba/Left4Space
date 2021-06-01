using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour {

    [SerializeField] float topPoint;
    [SerializeField]
    float bottomPoint;
    [SerializeField]
    float radius;

    [SerializeField] LayerMask[] layersToCheck;
    LayerMask _layerMask = 0;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < layersToCheck.Length; i++)
        {
            _layerMask = _layerMask | layersToCheck[i];
        }
	}
	

    IEnumerator CheckBoundaries()
    {
        while (true)
        {

            foreach (ShipEngine ship in FindObjectsOfType<ShipEngine>())
            {
             //   ship.swi
            }
            foreach (Collider c in Physics.OverlapCapsule(Vector3.up * topPoint, Vector3.up * bottomPoint, radius))
            {
                if (new Vector2(c.transform.position.x,c.transform.position.z).magnitude > radius * .8f)
                {
                    //comeback
                }
                else
                {

                }
            }
            yield return new WaitForSeconds(1f);
        }
     
    }
}
