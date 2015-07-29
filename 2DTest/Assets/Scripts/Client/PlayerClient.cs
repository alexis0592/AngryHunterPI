using UnityEngine;
using System.Collections;

public class PlayerClient : MonoBehaviour {

	private const string typeName = "abcd1234";
	private const string gameName = "DuckHunter";
	
	private bool isRefreshingHostList = false;
	private HostData[] hostList;
	
	private NetworkView nView;
	public Player player = null;
	
	public string message;

	private int idCurrentPlayer;

	static float alpha = 0.5f;
	int id = 0;
	int points = 0;
	string nick = "";
	float Gx = 0f;
	float Gy = 0f;
	float Gz = 0f;
	Vector3 vectorPlayer;
	GameObject mira;

	#region MonoBehaviour Methods

	void Awake(){
		Debug.Log ("Awake method");
		RefreshHostList ();
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
		Debug.Log ("Start method");
		RefreshHostList ();
		if (hostList != null) {
			for(int i = 0; i < hostList.Length; i++){
				JoinServer(hostList[i]);
			}
		}
		message = "";
	}
	
	// Update is called once per frame
	void Update () {
		if (Network.peerType == NetworkPeerType.Client) {
			moveMira ();
		}
	}

	/// <summary>
	/// Raises the GU event.
	/// </summary>
	void OnGUI(){
		if (Network.peerType == NetworkPeerType.Client) {
			GUIStyle style = new GUIStyle ();
			style.normal.textColor = Color.black;
			GUI.Label (new Rect (50, 50, 400, 405), "Estado del Juego: " + message);
		}
	}

	void OnConnectedToServer(){
		Debug.Log ("OnConnectedToServer method");
		Update ();
	}

	/// <summary>
	/// Raises the master server event event.
	/// </summary>
	/// <param name="msEvent">Ms event.</param>
	void OnMasterServerEvent(MasterServerEvent msEvent){
		Debug.Log ("OnMasterServerEvent Method");
		if (msEvent == MasterServerEvent.HostListReceived) {
			hostList = MasterServer.PollHostList ();
		}
	}

	
	/// <summary>
	/// Raises the disconnected from server event.
	/// </summary>
	/// <param name="info">Info.</param>
	void OnDisconnectedFromServer(NetworkDisconnection info){
		Debug.Log ("OnDisconnectedFromServer method");
		Application.LoadLevel("OnDisconnectedFromServer");

	}

	/// <summary>
	/// Refreshs the host list.
	/// </summary>
	private void RefreshHostList(){
		Debug.Log("RefreshHostList Method");
		if (!isRefreshingHostList){
			isRefreshingHostList = true;
			MasterServer.RequestHostList(typeName);	
		}
	}

	/// <summary>
	/// Metodo por medio del cual se conectan los clientes al servidor
	/// </summary>
	/// <param name="hostData">Datos del host a conectar</param>
	private void JoinServer(HostData hostData){
		Debug.Log("JoinServer Method");
		Network.Connect(hostData);
	}

	#endregion

	#region private and public methods

	private float pitch(){
		return Mathf.Atan(Gy/(Mathf.Sqrt(Mathf.Pow(Gx,2) + Mathf.Pow(Gz,2))));
	}
	
	private float roll(){
		return Mathf.Atan(-Gx/Gz);
	}

	#endregion

	#region RPC Methods

	/// <summary>
	/// Llamada RPC que envia los datos del acelerometro al servidor, para mover la mira
	/// </summary>
	[RPC]
	void moveMira(){
		Gx = Input.acceleration.x * alpha + (Gx * (1.0f - alpha));
		Gy = Input.acceleration.y * alpha + (Gy * (1.0f - alpha));
		Gz = Input.acceleration.z * alpha + (Gz * (1.0f - alpha));

		vectorPlayer = new Vector3 (roll(), - pitch (), 0);
		GetComponent<NetworkView> ().RPC ("ReceivePlayerPosition", RPCMode.Server, vectorPlayer, id);
	}

	/// <summary>
	/// Llamada RPC que envia el disparo del jugador al servidor.
	/// </summary>
	[RPC]
	public void sendShootToServer(int idPlayer){
		GetComponent<NetworkView> ().RPC ("ReceivePlayerShoot", RPCMode.Server, id);
	}

	/// <summary>
	/// LLamada RPC del Script PlayerServer, que recibe los datos adquiridos por el acelerometro
	/// </summary>
	/// <param name="vectorReceived">Vector received.</param>
	[RPC]
	void ReceivePlayerPosition(Vector3 vectorReceived, int idPlayer){}

	[RPC]
	void ReceivePlayerShoot(int idPlayer){}

	[RPC] 
	void SendInfoToServer(){
		Debug.Log("Info sent to server");
		string info = "Hola servidor";
		GetComponent<NetworkView>().RPC("ReceiveInfoFromClient", RPCMode.Server, info);
	}
	
	[RPC]
	void ReceiveInfoFromServer(NetworkViewID idView, int idPlayer, Vector3 vectorPlayer){
		Debug.Log ("RPC ReceiveInfoFromServer");
		
		Transform clone = Instantiate(mira, vectorPlayer, Quaternion.identity) as Transform as Transform;
		NetworkView nView = clone.GetComponent<NetworkView>();
		nView.viewID = idView;
		
		Debug.Log ("Informacion del servidor: " + idPlayer + " Conectado");
	}
	
	[RPC]
	void ReceiveInfoFromClient(string info){}
	
	[RPC]
	void SendInfoToClient(){}
	#endregion
}
