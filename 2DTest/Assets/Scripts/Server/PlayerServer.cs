using UnityEngine;
using System.Collections;


public class PlayerServer : MonoBehaviour {

	Vector3 vector;
	public int shoot = 0;

	void Start () {
		vector = new Vector3 (11.0f, 0.0f, 0.0f);
	}

	void Update () {

		if (GetComponent<NetworkView> ().isMine) {

			this.resetTargetPosition (transform.position.x, transform.position.y);

			transform.eulerAngles = vector;
			transform.Translate(vector * 0.5f);
			//shoot = 0;
			//transform.position = vector *Time.deltaTime *5;
		}
	}
	/// <summary>
	/// Resetea la posicion de la mira cuando esta se sale de pantalla.
	/// </summary>
	/// <param name="xPos">Posicion X de la mira</param>
	/// <param name="yPos">Posicion Y de la mira</param>
	private void resetTargetPosition(float xPos, float yPos){
		Vector3 targetPos;
		if (xPos < -2.3f ) {
			targetPos = new Vector3(-2.2f, yPos, 0);
			transform.position = targetPos;
		}
		if (xPos > 23.4f) {
			targetPos = new Vector3(23.3f, yPos, 0);
			transform.position = targetPos;
		}
		if (yPos < -3.1f) {
			targetPos = new Vector3(xPos, -3.0f, 0);
			transform.position = targetPos;
		}
		if (yPos > 4.4f) {
			targetPos = new Vector3(xPos, 4.3f, 0);
			transform.position = targetPos;
		}
	}

	/// <summary>
	/// Receives the player position.
	/// </summary>
	/// <param name="vectorReceived">Vector received.</param>
	[RPC]
	void ReceivePlayerPosition(Vector3 vectorReceived){
		vector = vectorReceived;
	}

	[RPC]
	void ReceivePlayerShoot(int shoot){
		this.shoot = shoot;
	}
	
}
