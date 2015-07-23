using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManagerClient : MonoBehaviour {
	
	
	private const string typeName = "abcd1234";
	private const string gameName = "DuckHunter";
	
	private bool isRefreshingHostList = false;
	private HostData[] hostList;
	
	private NetworkView nView;
	public GameObject gobjMiraPlayer1;
	public GameObject gobjMiraPlayer2;
	public GameObject gobjMiraPlayer3;
	public Player player;

	public string message;
	public InputField inputFieldSessionGame;

	void Awake(){

	}

	void Start(){
		message = "";
		inputFieldSessionGame = GetComponent<InputField> ();
	}

	void OnGUI(){
		if (Network.peerType == NetworkPeerType.Client) {
			GUIStyle style = new GUIStyle ();
			style.normal.textColor = Color.black;
			GUI.Label (new Rect (50, 50, 400, 405), "Estado del Juego: " + message);
		}
	}

	public void joinToGame(){
		RefreshHostList ();
	}

	/// <summary>
	/// Metodo para cambiar la escena de Juego.
	/// </summary>
	/// <param name="scene">Scene a la cual se quiere cambiar</param>
	public void changeScene(string scene){
		Debug.Log("Entra change scene");
		//message = inputFieldSessionGame.text;
		//Debug.Log(message);
		RefreshHostList ();
		if (hostList != null) {
			for(int i = 0; i < hostList.Length; i++){
				JoinServer(hostList[i]);
			}
			Application.LoadLevel (scene);
		}
	}
	
	private void RefreshHostList(){
		Debug.Log("Entr a change RefreshHostList");
		if (!isRefreshingHostList){
			isRefreshingHostList = true;
			MasterServer.RequestHostList(typeName);	
		}
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent){
		Debug.Log("Entr a change OnMasterServerEvent");
		if (msEvent == MasterServerEvent.HostListReceived) {
			hostList = MasterServer.PollHostList ();
		}
	}

	/// <summary>
	/// Metodo por medio del cual se conectan los clientes al servidor
	/// </summary>
	/// <param name="hostData">Datos del host a conectar</param>
	private void JoinServer(HostData hostData){
		Debug.Log("Entr a change JoinServer");
		Network.Connect(hostData);
	}

	/// <summary>
	/// Metodo que se ejecuta cuando un cliente se conecta al servidor Maestro
	/// </summary>
	void OnConnectedToServer(){
		Debug.Log("Entra change OnConnectedToServer");
		//SpawnPlayer ();
		SendInfoToServer();
		Debug.Log("Server Joined");
	}

	/// <summary>
	/// Raises the disconnected from server event.
	/// </summary>
	/// <param name="info">Info.</param>
	void OnDisconnectedFromServer(NetworkDisconnection info){
		Application.LoadLevel ("initScene");
		Debug.Log ("Disconnected from SERVER");
	}
	
	[RPC] 
	void SendInfoToServer(){
		Debug.Log("Info sent to server");
		string info = "Hola servidor";
		GetComponent<NetworkView>().RPC("ReceiveInfoFromClient", RPCMode.Server, info);
	}
	
	[RPC]
	void ReceiveInfoFromServer(int idPlayer){
		player = new Player ();
		player.Id = idPlayer;
		message = "CONECTADO";
		Debug.Log ("Informacion del servidor: " + idPlayer.ToString() + " CONECTADO");
	}
	
	[RPC]
	void ReceiveInfoFromClient(string info){}
	
	[RPC]
	void SendInfoToClient(){}

	private void SpawnPlayer()
	{
		Debug.Log("SpawnPlayer");
		Network.Instantiate(gobjMiraPlayer3, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
	}
}
