using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//using CodeStage.AntiCheat;// servono per offuscare variabili di player prefs
//using CodeStage.AntiCheat.ObscuredTypes;// servono per offuscare variabili di player prefs
using System.Text.RegularExpressions;

//attenzione questo per controllo email
//using Facebook.MiniJSON;
using System;

//serve per postfacebook callback
//using System.Linq;
//using Holoville.HOTween;
//using Holoville.HOTween.Plugins;


//*********************************************************************************
// New collection tool cele ver 2 20 febbraio 2015
//*********************************************************************************
//              first release 2 novembre 2013


public class sc_tools : MonoBehaviour
{

	#if UNITY_EDITOR
	public bool resetPlayerPref;
	public bool inibisciTalkHomeEditor;
	#endif
	public bool inibisciTalkHomeGenerale;
	public bool checkValidation;
	bool blockApp;
	public string validationUrl;
	public GameObject canvasValidation;

	public bool webcamOnAtStart;
	public bool ouya720p;
	//forza il rendering a 720p per OUYA o fire TV
	public bool overscanAttivo;
	public float overscanParametro;
	public bool forzaFrameRate;
	//forza il rendering a 720p per OUYA
	public int targetFPS;
	// funziona solo se forzaFrameRate è true
	// public bool disattivaCeleGUI;
	[HideInInspector]
	public bool
		musicOn;
	[HideInInspector]
	public bool
		sfxOn;
	public GameObject CameraAudioListener;
	//	static AudioListener AudioGenerale;
	// public sc_user user;


	void Awake ()
	{
		AwakeUI ();
		GetTime ();
		if (ouya720p) {
			Screen.SetResolution (1280, 720, true, 60);
		}
		if (forzaFrameRate) {
			Application.targetFrameRate = targetFPS;
		}
		//whWebcamRatio = 1.0f;
		//	Debug.Log ("qui awake");
		//******* iOS GameCenter Inizializzazione 1.0      first release 25dic2013 
		#if UNITY_IPHONE
		/*^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
				Social.localUser.Authenticate (success => {
						if (success) {
								Debug.Log ("GAMECENTER Authentication successful");
								gameCenter = true;
								string userInfo = "Username: " + Social.localUser.userName + 
										"\nUser ID: " + Social.localUser.id + 
										"\nIsUnderage: " + Social.localUser.underage;
								Debug.Log ("Gamecenter user info : " + userInfo);
						} else
								gameCenter = true;// boh lo forzo a true e funziona in test lo mantengo
						Debug.Log ("GAMECENTER iPhone Authentication failed");
				});

		^^^^^^^^^^^^^^^^^^^^^^^*/
		#endif	
		//********questo serve per ShareAndroid
		// Unity Editor throws JNI error. Can only test on Android device or emulator.
		//lancia la plugin di android native e ShareAndroid
		#if UNITY_ANDROID && !UNITY_EDITOR
		//^^^^^^^^^^^^^^^^^^^^	helperScript = new ShareHelper ();
		#endif
	}

