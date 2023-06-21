using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class sc_random_number : MonoBehaviour
{

	public Text number;


	void Start ()
	{
		StartCoroutine (ShuffleNumbers ());
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}


	IEnumerator ShuffleNumbers ()
	{
		Debug.Log ("start shuffle number");
		int c = 0;
		while (c < 1000) {
			c = c + 1;
			yield return new WaitForSeconds (0.05f);
			number.text = Random.Range (111111111, 999999999).ToString ();
		}
		yield return null;
	}
}
