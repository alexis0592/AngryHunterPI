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

	public GameObject mira1;
	public GameObject mira2;

	static int points = 0;

	Vector3 vector;
	public int shoot = 0;
	List<Movement> movements;

	private Ray pulsacion;
	private RaycastHit colision;
	
	float Gx = 0f;
	float Gy = 0f;
	float Gz = 0f;
	static float alpha = 0.5f;
	Vector3 vector1;
	Vector3 vector2;
	Vector3 vector3;
	NetworkManager nManager;
	NetworkManagerClient nmClient;

	ArrayList  posicionesMira1List;
	ArrayList posicionesMira2List;
	//GIO:
	//public GameObject target1;

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
		nManager = GetComponent<NetworkManager> ();

		posicionesMira1List = new ArrayList ();
		posicionesMira2List = new ArrayList ();

	}

	void Update () {

		if (GetComponent<NetworkView> ().isMine) {
			this.resetTargetPosition (transform.position.x, transform.position.y);

			string transformName = transform.name;
			Vector3 vecAux;
			if (transformName.Substring (5, 1) == "1") {
				transform.eulerAngles = vector1;
				transform.Translate(vector1 * 0.5f);
				//Debug.Log(transform.gameObject.name + " , " + transform.position + " , " + vector1);
			}else if(transformName.Substring (5, 1) == "2"){
				transform.eulerAngles = vector2;
				transform.Translate(vector2 * 0.5f);
				//Debug.Log(transform.gameObject.name + " , " + transform.position + " , " + vector2);
			}



		}

		if (Network.peerType == NetworkPeerType.Client) {
			moveMira ();
			for(int i = 0; i < Input.touchCount; i++){
				if(Input.GetTouch(i).phase == TouchPhase.Began){
					sendShootToServer(1);
				}
			}
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
		if (xPos < -7.91f && yPos < -4.1f) {
			targetPos = new Vector3(-7.9f, 4.0f, 0);
			transform.position = targetPos;
		}
		if (xPos < -7.91f ) {
			targetPos = new Vector3(-7.9f, yPos, 0);
			transform.position = targetPos;
		}
		if (xPos > 8.0f) {
			targetPos = new Vector3(7.9f, yPos, 0);
			transform.position = targetPos;
		}
		if (yPos < -4.1f) {
			targetPos = new Vector3(xPos, -4.0f, 0);
			transform.position = targetPos;
		}
		if (yPos > 4.1f) {
			targetPos = new Vector3(xPos, 4.0f, 0);
			transform.position = targetPos;
		}
	}

	/// <summary>
	/// Receives the player position.
	/// </summary>
	/// <param name="vectorReceived">Vector received.</param>
	[RPC]
	void ReceivePlayerPosition(Vector3 vectorReceived, string idPlayer){
		if (idPlayer == "1") {
			vector1 =  vectorReceived;
		} else if(idPlayer == "2"){
			vector2 = vectorReceived;
		}
		//vector = vectorReceived;
		//Debug.Log ("Recibido soy: " + transform.name);
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
				//points++;
				switch (colisionName){
				case "angry-bird-red":
					movementRed.validateShoot(points);
					points = points + 2;
					break;
				case "angry-bird-black":
					movementBlack.validateShoot(points);
					points = points + 1;
					break;
				case "angry-bird-yellow":
					movementYellow.validateShoot(points);
					points = points + 2;
					break;
				case "angry-bird-green":
					movementGreen.validateShoot(points);;
					points = points + 2;
					break;
				case "angry-bird-blue":
					movementBlue.validateShoot(points);
					Debug.Log(colisionName);
					points = points + 4;
					break;
				case "angry-bird-white":
					movementWhite.validateShoot(points);
					break;
				case "angry-bird-bigRed":
					movementBigRed.validateShoot(points);
					break;
				default:
					break;
				}
			}
		}
		this.shoot = shoot;
	}

	//************************Metodos del cliente*****************************

	private float pitch(){
		return Mathf.Atan(Gy/(Mathf.Sqrt(Mathf.Pow(Gx,2) + Mathf.Pow(Gz,2))));
	}
	
	private float roll(){
		return Mathf.Atan(-Gx/Gz);
	}
	
	void OnConnectedToServer(){
		Update ();
	}
	
	[RPC]
	void moveMira(){
		GameObject shareObject = GameObject.Find("shareObject");
		nmClient = shareObject.GetComponent<NetworkManagerClient> ();
		Gx = Input.acceleration.x * alpha + (Gx * (1.0f - alpha));
		Gy = Input.acceleration.y * alpha + (Gy * (1.0f - alpha));
		Gz = Input.acceleration.z * alpha + (Gz * (1.0f - alpha));

		string transformId = transform.name.Substring(5, 1);

		vector = new Vector3 (roll(), - pitch (), 0);
		
		GetComponent<NetworkView>().RPC ("ReceivePlayerPosition", RPCMode.All, vector,nmClient.playerId);
	}

	[RPC]
	public void sendShootToServer(int shootToServer){
		
		GetComponent<NetworkView> ().RPC ("ReceivePlayerShoot", RPCMode.Server, shootToServer);
	}
	
}
