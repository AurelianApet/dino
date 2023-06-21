using UnityEngine;
using System.Collections;

public class sc_canvas_awake : MonoBehaviour
{

	sc_ui[] uiChild;
	void Awake ()
	{
		uiChild = transform.GetComponentsInChildren<sc_ui> (true);
		//	Debug.Log ("ui children" + uiChild.Length.ToString ());
		for (int c=0; c<uiChild.Length; ++c) {
			uiChild [c].gameObject.SetActive (true);
		}
	}

}
