using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControlClientScene : MonoBehaviour {

	#region Methods

	/// <summary>
	/// Metodo para cambiar la escena de Juego.
	/// </summary>
	/// <param name="scene">Scene a la cual se quiere cambiar</param>
	public void changeScene(string scene){
		Debug.Log("changeScene method");

		GameObject inputFieldGo = GameObject.Find("InputField 1");
		InputField inputFieldCo = inputFieldGo.GetComponent<InputField>();
		string message = inputFieldCo.text;
		if (message.Equals ("")) {
			Debug.Log ("Debe ingresar el numero de servidor");
		} else {
			Application.LoadLevel (scene);
		}
	}

	#endregion
}
