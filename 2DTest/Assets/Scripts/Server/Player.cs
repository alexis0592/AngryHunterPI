using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	float Gx = 0f;
	float Gy = 0f;
	float Gz = 0f;
	static float alpha = 0.8f;
	Vector3 vector;


	//float yRotation = 5.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Gx = Input.acceleration.x * alpha + (Gx * (1.0f - alpha));
		Gy = Input.acceleration.y * alpha + (Gy * (1.0f - alpha));
		Gz = Input.acceleration.z * alpha + (Gz * (1.0f - alpha));


		//transform.Translate(Input.acceleration.x * 0.3f, Input.acceleration.y * 0.3f,0);
		//yRotation += Input.GetAxis("Horizontal");
		//transform.eulerAngles = new Vector3(10, yRotation, 0);
		this.resetTargetPosition (transform.position.x, transform.position.y);

		vector = new Vector3 (roll(), - pitch (), 0);
		transform.eulerAngles = vector;
		transform.Translate(vector * 0.3f);
		
	}

	private float pitch(){
		return Mathf.Atan(Gy/(Mathf.Sqrt(Mathf.Pow(Gx,2) + Mathf.Pow(Gz,2))));
	}

	private float roll(){
		return Mathf.Atan(-Gx/Gz);
	}

	private void resetTargetPosition(float xPos, float yPos){
		Vector3 targetPos;
		if (xPos < -1.3f ) {
			targetPos = new Vector3(-1.4f, yPos, 0);
			transform.position = targetPos;
		}
		if (xPos > 28.5f) {
			targetPos = new Vector3(28.0f, yPos, 0);
			transform.position = targetPos;
		}
		if (yPos < -3.1f) {
			targetPos = new Vector3(xPos, -3.0f, 0);
			transform.position = targetPos;
		}
		if (yPos > 3.6f) {
			targetPos = new Vector3(xPos, 3.5f, 0);
			transform.position = targetPos;
		}
		
	}
}
