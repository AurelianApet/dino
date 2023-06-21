using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class sc_adjust_aspectratio : MonoBehaviour
{

	public float width;
	public float Hight;
	public RectTransform rect;
	//RawImage screenCaptured;
	public float ratio;
	public float moltiplicator;


	void Start ()
	{
		width = Screen.width;
		Hight = Screen.height;
		ratio = (width / Hight);
		//Debug.Log (ratio + "- " + Screen.width + "- " + Screen.height);
		//screenCaptured = GetComponent<RawImage> ();
		//rect = GetComponent<RectTransform> ();
		//rect.rect.width = rect.rect.height * Screen.width / Screen.height;
		rect.localScale = new Vector3 (ratio * moltiplicator, moltiplicator, moltiplicator);
		//	Screen.width;
		//	Screen.height;
	}
	
	// Update is called once per frame
	//	void Update ()
	//	{
	//		Debug.Log (ratio + "- " + Screen.width + "- " + Screen.height);
	//	}
}
