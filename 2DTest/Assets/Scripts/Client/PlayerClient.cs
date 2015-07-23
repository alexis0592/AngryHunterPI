using UnityEngine;
using System.Collections;

public class PlayerClient : MonoBehaviour {

	float Gx = 0f;
	float Gy = 0f;
	float Gz = 0f;
	static float alpha = 0.5f;
	Vector3 vectorPlayer;
	private bool clicked = false;
	NetworkManagerClient networkManagerClient;


	// Use this for initialization
	void Start () {
		networkManagerClient = GetComponent<NetworkManagerClient> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Network.peerType == NetworkPeerType.Client) {
			moveMira ();
		}
	}
	
	private float pitch(){
		return Mathf.Atan(Gy/(Mathf.Sqrt(Mathf.Pow(Gx,2) + Mathf.Pow(Gz,2))));
	}
	
	private float roll(){
		return Mathf.Atan(-Gx/Gz);
	}

	void OnConnectedToServer(){
		Update ();
	}

	public void OnMouseDown(){
		sendShootToServer (1);
	}

	/// <summary>
	/// Llamada RPC que envia los datos del acelerometro al servidor, para mover la mira
	/// </summary>
	[RPC]
	void moveMira(){
		Gx = Input.acceleration.x * alpha + (Gx * (1.0f - alpha));
		Gy = Input.acceleration.y * alpha + (Gy * (1.0f - alpha));
		Gz = Input.acceleration.z * alpha + (Gz * (1.0f - alpha));

		vectorPlayer = new Vector3 (roll(), - pitch (), 0);
		GetComponent<NetworkView> ().RPC ("ReceivePlayerPosition", RPCMode.Server, vectorPlayer, 2);//networkManagerClient.player.Id);
	}

	/// <summary>
	/// Llamada RPC que envia el disparo del jugador al servidor.
	/// </summary>
	[RPC]
	public void sendShootToServer(int idPlayer){
		GetComponent<NetworkView> ().RPC ("ReceivePlayerShoot", RPCMode.Server, 2);//networkManagerClient.player.Id);
	}

	/// <summary>
	/// LLamada RPC del Script PlayerServer, que recibe los datos adquiridos por el acelerometro
	/// </summary>
	/// <param name="vectorReceived">Vector received.</param>
	[RPC]
	void ReceivePlayerPosition(Vector3 vectorReceived, int idPlayer){}

	[RPC]
	void ReceivePlayerShoot(int idPlayer){}
}
