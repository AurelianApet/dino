using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class sc_screen_capture_and_share : MonoBehaviour
{
	//public GameObject canvas;
	public GameObject sharePanel;
	public GameObject flash;
	public GameObject logo;
	//public RTSCam camscript;
	Texture2D textureScreenshot;
	byte[] bytes;
	public string nome;
	public string path;
	public float delay;
	public Material testMaterial;
	public RawImage capturedImage;
	//	public Diffusion diffusion;  //PLUGIN DIFUSION
	int groupId;


	void Start ()
	{
		path = Application.persistentDataPath;
		//print ("_____________" + Application.persistentDataPath);
	}


	public void Btn_Capture_Photo ()
	{
		flash.SetActive (true);
		SaveScreenShotPath ();
	}

	public void SaveScreenShotPath ()
	{
		StartCoroutine (SaveScreenshot_ReadPixelsAsynchPath (nome, path, delay));
	}

	IEnumerator SaveScreenshot_ReadPixelsAsynchPath (string nome, string path, float delay)
	{
		yield return new WaitForEndOfFrame ();
		flash.SetActive (false);	
		logo.SetActive (true);	
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
		//yield return new WaitForSeconds (0.2f);
		yield return new WaitForEndOfFrame ();
		//  canvas [groupId].SetActive (true);
		logo.SetActive (false);	
		sharePanel.SetActive (true);
		//capturedImage.texture = textureScreenshot;
		LoadPng ();
		//Tell unity to delete the texture, by default it seems to keep hold of it and memory crashes will occur after too many screenshots.
		// DestroyObject(texture);
	}

	bool imageReady;
	public Texture2D imageloaded;

	public void LoadPng ()
	{
		imageReady = false;
		StartCoroutine (CaricaTexture (nome));
	}

	IEnumerator CaricaTexture (string nome)
	{
		testMaterial.mainTexture = imageloaded;
		WWW www = new WWW ("file://" + Application.persistentDataPath + "/" + nome + ".png");
		yield return www;
		imageloaded = www.texture; 
		//testMaterial.mainTexture = imageloaded;
		capturedImage.texture = imageloaded;
		imageReady = true;
	}

	public void Close ()
	{
		sharePanel.SetActive (false);
		//	canvas [groupId].SetActive (true);
	}


	//	public void ShareFacebook ()
	//	{
	//		diffusion.PostToFacebook ("Check out my Mooney Plane!", null, "file://" + Application.persistentDataPath + "/" + nome + ".png");
	//	}
	//
	//	public void ShareTwitter ()
	//	{
	//		diffusion.PostToTwitter ("Check out my Mooney Plane!! #mooney", null, "file://" + Application.persistentDataPath + "/" + nome + ".png");
	//	}
	//
	//	public void ShareNative ()
	//	{
	//		diffusion.Share ("Check out my Mooney Plane!", null, "file://" + Application.persistentDataPath + "/" + nome + ".png");
	//	}
	//
	//	public void ShareEmail ()
	//	{
	//		IOSSocialManager.instance.SendMail ("Check out my Mooney Plane!", "", "", textureScreenshot);
	//	}





	//	public void SaveScreenShot (string nome)
	//	{
	//		//^^^^^okScreenshot = false;
	//		StartCoroutine (SaveScreenshot_ReadPixelsAsynch (nome));
	//	}
	
	
	//	public void SaveScreenShot (string nome)
	//	{
	//		//^^^^^okScreenshot = false;
	//		StartCoroutine (SaveScreenshot_ReadPixelsAsynch (nome));
	//	}
	//
	//	IEnumerator SaveScreenshot_ReadPixelsAsynch (string nome)
	//	{
	//		//Wait for graphics to render
	//		yield return new WaitForEndOfFrame ();
	//		#if !UNITY_WEBPLAYER
	//		//Create a texture to pass to encoding
	//		textureScreenshot = new Texture2D (Screen.width, Screen.height, TextureFormat.RGB24, false);
	//		//Put buffer into texture
	//		textureScreenshot.ReadPixels (new Rect (0, 0, Screen.width, Screen.height), 0, 0);
	//		//Split the process up--ReadPixels() and the GetPixels() call inside of the encoder are both pretty heavy
	//		yield return 0;
	//		textureScreenshot.Apply ();
	//		//^^^^^mat_photoDaShare.mainTexture = textureScreenshot;
	//		bytes = textureScreenshot.EncodeToPNG ();
	//		//^^^^^okScreenshot = true;
	//
	//		//byte[] screenshot = tex.EncodeToPNG ();
	//		//^^^^^	wwwwFormFBScreenshot = new WWWForm ();
	//		//^^^^^wwwwFormFBScreenshot.AddBinaryData ("image", bytes, "InteractiveConsole.png");
	//		//wwwForm.AddField ("message", "herp derp.  I did a thing!  Did I do this right?");
	//
	//
	//		Debug.Log ("screen ok to png todavia no saved");
	//		// yield return new WaitForSeconds(3.0F);
	//		//Save our test image (could also upload to WWW)
	//		//^^^^^  System.IO.File.WriteAllBytes (Application.persistentDataPath + "/" + versionM.gameName + nome, bytes);//+ ".png"+ "/"
	//		Debug.Log ("screen ok to png todavia YES saved");
	//		// File.WriteAllBytes(filePath, bytes);
	//		//yield return new WaitForSeconds (2);
	//		//Tell unity to delete the texture, by default it seems to keep hold of it and memory crashes will occur after too many screenshots.
	//		// DestroyObject(texture);
	//		#endif
	//	}

}
