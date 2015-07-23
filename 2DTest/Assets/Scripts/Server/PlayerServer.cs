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

	public GameObject gobjMiraPlayer1;
	public GameObject gobjMiraPlayer2;
	public GameObject gobjMiraPlayer3;

	private GameObject currentMira;

	static int points = 0;

	Vector3 vector;
	Player player;

	public int shoot = 0;
	List<Movement> movements;
	private Ray pulsacion;
	private RaycastHit colision;

	NetworkManager networkManager;

	void Start () {
		currentMira = gobjMiraPlayer1;
		networkManager = GetComponent<NetworkManager> ();
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

			this.resetTargetPosition (currentMira.transform.position.x, currentMira.transform.position.y);

			currentMira.transform.eulerAngles = vector;
			currentMira.transform.Translate(vector * 0.5f);

		}
	}

	void OnGUI(){
		if (Network.peerType == NetworkPeerType.Server) {
			GUIStyle style = new GUIStyle();
			style.normal.textColor = Color.black;		
			style.fontSize = 12;
			GUI.Label (new Rect (370, 20, 100, 25), "Puntaje: " + points, style);
		}
	}

	/// <summary>
	/// Resetea la posicion de la mira cuando esta se sale de pantalla.
	/// </summary>
	/// <param name="xPos">Posicion X de la mira</param>
	/// <param name="yPos">Posicion Y de la mira</param>
	private void resetTargetPosition(float xPos, float yPos){
		Vector3 targetPos;
		if (xPos < -7.9f ) {
			targetPos = new Vector3(-7.8f, yPos, 0);
			currentMira.transform.position = targetPos;
		}
		if (xPos > 7.9f) {
			targetPos = new Vector3(7.8f, yPos, 0);
			currentMira.transform.position = targetPos;
		}
		if (yPos < -4.1f) {
			targetPos = new Vector3(xPos, -4.0f, 0);
			currentMira.transform.position = targetPos;
		}
		if (yPos > 4.1f) {
			targetPos = new Vector3(xPos, 4.0f, 0);
			currentMira.transform.position = targetPos;
		}
	}

	/// <summary>
	/// Receives the player position.
	/// </summary>
	/// <param name="vectorReceived">Vector received.</param>
	[RPC]
	void ReceivePlayerPosition(Vector3 vectorReceived, int idPlayer){
		player = networkManager.getPlayer (idPlayer);
		currentMira = (GameObject) player.Mira;
		vector = vectorReceived;
	}

	/// <summary>
	/// Llamada RPC que recibe el disparo del jugador
	/// </summary>
	/// <param name="shoot">Shoot.</param>
	[RPC]
	void ReceivePlayerShoot(int idPlayer){
		player = networkManager.getPlayer (idPlayer);
		Vector3 vecAux = Camera.main.WorldToScreenPoint(currentMira.transform.position);
		pulsacion = Camera.main.ScreenPointToRay(vecAux);	

		if(Physics.Raycast(pulsacion,out colision)){
			if(colision.collider.tag == "Bird"){
				string colisionName = colision.collider.name;
				switch (colisionName){
				case "angry-bird-red":
					movementRed.validateShoot(player.Points);
					player.Points = player.Points + 1;
					break;
				case "angry-bird-black":
					movementBlack.validateShoot(player.Points);
					player.Points = player.Points + 1;
					break;
				case "angry-bird-yellow":
					movementYellow.validateShoot(player.Points);
					player.Points = player.Points + 2;
					break;
				case "angry-bird-green":
					movementGreen.validateShoot(player.Points);;
					player.Points = player.Points + 2;
					break;
				case "angry-bird-blue":
					movementBlue.validateShoot(player.Points);
					player.Points = player.Points + 4;
					Debug.Log(colisionName);
					break;
				case "angry-bird-white":
					movementWhite.validateShoot(player.Points);
					player.Points = player.Points + 1;
					break;
				case "angry-bird-bigRed":
					movementBigRed.validateShoot(player.Points);
					player.Points = player.Points + 1;
					break;
				default:
					break;
				}
			}
		}
		this.shoot = shoot;
	}
	
}
