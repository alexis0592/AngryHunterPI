using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerServer : MonoBehaviour {
	Movement movementGreen;
	Movement movementRed;
	Movement movementYellow;
	Movement movementBlack;
	Movement movementBlue;
	Movement movementWhite;
	Movement movementBigRed;
	public GameObject green;
	public GameObject red;
	public GameObject yellow;
	public GameObject black;
	public GameObject blue;
	public GameObject white;
	public GameObject bigRed;
	
	Vector3 vector;
	public int shoot = 0;
	List<Movement> movements;

	private Ray pulsacion;
	private RaycastHit colision;

	void Start () {
		vector = new Vector3 (0.0f, 0.0f, 0.0f);
		movements = new List<Movement> ();

		movementGreen = green.GetComponent<Movement> ();
		movementRed = red.GetComponent<Movement> ();
		movementYellow = yellow.GetComponent<Movement> ();
		movementBlack = black.GetComponent<Movement> ();
		movementBlue = blue.GetComponent<Movement> ();
		movementWhite = white.GetComponent<Movement> ();
		movementBigRed = bigRed.GetComponent<Movement> ();
	}

	void Update () {

		if (GetComponent<NetworkView> ().isMine) {

			this.resetTargetPosition (transform.position.x, transform.position.y);

			transform.eulerAngles = vector;
			transform.Translate(vector * 0.5f);

		}
	}

	/// <summary>
	/// Resetea la posicion de la mira cuando esta se sale de pantalla.
	/// </summary>
	/// <param name="xPos">Posicion X de la mira</param>
	/// <param name="yPos">Posicion Y de la mira</param>
	private void resetTargetPosition(float xPos, float yPos){
		Vector3 targetPos;
		if (xPos < 2.8f ) {
			targetPos = new Vector3(2.7f, yPos, 0);
			transform.position = targetPos;
		}
		if (xPos > 32.4f) {
			targetPos = new Vector3(32.3f, yPos, 0);
			transform.position = targetPos;
		}
		if (yPos < -10.6f) {
			targetPos = new Vector3(xPos, -10.5f, 0);
			transform.position = targetPos;
		}
		if (yPos > -3.6f) {
			targetPos = new Vector3(xPos, -3.5f, 0);
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

	/// <summary>
	/// Llamada RPC que recibe el disparo del jugador
	/// </summary>
	/// <param name="shoot">Shoot.</param>
	[RPC]
	void ReceivePlayerShoot(int shoot){
		Vector3 vecAux = Camera.main.WorldToScreenPoint(transform.position);

		pulsacion=Camera.main.ScreenPointToRay(vecAux);	
		if(Physics.Raycast(pulsacion,out colision)){
			if(colision.collider.tag == "Bird"){
				string colisionName = colision.collider.name;
				switch (colisionName){
				case "angry-bird-red":
					movementRed.validateShoot();
					break;
				case "angry-bird-black":
					movementBlack.validateShoot();
					break;
				case "angry-bird-yellow":
					movementYellow.validateShoot();
					break;
				case "angry-bird-green":
					movementGreen.validateShoot();;
					break;
				case "angry-bird-blue":
					movementBlue.validateShoot();
					Debug.Log(colisionName);
					break;
				case "angry-bird-white":
					movementWhite.validateShoot();
					break;
				case "angry-bird-bigRed":
					movementBigRed.validateShoot();
					break;
				default:
					break;
				}
			}
		}
		this.shoot = shoot;
	}
	
}
