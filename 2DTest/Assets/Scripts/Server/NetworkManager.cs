using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {

	#region ATRIBUTOS PRIVADOS

	//Server Configuration
	private string typeName = "abcd1234";
	private const string gameName = "DuckHunter";
	private const int numMaxGamers = 3;
	private const int numPort = 25000;

	private bool serverStarted = false;
	private Ray pulsacion;
	private RaycastHit colision;

	Movement movementGreen;
	Movement movementRed;
	Movement movementYellow;
	Movement movementBlack;
	Movement movementBlue;
	Movement movementWhite;
	Movement movementBigRed;

	Vector3 vector;
	Player player;

	#endregion

	#region ATRIBUTOS PUBLICOS

	public GameObject gobjMiraPlayer;
	public GameObject green;
	public GameObject red;
	public GameObject yellow;
	public GameObject black;
	public GameObject blue;
	public GameObject white; 
	public GameObject bigRed;
	public List<Player> listPlayers;
	public int shoot = 0;

	#endregion


	#region MONOBEHAVIOUR METHODS

	void Awake(){
		Debug.Log("Awake Method");
		StartServer ();
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start(){
		Debug.Log("Start Method");
		StartMovementBirds ();
		StartPlayers();
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update(){
		Debug.Log("Update Method");
		if (GetComponent<NetworkView> ().isMine) {
			if(player != null){
				this.resetTargetPosition (player.gameObject.transform.position.x, player.gameObject.transform.position.y);
				player.gameObject.transform.eulerAngles = vector;
				player.gameObject.transform.Translate(vector * 0.5f);
			}else
				Debug.Log("Player is null");
		}
	}

	/// <summary>
	/// Raises the GU event.
	/// </summary>
	void OnGUI(){
		GUIStyle style = new GUIStyle();
		style.normal.textColor = Color.black;		
		style.fontSize = 15;

		if (Network.peerType == NetworkPeerType.Server) {
			GUI.Label(new Rect(20,20,100,25), "Numero Sesion: " + typeName, style);
			GUI.Label(new Rect(380,20,100,25), "Jugadores: " + Network.connections.Length, style);
			GUI.Label (new Rect (10, 270, 100, 25), "Puntaje Jugador 1: " + listPlayers[0].Points, style);
			GUI.Label (new Rect (190, 270, 100, 25), "Puntaje Jugador 2: " + listPlayers[1].Points, style);
			GUI.Label (new Rect (380, 270, 100, 25), "Puntaje Jugador 3: " + listPlayers[2].Points, style);
		}
	}

	/// <summary>
	/// Raises the server initialized event.
	/// </summary>
	void OnServerInitialized()
	{
		Debug.Log("OnServerInitialized Method");
		if (!serverStarted) {
			Debug.Log ("Server Initializied");
			serverStarted = true;
		} 
	}
	
	void OnPlayerConnected(NetworkPlayer player) 
	{
		Debug.Log("OnPlayerConnected Method");
		Debug.Log("Player connected " + player.ToString());
		if (Network.connections.Length > numMaxGamers) {
			Debug.Log("No es permitido mas usuarios.");
		} else {
			//Se captura el id de un nuevo jugador
			int idNewPlayerConnected = connectNewPlayer();
			if(idNewPlayerConnected != 0){
				SendInfoToClient(idNewPlayerConnected);
			}else{
				Debug.Log("No es permitido mas usuarios.");
			}
			
		}
	}

	// This is called on the server
	void OnPlayerDisconnected(NetworkPlayer player) 
	{
		Debug.Log("OnPlayerDisconnected Method");
		/*
		foreach (Player oldPlayer in listPlayers) {
			if(oldPlayer.Id.Equals(player.ToString()))
			{
				oldPlayer.Connected = false;
				break;
			}
		}
		Debug.Log("Player disconnected " + player.ToString());
		*/
	}
	
	/// <summary>
	/// Raises the failed to connect to master server event.
	/// </summary>
	/// <param name="info">Info.</param>
	void OnFailedToConnectToMasterServer(NetworkConnectionError info){
		Debug.Log("OnFailedToConnectToMasterServer Method");
		Debug.Log ("Problemas en la conexion: " + info);
		serverStarted = false;
	}

	#endregion

	#region PRIVATE METHODS

	/// <summary>
	/// Starts the server. 
	/// </summary>
	private void StartServer()
	{
		try{
			Debug.Log("StartServer Method started");
			if(!serverStarted){
				Network.InitializeServer(numMaxGamers, numPort, !Network.HavePublicAddress());
				MasterServer.RegisterHost(typeName, gameName);
				//typeName = Random.Range(1000, 9999).ToString();
				Debug.Log("The server started");
			}
			else
				Debug.Log("The server didn't start");

			Debug.Log("StartServer Method Finished");
		}catch(UnityException ex){
			Debug.LogError("Exception when the server started: " + ex.Message);
			throw ex;
		}
	}

	/// <summary>
	/// Starts the movement birds.
	/// </summary>
	private void StartMovementBirds (){
		Debug.Log("StartMovementBirds method started");
		vector = new Vector3 (0.0f, 0.0f, 0.0f);

		movementGreen = green.GetComponent<Movement> ();
		movementRed = red.GetComponent<Movement> ();
		movementYellow = yellow.GetComponent<Movement> ();
		movementBlack = black.GetComponent<Movement> ();
		movementBlue = blue.GetComponent<Movement> ();
		movementWhite = white.GetComponent<Movement> ();
		movementBigRed = bigRed.GetComponent<Movement> ();

		Debug.Log("StartMovementBirds method finished");
	}

	/// <summary>
	/// Starts the players.
	/// </summary>
	private void StartPlayers(){
		Debug.Log("StartPlayers method started");
		Player player1 = new Player (1, "Player 1", 0, false);
		listPlayers.Add (player1);

		Player player2 = new Player (2, "Player 2", 0, false);
		listPlayers.Add (player2);

		Player player3 = new Player (3, "Player 3", 0, false);
		listPlayers.Add (player3);
		Debug.Log("StartPlayers method finished");
	}

	/// <summary>
	/// Connects the new player.
	/// </summary>
	/// <returns>The new player.</returns>
	private int connectNewPlayer(){
		int idPlayer = 0;
		foreach (Player player in listPlayers) {
			if(!player.Connected)
			{
				player.Connected = true;
				break;
			}
		}

		return idPlayer;
	}

	/// <summary>
	/// Gets the player.
	/// </summary>
	/// <returns>The player.</returns>
	/// <param name="idPlayer">Identifier player.</param>
	public Player getPlayer(int idPlayer){
		Player newPlayer = null;
		foreach (Player player in listPlayers) {
			if(player.Id.Equals(idPlayer))
			{
				newPlayer = player;
				break;
			}
		}
		return newPlayer;
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
			player.gameObject.transform.position = targetPos;
		}
		if (xPos > 7.9f) {
			targetPos = new Vector3(7.8f, yPos, 0);
			player.gameObject.transform.position = targetPos;
		}
		if (yPos < -4.1f) {
			targetPos = new Vector3(xPos, -4.0f, 0);
			player.gameObject.transform.position = targetPos;
		}
		if (yPos > 4.1f) {
			targetPos = new Vector3(xPos, 4.0f, 0);
			player.gameObject.transform.position = targetPos;
		}
	}
	
	#endregion

	#region RPC METHODS

	[RPC]
	void ReceiveInfoFromClient(int idPlayer){
		Debug.Log ("RPC ReceiveInfoFromClient: " + idPlayer);
	}
	
	[RPC]
	void ReceiveInfoFromServer(int idPlayer, Vector3 vectorPlayer){
		Debug.Log ("RPC ReceiveInfoFromServer: " + idPlayer);
	}
	
	[RPC]
	void SendInfoToServer(){
		Debug.Log ("RPC SendInfoToServer");
	}

	[RPC]
	void SendInfoToClient(int idPlayer){
		Debug.Log ("RPC SendInfoToClient");
		NetworkViewID viewID = Network.AllocateViewID();
		GetComponent<NetworkView>().RPC ("ReceiveInfoFromServer", RPCMode.AllBuffered, viewID, player.Id, player.gameObject.transform.position);
	}

	/// <summary>
	/// Receives the player position.
	/// </summary>
	/// <param name="vectorReceived">Vector received.</param>
	[RPC]
	void ReceivePlayerPosition(Vector3 vectorReceived, int idPlayer){
		Debug.Log ("RPC ReceivePlayerPosition");
		player = getPlayer (idPlayer);
		vector = vectorReceived;
	}
	
	/// <summary>
	/// Llamada RPC que recibe el disparo del jugador
	/// </summary>
	/// <param name="shoot">Shoot.</param>
	[RPC]
	void ReceivePlayerShoot(int idPlayer){
		Debug.Log ("RPC ReceivePlayerShoot");
		player = getPlayer (idPlayer);
		Vector3 vecAux = Camera.main.WorldToScreenPoint(player.gameObject.transform.position);
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

	#endregion
}
