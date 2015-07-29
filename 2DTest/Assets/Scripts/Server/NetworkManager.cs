﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {
	
	string typeName;
	private const string gameName = "DuckHunter";
	private const int numMaxGamers = 10;
	private const int numPort = 25000;
	private bool serverStarted = false;
	public GameObject gobjMira;
	string message;

	public ArrayList playersList;

	//Gio
	//public PlayerServer server;

	public Text text;

	void Start(){

		//server = GetComponent<PlayerServer> ();
		//server.target1 = Instantiate (gameObject);
		//text = transform.GetComponent<Text> ();
		typeName = "abcd1234";
		message = "";
		playersList = new ArrayList();
	}

	void Update(){
		
	}
	
	void OnGUI(){
		if (Network.peerType == NetworkPeerType.Server) {
			GUIStyle style = new GUIStyle();
			style.normal.textColor = Color.black;		
			style.fontSize = 15;
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
	private void StartServer(){
		Debug.Log("Start Server");
		try{
			Network.InitializeServer(numMaxGamers, numPort, !Network.HavePublicAddress());
			MasterServer.RegisterHost(typeName, gameName);
		}catch(UnityException ex){
			throw ex;
		}
	}
	
	/// <summary>
	/// Raises the server initialized event.
	/// </summary>
	void OnServerInitialized(){
		Debug.Log("Server Initializied");
		serverStarted = true;
		//SpawnPlayer ();
	}
	
	void OnPlayerConnected(NetworkPlayer player) {
		Debug.Log("Player connected " + player.ToString());
		if (Network.connections.Length > numMaxGamers) {
			Debug.Log("No es permitido mas usuarios.");
			
		} else {
			NetworkViewID nViewId = Network.AllocateViewID();
			SpawnPlayer(player);
			SendInfoToClient (player.ToString());

			//GetComponent<NetworkView>().RPC ("SendInfoToClient", RPCMode.All, null);
			//GetComponent<NetworkView>().RPC("SendInfoToClient",RPCMode.All);
		}
	}
	
	// This is called on the server
	void OnPlayerDisconnected(NetworkPlayer player){
		Debug.Log("Player disconnected " + player.ToString());

		if (Network.isServer) {
			Network.RemoveRPCs(gobjMira.GetComponent<NetworkView>().viewID);
			Network.Destroy(gobjMira);
		}
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
	void ReceiveInfoFromClient(string info){
		message = info;
		Debug.Log ("RPC ReceiveInfoFromClient: " + info);
	}

	[RPC]
	void SendInfoToClient(string idPlayer){
		Debug.Log ("RPC SendInfoToClient");
		GetComponent<NetworkView>().RPC ("ReceiveInfoFromServer", RPCMode.All, "CONECTADO!!!", idPlayer);
	}
	
	public void getPosition(Vector3 getPosition){
		//gobjMira.transform.position = Vector3.Lerp (gobjMira.transform.position, getPosition, speed * Time.deltaTime);
	}

	private void SpawnPlayer(NetworkPlayer player){
		Debug.Log("SpawnPlayer");
		gobjMira.name = "Mira_" + player.ToString ();
		GameObject clone = (GameObject)Network.Instantiate (gobjMira, new Vector3 (0f, 0f, 0f), Quaternion.identity, 0);
		//clone.GetComponent<NetworkView>().name = "Mira_" + player.ToString ();
		playersList.Add (player);
	}
	
	//************Declaracion de Metodos RPC del lado del cliente***************************

	[RPC]
	void ReceiveInfoFromServer(string info, string idPlayer){}
	
	[RPC]
	void SendInfoToServer(){}
}