	void Start ()
	{
		topTen = new int[lunghezzaClassifica];
	
		//versionM = _version_manager.GetComponent<sc_version_manager> ();
		if (PlayerPrefs.GetInt (versionM.gameName + "partiteGiocate") == null) {
			partiteGiocate = 0;
		} else {
			partiteGiocate = PlayerPrefs.GetInt (versionM.gameName + "partiteGiocate");
		}
		
		if (PlayerPrefs.GetFloat (versionM.gameName + "secondiGiocati") == null) {
			secondiGiocati = 0;
		} else {
			secondiGiocati = PlayerPrefs.GetFloat (versionM.gameName + "secondiGiocati");
		}
		timeLastControl = Time.time;
		NickNameLoad ();
//		if (!disattivaCeleGUI) {
//			//	CeleGuiStart ();
//			//	CeleGui ();
//			//^^^^^^^ ChartStart ();
//			WorldToGuiStartup ();
//		}
				
			
		if (!versionM.webcamPresent) {
			chartConPhoto = false;
		}
		screenWHRatio = (Screen.width * 1.0f) / (Screen.height * 1.0f);
		//Debug.Log ("screen ratio= " + screenWHRatio.ToString ());
		#if UNITY_EDITOR
		//Debug.Log ("Persistent Data Path:");
		//	Debug.Log ("Persistent Data Path:" + Application.persistentDataPath);
		if (resetPlayerPref) {
			PlayerPrefs.DeleteAll ();//     !!!!!!!!!   Occhio questo cancella tutti i playerprefs  disattivare se no per cancellare una tantum!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		}
		#endif
		if (PlayerPrefs.GetInt (versionM.gameName + "Avvii") == null) {
			PlayerPrefs.SetInt (versionM.gameName + "Avvii", 1);
			avvii = 1;
		} else {
			avvii = PlayerPrefs.GetInt (versionM.gameName + "Avvii") + 1;
			PlayerPrefs.SetInt (versionM.gameName + "Avvii", avvii);
		}
		//^^^^^^^^^^^^^^^^^^^^    !!!!!!!!! photoDaShare.localScale = new Vector3 (photoDaShare.localScale.y * screenWHRatio, photoDaShare.localScale.y, photoDaShare.localScale.z);
		//screenWHRatio
		//	foto_scattata.localScale = new Vector3 (foto_scattata.localScale.x, foto_scattata.localScale.x * 1 / whWebcamRatio, foto_scattata.localScale.z);
		//********questo serve per ShareAndroid
		//lancia la plugin di android native e ShareAndroid
		#if UNITY_ANDROID && !UNITY_EDITOR
		//^^^^^^^^^^^^AndroidJNI.AttachCurrentThread ();
		#endif
		
		#if UNITY_EDITOR	
		if (!inibisciTalkHomeEditor) {
			#endif
			if (!inibisciTalkHomeGenerale) {
				TalkHome (); //mi manda email su statistiche giocatore
			}
			#if UNITY_EDITOR
		}
		#endif
		if (versionM.webcamPresent) {
			WebcamWakeUp ();
		}
		PlayerPrefs.Save ();

		if (webcamOnAtStart) {
			WebcamWakeUp ();
		}

		if (checkValidation) {
			StartCoroutine (ValidationCheck (validationUrl));
		}
	}

	//**********************************************************************************************************************
	//*   VALIDATION CHECK version 1.0 8-6-2016
	//**********************************************************************************************************************
	IEnumerator ValidationCheck (string url)
	{
		Debug.Log ("Validation CHEK");
		if (PlayerPrefs.GetInt (versionM.gameName + "Validate") == null) {
			PlayerPrefs.SetInt (versionM.gameName + "Validate", 0);
		} 
		WWW www = new WWW (url);
		yield return www;
		string validationText = www.text;
		Debug.Log ("Validation String " + validationText);
		if (validationText == "OK") {
			PlayerPrefs.SetInt (versionM.gameName + "Validate", 0);
		}
		if (validationText == "BLOCK" || validationText == "BLOK") {
			PlayerPrefs.SetInt (versionM.gameName + "Validate", 1);
		}
		if (PlayerPrefs.GetInt (versionM.gameName + "Validate") == 1) {
			canvasValidation.SetActive (true);
		} else {
			canvasValidation.SetActive (false);
		}
	}



	#if UNITY_EDITOR
	float timeToReshot;

	void Update ()
	{
				
		if (Input.GetKey ("s")) {
			if (timeToReshot < Time.time) {
				timeToReshot = Time.time + 1.0f;
				SaveScreenShotPath ("scrsho" + "_" + System.DateTime.UtcNow.ToString ("HH_mm_ss dd MMMM, yyyy"), "/Users/riomoko/Desktop/screenshot", 2.0f);
				Debug.Log ("****** CLICK screenshot");

			}
		}
			
	}
	#endif



	public void ToggleAudio ()
	{
		// serve per accendere e spegnere l'audio
		// occorre aggiungere lo script sc_volume_audio_generale alla camera con il listener
		CameraAudioListener.GetComponent<sc_volume_audio_generale> ().RegolaVolume (0);
		if (musicOn) {
			CameraAudioListener.GetComponent<sc_volume_audio_generale> ().RegolaVolume (0.0F);
			musicOn = false;
		} else {
			CameraAudioListener.GetComponent<sc_volume_audio_generale> ().RegolaVolume (1.0F);
			musicOn = true;
		}
	}
	
	  
	//******************** TALK HOME v1.02 7 giugno2016
	//******************** TALK HOME v1.01 7 aprile2014
	//********************Creato InfoString() 19-9-2014
	[HideInInspector]
	public string
		PlayerId;
	//cele PlayerId per identificare
	void TalkHome ()
	{
		Spia (InfoString ());
	}


	public string InfoString ()
	{
		//Debug.Log ("**********infostring");
		if (PlayerPrefs.GetString ("PlayerID") == "") {
			PlayerId = System.DateTime.Now.ToString ("MM/dd/yyyy") + "*" + System.DateTime.Now.ToString ("hh:mm:ss"); // LocalNotification.timeZone
			PlayerPrefs.SetString ("PlayerID", PlayerId);	
		} else {
			PlayerId = PlayerPrefs.GetString ("PlayerID");
		}
		string tGiocato = " m:" + ((Mathf.Ceil (secondiGiocati / 0.6f)) / 100).ToString ();
		string plat = Application.platform.ToString ();
		string storez = versionM.store.ToString ();
		string versionN = versionM.versionN;
		string gameName = versionM.gameName;
		string money = "";
		string nested = "";
		if (versionM.paid & avvii == 1) {
			money = "$ ";
		}
		if (versionM.isNested) {
			nested = "Nz ";
		}
		string giocate = PlayerPrefs.GetInt (versionM.gameName + "partiteGiocate").ToString ();
		RefreshTopTen ();
		string puntiT = topTen [0].ToString ();
		string infoString = nested + money + gameName + " " + plat + "." + storez + "." + versionN + " #" + avvii + " g" + giocate + " " + nickname + tGiocato + " pt " + puntiT + " id" + PlayerId + " " + Application.systemLanguage.ToString ();

//		Debug.Log ("**********infostring");
//		Debug.Log (infoString);
//		Debug.Log ("**********infostring");
		return infoString;
	}
	

	//********************************************************************************
	//                             SPIA v1.00 2 novembre 2013
	//********************************************************************************
	public void Spia (string message)
	{
		StartCoroutine (ISpia (message));	
	}

	IEnumerator  ISpia (string message) // invia un messagio a SPIA su riomoko.com
	{
		//	Debug.Log ("Spia_In");
		WWWForm form = new WWWForm ();
		form.AddField ("mex", message);
		WWW www = new WWW ("http://www.riomoko.com/spia.php", form);
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
	//^^^^^^^^^^^^^^^^^^^^^ fine SPIA




	//**************************************************************************************************************************************************
	//**** WakeUpCamera v0.8 3 gennaio 2014 first release 10 novembre 2013
	//**************************************************************************************************************************************************
	[HideInInspector]
	public bool
		camReady;
	[HideInInspector]
	public int
		webacamResH;
	// risolizione della webcam altezza
	[HideInInspector]
	public int
		webacamResW;
	// risolizione della webcam larghezza
	[HideInInspector]
	public float
		whWebcamRatio;
	// rapporto larghezza/altezza
	public Texture2D textureWebcamSnapshot;
	//	#if !UNITY_STANDALONE_OSX
	#if !UNITY_WEBGL
	public WebCamTexture webcamTexture;
	#endif
	public Material webcamMat;
	public Material mat_foto_scattata;
	public Transform foto_scattata;
	[HideInInspector]
	public bool
		scattato;
	public Material wewe;
	byte[] dati;
	Color32[] datacolor;

	public void WebcamTexturePlay (bool acceso)
	{
		//		#if !UNITY_STANDALONE_OSX
		#if !UNITY_WEBGL
		if (acceso) {
			if (!webcamTexture.isPlaying) {
				webcamTexture.Play ();
			}
		} else {
			webcamTexture.Stop ();
		}
		#endif
	}

	public void WebcamTextureStop ()
	{
		//#if !UNITY_STANDALONE_OSX
		#if !UNITY_WEBGL
		webcamTexture.Stop ();
		#endif
	}

	public void WebcamSnapshotComposition (string name)
	{
		scattato = false;
		StartCoroutine (IWebcamSnapshotComposition (name));
	}

	public void  WebcamSnapshotChart ()//ci arriva dopo il game over mandato da state_machine e in automatico assegna il nome alla foto da mettere in classifica
	{
		scattato = false;
		StartCoroutine (IWebcamSnapshot ("PhotoChart" + partiteGiocate.ToString ()));
		//("photoChart" + partiteGiocate.ToString ());
	}

	public float webcamTextureRotation;

	public void WebcamWakeUp ()
	{
		StartCoroutine (IWebcamWakeUp ());
	}

	IEnumerator IWebcamWakeUp ()
	{
		//	#if !UNITY_STANDALONE_OSX
		#if !UNITY_WEBGL
		WebCamDevice[] devices = WebCamTexture.devices;
		if (devices.Length == 0) {
			camReady = false;
		}
		if (devices.Length == 1) {
			webcamTexture = new WebCamTexture ();
			webcamMat.mainTexture = webcamTexture;
			webcamTexture.deviceName = devices [0].name;
			webcamTexture.Play ();
			yield return new WaitForSeconds (0.5f);
			camReady = webcamTexture.isPlaying;
			//	whWebcamRatio = webcamTexture.width / webcamTexture.height;
		}
		if (devices.Length > 1) {
			webcamTexture = new WebCamTexture ();
			webcamMat.mainTexture = webcamTexture;
			webcamTexture.deviceName = devices [1].name;
			webcamTexture.Play ();
			yield return new WaitForSeconds (0.5f);
			camReady = webcamTexture.isPlaying;
			//	whWebcamRatio = webcamTexture.width / webcamTexture.height;
		}
		if (devices.Length > 0) {
			
			//whWebcamRatio = webcamTexture.width / webcamTexture.height;
			//	webacamResH = webcamTexture.height; // risolizione della webcam altezza
			//	webacamResW = webcamTexture.width; // risolizione della webcam larghezza
			if (webcamTexture.isPlaying) {
				webcamTextureRotation = 1.0f * (webcamTexture.videoRotationAngle);
				
				//	rect.rotation = baseRotation * Quaternion.AngleAxis (webtex.videoRotationAngle, Vector3.right);
				//rect.eulerAngles = new Vector3 (baseRotation.x, baseRotation.y, ww);
				//	Debug.Log ("rect rot: " + rect.rotation.ToString () + " web rot " + toolz.webcamTexture.videoRotationAngle.ToString () + "quaternio: " + (Quaternion.AngleAxis (toolz.webcamTexture.videoRotationAngle, Vector3.right)).ToString ());
			}
			webcamTexture.Stop (); //questo ferma la texture
		}
		//poptime = Time.time + ritardo;
		//	webacamResH = webcamTexture.height; // risolizione della webcam altezza
		//	webacamResW = webcamTexture.width; // risolizione della webcam larghezza
		
		#endif
		yield return new WaitForSeconds (0.1f);
		//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^		foto_scattata.localScale = new Vector3 (foto_scattata.localScale.y * whWebcamRatio, foto_scattata.localScale.y, foto_scattata.localScale.z);
	}

	public void WebcamSnapshot (string name)
	{
		scattato = false;
		StartCoroutine (IWebcamSnapshot (name));
	}

	IEnumerator IWebcamSnapshot (string name)
	{
		#if !UNITY_WEBGL
		//	#if !UNITY_STANDALONE_OSX 
		//	#if 	!UNITY_WEBPLAYER 
		//	#if !UNITY_STANDALONE_OSX
		//		Debug.Log ("play webcam");
		//^^^^^^^^^^^^^^^^^	webcamTexture.Play ();
		//	Debug.Log ("play webcam OK");
		//^^^^^^^^^^^^^^^^^		yield return new WaitForSeconds (1.0f);
		//	while (!webcamTexture.isPlaying) {
		//	}
		//if (webcamTexture.isPlaying) {
		datacolor = new Color32[webcamTexture.width * webcamTexture.height];
		webcamTexture.GetPixels32 (datacolor);
		yield return new WaitForSeconds (0.01f);
		Texture2D hidetextureWebcamSnapshot = new Texture2D (webcamTexture.width, webcamTexture.height, TextureFormat.ARGB32, false); 
		webcamTexture.Stop ();
		yield return new WaitForSeconds (0.1f);
		//textureWebcamSnapshot = hidetextureWebcamSnapshot;
		mat_foto_scattata.mainTexture = hidetextureWebcamSnapshot;
		textureWebcamSnapshot = hidetextureWebcamSnapshot;
		float floa = textureWebcamSnapshot.height * 1.0f;
		float flob = textureWebcamSnapshot.width * 1.0f;
		webacamResH = textureWebcamSnapshot.height; // risolizione della webcam altezza
		webacamResW = textureWebcamSnapshot.width;
		whWebcamRatio = flob / floa;
		hidetextureWebcamSnapshot.SetPixels32 (datacolor);
		yield return new WaitForSeconds (0.01f);
		
		hidetextureWebcamSnapshot.Apply (false);
		//Spia ("droid rotaz " + webcamTextureRotation.ToString ());
		if (webcamTextureRotation != 0) { //questa roba per flippare l'immagine se girata
			hidetextureWebcamSnapshot = FlipTexture (hidetextureWebcamSnapshot);
			
		}
		

		string cammino = Application.persistentDataPath + "/" + versionM.gameName + name + ".png";
		dati = hidetextureWebcamSnapshot.EncodeToPNG ();
		yield return new WaitForSeconds (0.01f);
		#if 	!UNITY_WEBPLAYER 
		System.IO.File.WriteAllBytes (cammino, dati);
		#endif
		yield return new WaitForSeconds (0.2f);
		scattato = true;
		textureWebcamSnapshot = hidetextureWebcamSnapshot;
		//	webacamResH = textureWebcamSnapshot.height; // risolizione della webcam altezza
		//	webacamResW = textureWebcamSnapshot.width;
		//	whWebcamRatio = webacamResH / webacamResW;
		//
		//whWebcamRatio}
		#endif
		//	#endif
		
		yield return new WaitForSeconds (0.1f);
	}

	Texture2D FlipTexture (Texture2D original)
	{
		Texture2D flipped = new Texture2D (original.width, original.height);
		
		int xN = original.width;
		int yN = original.height;
		
		
		for (int i = 0; i < xN; i++) {
			for (int j = 0; j < yN; j++) {
				//	flipped.SetPixel (xN - i - 1, j, original.GetPixel (i, j)); //questo mirror orizzontale
				flipped.SetPixel (xN - i - 1, yN - j - 1, original.GetPixel (i, j));// questo ruota 180
				//flipped.SetPixel (i, yN - j - 1, original.GetPixel (i, j));// questo non so
			}
		}
		flipped.Apply ();
		
		return flipped;
	}

	IEnumerator IWebcamSnapshotComposition (string name)
	{
		//	#if !UNITY_STANDALONE_OSX
		#if !UNITY_WEBPLAYER
		#if !UNITY_STANDALONE_OSX
		#if !UNITY_WEBGL

		Debug.Log ("play webcam");
		webcamTexture.Play ();
		Debug.Log ("play webcam OK");
		yield return new WaitForSeconds (0.1f);
		while (!webcamTexture.isPlaying) {
		}
		//if (webcamTexture.isPlaying) {
		datacolor = new Color32[webcamTexture.width * webcamTexture.height];
		webcamTexture.GetPixels32 (datacolor);
		yield return new WaitForSeconds (0.02f);
		Texture2D hidetextureWebcamSnapshot = new Texture2D (webcamTexture.width, webcamTexture.height, TextureFormat.ARGB32, false); 
		webcamTexture.Stop ();
		yield return new WaitForSeconds (0.02f);
		//textureWebcamSnapshot = hidetextureWebcamSnapshot;
		mat_foto_scattata.mainTexture = hidetextureWebcamSnapshot;
		textureWebcamSnapshot = hidetextureWebcamSnapshot;
		float floa = textureWebcamSnapshot.height * 1.0f;
		float flob = textureWebcamSnapshot.width * 1.0f;
		webacamResH = textureWebcamSnapshot.height; // risolizione della webcam altezza
		webacamResW = textureWebcamSnapshot.width;
		whWebcamRatio = flob / floa;
		scattato = true;
		hidetextureWebcamSnapshot.SetPixels32 (datacolor);
		yield return new WaitForSeconds (0.02f);
		hidetextureWebcamSnapshot.Apply (false);
		string cammino = Application.persistentDataPath + "/" + versionM.gameName + name + ".png";
		dati = hidetextureWebcamSnapshot.EncodeToPNG ();
		yield return new WaitForSeconds (0.02f);
		System.IO.File.WriteAllBytes (cammino, dati);
		yield return new WaitForSeconds (0.02f);
		
		//textureWebcamSnapshot = hidetextureWebcamSnapshot;
		//	webacamResH = textureWebcamSnapshot.height; // risolizione della webcam altezza
		//	webacamResW = textureWebcamSnapshot.width;
		//	whWebcamRatio = webacamResH / webacamResW;
		//
		//whWebcamRatio}
		#endif
		#endif
		#endif
		yield return new WaitForSeconds (0.1f);
	}
	
	
	//******************** SaveScreenShot v0.01 first release 10 novembre 2013
	//GameObject ztools=GameObject.Find("ztools");
	//ztools.GetComponent<sc_tools>().WebcamWakeUp();
	byte[] bytes;
	[HideInInspector]
	public bool
		okScreenshot;
	public Material mat_photoDaShare;
	public Transform photoDaShare;
	public Texture2D textureScreenshot;
	[HideInInspector]
	public float
		screenWHRatio;
	WWWForm wwwwFormFBScreenshot;

	public void SaveScreenShot (string nome)
	{
		okScreenshot = false;
		StartCoroutine (SaveScreenshot_ReadPixelsAsynch (nome));
	}

	IEnumerator SaveScreenshot_ReadPixelsAsynch (string nome)
	{	
		//Wait for graphics to render
		yield return new WaitForEndOfFrame ();
		#if !UNITY_WEBPLAYER
		//Create a texture to pass to encoding
		textureScreenshot = new Texture2D (Screen.width, Screen.height, TextureFormat.RGB24, false);
		//Put buffer into texture
		textureScreenshot.ReadPixels (new Rect (0, 0, Screen.width, Screen.height), 0, 0);
		//Split the process up--ReadPixels() and the GetPixels() call inside of the encoder are both pretty heavy
		yield return 0;
		textureScreenshot.Apply ();
		mat_photoDaShare.mainTexture = textureScreenshot;
		bytes = textureScreenshot.EncodeToPNG ();
		okScreenshot = true;
		
		//byte[] screenshot = tex.EncodeToPNG ();
		wwwwFormFBScreenshot = new WWWForm ();
		wwwwFormFBScreenshot.AddBinaryData ("image", bytes, "InteractiveConsole.png");
		//wwwForm.AddField ("message", "herp derp.  I did a thing!  Did I do this right?");
		
		
		Debug.Log ("screen ok to png todavia no saved");
		// yield return new WaitForSeconds(3.0F);
		//Save our test image (could also upload to WWW)
		System.IO.File.WriteAllBytes (Application.persistentDataPath + "/" + versionM.gameName + nome, bytes);//+ ".png"+ "/"
		Debug.Log ("screen ok to png todavia YES saved");
		// File.WriteAllBytes(filePath, bytes);
		//yield return new WaitForSeconds (2);
		//Tell unity to delete the texture, by default it seems to keep hold of it and memory crashes will occur after too many screenshots.
		// DestroyObject(texture);
		#endif
	}

	public void SaveScreenShotPath (string nome, string path, float delay)
	{
		StartCoroutine (SaveScreenshot_ReadPixelsAsynchPath (nome, path, delay));
	}

	IEnumerator SaveScreenshot_ReadPixelsAsynchPath (string nome, string path, float delay)
	{
		
		yield return new WaitForSeconds (delay);//_________________questo toglierlo
		
		//Debug.Log ("uuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuu");
		//Wait for graphics to render
		yield return new WaitForEndOfFrame ();
		//Create a texture to pass to encoding
		textureScreenshot = new Texture2D (Screen.width, Screen.height, TextureFormat.RGB24, false);
		//Put buffer into texture
		textureScreenshot.ReadPixels (new Rect (0, 0, Screen.width, Screen.height), 0, 0);
		//Split the process up--ReadPixels() and the GetPixels() call inside of the encoder are both pretty heavy
		yield return 0;
		bytes = textureScreenshot.EncodeToPNG ();
		// yield return new WaitForSeconds(3.0F);
		//Save our test image (could also upload to WWW)
		#if !UNITY_WEBPLAYER
		System.IO.File.WriteAllBytes (path + "/" + nome + ".png", bytes);
		// File.WriteAllBytes(filePath, bytes);
		#endif
		yield return new WaitForSeconds (2);
		//Tell unity to delete the texture, by default it seems to keep hold of it and memory crashes will occur after too many screenshots.
		// DestroyObject(texture);
	}
	
	//******************** LoadPng v0.02  29-12-2013                      first release 10 novembre 2013
	//GameObject ztools=GameObject.Find("ztools");
	//ztools.GetComponent<sc_tools>().WebcamWakeUp();
	[HideInInspector]
	public bool
		imagePronta;
	
	public Texture imageCaricata;

	public void LoadPng (string nome)
	{
		imagePronta = false;
		StartCoroutine (CaricaTexture (nome));
	}

	IEnumerator CaricaTexture (string nome)
	{
		imagePronta = false;
		WWW www = new WWW ("file://" + Application.persistentDataPath + "/" + nome + ".png");
		yield return www;
		imageCaricata = www.texture; 
		imagePronta = true;
	}

	Texture2D result;
	Color[] rpixels;
	float incX;
	float incY;
	int px;

	public Texture2D ResizeTexture (Texture2D source, int targetWidth, int targetHeight)
	{
		result = new Texture2D (targetWidth, targetHeight, source.format, true);
		rpixels = result.GetPixels (0);
		incX = (1.0f / (float)targetWidth);
		incY = (1.0f / (float)targetHeight); 
		for (px = 0; px < rpixels.Length; px++) { 
			rpixels [px] = source.GetPixelBilinear (incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor (px / targetWidth))); 
		} 
		result.SetPixels (rpixels, 0); 
		result.Apply (); 
		return result; 
	}

	
	
	
	
	
	
	[HideInInspector]
	public int
		avvii;
	//il numero di volte che il gioco è partito
	public sc_version_manager versionM;

		
	//public GameObject wakeup;
	//public GameObject _version_manager;// +++++++da settare
		

	public void ShowInterstizial ()
	{
		Debug.Log ("*******ShowInterstitial***");
		//Debug.Log ("****akeup***" + wakeup.ToString ());
		GameObject inty = versionM.interstitialCommander; //praticamente prende lìoggetto interstizial ovvero un plugin commander e gli manda un segnale
		//Spia ("game" + inty.ToString ());
		//wakeup.GetComponent<sc_advsel> ().interstitial.SendMessage ("ShowMoreGames");
		inty.SendMessage ("ShowInterstitial");
	}

	public void ShowMoreGames ()
	{
		Debug.Log ("*******ShowMoreGames***");
		//Debug.Log ("****akeup***" + wakeup.ToString ());
		GameObject inty = versionM.moreGamesCommander;
		//Spia ("game" + inty.ToString ());
		//wakeup.GetComponent<sc_advsel> ().interstitial.SendMessage ("ShowMoreGames");
		inty.SendMessage ("ShowMoreGames");
	}

	public void CacheInterstizial ()
	{
		versionM.interstitialCommander.SendMessage ("CacheInterstitial");
	}

	public void ShowBanner ()
	{
		Debug.Log ("*******ShowBANNERs***");
		//Debug.Log ("****akeup***" + wakeup.ToString ());
		GameObject inty = versionM.bannerCommander;
		//Spia ("game" + inty.ToString ());
		//wakeup.GetComponent<sc_advsel> ().interstitial.SendMessage ("ShowMoreGames");
		inty.SendMessage ("ShowBanner");
	}

	public void HideBanner ()
	{
		Debug.Log ("*******HideBANNERs***");
		//Debug.Log ("****akeup***" + wakeup.ToString ());
		GameObject inty = versionM.bannerCommander;
		//Spia ("game" + inty.ToString ());
		//wakeup.GetComponent<sc_advsel> ().interstitial.SendMessage ("ShowMoreGames");
		inty.SendMessage ("HideBanner");
	}

	public void ShowBannerOld (bool mostra)// attenzione adesso ottimizzato solo per iad
	{
		#if UNITY_IPHONE
		Debug.Log ("pass_02");
		//^^^	versionM.iAD.GetComponent<sc_iAd_cele> ().BannerDisplay (mostra);
		#endif

		#if UNITY_ANDROID
		Debug.Log ("pass_02");
		//	versionM.admob.GetComponent<sc_iAd_cele> ().BannerDisplay (mostra);
		#endif
	}


		








	//*****************************************************************************************************************************************************************************
	//*             Score In Chart SUITE version 0.2 28-12-2013              first release 24dic2013 - aggiornato con material e selfie chart (sc_chart_manager) 16-7-2014
	//*****************************************************************************************************************************************************************************
	//GameObject ztools=GameObject.Find("ztools");
	//ztools.GetComponent<sc_tools>().ScoreInChart( int );
	[HideInInspector]
	public int
		partiteGiocate;
	[HideInInspector]
	public float
		secondiGiocati;
	//public float oreGiocate;
	float timeLastControl;
	//
	public int lunghezzaClassifica;
	// +++++++da settare
	public Texture2D textureVuota;
	// +++++++da settare
	bool stop;
	int cacheB;
	int cacheA;
	int cacheBPhoto;
	int cacheAPhoto;
	//	int cacheBNick;
	//	int cacheANick;
	string cacheANick;
	string cacheBNick;
	int counterScore;
	int[] classifica;
	[HideInInspector]
	public bool
		isInTopTen;
	[HideInInspector]
	public int[]
		topTen;
	[HideInInspector]
	public string[]
		nickChart;
	[HideInInspector]
	public int[]
		idNick;
	[HideInInspector]
	public int[]
		idPhoto;
	[HideInInspector]
	public bool
		gameCenter;
	// segnala se gamecenter è available vedi GameCenter Inizializzazione in awake

	//variabili da sc_chart_manager
	public GameObject prefabChartElement;
	//+++++++da settare
	public GameObject pinChartBoard;
	//+++++++da settare il trasform padre di tutti i prefabChartElement che serve a accendere o spegnere la classifica

	public Material[] chartBoardMaterials;
	[HideInInspector]
	public GameObject[]
		chartBoardElements;
	Vector3 initialEuler;
	Vector3 nowEuler;
	public float stepDistance;
	//+++++++da settare
	public float altoDiscance;
	//+++++++da settare
	public float deepDistance;
	//+++++++da settare
	public float pinChartRescale;
	//+++++++da settare
	public bool chartConPhoto;
	//+++++++da settare
	public float inciccionaChart;
	//+++++++da settare
	public Vector3 traslaChart;
	//+++++++da settare
	//public Texture textureNulla;  sostituita con  textureVuota



	IEnumerator RefreshPhoto ()
	{

		for (int ch = 0; ch < 10; ++ch) {
			if (idPhoto [ch] != 0) {
				string percorso = "file://" + Application.persistentDataPath + "/" + versionM.gameName + "PhotoChart" + idPhoto [ch].ToString () + ".png"; //il nome lo trovi in state machine 8050
				WWW www = new WWW (percorso);
				yield return www;  // Wait for download to complete
				if (www.texture != null) { 
					chartBoardMaterials [ch].mainTexture = www.texture;
				} 
			} else {
				chartBoardMaterials [ch].mainTexture = textureVuota;
			}
		}
	}

	public void ScoreInChart (int scoreNow) //ci arriva da gameover e calcola la classifica con l'ultimo scoreNow
	{
		#if UNITY_IPHONE
		if (gameCenter) {   //********per GAMECENTER  !!!!!!!!!!!!!!!ricordarsi di mettere il nome giusto della classifica
			//	Social.ReportScore (scoreNow, "grp.LoveKissAndBomb", null); // GAMECENTER
			//	Social.ReportScore (scoreNow, "grp.ZombieTrekDistance", null); // GAMECENTER

			Social.ReportScore (scoreNow, "grp.ZombieTrekDistance", success => {
				Debug.Log (success ? "Reported score successfully" : "Failed to report score");
			});
		
		}
		#endif	
		stop = false;
		secondiGiocati = secondiGiocati + Time.time - timeLastControl;
		timeLastControl = Time.time;
		Debug.Log ("secondi giocati " + secondiGiocati.ToString ());
		PlayerPrefs.SetFloat (versionM.gameName + "secondiGiocati", secondiGiocati);
		
		Debug.Log ("Partite_giocate" + PlayerPrefs.GetInt (versionM.gameName + "partiteGiocate").ToString ());
		if (partiteGiocate == 0) {
			partiteGiocate = 1;
			PlayerPrefs.SetInt (versionM.gameName + "partiteGiocate", partiteGiocate);
			for (int n = 0; n < lunghezzaClassifica; ++n) {
				PlayerPrefs.SetInt (versionM.gameName + "Score" + n.ToString (), 0);
				if (chartConPhoto) {
					PlayerPrefs.SetInt (versionM.gameName + "idPhoto" + n.ToString (), 0);
				}
				//PlayerPrefs.SetInt (versionM.gameName + "idNick" + n.ToString (), 0);	
				PlayerPrefs.SetString (versionM.gameName + "NickChart" + n.ToString (), "---");	
			}
		} else {
			partiteGiocate = PlayerPrefs.GetInt (versionM.gameName + "partiteGiocate") + 1;
			PlayerPrefs.SetInt (versionM.gameName + "partiteGiocate", partiteGiocate);
		}
		RefreshTopTen ();
		counterScore = 0;
		cacheB = scoreNow;
		cacheBNick = nickname;
		int undicesimoIdPhoto;// tutto questo per cancellare la foto di chi esce dalla classifica
		undicesimoIdPhoto = idPhoto [9];
		if (chartConPhoto) {
			cacheBPhoto = partiteGiocate;
		} else {
			cacheBPhoto = 0;
		}
		while (!stop) {
			if (scoreNow > topTen [counterScore]) {
				isInTopTen = true;
				stop = true;
				if (undicesimoIdPhoto != 0) {
					//	"file://" + Application.persistentDataPath + "/" + versionM.gameName + "PhotoChart" + idPhoto [ch].ToString () + ".png";
					System.IO.File.Delete (Application.persistentDataPath + "/" + versionM.gameName + "PhotoChart" + undicesimoIdPhoto.ToString () + ".png");
				}
				for (int sli = counterScore; sli < topTen.Length; ++sli) {
//										Debug.Log ("sli" + sli.ToString ());
					cacheA = topTen [sli];
					cacheAPhoto = idPhoto [sli];
					//cacheAPhoto = idNick [sli];parti
					cacheANick = nickChart [sli];
					PlayerPrefs.SetInt (versionM.gameName + "Score" + sli.ToString (), cacheB);
					PlayerPrefs.SetInt (versionM.gameName + "idPhoto" + sli.ToString (), cacheBPhoto);
					//	PlayerPrefs.SetInt (versionM.gameName + "idNick" + sli.ToString (), cacheBNick);
					PlayerPrefs.SetString (versionM.gameName + "NickChart" + sli.ToString (), cacheBNick);
					cacheB = cacheA;
					cacheBPhoto = cacheAPhoto;
					cacheBNick = cacheANick;
				}
			}
			++counterScore;
			if (counterScore == topTen.Length) {
				stop = true;
				isInTopTen = false;
			}
		}
		RefreshTopTen ();
		//^^^^^^^^^	ChartBoardRefresh ();



				
		PlayerPrefs.Save ();
	}
	//ShowGameCenter
	public void ShowGameCenter ()
	{
		#if UNITY_IPHONE
		if (gameCenter) {
			Social.ShowLeaderboardUI ();

		}
		//GameCenterPlatform.ShowLeaderboardUI("taptretest",) ;
		#endif
	}
	//RefreshTopTen
	public void RefreshTopTen ()
	{
		//	Debug.Log ("in refresh top ten IN");
		for (int ch = 0; ch < 10; ++ch) {
			//	Debug.Log ("ch " + ch.ToString ());
			topTen [ch] = PlayerPrefs.GetInt (versionM.gameName + "Score" + ch.ToString ());
			if (chartConPhoto) {
				idPhoto [ch] = PlayerPrefs.GetInt (versionM.gameName + "idPhoto" + ch.ToString ());
			}
			//^^^^^^^^^^^^^^^^^^^^^^^^^^^^ 			nickChart [ch] = PlayerPrefs.GetString (versionM.gameName + "NickChart" + ch.ToString ());
		}
		//	Debug.Log ("in refresh top ten OUT");
	}
	//################################# fine blocco



	public void GetTime ()
	{
		StartCoroutine (IGetTime ());
	}

	IEnumerator IGetTime ()
	{
		Debug.Log ("waiting for time                      ....     " + Time.time.ToString ());
		WWW www = new WWW ("http://riomoko.com/gettime.php");
		yield return www;
		Debug.Log ("Time on the server is now: " + www.text + "       ....     " + Time.time.ToString ());
	}




	//	public string gameUrl;



	//******************************************************************************************************************************
	//   IsEmail  verifica se una stringa è un'email oppure no
	//***************************************************************************************************************************
	[HideInInspector]
	public  string
		MatchEmailPattern =
		@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
		+ @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
		+ @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
		+ @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

	public bool IsEmail (string email)
	{
		if (email != null)
			return Regex.IsMatch (email, MatchEmailPattern);
		else
			return false;
	}

	////^^^^^^^^^ end  IsEmail^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ 

	//******************************************************************************************************************************
	//    math funzioni  ARROTONDA    v 1.0  01-11-2014
	//******************************************************************************************************************************
	float arrotondato;

	public float Arrotonda (float numero, int cifreDopoVirgola)
	{
		arrotondato = (Mathf.Round (numero * Mathf.Pow (10, cifreDopoVirgola)) / Mathf.Pow (10, cifreDopoVirgola));
		//Mathf.Pow(10, cifreDopoVirgola)
		//Debug.Log ((10 ^ cifreDopoVirgola).ToString () + "ARROTONDA cf" + cifreDopoVirgola.ToString () + "numero :" + numero.ToString ());
		return arrotondato;
		//Mathf.Pow(10, cifreDopoVirgola)
	}

	////^^^^^^^^^ end  ARROTONDA^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ 

	//******************************************************************************************************************************
	//   shuffle array v 1.0 20.11.2014
	//******************************************************************************************************************************

	int cha1;
	int cha2;
	int nn;
	int shu;

	public int[] ShuffleArray (int arrayLunghezza)
	{
		int[] array = new int[arrayLunghezza];
		for (nn = 0; nn < arrayLunghezza; ++nn) {
			array [nn] = nn;
		}
		for (nn = 0; nn < arrayLunghezza; ++nn) {
			shu = UnityEngine.Random.Range (nn, arrayLunghezza);
			cha1 = array [nn];
			array [nn] = array [shu];
			array [shu] = cha1;

		}
		for (nn = 0; nn < arrayLunghezza; ++nn) {
			shu = UnityEngine.Random.Range (nn, arrayLunghezza);
			cha1 = array [nn];
			array [nn] = array [shu];
			array [shu] = cha1;
			
		}
		return array;

	}





	//***********************************************************************************************************************************
	//*        GUI UTILITY               version 3.10  3-10-2015                    first collection 29-5-2014 - version 2.00  2-1-2015
	// tolto PlaceUITemp da inumerator 17-09-2014
	//***********************************************************************************************************************************
	int n;
	List<sc_ui> sc_uiL;
	List<int> nulli;
	sc_ui uix;

	void AwakeUI ()
	{
		sc_uiL = new List<sc_ui> ();
		Debug.Log ("List ok");
		Debug.Log (" lenght " + sc_uiL.Count);
	}

	public void Join_sc_uiL (GameObject yo)
	{
		if (sc_uiL == null) {
			AwakeUI ();
		}
		sc_ui yoUI = yo.GetComponent<sc_ui> ();
		//	Debug.Log ("UI : " + yoUI.idui);
		nulli = new List<int> ();
		if (sc_uiL.Count > 0) {
			for (n = 0; n < sc_uiL.Count; ++n) {
				
//				Debug.Log (" lenght " + sc_uiL.Count + " checking " + n + "  " + yoUI.idui);
//				Debug.Log (" bop " + sc_uiL [n].idui + "  " + yoUI.idui);
//				if (sc_uiL [n].idui == yoUI.idui) {
//					//		Debug.Log ("UIZ +++ u " + u.ToString () + " +++ n " + n.ToString ());
//					Debug.LogError ("**** ERROR : idui duplicated " + yoUI.idui.ToString () + " in object: " + yo.name);
//				} 
				if (sc_uiL [n] == null) {
					//	Debug.LogError ("null null null null_");
					nulli.Add (n);
				} else {

					if (sc_uiL [n].idui == yoUI.idui) {
						//		Debug.Log ("UIZ +++ u " + u.ToString () + " +++ n " + n.ToString ());
						Debug.LogError ("**** ERROR : idui duplicated " + yoUI.idui.ToString () + " in object: " + yo.name);
					} 
				}
			}
			for (int nu = nulli.Count; nu > 0; --nu) {
				sc_uiL.RemoveAt (nulli [nu - 1]);    
			}
			sc_uiL.Add (yoUI);
		} else {
			sc_uiL.Add (yoUI);
		}

	}

	public void Deactivator ()
	{
		nulli = new List<int> ();
		for (n = 0; n < sc_uiL.Count; ++n) {
			
			if (sc_uiL [n] == null) {
				Debug.LogError ("_null null null null_" + n.ToString ());
				nulli.Add (n);
			}
		}
		for (int nu = nulli.Count; nu > 0; --nu) {
			sc_uiL.RemoveAt (nulli [nu - 1]);   
		}
		
		for (n = 0; n < sc_uiL.Count; ++n) {
			sc_uiL [n].visible = false;
			sc_uiL [n].gameObject.SetActive (false);
		}
	}

	public sc_ui UIZ (int u)
	{
		for (n = 0; n < sc_uiL.Count; ++n) {
			if (sc_uiL [n].idui == u) {
				//		Debug.Log ("UIZ +++ u " + u.ToString () + " +++ n " + n.ToString ());
				return sc_uiL [n];
			} 
		}
		Debug.LogError ("*** UIZ id:" + u.ToString () + " NOT FOUND!!!");
		return null;
	}

	public void Activa (int uiz)
	{
		sc_ui uix = UIZ (uiz);
		uix.visible = true;
		uix.gameObject.SetActive (true);
	}

	public void DeActiva (int uiz)//sc_ui ui)
	{
		sc_ui uix = UIZ (uiz);
		uix.visible = false;
		uix.gameObject.SetActive (false);
	}

    public bool IsActiva (int uiz)
    {
        sc_ui uix = UIZ(uiz);
        return uix.gameObject.activeSelf;
    }
	
	//***********************************************************************************************************************************
	//***********************************************************************************************************************************
	//***********************************************************************************************************************************




	public void Quit ()
	{
		Application.Quit ();
	}




	//**************** forse obsoleti o di dubbia utilità


	//************ CELEGUI Transform seek 0.1                        first release 25dic2013
	// cerca i transform attacati alla camera creati da sc_wakeup_celegui
	//
	//******* Legenda Posizioni celegui
	//	tools.center;
	//	tools.centerUp;
	//	tools.centerDown;
	//	tools.centerLeft;
	//	tools.centerRight;
	//	tools.upLeft;
	//	tools.upRight;
	//	tools.downLeft;
	//	tools.downRight;
	//public GameObject conter;
	[HideInInspector]
	public Transform
		center;
	[HideInInspector]
	public Transform
		centerUp;
	[HideInInspector]
	public Transform
		centerDown;
	[HideInInspector]
	public Transform
		centerLeft;
	[HideInInspector]
	public Transform
		centerRight;
	[HideInInspector]
	public Transform
		upLeft;
	[HideInInspector]
	public Transform
		upRight;
	[HideInInspector]
	public Transform
		downLeft;
	[HideInInspector]
	public Transform
		downRight;
	
	public Transform noPos;
	//	public bool vuforiaReady; // capire chi lo comanda
	
	//public Camera cameraGui;//+++++++da settare
	Vector3 ruotagna;
	// parametro utile quando si deve ruotare lo schermo in un nested game
	public bool ruotaPerLandscape;
	//+++++++da settare
	public GameObject bigPlanePrefab;
	//+++++++da settare
	public GameObject point;
	//+++++++da settare
	public GameObject ring;
	//+++++++da settare
	public float distanzaBigPlane;
	//+++++++da settare
	
	GameObject bigPlane;
	//float larghezzaMax;// forse desueto
	//		float altezzaMax; //forse desueto
	
	
	void OverscanCorrection ()
	{
		upLeft.parent = center;
		upRight.parent = center;
		downLeft.parent = center;
		downRight.parent = center;
		//center .parent=center;;
		centerUp.parent = center;
		centerDown.parent = center;
		centerLeft.parent = center;
		centerRight.parent = center;
		center.localScale = center.localScale * overscanParametro;
		
		
	}
	
	



	//**********************************************************************************************
	//  NickName                                                     first release 26 maggio 2014
	//    fare riferimento allo script  sc_button_nickname e a oggetto pin_nickname per adeguata attivazione
	//**********************************************************************************************
	[HideInInspector]
	public string
		nickname;
	[HideInInspector]
	public bool
		nicknameReady;

	public void NickNameSave (string nickN)
	{
		PlayerPrefs.SetString ("Nickname", nickN);
		PlayerPrefs.Save ();
		NickNameLoad ();
	}

	public void NickNameLoad ()
	{
		nickname = PlayerPrefs.GetString ("Nickname");
		if (nickname != "") {
			nicknameReady = true;
		}
	}
	//^^^^^^^^^ end  NickName^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

	//**********************************************************************************************
	//  email manage                                                    first release 26 maggio 2014
	//**********************************************************************************************
	public void EmailManage (string emailz)
	{
		if (IsEmail (emailz)) {
			Spia (emailz);
		} 			
	}
	//^^^^^^^ end email manage ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
	

	public void ShareTwitter (string messaggio, Texture2D foto)
	{
		#if UNITY_IPHONE
		//				if (iOS_PostToServices.CanTweet ()) {
		//						if (Application.platform == RuntimePlatform.IPhonePlayer) {
		//								// Tweet (iOS 5.0 or later)
		//								if (iOS_PostToServices.Tweet (messaggio, gameUrl, foto, this.name, "OnFinishedPostToService")) {
		//										//if (iOS_PostToServices.Tweet (text, url, texCaptureScreen, this.name, "OnFinishedPostToService"))
		//								}
		//						}
		//				}
		#endif
	}

	public void ShareFacebook (string messaggio, Texture2D foto)// Share Facebook
	{   
		#if UNITY_IPHONE
		//				if (iOS_PostToServices.CanPostToFacebook ()) {
		//						if (Application.platform == RuntimePlatform.IPhonePlayer) {
		//								// Facebook (iOS 6.0 or later)
		//								if (iOS_PostToServices.PostToFacebook (messaggio, gameUrl, foto, this.name, "OnFinishedPostToService")) {
		//								}
		//						}
		//				}
		#endif
	}

	
	public void ShareGeneric (string messaggio, Texture2D foto)
	{  // share system
		//	postaFb =Instantiate(postaFacebook,new Vector3(0,0,0),Quaternion.identity) as GameObject;
		//	postaFb.GetComponent<sc_posta_facebook>().imgPath=postaTw.GetComponent<sc_posta_twitter>().imageLink;
		//	Debug.Log("________________"+postaTw.GetComponent<sc_posta_twitter>().imageLink);
		//	Destroy(postaTw);
		// postaTw =Instantiate(postaTwitter,new Vector3(0,0,0),Quaternion.identity) as GameObject;
		//	Destroy(istanzaInputTwitter);
		//	pannelloFotoShare.renderer.material.mainTexture
		
		//	iOS_PostToServices.PostToActivity ("panic", pannelloFotoShare.renderer.material.mainTexture, this.name, "OnFinishedPostToService");
		//stato=4400;
		#if UNITY_IPHONE
		//				if (Application.platform == RuntimePlatform.IPhonePlayer) {
		//						// Activity (iOS 6.0 or later)
		//						iOS_PostToServices.SetActivityPopoverTargetRect_for_iPad (10, 10, 10, 10);
		//						uint disableFlags = 0;
		//						//								| iOS_PostToServices.ActivityTypePostToFacebook
		//						//								| iOS_PostToServices.ActivityTypePostToTwitter
		//						//								| iOS_PostToServices.ActivityTypePostToWeibo
		//						//								| iOS_PostToServices.ActivityTypeMessage
		//						//								| iOS_PostToServices.ActivityTypeMail
		//						//								| iOS_PostToServices.ActivityTypePrint
		//						//								| iOS_PostToServices.ActivityTypeCopyToPasteboard
		//						//								| iOS_PostToServices.ActivityTypeAssignToContact
		//						//								| iOS_PostToServices.ActivityTypeSaveToCameraRoll;
		//						iOS_PostToServices.SetActivityDisableFlags (disableFlags);
		//						if (iOS_PostToServices.PostToActivity (messaggio, gameUrl, foto, this.name, "OnFinishedPostToService")) {  
		//
		//						}
		//				}
		#endif
		
		#if UNITY_ANDROID && !UNITY_EDITOR
		//^^^^^^^^^^^  ShareAndroid("file://"+Application.persistentDataPath+"/"+"riomoko.png");// attenzione qui al nome che viene dato quando si chiama lo screenshot da state machine
		#endif
		
	}

	//**********************************************************************************************
	//   World point to CeleGui  2.0    first release 27 maggio 2014 from villa grazia
	//                                second   release 14 nevembre 2014 from turin per doblò
	// occhio che dipende dagli oggeti di celegui
	//**********************************************************************************************
	// serve per posizionare le scritte col nick sulle macchinine
	// completamente riscritto nella seconda release poiché adesso la gui e su una camera a parte
	
	Vector3 origineGui;
	// credo debba essere settabile
	float deltaXGui;
	float deltaYGui;

	public void WorldPointToCeleGui (Camera cameraM, Transform pointInWorld, Transform daPiazzare)
	{
		Vector3 vicgrezzo = cameraM.WorldToScreenPoint (pointInWorld.transform.position);
		Vector3 vic = new Vector3 (vicgrezzo.x / Screen.width, vicgrezzo.y / Screen.height, vicgrezzo.z);
		
		daPiazzare.position = new Vector3 (vic.x * deltaXGui + origineGui.x, vic.y * deltaYGui + origineGui.y, origineGui.z);
		//cameraGui
	}

	public void WorldToGuiStartup ()
	{
		origineGui = downLeft.position;
		deltaXGui = upRight.position.x - origineGui.x;
		deltaYGui = upRight.position.y - origineGui.y;
	}
	// ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^  end  World point to CeleGui  ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
	
	
	
	















	//***********************   OBSOLETI

	//void CeleGuiStart ()
	//{
	//	Vector3 posiz = cameraGui.transform.position;
	//	//				Debug.Log ("***********_celegui_start");
	//	/*	upLeft = Instantiate (point, posiz, Quaternion.identity) as GameObject;
	//				upRight = Instantiate (point, posiz, Quaternion.identity) as GameObject;
	//				downLeft = Instantiate (point, posiz, Quaternion.identity) as GameObject;
	//				downRight = Instantiate (point, posiz, Quaternion.identity) as GameObject;
	//				//conter = Instantiate (point, posiz, Quaternion.identity) as GameObject;
	//				centerUp = Instantiate (point, posiz, Quaternion.identity) as GameObject;
	//				center = Instantiate (point, posiz, Quaternion.identity) as GameObject;
	//				centerDown = Instantiate (point, posiz, Quaternion.identity) as GameObject;
	//				centerLeft = Instantiate (point, posiz, Quaternion.identity) as GameObject;
	//				centerRight = Instantiate (point, posiz, Quaternion.identity) as GameObject;
	//
	//				upLeft = (Transform)Instantiate (point, posiz, Quaternion.identity);
	//				upRight = (Transform)Instantiate (point, posiz, Quaternion.identity);
	//				downLeft = (Transform)Instantiate (point, posiz, Quaternion.identity);
	//				downRight = (Transform)Instantiate (point, posiz, Quaternion.identity);
	//				//conter = Instantiate (point, posiz, Quaternion.identity) as GameObject;
	//				centerUp = (Transform)Instantiate (point, posiz, Quaternion.identity);
	//				center = (Transform)Instantiate (point, posiz, Quaternion.identity);
	//				centerDown = (Transform)Instantiate (point, posiz, Quaternion.identity);
	//				centerLeft = (Transform)Instantiate (point, posiz, Quaternion.identity);
	//		centerRight = (Transform)Instantiate (point, posiz, Quaternion.identity);*/
	//
	//	upLeft = (Instantiate (point, posiz, Quaternion.identity) as GameObject).transform;
	//	upRight = (Instantiate (point, posiz, Quaternion.identity) as GameObject).transform;
	//	downLeft = (Instantiate (point, posiz, Quaternion.identity) as GameObject).transform;
	//	downRight = (Instantiate (point, posiz, Quaternion.identity) as GameObject).transform;
	//
	//	center = (Instantiate (point, posiz, Quaternion.identity) as GameObject).transform;
	//	centerUp = (Instantiate (point, posiz, Quaternion.identity) as GameObject).transform;
	//	centerDown = (Instantiate (point, posiz, Quaternion.identity) as GameObject).transform;
	//	centerLeft = (Instantiate (point, posiz, Quaternion.identity) as GameObject).transform;
	//	centerRight = (Instantiate (point, posiz, Quaternion.identity) as GameObject).transform;
	//
	//	//	conter.transform.parent = transform;
	//	center.transform.parent = cameraGui.transform;
	//	centerUp.transform.parent = cameraGui.transform;
	//	centerDown.transform.parent = cameraGui.transform;
	//	centerLeft.transform.parent = cameraGui.transform;
	//	centerRight.transform.parent = cameraGui.transform;
	//	upLeft.transform.parent = cameraGui.transform;
	//	upRight.transform.parent = cameraGui.transform;
	//	downLeft.transform.parent = cameraGui.transform;
	//	downRight.transform.parent = cameraGui.transform;
	//	noPos.transform.parent = cameraGui.transform;
	//
	//
	//
	//}


	//	if (ruotaPerLandscape) {
	//
	//
	//		upLeft.name = "upRight";
	//		upRight.name = "downRight";
	//		downLeft.name = "upLeft";
	//		downRight.name = "downLeft";
	//		center.name = "center";
	//		centerUp.name = "centerRight";
	//		centerDown.name = "centerLeft";
	//		centerLeft.name = "centerUp";
	//		centerRight.name = "centerDown";
	//
	//
	//		// toolz = ztools.GetComponent<sc_tools> ();
	//
	//		cameraGui.fieldOfView = 48;
	//		Vector3 camRotIniziale = cameraGui.transform.eulerAngles + new Vector3 (0, 0, -90);
	//		cameraGui.transform.eulerAngles = new Vector3 (0, 0, 0);
	//		Vector3 planePos = new Vector3 (0, 0, distanzaBigPlane);
	//		planePos = cameraGui.transform.position + planePos;
	//
	//		Quaternion rotaca = cameraGui.transform.rotation;
	//
	//		bigPlane = Instantiate (bigPlanePrefab, planePos, rotaca) as GameObject;
	//		bigPlane.transform.parent = cameraGui.transform;
	//
	//		RaycastHit hit;
	//		// upLeft
	//		Ray ray = cameraGui.ScreenPointToRay (new Vector3 (0, Screen.height, 0));
	//		Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);
	//		if (Physics.Raycast (ray, out hit)) {
	//			if (hit.rigidbody != null) {
	//				upLeft.position = hit.point;
	//				upLeft.rotation = rotaca;
	//
	//				ruotagna = upLeft.transform.localEulerAngles + new Vector3 (0, 0, 90);
	//				upLeft.localEulerAngles = ruotagna;
	//				upLeft.parent = cameraGui.transform;
	//				//	toolz.upRight = upLeft.transform;
	//			} else {
	//				Instantiate (ring, transform.position, transform.rotation);
	//			}
	//		}
	//
	//		// upRight
	//		ray = cameraGui.ScreenPointToRay (new Vector3 (Screen.width, Screen.height, 0));
	//		Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);
	//		if (Physics.Raycast (ray, out hit)) {
	//			if (hit.rigidbody != null) {
	//				upRight.position = hit.point;
	//				//upRight.rotation = rotaca;
	//				upRight.localEulerAngles = ruotagna;
	//				upRight.parent = cameraGui.transform;
	//				//	toolz.downRight = upRight.transform;
	//			} else {
	//				Instantiate (ring, transform.position, transform.rotation);
	//			}
	//		}
	//
	//		// downLeft
	//		ray = cameraGui.ScreenPointToRay (new Vector3 (0, 0, 0));
	//		Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);
	//		if (Physics.Raycast (ray, out hit)) {
	//			if (hit.rigidbody != null) {
	//				downLeft.position = hit.point;
	//				//downLeft.rotation = rotaca;
	//				downLeft.localEulerAngles = ruotagna;
	//				downLeft.parent = cameraGui.transform;
	//				//	toolz.upLeft = downLeft.transform;
	//			} else {
	//				Instantiate (ring, transform.position, transform.rotation);
	//			}
	//		}
	//		//downRight
	//		ray = cameraGui.ScreenPointToRay (new Vector3 (Screen.width, 0, 0));
	//		Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);
	//		if (Physics.Raycast (ray, out hit)) {
	//			if (hit.rigidbody != null) {
	//				downRight.position = hit.point;
	//				//	downRight.rotation = rotaca;
	//				downRight.localEulerAngles = ruotagna;
	//				downRight.parent = cameraGui.transform;
	//				//	toolz.downLeft = downRight.transform;
	//			} else {
	//				Instantiate (ring, transform.position, transform.rotation);
	//			}
	//		}
	//		//center
	//		ray = cameraGui.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height / 2, 0));
	//		Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);
	//		if (Physics.Raycast (ray, out hit)) {
	//			if (hit.rigidbody != null) {
	//				center.position = hit.point;
	//				//	center.rotation = rotaca;
	//				center.localEulerAngles = ruotagna;
	//				//center.transform.localEulerAngles = center.transform.localEulerAngles + new Vector3 (0, 0, 90);
	//				center.parent = cameraGui.transform;
	//				// toolz.center = center.transform;
	//			} else {
	//				Instantiate (ring, transform.position, transform.rotation);
	//			}
	//		}
	//
	//
	//		//centerUp
	//		ray = cameraGui.ScreenPointToRay (new Vector3 (Screen.width, Screen.height / 2, 0));
	//		Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);
	//		if (Physics.Raycast (ray, out hit)) {
	//			if (hit.rigidbody != null) {
	//				centerUp.position = hit.point;
	//				//centerUp.rotation = rotaca;
	//				centerUp.localEulerAngles = ruotagna;
	//				centerUp.parent = cameraGui.transform;
	//				//	toolz.centerRight = centerUp.transform;
	//			}
	//		}
	//
	//
	//		//centerDown
	//		ray = cameraGui.ScreenPointToRay (new Vector3 (Screen.width / 2, 0, 0));
	//		Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);
	//		if (Physics.Raycast (ray, out hit)) {
	//			if (hit.rigidbody != null) {
	//				centerDown.position = hit.point;
	//				//centerDown.rotation = rotaca;
	//
	//				centerDown.localEulerAngles = ruotagna;
	//				centerDown.parent = cameraGui.transform;
	//				//	toolz.centerLeft = centerDown.transform;
	//			}
	//		}
	//
	//		//centerLeft
	//		ray = cameraGui.ScreenPointToRay (new Vector3 (0, Screen.height / 2, 0));
	//		Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);
	//		if (Physics.Raycast (ray, out hit)) {
	//			if (hit.rigidbody != null) {
	//				centerLeft.position = hit.point;
	//				//centerLeft.rotation = rotaca;
	//				centerLeft.localEulerAngles = ruotagna;
	//				centerLeft.parent = cameraGui.transform;
	//				//	toolz.centerUp = centerLeft.transform;
	//			}
	//		}
	//
	//		//centerRight
	//		ray = cameraGui.ScreenPointToRay (new Vector3 (Screen.width, Screen.height / 2, 0));
	//		Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);
	//		if (Physics.Raycast (ray, out hit)) {
	//			if (hit.rigidbody != null) {
	//				centerRight.position = hit.point;
	//				//centerRight.rotation = rotaca;
	//				centerRight.localEulerAngles = ruotagna;
	//				centerRight.parent = cameraGui.transform;
	//				//	toolz.centerDown = centerRight.transform;
	//			}
	//		}
	//
	//		//forse linee desuete
	//
	//		Vector3 large = centerLeft.transform.position - centerRight.transform.position;
	//		//		larghezzaMax = Mathf.Abs (large.x);
	//		Vector3 altura = centerUp.transform.position - centerDown.transform.position;
	//		//		altezzaMax = Mathf.Abs (altura.y);
	//		cameraGui.transform.eulerAngles = camRotIniziale;
	//		//hit.rigidbody.AddForceAtPosition(ray.direction * pokeForce, hit.point);
	//		DestroyObject (bigPlane);
	//
	//		//cameraGui.transform.position=cam_pos_menu.transform.position;
	//		//cameraGui.transform.rotation=cam_pos_menu.transform.rotation;
	//		//	cameraGui.transform.parent=player.transform;
	//
	//
	//
	//	}
	//
	//	//	Debug.Log ("***********_celegui_MAIN____END");
	//
	//	if (overscanAttivo) {
	//		OverscanCorrection ();
	//	}
	//}




	//public void CeleGui () //riscritta 24 aprile 2014
	//{
	//	/*	center.transform.parent = cameraGui.transform;
	//				centerUp.transform.parent = cameraGui.transform;
	//				centerDown.transform.parent = cameraGui.transform;
	//				centerLeft.transform.parent = cameraGui.transform;
	//				centerRight.transform.parent = cameraGui.transform;
	//				upLeft.transform.parent = cameraGui.transform;
	//				upRight.transform.parent = cameraGui.transform;
	//				downLeft.transform.parent = cameraGui.transform;
	//				downRight.transform.parent = cameraGui.transform;
	//				noPos.transform.parent = cameraGui.transform;*/
	//	//	Debug.Log ("***********_celegui_MAIN____start");
	//	if (!ruotaPerLandscape) {
	//		//toolz = ztools.GetComponent<sc_tools> ();
	//		upLeft.name = "upLeft";
	//		upRight.name = " upRight";
	//		downLeft .name = "downLeft";
	//		downRight.name = "downRight";
	//		center .name = "center";
	//		centerUp.name = "centerUp";
	//		centerDown .name = "centerDown";
	//		centerLeft .name = "centerLeft";
	//		centerRight.name = "centerRight";
	//
	//		Vector3 camRotIniziale = cameraGui.transform.eulerAngles;
	//		cameraGui.transform.eulerAngles = new Vector3 (0, 0, 0);
	//		Vector3 planePos = new Vector3 (0, 0, distanzaBigPlane);
	//		planePos = cameraGui.transform.position + planePos;
	//
	//		Quaternion rotaca = cameraGui.transform.rotation;
	//
	//		bigPlane = Instantiate (bigPlanePrefab, planePos, rotaca) as GameObject;
	//		bigPlane.transform.parent = cameraGui.transform;
	//
	//		RaycastHit hit;
	//		// upLeft
	//		Ray ray = cameraGui.ScreenPointToRay (new Vector3 (0, Screen.height, 0));
	//		Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);
	//		if (Physics.Raycast (ray, out hit)) {
	//			//	Debug.Log ("cele_raycasta");
	//			if (hit.rigidbody != null) {
	//				//Debug.Log ("cele_hitta");
	//				upLeft.position = hit.point;
	//				upLeft.rotation = rotaca;
	//				upLeft.parent = cameraGui.transform;
	//				//	toolz.upLeft = upLeft.transform;
	//			} else {
	//				//	Debug.Log ("cele_NOOO_hitta");
	//				Instantiate (ring, transform.position, transform.rotation);
	//			}
	//		}
	//
	//		// upRight
	//		ray = cameraGui.ScreenPointToRay (new Vector3 (Screen.width, Screen.height, 0));
	//		Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);
	//		if (Physics.Raycast (ray, out hit)) {
	//			if (hit.rigidbody != null) {
	//				upRight.position = hit.point;
	//				upRight.rotation = rotaca;
	//				upRight.parent = cameraGui.transform;
	//				//	toolz.upRight = upRight.transform;
	//			} else {
	//				Instantiate (ring, transform.position, transform.rotation);
	//			}
	//		}
	//
	//
	//		// downLeft
	//		ray = cameraGui.ScreenPointToRay (new Vector3 (0, 0, 0));
	//		Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);
	//		if (Physics.Raycast (ray, out hit)) {
	//			if (hit.rigidbody != null) {
	//				downLeft.position = hit.point;
	//				downLeft.rotation = rotaca;
	//				downLeft.parent = cameraGui.transform;
	//				//	toolz.downLeft = downLeft.transform;
	//			} else {
	//				Instantiate (ring, transform.position, transform.rotation);
	//			}
	//		}
	//		//downRight
	//		ray = cameraGui.ScreenPointToRay (new Vector3 (Screen.width, 0, 0));
	//		Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);
	//		if (Physics.Raycast (ray, out hit)) {
	//			if (hit.rigidbody != null) {
	//				downRight.position = hit.point;
	//				downRight.rotation = rotaca;
	//				downRight.parent = cameraGui.transform;
	//				//toolz.downRight = downRight.transform;
	//			} else {
	//				Instantiate (ring, transform.position, transform.rotation);
	//			}
	//		}
	//		//center
	//		ray = cameraGui.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height / 2, 0));
	//		Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);
	//		if (Physics.Raycast (ray, out hit)) {
	//			if (hit.rigidbody != null) {
	//				center.position = hit.point;
	//				center.rotation = rotaca;
	//				center.parent = cameraGui.transform;
	//				//	toolz.center = center.transform;
	//			} else {
	//				Instantiate (ring, transform.position, transform.rotation);
	//			}
	//		}
	//
	//
	//		//centerUp
	//		ray = cameraGui.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height, 0));
	//		Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);
	//		if (Physics.Raycast (ray, out hit)) {
	//			if (hit.rigidbody != null) {
	//				centerUp.position = hit.point;
	//				centerUp.rotation = rotaca;
	//				centerUp.parent = cameraGui.transform;
	//				//	toolz.centerUp = centerUp.transform;
	//			}
	//		}
	//
	//
	//		//centerDown
	//		ray = cameraGui.ScreenPointToRay (new Vector3 (Screen.width / 2, 0, 0));
	//		Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);
	//		if (Physics.Raycast (ray, out hit)) {
	//			if (hit.rigidbody != null) {
	//				centerDown.position = hit.point;
	//				centerDown.rotation = rotaca;
	//				centerDown.parent = cameraGui.transform;
	//				//	toolz.centerDown = centerDown.transform;
	//			}
	//		}
	//
	//		//centerLeft
	//		ray = cameraGui.ScreenPointToRay (new Vector3 (0, Screen.height / 2, 0));
	//		Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);
	//		if (Physics.Raycast (ray, out hit)) {
	//			if (hit.rigidbody != null) {
	//				centerLeft.position = hit.point;
	//				centerLeft.rotation = rotaca;
	//				centerLeft.parent = cameraGui.transform;
	//				//	toolz.centerLeft = centerLeft.transform;
	//			}
	//		}
	//
	//		//centerRight
	//		ray = cameraGui.ScreenPointToRay (new Vector3 (Screen.width, Screen.height / 2, 0));
	//		Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);
	//		if (Physics.Raycast (ray, out hit)) {
	//			if (hit.rigidbody != null) {
	//				centerRight.position = hit.point;
	//				centerRight.rotation = rotaca;
	//				centerRight.parent = cameraGui.transform;
	//				//	toolz.centerRight = centerRight.transform;
	//			}
	//		}
	//
	//		//forse linee desuete
	//		Vector3 large = centerLeft.transform.position - centerRight.transform.position;
	//		//	larghezzaMax = Mathf.Abs (large.x);
	//		Vector3 altura = centerUp.transform.position - centerDown.transform.position;
	//		//	altezzaMax = Mathf.Abs (altura.y);
	//		cameraGui.transform.eulerAngles = camRotIniziale;
	//		//hit.rigidbody.AddForceAtPosition(ray.direction * pokeForce, hit.point);
	//		DestroyObject (bigPlane);
	//
	//		//camera.transform.position=cam_pos_menu.transform.position;
	//		//camera.transform.rotation=cam_pos_menu.transform.rotation;
	//		//	camera.transform.parent=player.transform;
	//
	//
	//	}

	// in disuso d 23 APRILE 2014 per inglobamento di wakeup_celgui
	/*	public void CeleguiTransformSeek ()
		{
				center = GameObject.Find ("center").transform;
				centerUp = GameObject.Find ("centerUp").transform;
				centerDown = GameObject.Find ("centerDown").transform;
				centerLeft = GameObject.Find ("centerLeft").transform;
				centerRight = GameObject.Find ("centerRight").transform;
				upLeft = GameObject.Find ("upLeft").transform;
				upRight = GameObject.Find ("upRight").transform;
				downLeft = GameObject.Find ("downLeft").transform;
				downRight = GameObject.Find ("downRight").transform;
				noPos = GameObject.Find ("noPos").transform;
		}*/
	//################################# fine blocco
	
	/*      //^^^^^^^^^^^^^^^^^^
		//*******************ShareAndroid v1.0 first release 7 novembre 2013
		//GameObject ztools=GameObject.Find("ztools");
		//ztools.GetComponent<sc_tools>().ShareAndroid(" imagepath");
	#if UNITY_ANDROID && !UNITY_EDITOR

	private const string UNITY_EDITOR = "non-Android device";
	private string shareText = "Share Text with another app";
	private string shareImage = "Share Image with another app";
	private string shareGeneric = "Generic share intent";
	static ShareHelper helperScript = null;
		
		public void ShareAndroid (string path)
		{
				Debug.Log ("shareAndroid");
				if (helperScript != null) {	
						Debug.Log ("shareAndroid NO NULLL");
						helperScript.ShareImage (path, "Choose app");
				}
		}

	#endif
	*/

	//	void ChartStart ()
	//	{
	//		classifica = new int[lunghezzaClassifica];//vedi ScoreInChart()
	//		topTen = new int[10];//vedi ScoreInChart()
	//		nickChart = new string[10];//vedi ScoreInChart()
	//		idNick = new int[10];//vedi ScoreInChart()
	//		idPhoto = new int[10];//vedi ScoreInChart()
	//
	//		//refreDebug.Log ("Partite_giocate" + PlayerPrefs.GetInt ("partiteGiocate").ToString ());
	//		if (partiteGiocate > 0) {
	//			RefreshTopTen ();
	//		}
	//
	//
	//		prefabChartElement.SetActive (false);
	//		chartBoardElements = new GameObject[10];
	//		initialEuler = pinChartBoard.transform.eulerAngles;
	//		pinChartBoard.transform.localScale = new Vector3 (pinChartRescale, pinChartRescale, pinChartRescale);
	//		BuildChartBoard ();
	//	}

	//	void BuildChartBoard ()
	//	{
	//
	//		Vector3 placePrefab = pinChartBoard.transform.position + new Vector3 (0, -altoDiscance, deepDistance);
	//		for (int n=0; n<10; ++n) {
	//			chartBoardElements [n] = Instantiate (prefabChartElement, placePrefab, Quaternion.identity)as GameObject;
	//			chartBoardElements [n].SetActive (true);
	//			chartBoardElements [n].transform.parent = pinChartBoard.transform;
	//			chartBoardElements [n].transform.localScale = prefabChartElement.transform.localScale;
	//			//	chartElements [n].transform.localEulerAngles = new Vector3 (0, 180, 0);
	//			placePrefab = placePrefab + new Vector3 (0, -stepDistance, 0);
	//			// aspect ratio foto
	//			Transform fotoDaScalare = chartBoardElements [n].GetComponent<sc_element_chart> ().photo.transform;
	//			Transform fotoDaScalareParent = fotoDaScalare.parent;
	//			fotoDaScalare.parent = null;
	//			Vector3 scalePhoto = fotoDaScalare.localScale;
	//			fotoDaScalare.localScale = new Vector3 (scalePhoto.x, scalePhoto.x * 1 / whWebcamRatio, scalePhoto.z);
	//			fotoDaScalare.parent = fotoDaScalareParent;
	//		}
	//		if (!chartConPhoto) {
	//			CorrectChartBoardNoPhoto ();
	//		}
	//		ChartBoardRefresh ();
	//	}

	//	void CorrectChartBoardNoPhoto ()
	//	{
	//		for (int n=0; n<10; ++n) {
	//			chartBoardElements [n].transform.localScale = chartBoardElements [n].transform.localScale * inciccionaChart;
	//			chartBoardElements [n].transform.position = chartBoardElements [n].transform.position + traslaChart;
	//			chartBoardElements [n].GetComponent<sc_element_chart> ().photo.SetActive (false);
	//			if (!chartConPhoto) {
	//				Destroy (chartBoardElements [n].GetComponent<sc_element_chart> ().barraCliccabile.GetComponent<sc_button_chart_bar> ());//.enabled = false;
	//			}
	//
	//		}
	//
	//	}

	//	public void ChartBoardRefresh ()
	//	{
	//		if (chartConPhoto) {
	//			StartCoroutine (RefreshPhoto ());
	//		}
	//		for (int ch=0; ch<10; ++ch) {
	//			string nickOk = nickChart [ch];
	//			if (nickOk == null) {
	//				nickOk = "---";
	//			}
	//			chartBoardElements [ch].GetComponent<sc_element_chart> ().ElementChartSetup (ch + 1, topTen [ch], chartBoardMaterials [ch], nickOk);
	//			//topTen [ch] = PlayerPrefs.GetInt ("Score" + ch.ToString ());
	//		}
	//	}

	/*

		//**********************************************************************************************
		//                          FaceBook tools                                        maggio 2014
		//**********************************************************************************************

		public bool FacebookLogged;



		private static List<object>                 friends = null;
		private static Dictionary<string, string>   profile = null;
		private static List<object>                 scores = null;
		private static Dictionary<string, Texture>  friendImages = new Dictionary<string, Texture> ();

		//praticamente attiva facebook prima ancora di fare il login
		public void CallFBInit ()
		{
				FB.Init (SetInit, OnHideUnity);
		}

		public  void CallFBLogin ()
		{
				FB.Login ("email,publish_actions", LoginCallback);
				myFB_material.mainTexture = myFB_texture; // di test trovargli un posto migliore
		}

		//The LoginCallback function is called when the user has completed a login. If the login was successful we call the function OnLoggedIn.
		void LoginCallback (FBResult result)
		{
				if (result.Error != null) {
						//lastResponse = "Error Response:\n" + result.Error;
				} else if (!FB.IsLoggedIn) {
						//lastResponse = "Login cancelled by Player";
				} else {
						FacebookLogged = true;
						FB_UserID = FB.UserId;
						//lastResponse = "Login was successful!";
				}
		}
		public string FB_UserID;
		void OnLoggedIn ()
		{                                                                                          
				//Util.Log("Logged in. ID: " + FB.UserId);  
				FB_UserID = FB.UserId;
		}

		private void SetInit ()
		{
				if (FB.IsLoggedIn) {
						FacebookLogged = true;
						FB_UserID = FB.UserId;
						//Util.Log("Already logged in");
						//OnLoggedIn();
						//loadingState = LoadingState.WAITING_FOR_INITIAL_PLAYER_DATA;
				} else {
						FacebookLogged = false;
						//loadingState = LoadingState.DONE;
				}
		}
	
		private void OnHideUnity (bool isGameShown) //questo succede (credo) quando un messaggio fa appare sul gioco
		{
				//Util.Log("OnHideUnity");
				if (!isGameShown) {
						// pause the game - we will need to hide
						Time.timeScale = 0;
				} else {
						// start the game back up - we're getting focus again
						Time.timeScale = 1;
				}
		}

		public void FacebookFeed (string caption, string pic, string lname, string link)
		{
				FB.Feed (
		linkCaption: "I just smashed " + " friends! Can you beat it?",
		picture: "http://www.friendsmash.com/images/logo_large.jpg",
		linkName: "Checkout my Friend Smash greatness!",
		link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest")
				);
		}

		public void FacebookFeedPhoto () //mi pare non funga per via dell indirizzo foto
		{

				//photoDaShare.mainTexture
				FB.Feed (
			linkCaption: "I just smashed " + " friends! Can you beat it?",
			picture: Application.persistentDataPath + "/" + "riomoko.png",
			linkName: "Checkout my Friend Smash greatness!",
			link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest")
				);
		}
	

		public void FacebookFeedTest ()
		{
				FB.Feed (
			linkCaption: "Test " + " test ",
			picture: "http://www.friendsmash.com/images/logo_large.jpg",
			linkName: "test",
			link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest")
				);
		}

		public void FacebookScreenshot (string testoFoto)
		{
				StartCoroutine (IFacebookScreenshot (testoFoto));
		}
		public IEnumerator IFacebookScreenshot (string itestoFoto)
		{
				if (!FacebookLogged) {
						CallFBLogin ();
				}
				//	Debug.Log ("toolz facebook step 1");
				yield return new WaitForEndOfFrame ();
				//	Debug.Log ("toolz facebook step 2");
				var width = Screen.width;
				var height = Screen.height;
				var tex = new Texture2D (width, height, TextureFormat.RGB24, false);
				// Read screen contents into the texture
				tex.ReadPixels (new Rect (0, 0, width, height), 0, 0);
				tex.Apply ();
				Debug.Log ("toolz facebook step 3");
				//byte[] screenshot = tex.EncodeToPNG ();
				//var wwwForm = new WWWForm ();
				//wwwForm.AddBinaryData ("image", screenshot, "InteractiveConsole.png");
				//wwwForm.AddField ("message", "herp derp.  I did a thing!  Did I do this right?");
				wwwwFormFBScreenshot.AddField ("message", itestoFoto);
				Debug.Log ("toolz facebook step 5");
				Debug.Log ("facebook Screenshot");
				FB.API ("me/photos", Facebook.HttpMethod.POST, PostScreenshotCallback, wwwwFormFBScreenshot);
				Debug.Log ("toolz facebook step 6 finish");
		}

		private Texture2D lastResponseTexture;
		private string lastResponse = "";
		private string ApiQuery = "";


		void PostScreenshotCallback (FBResult result)
		{
				lastResponseTexture = null;
				// Some platforms return the empty string instead of null.
				if (!String.IsNullOrEmpty (result.Error))
						lastResponse = "Error Response:\n" + result.Error;
				else if (!ApiQuery.Contains ("/picture"))
						lastResponse = "Success Response:\n" + result.Text;
				else {
						lastResponseTexture = result.Texture;
						lastResponse = "Success Response:\n";
				}
		}



		public void CaricaMyImage ()
		{
				//	FB.API ("/me?fields=id,first_name,friends.limit(100).fields(first_name,id)", Facebook.HttpMethod.GET, APICallback);
				LoadPicture (Util.GetPictureURL ("me", 128, 128), MyPictureCallback);
		}

		public  void CaricaFB_UserIDImage (string FB_ID, GameObject fotoOwner)
		{
				//	FB.API ("/me?fields=id,first_name,friends.limit(100).fields(first_name,id)", Facebook.HttpMethod.GET, APICallback);
				LoadPictureID (Util.GetPictureURL (FB_ID, 128, 128), FB_ID_PictureCallback, fotoOwner);
		}

		IEnumerator LoadPictureEnumerator (string url, LoadPictureCallback callback)
		{
				WWW www = new WWW (url);
				yield return www;
				callback (www.texture);
		}
		void LoadPicture (string url, LoadPictureCallback callback)
		{
				FB.API (url, Facebook.HttpMethod.GET, result =>
				{
						if (result.Error != null) {
								Util.LogError (result.Error);
								return;
						}
			
						var imageUrl = Util.DeserializePictureURLString (result.Text);
			
						StartCoroutine (LoadPictureEnumerator (imageUrl, callback));
				});
		}


		IEnumerator LoadPictureEnumeratorID (string url, LoadPictureCallbackID callback, GameObject fotoOwnerXX)
		{
				WWW www = new WWW (url);
				yield return www;
				callback (www.texture, fotoOwnerXX);
		}
		void LoadPictureID (string url, LoadPictureCallbackID callback, GameObject fotoOwnerX)
		{
				FB.API (url, Facebook.HttpMethod.GET, result =>
				{
						if (result.Error != null) {
								Util.LogError (result.Error);
								return;
						}
			
						var imageUrl = Util.DeserializePictureURLString (result.Text);
			
						StartCoroutine (LoadPictureEnumeratorID (imageUrl, callback, fotoOwnerX));
				});
		}

		public Texture myFB_texture;
		public Material myFB_material;
		void MyPictureCallback (Texture texture)
		{
				//	Util.Log ("MyPictureCallback");
		
				if (texture == null) {
						// Let's just try again
						LoadPicture (Util.GetPictureURL ("me", 128, 128), MyPictureCallback);
			
						return;
				}
				myFB_texture = texture;
				myFB_material.mainTexture = myFB_texture;
				//	GameStateManager.UserTexture = texture;
				//	haveUserPicture = true;
				//	checkIfUserDataReady ();
		}


		void FB_ID_PictureCallback (Texture texture, GameObject fotoOwnerXXX)
		{
				//Util.Log ("MyPictureCallback");
		
				if (texture == null) {
						// Let's just try again
						//______________________		LoadPicture (Util.GetPictureURL ("me", 128, 128), FB_ID_PictureCallback);
			
						return;
				}
				fotoOwnerXXX.GetComponent<sc_FB_texture> ().FB_foto = texture;
				//myFB_texture = texture;
				//myFB_material.mainTexture = myFB_texture;
				//	GameStateManager.UserTexture = texture;
				//	haveUserPicture = true;
				//	checkIfUserDataReady ();
		}

		public static void FriendPictureCallback (Texture texture)
		{
				//	GameStateManager.FriendTexture = texture;
		}
	
		delegate void LoadPictureCallback (Texture texture);
		delegate void LoadPictureCallbackID (Texture texture,GameObject owner);


		void APICallback (FBResult result)
		{
				Util.Log ("APICallback");
				if (result.Error != null) {
						Util.LogError (result.Error);
						// Let's just try again
						FB.API ("/me?fields=id,first_name,friends.limit(100).fields(first_name,id)", Facebook.HttpMethod.GET, APICallback);
						return;
				}
		
				profile = Util.DeserializeJSONProfile (result.Text);
				//	GameStateManager.Username = profile ["first_name"];
				friends = Util.DeserializeJSONFriends (result.Text);
				//	checkIfUserDataReady ();
		}

		//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^  end Facebook  ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

	*/
	//**********************************************************************************************
	//                          FaceBook tools   DEBUG                                       maggio 2014
	//**********************************************************************************************
	//	[HideInInspector]
	//	public bool
	//		FacebookLogged;
	//
	//
	//
	////	private static List<object>                 friends = null;
	////	private static Dictionary<string, string>   profile = null;
	////	private static List<object>                 scores = null;
	////	private static Dictionary<string, Texture>  friendImages = new Dictionary<string, Texture> ();
	//
	//	//praticamente attiva facebook prima ancora di fare il login
	//	public void CallFBInit ()
	//	{
	//		//^^^^^^^^^	FB.Init (SetInit, OnHideUnity);
	//	}
	//
	//	public  void CallFBLogin ()
	//	{
	//		//^^^^^^^^^	FB.Login ("email,publish_actions", LoginCallback);
	//		myFB_material.mainTexture = myFB_texture; // di test trovargli un posto migliore
	//	}
	//
	//	//The LoginCallback function is called when the user has completed a login. If the login was successful we call the function OnLoggedIn.
	//	/*^^^^^^^^^^^^^^^^^^^^^^^^^^^^	void LoginCallback (FBResult result)
	//		{
	//				if (result.Error != null) {
	//						//lastResponse = "Error Response:\n" + result.Error;
	//				} else if (!FB.IsLoggedIn) {
	//						//lastResponse = "Login cancelled by Player";
	//				} else {
	//						FacebookLogged = true;
	//						FB_UserID = FB.UserId;
	//						//lastResponse = "Login was successful!";
	//				}
	//		}
	//	*/
	//	[HideInInspector]
	//	public string
	//		FB_UserID;
	//	void OnLoggedIn ()
	//	{
	//		//Util.Log("Logged in. ID: " + FB.UserId);
	//		//^^^^^^^^^ 	FB_UserID = FB.UserId;
	//	}
	//
	//	private void SetInit ()
	//	{
	////				if (FB.IsLoggedIn) {
	////						FacebookLogged = true;
	////						FB_UserID = FB.UserId;
	////						//Util.Log("Already logged in");
	////						//OnLoggedIn();
	////						//loadingState = LoadingState.WAITING_FOR_INITIAL_PLAYER_DATA;
	////				} else {
	////						FacebookLogged = false;
	////						//loadingState = LoadingState.DONE;
	////				}
	//	}
	//
	//	private void OnHideUnity (bool isGameShown) //questo succede (credo) quando un messaggio fa appare sul gioco
	//	{
	//		//Util.Log("OnHideUnity");
	//		if (!isGameShown) {
	//			// pause the game - we will need to hide
	//			Time.timeScale = 0;
	//		} else {
	//			// start the game back up - we're getting focus again
	//			Time.timeScale = 1;
	//		}
	//	}
	//
	//	public void FacebookFeed (string caption, string pic, string lname, string link)
	//	{
	////				FB.Feed (
	////			linkCaption: "I just smashed " + " friends! Can you beat it?",
	////			picture: "http://www.friendsmash.com/images/logo_large.jpg",
	////			linkName: "Checkout my Friend Smash greatness!",
	////			link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest")
	////				);
	//	}
	//
	//	public void FacebookFeedPhoto () //mi pare non funga per via dell indirizzo foto
	//	{
	//
	//		//photoDaShare.mainTexture
	////				FB.Feed (
	////			linkCaption: "I just smashed " + " friends! Can you beat it?",
	////			picture: Application.persistentDataPath + "/" + "riomoko.png",
	////			linkName: "Checkout my Friend Smash greatness!",
	////			link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest")
	////				);
	//	}
	//
	//
	//	public void FacebookFeedTest ()
	//	{
	////				FB.Feed (
	////			linkCaption: "Test " + " test ",
	////			picture: "http://www.friendsmash.com/images/logo_large.jpg",
	////			linkName: "test",
	////			link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest")
	////				);
	//	}
	//
	//	public void FacebookScreenshot (string testoFoto)
	//	{
	//		StartCoroutine (IFacebookScreenshot (testoFoto));
	//	}
	//	public IEnumerator IFacebookScreenshot (string itestoFoto)
	//	{
	//		if (!FacebookLogged) {
	//			CallFBLogin ();
	//		}
	//		//	Debug.Log ("toolz facebook step 1");
	//		yield return new WaitForEndOfFrame ();
	//		//	Debug.Log ("toolz facebook step 2");
	//		var width = Screen.width;
	//		var height = Screen.height;
	//		var tex = new Texture2D (width, height, TextureFormat.RGB24, false);
	//		// Read screen contents into the texture
	//		tex.ReadPixels (new Rect (0, 0, width, height), 0, 0);
	//		tex.Apply ();
	//		Debug.Log ("toolz facebook step 3");
	//		//byte[] screenshot = tex.EncodeToPNG ();
	//		//var wwwForm = new WWWForm ();
	//		//wwwForm.AddBinaryData ("image", screenshot, "InteractiveConsole.png");
	//		//wwwForm.AddField ("message", "herp derp.  I did a thing!  Did I do this right?");
	//		wwwwFormFBScreenshot.AddField ("message", itestoFoto);
	//		Debug.Log ("toolz facebook step 5");
	//		Debug.Log ("facebook Screenshot");
	//		//	FB.API ("me/photos", Facebook.HttpMethod.POST, PostScreenshotCallback, wwwwFormFBScreenshot);
	//		Debug.Log ("toolz facebook step 6 finish");
	//	}
	//
	//	private Texture2D lastResponseTexture;
	//	private string lastResponse = "";
	//	private string ApiQuery = "";
	//
	//
	////		void PostScreenshotCallback (FBResult result)
	////		{
	////				lastResponseTexture = null;
	////				// Some platforms return the empty string instead of null.
	////				if (!String.IsNullOrEmpty (result.Error))
	////						lastResponse = "Error Response:\n" + result.Error;
	////				else if (!ApiQuery.Contains ("/picture"))
	////						lastResponse = "Success Response:\n" + result.Text;
	////				else {
	////						lastResponseTexture = result.Texture;
	////						lastResponse = "Success Response:\n";
	////				}
	////		}
	//
	//
	//
	//	public void CaricaMyImage ()
	//	{
	//		//	FB.API ("/me?fields=id,first_name,friends.limit(100).fields(first_name,id)", Facebook.HttpMethod.GET, APICallback);
	//		//	LoadPicture (Util.GetPictureURL ("me", 128, 128), MyPictureCallback);
	//	}
	//
	//	public  void CaricaFB_UserIDImage (string FB_ID, GameObject fotoOwner)
	//	{
	//		//	FB.API ("/me?fields=id,first_name,friends.limit(100).fields(first_name,id)", Facebook.HttpMethod.GET, APICallback);
	//		//	LoadPictureID (Util.GetPictureURL (FB_ID, 128, 128), FB_ID_PictureCallback, fotoOwner);
	//	}
	//
	//	IEnumerator LoadPictureEnumerator (string url, LoadPictureCallback callback)
	//	{
	//		WWW www = new WWW (url);
	//		yield return www;
	//		callback (www.texture);
	//	}
	//	void LoadPicture (string url, LoadPictureCallback callback)
	//	{
	////				FB.API (url, Facebook.HttpMethod.GET, result =>
	////				{
	////						if (result.Error != null) {
	////								Util.LogError (result.Error);
	////								return;
	////						}
	////
	////						var imageUrl = Util.DeserializePictureURLString (result.Text);
	////
	////						StartCoroutine (LoadPictureEnumerator (imageUrl, callback));
	////				});
	//	}
	//
	//
	//	IEnumerator LoadPictureEnumeratorID (string url, LoadPictureCallbackID callback, GameObject fotoOwnerXX)
	//	{
	//		WWW www = new WWW (url);
	//		yield return www;
	//		callback (www.texture, fotoOwnerXX);
	//	}
	//	void LoadPictureID (string url, LoadPictureCallbackID callback, GameObject fotoOwnerX)
	//	{
	////				FB.API (url, Facebook.HttpMethod.GET, result =>
	////				{
	////						if (result.Error != null) {
	////								Util.LogError (result.Error);
	////								return;
	////						}
	////
	////						var imageUrl = Util.DeserializePictureURLString (result.Text);
	////
	////						StartCoroutine (LoadPictureEnumeratorID (imageUrl, callback, fotoOwnerX));
	////				});
	//	}
	//
	//	public Texture myFB_texture;
	//	public Material myFB_material;
	//	void MyPictureCallback (Texture texture)
	//	{
	//		//	Util.Log ("MyPictureCallback");
	//
	//		if (texture == null) {
	//			// Let's just try again
	//			//	LoadPicture (Util.GetPictureURL ("me", 128, 128), MyPictureCallback);
	//
	//			return;
	//		}
	//		myFB_texture = texture;
	//		myFB_material.mainTexture = myFB_texture;
	//		//	GameStateManager.UserTexture = texture;
	//		//	haveUserPicture = true;
	//		//	checkIfUserDataReady ();
	//	}
	//
	//
	////	void FB_ID_PictureCallback (Texture texture, GameObject fotoOwnerXXX)
	////	{
	////		//Util.Log ("MyPictureCallback");
	////
	////		if (texture == null) {
	////			// Let's just try again
	////			//______________________		LoadPicture (Util.GetPictureURL ("me", 128, 128), FB_ID_PictureCallback);
	////
	////			return;
	////		}
	////		fotoOwnerXXX.GetComponent<sc_FB_texture> ().FB_foto = texture;
	////		//myFB_texture = texture;
	////		//myFB_material.mainTexture = myFB_texture;
	////		//	GameStateManager.UserTexture = texture;
	////		//	haveUserPicture = true;
	////		//	checkIfUserDataReady ();
	////	}
	//
	//	public static void FriendPictureCallback (Texture texture)
	//	{
	//		//	GameStateManager.FriendTexture = texture;
	//	}
	//
	//	delegate void LoadPictureCallback (Texture texture);
	//	delegate void LoadPictureCallbackID (Texture texture,GameObject owner);
	//
	//
	////		void APICallback (FBResult result)
	////		{
	////				Util.Log ("APICallback");
	////				if (result.Error != null) {
	////						Util.LogError (result.Error);
	////						// Let's just try again
	////						FB.API ("/me?fields=id,first_name,friends.limit(100).fields(first_name,id)", Facebook.HttpMethod.GET, APICallback);
	////						return;
	////				}
	////
	////				profile = Util.DeserializeJSONProfile (result.Text);
	////				//	GameStateManager.Username = profile ["first_name"];
	////				friends = Util.DeserializeJSONFriends (result.Text);
	////				//	checkIfUserDataReady ();
	////		}
	//
	//	//^^^^^^^^^^^^^^^^end Facebook   DEBUG^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
	


}
