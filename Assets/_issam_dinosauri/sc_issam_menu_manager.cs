using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Android;
using Vuforia;

public class sc_issam_menu_manager : MonoBehaviour
{

	public GameObject[] slide;
	public Transform segnapostoCentrale;
	public Transform segnapostoSx;
	public Transform segnapostoDx;
	public Text loadingYears;

	public GameObject mainCanvas;
	public GameObject mainCam;

    public Qr_Code_Scanner qrScannerScript;

    int nowSlide;
	int pastSlide;

	bool SlideActive;


	public sc_tools toolz;

	void Awake ()
	{
        GameObject zcore = (GameObject.Find ("zcore")); 
		if (zcore == null) {
			//Debug.LogError ("MENU MANAGER ERROR : ZCORE not found FAKE MODE");
			zcore = (GameObject.Find ("zcore_FAKE")); 
		}

		if(zcore!=null)
		toolz = zcore.GetComponent<sc_tools > ();
	}



	void Start ()
	{
        //if (PlayerPrefs.GetInt("unlocked") == 1)
        //    PlayerPrefs.DeleteKey("unlocked");

        //PlayerPrefs.SetInt("unlocked", 1);

        UI_Refresh ();
	}

	bool loadingAR;

	public void Btn_Load_AR ()
	{		
		//if (PlayerPrefs.GetInt ("unlocked") == 1)
  //      {
            mainCanvas.SetActive(true);
            mainCam.SetActive(true);
            loadingAR = true;
			UI_Refresh ();
			StartCoroutine (AsynchronousLoad ("us_issam_AR"));
		//}
  //      else
  //      {
  //          // QRScan active
  //          qrScannerScript.PlayScan();
  //          mainCanvas.SetActive(false);
  //          mainCam.SetActive(false);

  //          //			SceneManager.LoadScene ("QRScanScene");
  //          //			qr_Code_ScannerPanel.SetActive (true);
  //      }
    }



	IEnumerator AsynchronousLoad (string scene)
	{
		loadingYears.gameObject.SetActive (true);

        // Vuforia
        //yield return null;
        //if (VuforiaRuntime.Instance.InitializationState != VuforiaRuntime.InitState.INITIALIZED)
        //{
        //    Debug.LogError("Vuforia Init start");
        //    VuforiaRuntime.Instance.InitVuforia();
        //    Debug.LogError("Vuforia Init finish");
        //}
        //yield return null;

        int c = 0;
		while (c < 70) {
			c = c + 1;
			yield return new WaitForSeconds (0.05f);
			//loadingYears.text = Random.Range (111111111, 999999999).ToString ();
		}
		loadingYears.gameObject.SetActive (true);
		yield return null;

		AsyncOperation ao = SceneManager.LoadSceneAsync (scene);
		ao.allowSceneActivation = false;

		while (!ao.isDone) {
			// [0, 0.9] > [0, 1]
			float progress = Mathf.Clamp01 (ao.progress / 0.9f);
			Debug.Log ("Loading progress: " + (progress * 100) + "%");

			// Loading completed
			if (ao.progress == 0.9f) {
//				Debug.Log("Press a key to start");
//				if (Input.AnyKey())
				ao.allowSceneActivation = true;
			}

			yield return null;
		}
	}






	public void Btn_Load_SLIDE ()
	{
		nowSlide = 0;
		pastSlide = 0;
		SlideActive = true;
		UI_Refresh ();

		PosizionaSlideInizio ();

	}

	public void BtnZ_Close ()
	{
		SlideActive = false;
		nowSlide = 0;
		pastSlide = 0;
		UI_Refresh ();
	

	}

	public void Btn_More_SLIDE ()
	{
		Debug.Log ("more_slide***");
		pastSlide = nowSlide;
		nowSlide = nowSlide + 1;
		if (nowSlide > slide.Length - 1) {
			nowSlide = slide.Length - 1;
		}
		PosizionaSlide ();
	
	}

	public void Btn_Less_SLIDE ()
	{
		pastSlide = nowSlide;
		nowSlide = nowSlide - 1;
		if (nowSlide < 0) {
			nowSlide = 0;
		}
		PosizionaSlide ();
	
	}







	public void PosizionaSlideInizio ()
	{
		for (int n = 0; n < slide.Length; ++n) {
			if (n < pastSlide) {
				slide [n].transform.position = segnapostoSx.position;
			}
			if (n == pastSlide) {
				slide [n].transform.position = segnapostoCentrale.position;
			}
			if (n > pastSlide) {
				slide [n].transform.position = segnapostoDx.position;
			}



		}
	}

	public void PosizionaSlide ()
	{
		for (int n = 0; n < slide.Length; ++n) {
			if (n < pastSlide) {
				slide [n].transform.position = segnapostoSx.position;
			}
			if (n == pastSlide) {
				slide [n].transform.position = segnapostoCentrale.position;
			}
			if (n > pastSlide) {
				slide [n].transform.position = segnapostoDx.position;
			}

			if (pastSlide < nowSlide) {
				StartCoroutine (SlideMove	(slide [pastSlide].transform, segnapostoSx));
				StartCoroutine (SlideMove	(slide [nowSlide].transform, segnapostoCentrale));
			}
			if (pastSlide > nowSlide) {
				StartCoroutine (SlideMove	(slide [pastSlide].transform, segnapostoDx));
				StartCoroutine (SlideMove	(slide [nowSlide].transform, segnapostoCentrale));
			}

		}
		UI_Refresh ();
	}


	IEnumerator SlideMove (Transform slidez, Transform toPos)
	{
		Vector3 startPosition = slidez.position;
		for (float n = 0.0f; n < 1.1f; n = n + 0.2f) {
			slidez.position = Vector3.Lerp (startPosition, toPos.position, n);
			yield return new WaitForSeconds (0.05f);
		}
		slidez.position = Vector3.Lerp (startPosition, toPos.position, 1);
	}

	public void UI_Refresh ()
	{
		if (!loadingAR) {
            Debug.Log("MenuManager: NotLoadingAR");
            if (SlideActive) {
                Debug.Log("MenuManager: SlideActive");

                toolz.Deactivator ();
				toolz.DeActiva (10);
				toolz.Activa (20);
				toolz.DeActiva (25);
				toolz.Activa (100);
				Debug.Log ("nowslide:" + nowSlide);
				if (nowSlide > 0) {
					toolz.Activa (502);//freccia sx
				} else {
					toolz.DeActiva (502);
				}


				if (nowSlide < slide.Length - 1) {
					toolz.Activa (501);//freccia dx
				} else {
					toolz.DeActiva (501);
				}
		
			} else {
                toolz.Deactivator();
                toolz.Activa(10);
                toolz.Activa(20);
                toolz.Activa(25);

                Debug.Log("MenuManager: NotSlideActive");
            }
		} else {
            Debug.Log("MenuManager: LoadingAR");
            //LOADING AR
            toolz.Deactivator ();
			toolz.Activa (69);
		}
	}

}
