using UnityEngine;
using System.Collections;

public class sc_volume_audio_generale : MonoBehaviour
{


		//***************************************************************************************************************************************
		//Questo scripst ServerTime per regolare il sc_volume_audio_generale dell'audiolistener e per questo va piazzato sulla camera che contiene l'audio listener
		//*************************************************************************************************************************************** v1  4 settembre 2014

		// Use this for initialization
		public void RegolaVolume (float volume)
		{
				AudioListener.volume = volume;
		}
	

}
