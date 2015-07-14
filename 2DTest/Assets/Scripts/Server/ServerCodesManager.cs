using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ServerCodesManager : MonoBehaviour {

	private InputField inputField;

	void Awake(){
		//text = GetComponent<Text> ();
	}
	
	void Start(){
		float randomNumber = Random.Range (1000.0F, 9999.0F);
		//text.text = "Codigo Sala: " + (int)randomNumber;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
