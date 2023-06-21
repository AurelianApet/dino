using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class sc_blink_image : MonoBehaviour
{
	public float onTime;
	public float offTime;
	Image image;
	bool on;
	float timer;

	void Start ()
	{
		image = GetComponent<Image> ();
		on = true;
		timer = Time.time + onTime;

	}
	

	void Update ()
	{
		if (timer < Time.time) {
			if (on) {
				timer = Time.time + offTime;
				image.enabled = false;
				on = false;
			} else {
				timer = Time.time + onTime;
				image.enabled = true;
				on = true;
			}
		}
	}
}
