using UnityEngine;
using System.Collections;

public class RayCastCollider : MonoBehaviour {

	private Ray pulsacion;
	private RaycastHit colision;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(0))
		{
			Vector3 v = Input.mousePosition;
			pulsacion=Camera.main.ScreenPointToRay(v);

			if(Physics.Raycast(pulsacion,out colision))
			{
				Debug.Log(colision.collider.name); 
			}
		}
	}
}
