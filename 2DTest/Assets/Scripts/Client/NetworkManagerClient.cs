using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManagerClient : MonoBehaviour {
	
	
	private const string typeName = "abcd1234";
	private const string gameName = "DuckHunter";
	
	private bool isRefreshingHostList = false;
	private HostData[] hostList;
	
	private NetworkView nView;
	public GameObject gobjMira;

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
			GUI.Label (new Rect (50, 50, 200, 205), "Numero Sesion: " + message);

		}
	}
	
	public void changeScene(string scene){
		//JoinServer (hostList[0]);
		Debug.Log("Entr a change scene");
		//message = inputFieldSessionGame.text;
		//Debug.Log(message);
		RefreshHostList ();
		if (hostList != null) {
			JoinServer(hostList[0]);
			//GetComponent<NetworkView>().RPC("SendInfoToServer", RPCMode.All, "HOLA SERVER");
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
	
	private void JoinServer(HostData hostData){
		Debug.Log("Entr a change JoinServer");
		Network.Connect(hostData);

	}
	
	void OnConnectedToServer(){
		SpawnPlayer ();
		//Network.Instantiate (gobjMira, new Vector3 (0f, 0f, 0f), Quaternion.identity, 0);
		SendInfoToServer();
		Debug.Log("Entr a change OnConnectedToServer");
		Debug.Log("Server Joined");
	}

	
	[RPC] 
	void SendInfoToServer(){
		Debug.Log("Info sent to server");
		string info = "Hola servidor";
		GetComponent<NetworkView>().RPC("ReceiveInfoFromClient", RPCMode.Server, info);
	}
	
	[RPC]
	void ReceiveInfoFromServer(string info){
		message = info;
		Debug.Log ("Informacion del servidor: " + info);
		//OnGUI ();
		//Network.isMessageQueueRunning = false;
	}
	
	[RPC]
	void ReceiveInfoFromClient(string info){}
	
	[RPC]
	void SendInfoToClient(){}

	private void SpawnPlayer()
	{
		Debug.Log("SpawnPlayer");
		Network.Instantiate(gobjMira, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
	}
}
