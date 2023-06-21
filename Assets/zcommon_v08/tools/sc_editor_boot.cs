using UnityEngine;
using System.Collections;

public class sc_editor_boot : MonoBehaviour
{
	//sc_tools toolz;
	//public string scenaDiPartenza
	void Start ()
	{
		#if UNITY_EDITOR
		//Debug.LogError (" EDITOR BOOT AWAKE");
		GameObject tools = (GameObject.Find ("ztools")); // connessione a tools
		//toolz = tools.GetComponent<sc_tools> ();

		if (tools == null) {

			Debug.LogError ("°°°°°°°°°°°°°°°°°° CELE EDITOR BOOT load scene 0");

			Application.LoadLevel (0);
		} 



		#endif
	}
	


}
