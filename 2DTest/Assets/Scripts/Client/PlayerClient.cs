using UnityEngine;
using System.Collections;

public class PlayerClient : MonoBehaviour {

	float Gx = 0f;
	float Gy = 0f;
	float Gz = 0f;
	static float alpha = 0.8f;
	Vector3 vector;

	// Use this for initialization
	void Start () {
	
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

	[RPC]
	void moveMira(){
		Gx = Input.acceleration.x * alpha + (Gx * (1.0f - alpha));
		Gy = Input.acceleration.y * alpha + (Gy * (1.0f - alpha));
		Gz = Input.acceleration.z * alpha + (Gz * (1.0f - alpha));

		vector = new Vector3 (roll(), - pitch (), 0);

		GetComponent<NetworkView>().RPC ("ReceivePlayerPosition", RPCMode.All, vector);
	}

	[RPC]
	void ReceivePlayerPosition(Vector3 vectorReceived){}
}
