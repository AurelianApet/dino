using UnityEngine;
using System.Collections;

public class sc_jukebox : MonoBehaviour
{



	public AudioClip[] clip;

	public float[] clipVolume;


	void Start ()
	{
	
	}

	//**********   per testare il volume poi disattivare tutto
	public bool next;
	public bool again;
	public int index;
	void Update ()
	{
		if (again) {
			PlayClipN (index);
			again = false;
		}

		if (next) {
			index = index + 1;
			if (index == clip.Length) {
				index = 0;
			}
			PlayClipN (index);
			next = false;
		}


	}

	//**********   per testare il volume poi disattivare tutto
	

	public void PlayClipN (int clipZ)
	{
		GetComponent<AudioSource>().pitch = (Random.Range (0.6f, 1.4f));
		GetComponent<AudioSource>().volume = clipVolume [clipZ];
		GetComponent<AudioSource>().clip = clip [clipZ];
		GetComponent<AudioSource>().Play ();
	}
}
