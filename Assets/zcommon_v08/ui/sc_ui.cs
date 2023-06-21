using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class sc_ui : MonoBehaviour
{
	//*************************************************************
	//
	public int idui;
	public bool visible;
	public Text[] text;
	public RawImage[] rawImage;
	public GameObject
		gameObjectUI;

	public sc_tools toolz;

	void Awake ()
	{
		gameObject.name = gameObject.name + "_" + idui.ToString ();
		gameObjectUI = gameObject;
				
		GameObject zcore = (GameObject.Find ("zcore")); 
		if (zcore == null) {
			//Debug.LogError ("%%% sc_UI_ERROR : ZCORE not found FAKE MODE " + idui);
			zcore = (GameObject.Find ("zcore_FAKE")); 
		}
		toolz = zcore.GetComponent<sc_tools > ();
		toolz.Join_sc_uiL (gameObject);

	}

	public void Activa ()
	{
		Debug.LogError (" somebody call activa inside sc_ui");
				
	}

	//	IEnumerator Iattiva ()
	//	{
	//
	//		while (visible) {
	//			yield return null;
	//		}
	//		gameObject.SetActive (false);
	//		yield return new WaitForEndOfFrame ();
	//
	//	}
	//
	//
	//	public void AttivaConDelay (float delay)
	//	{
	//		visible = true;
	//		gameObject.SetActive (true);
	//		foreach (Transform child in transform) {
	//			child.gameObject.SetActive (false);
	//		}
	//		StartCoroutine (IAttivaConDelay (delay));
	//
	//	}
	//	IEnumerator IAttivaConDelay (float delayz)
	//	{
	//		yield return new WaitForSeconds (delayz);
	//
	//		foreach (Transform child in transform) {
	//			child.gameObject.SetActive (true);
	//		}
	//		//visible = true;
	//
	//	}
	//
	//	public void Disattiva ()
	//	{
	//		visible = false;
	//
	//	}
	//	IEnumerator IDisattiva ()
	//	{
	//
	//		gameObject.SetActive (false);
	//		yield return new WaitForEndOfFrame ();
	//	}
	//
	//	public void DisattivaConDelay (float delay)
	//	{
	//		StartCoroutine (IDisattivaConDelay (delay));
	//	}
	//	IEnumerator IDisattivaConDelay (float delayz)
	//	{
	//		yield return new WaitForSeconds (delayz);
	//		visible = false;
	//		gameObject.SetActive (false);
	//	}
	//
	//
	//	public void BlinkText (string textx, int textid, float timeOn, float timeOff)
	//	{
	//		visible = true;
	//		StartCoroutine (IBlinkText (textx, textid, timeOn, timeOff));
	//	}
	//	IEnumerator IBlinkText (string textx, int textid, float timeOn, float timeOff)
	//	{
	//		while (visible) {
	//			text [textid].text = textx;
	//			yield return new WaitForSeconds (timeOn);
	//			text [textid].text = "";
	//			yield return new WaitForSeconds (timeOff);
	//		}
	//
	//
	//	}
	//
	//
	//
	//
	//
	//	public void FadeIn (float speed)
	//	{
	//		while (!visible) {
	//
	//
	//		}
	//	}
}
