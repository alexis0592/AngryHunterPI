using UnityEngine;
using System.Collections;

public class ControlMainScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void changeScene(string scene){
		Debug.Log("chanse scene");
		Application.LoadLevel (scene);
	}
}
