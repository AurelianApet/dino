using UnityEngine;
using UnityEngine.UI;


using System.Collections;

public class sc_form_email : MonoBehaviour
{
	public InputField emailInputText;
	public string urlPHP;


	public void SubmitEmail ()
	{
		Debug.Log ("SUBMIT EMAIL");
		string email = emailInputText.text;
		StartCoroutine (ISpia (email));	
	}

	IEnumerator  ISpia (string message) // invia un messagio a SPIA su riomoko.com
	{
		//	Debug.Log ("Spia_In");
		WWWForm form = new WWWForm ();
		form.AddField ("mex", message);
		//	WWW www = new WWW ("http://www.riomoko.com/sendemail.php", form);
		WWW www = new WWW (urlPHP, form);
		//WWW www = new WWW ("https://www.riomoko.c9.ixsecure.com/spia.php", form);
		yield return www;
		// check for errors
		if (www.error == null) {
			Debug.Log ("***************** WWW Ok!: " + www.text);
		} else {
			Debug.Log ("!!!!!!!!!!!!!!!!! WWW Error: " + www.error);
		}
		//	Debug.Log ("Spia_out");
	}
}
