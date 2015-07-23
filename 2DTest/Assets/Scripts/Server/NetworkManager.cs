using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {

	private const string typeName = "abcd1234";
	private const string gameName = "DuckHunter";
	private const int numMaxGamers = 3;
	private const int numPort = 25000;
	private bool serverStarted = false;
	public GameObject gobjMiraPlayer1;
	public GameObject gobjMiraPlayer2;
	public GameObject gobjMiraPlayer3;
	public IList listPlayers;

	string message;

	public Text text;

	void Start(){
		listPlayers = prepararJugadores();
		//int randomNumber = (int)Random.Range (1000.0F, 9999.0F);
		//typeName = randomNumber.ToString ();
		//typeName = "abcd1234";
		message = "";
	}

	void Update(){
		
	}

	void OnGUI(){
		GUIStyle style = new GUIStyle();
		style.normal.textColor = Color.black;		
		style.fontSize = 15;

		if (Network.peerType == NetworkPeerType.Server) {
			GUI.Label(new Rect(20,20,100,25), "Numero Sesion: " + typeName, style);
			GUI.Label(new Rect(20,40,100,25), "Jugadores: " + Network.connections.Length, style);
		}
	}
	
	public void changeScene(string scene){
		Debug.Log("chanse scene");
		StartServer ();
		if (serverStarted) {
			Application.LoadLevel (scene);
		}
	}
	
	/// <summary>
	/// Starts the server. 
	/// </summary>
	private void StartServer()
	{
		Debug.Log("Start Server");
		try{
			Network.InitializeServer(numMaxGamers, numPort, !Network.HavePublicAddress());
			MasterServer.RegisterHost(typeName, gameName);
		}catch(UnityException ex){
			throw ex;
		}
	}

	private IList prepararJugadores(){
		IList listNewPlayers = new ArrayList ();

		Player player1 = new Player (gobjMiraPlayer1.GetInstanceID(), "Player 1", 0, false, gobjMiraPlayer1);
		listNewPlayers.Add (player1);

		Player player2 = new Player (gobjMiraPlayer2.GetInstanceID(), "Player 2", 0, false, gobjMiraPlayer2);
		listNewPlayers.Add (player2);

		Player player3 = new Player (gobjMiraPlayer3.GetInstanceID(), "Player 3", 0, false, gobjMiraPlayer3);
		listNewPlayers.Add (player3);

		return listNewPlayers;
	}
	
	/// <summary>
	/// Raises the server initialized event.
	/// </summary>
	void OnServerInitialized()
	{
		Debug.Log("Server Initializied");
		serverStarted = true;
	}
	
	void OnPlayerConnected(NetworkPlayer player) 
	{
		Debug.Log("Player connected " + player.ToString());
		if (Network.connections.Length > numMaxGamers) {
			Debug.Log("No es permitido mas usuarios.");
		} else {
			//Se captura el id de un nuevo jugador
			Player newPlayerConnected = connectNewPlayer();
			if(newPlayerConnected != null){
				SendInfoToClient(newPlayerConnected.Id);
			}else{
				Debug.Log("No es permitido mas usuarios.");
			}

		}
	}

	private Player connectNewPlayer(){
		Player newPlayer = null;
		foreach (Player player in listPlayers) {
			if(!player.Connected)
			{
				newPlayer = player;
				newPlayer.Connected = true;
				break;
			}
		}
		return newPlayer;
	}

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


	// This is called on the server
	void OnPlayerDisconnected(NetworkPlayer player) 
	{
		Debug.Log("Player disconnected " + player.ToString());
	}
	
	/// <summary>
	/// Raises the failed to connect to master server event.
	/// </summary>
	/// <param name="info">Info.</param>
	void OnFailedToConnectToMasterServer(NetworkConnectionError info){
		Debug.Log ("Problemas en la conexion: " + info);
		serverStarted = false;
	}

	[RPC]
	void ReceiveInfoFromClient(int idPlayer){
		message = "OE";
		Debug.Log ("RPC ReceiveInfoFromClient: " + idPlayer.ToString());
	}
	
	[RPC]
	void ReceiveInfoFromServer(int idPlayer){
		
	}
	
	[RPC]
	void SendInfoToServer(){}

	[RPC]
	void SendInfoToClient(int idPlayer){
		Debug.Log ("RPC SendInfoToClient");
		GetComponent<NetworkView>().RPC ("ReceiveInfoFromServer", RPCMode.All, idPlayer);
	}
}
