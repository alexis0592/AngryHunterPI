﻿using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	
	Vector3 target;
	float timer = 0;
	public float speed;
	//public float angle;
	Vector3 initPos;
	 
	private Ray pulsacion;
	private RaycastHit colision;
	
	void Start (){
		
		initPos = new Vector3 (3.5f, 2.5f, 0);
	}
	
	void Update (){

		resetPosition (transform.position.x, transform.position.y);

		timer += Time.deltaTime;
		float step = speed * Time.deltaTime;
		Vector3 v= ResetTarget (initPos);
		initPos = v;
		transform.position = Vector3.MoveTowards (transform.position, v, step);

		if (Input.GetMouseButton (0)) {
			Vector3 mousePosition = Input.mousePosition;
			pulsacion=Camera.main.ScreenPointToRay(mousePosition);
			if(Physics.Raycast(pulsacion,out colision))
			{
				Debug.Log(colision.collider.name); 
			}
		}
	}

	/*
	 * Metodo para resetear el vector posicion bajo el movimiento descrito por un tiro parabolico
	 */
	public Vector3 ResetTarget (Vector3 vec){

		float x = (53.5f*Mathf.Cos(45) * timer) + vec.x;
		float y = (-0.5f *9.8f *Mathf.Pow(timer, 2)) + (12.5f * Mathf.Sin(45) * timer) + vec.y;
		Vector3 v3 = new Vector3(x , y, 0);
		
		return v3;
	}
	
	private int ResetSec (){
		timer = 0;
		return Random.Range(1, 3);
	}
	
	private void resetPosition(float xPosition, float yPosition){
		if(xPosition >= 33.5f || yPosition <= -11.0f || yPosition >= -1.0f){

			timer = 0;
			initPos= new Vector3(-1.1f, Random.Range(-5.0f, -9.0f), 0.0f);
			transform.position = initPos;

		}
	}

	public void validateShoot(){

		initPos= new Vector3(-1.1f, Random.Range(-5.0f, -9.0f), 0.0f);
		Vector3 v= ResetTarget (initPos);
		transform.position = initPos;
	}
}
