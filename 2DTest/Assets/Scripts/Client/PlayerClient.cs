using UnityEngine;
using System.Collections;

public class PlayerClient : MonoBehaviour {

	float Gx = 0f;
	float Gy = 0f;
	float Gz = 0f;
	static float alpha = 0.5f;
	Vector3 vector;
	private bool clicked = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Network.peerType == NetworkPeerType.Client) {
			moveMira ();
		}

		/*if(!clicked){
			sendShootToServer(0);
		}else{
			sendShootToServer(1);
		}*/
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

	public void OnMouseUp(){
		sendShootToServer (0);
	}

	/// <summary>
	/// Llamada RPC que envia los datos del acelerometro al servidor, para mover la mira
	/// </summary>
	[RPC]
	void moveMira(){
		Gx = Input.acceleration.x * alpha + (Gx * (1.0f - alpha));
		Gy = Input.acceleration.y * alpha + (Gy * (1.0f - alpha));
		Gz = Input.acceleration.z * alpha + (Gz * (1.0f - alpha));

		vector = new Vector3 (roll(), - pitch (), 0);

		GetComponent<NetworkView>().RPC ("ReceivePlayerPosition", RPCMode.Server, vector);
	}

	/// <summary>
	/// Llamada RPC que envia el disparo del jugador al servidor.
	/// </summary>
	[RPC]
	public void sendShootToServer(int shootToServer){

		GetComponent<NetworkView> ().RPC ("ReceivePlayerShoot", RPCMode.Server, shootToServer);
	}

	/// <summary>
	/// LLamada RPC del Script PlayerServer, que recibe los datos adquiridos por el acelerometro
	/// </summary>
	/// <param name="vectorReceived">Vector received.</param>
	[RPC]
	void ReceivePlayerPosition(Vector3 vectorReceived){}

	[RPC]
	void ReceivePlayerShoot(int shoot){}
}
