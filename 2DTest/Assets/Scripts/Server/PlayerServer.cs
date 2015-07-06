using UnityEngine;
using System.Collections;

public class PlayerServer : MonoBehaviour {

	Vector3 vector;

	void Start () {
		vector = new Vector3 (11.0f, 0.0f, 0.0f);
	}

	void Update () {

		if (GetComponent<NetworkView> ().isMine) {


			this.resetTargetPosition (transform.position.x, transform.position.y);

			transform.eulerAngles = vector;
			transform.Translate(vector * 0.3f);
			//transform.position = vector *Time.deltaTime *5;
		}
	}

	/*void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info){
		Vector3 pos = new Vector3(15.0f, 2.0f, 0.0f);
		Vector3 receivedPosition = new Vector3(10.0f, -2.0f, 0.0f);
		
		if (stream.isWriting) {

			pos = transform.position;
			stream.Serialize (ref pos);
		} else {
			stream.Serialize(ref receivedPosition);
			transform.position = receivedPosition;
		}
	}

	private float pitch(){
		return Mathf.Atan(Gy/(Mathf.Sqrt(Mathf.Pow(Gx,2) + Mathf.Pow(Gz,2))));
	}

	private float roll(){
		return Mathf.Atan(-Gx/Gz);
	}*/

	private void resetTargetPosition(float xPos, float yPos){
		Vector3 targetPos;
		if (xPos < -1.3f ) {
			targetPos = new Vector3(-1.4f, yPos, 0);
			transform.position = targetPos;
		}
		if (xPos > 28.5f) {
			targetPos = new Vector3(28.0f, yPos, 0);
			transform.position = targetPos;
		}
		if (yPos < -3.1f) {
			targetPos = new Vector3(xPos, -3.0f, 0);
			transform.position = targetPos;
		}
		if (yPos > 3.6f) {
			targetPos = new Vector3(xPos, 3.5f, 0);
			transform.position = targetPos;
		}
	}

	[RPC]
	void ReceivePlayerPosition(Vector3 vectorReceived){
		vector = vectorReceived;
	}
	
}
