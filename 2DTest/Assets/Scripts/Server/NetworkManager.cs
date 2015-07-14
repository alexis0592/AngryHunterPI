using UnityEngine;
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

	public Text text;

	/*void Awake(){
		Debug.Log ("En el AWAKE");
		int randomNumber = (int)Random.Range (1000.0F, 9999.0F);
		//text.text = "Codigo Sala: " + (int)randomNumber;
		typeName = randomNumber.ToString();
		//typeName = randomNumber.ToString();
	}*/

	void Start(){
		//text = transform.GetComponent<Text> ();
		typeName = "abcd1234";
		message = "";
	}

	void Update(){
		
	}
	
	/*private void Awake(){
		Debug.Log("Awake");
		if (Network.peerType == NetworkPeerType.Disconnected) {
			StartServer();
		}
	}*/
	
	void OnGUI(){
		if (Network.peerType == NetworkPeerType.Server) {
			GUIStyle style = new GUIStyle();
			style.normal.textColor = Color.black;			
			GUI.Label(new Rect(50,50,100,25), "Numero Sesion: " + typeName, style);
			GUI.Label(new Rect(100,100,100,25), "Conexiones: " + Network.connections.Length, style);
			GUI.Label (new Rect (80, 80, 200, 205), "Numero Sesion: " + message);
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
	
	/// <summary>
	/// Raises the server initialized event.
	/// </summary>
	void OnServerInitialized()
	{
		Debug.Log("Server Initializied");
		serverStarted = true;
		SpawnPlayer ();
	}
	
	void OnPlayerConnected(NetworkPlayer player) 
	{
		Debug.Log("Player connected " + player.ToString());
		if (Network.connections.Length > numMaxGamers) {
			Debug.Log("No es permitido mas usuarios.");
			
		} else {
			SendInfoToClient ();
			//GetComponent<NetworkView>().RPC ("SendInfoToClient", RPCMode.All, null);
			//GetComponent<NetworkView>().RPC("SendInfoToClient",RPCMode.All);
		}
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
	void ReceiveInfoFromClient(string info){
		message = info;
		Debug.Log ("RPC ReceiveInfoFromClient: " + info);
	}
	
	[RPC]
	void ReceiveInfoFromServer(string info){
		
	}
	
	[RPC]
	void SendInfoToServer(){}

	[RPC]
	void SendInfoToClient(){
		Debug.Log ("RPC SendInfoToClient");
		GetComponent<NetworkView>().RPC ("ReceiveInfoFromServer", RPCMode.All, "HOLA MUNDO!!!");
	}
	
	public void getPosition(Vector3 getPosition){
		//gobjMira.transform.position = Vector3.Lerp (gobjMira.transform.position, getPosition, speed * Time.deltaTime);
	}
	
	private void SpawnPlayer()
	{
		Debug.Log("SpawnPlayer");
		Network.Instantiate(gobjMira, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
	}
}
