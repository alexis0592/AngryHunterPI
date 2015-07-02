using UnityEngine;
using System.Collections;

public class Rotation : MonoBehaviour {

	private Quaternion rotTarget;
	private float rotateEverySecond = 1;
	private float lerpCounter;
	private Quaternion rotCache;
	
	// Use this for initialization
	void Start () {
		randomRot ();
		InvokeRepeating("randomRot", 0,rotateEverySecond);
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Lerp(transform.rotation, rotTarget, lerpCounter*Time.deltaTime);
		lerpCounter++;
	}
	
	private void randomRot() {
		rotTarget = Random.rotation;
		rotCache = transform.rotation;
		lerpCounter = 0;
	}
}
