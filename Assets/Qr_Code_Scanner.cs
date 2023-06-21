using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using LitJson;
using UnityEngine.Networking;

public class Qr_Code_Scanner : MonoBehaviour {

    public sc_issam_menu_manager issamMenuManagetScript;
	public QRCodeDecodeController e_qrController;
	public Text UiText;
    public GameObject alertDialog;
    public Text alertText;
    public Text alertHeading;

    public GameObject mainCanvas;
    public GameObject mainCam;

    void Awake()
    {
        
    }
	// Use this for initialization
	void Start () {
		if (e_qrController != null)
        {
			e_qrController.onQRScanFinished += qrScanFinished;//Add Finished Event
		}
        if (alertDialog != null)
        {
            alertDialog.SetActive(false);
        }
        if (UiText != null)
        {
            UiText.text = string.Empty;
        }
        if (alertText != null)
        {
            alertText.text = string.Empty;
        }
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void Autofocus()
    {
        //CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_TRIGGERAUTO);
    }

	/// <summary>
	/// reset the QRScanner Controller 
	/// </summary>
	public void Reset()
	{
		if (e_qrController != null) {
			e_qrController.Reset();
		}

		if (UiText != null) {
			UiText.text = "";	
		}
	}

    public void PlayScan()
    {
        this.gameObject.SetActive(true);

        Reset();
        if (this.e_qrController != null)
        {
            this.e_qrController.StartWork();
        }
    }

    //void qrScanFinished(string dataText)
    //{
    //    PlayerPrefs.SetInt("unlocked", 1);

    //    this.gameObject.SetActive(false);
    //    issamMenuManagetScript.Btn_Load_AR();
    //}

    void qrScanFinished(string dataText)
    {
        Debug.Log("QRCode: " + dataText);

        UiText.text = dataText;

        // Request url
        string url = "http://www.ilikecreation.com/i-like/webservice/index.php/api/scan_qrcode";

        WWWForm form = new WWWForm();
        form.AddField("ime_no", SystemInfo.deviceUniqueIdentifier);
        form.AddField("qrcode", dataText);

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        //WWW www = new WWW(url, form);

        string fullurl = "ime_no=" + SystemInfo.deviceUniqueIdentifier + "&qrcode=" + dataText;
        UiText.text = fullurl;

        StartCoroutine(WaitForQRScanRequest(www));
    }

    IEnumerator WaitForQRScanRequest(UnityWebRequest www)
    {
        yield return www.SendWebRequest();

        // check for errors
        if (www.isHttpError || www.isNetworkError)
        {
            Debug.LogError("QRScanRequest Error: " + www.error);
            //ShowAlert("Internet Error! ", "Please check if your Internet connection is valid.");
            ShowAlert("Internet Error! ", www.error);
        }
        else
        {
            Debug.LogError("QRScanRequest Success: " + www.downloadHandler.text);
            ProcessQRScanResponse(www.downloadHandler.text);
        }
    }

    //IEnumerator WaitForQRScanRequest(WWW www)
    //{
    //    yield return www;

    //    // check for errors
    //    if (www.error == null)
    //    {
    //        Debug.LogError("QRScanRequest Success: " + www.text);
    //        ProcessQRScanResponse(www.text);
    //    }
    //    else
    //    {
    //        Debug.LogError("QRScanRequest Error: "+ www.error);
    //        //ShowAlert("Internet Error! ", "Please check if your Internet connection is valid.");
    //        ShowAlert("Internet Error! ", www.error);
    //    }
    //}

    private void ProcessQRScanResponse(string jsonString)
    {
        Debug.Log("QRScanResponse: " + jsonString);

        JsonData jsonvale = JsonMapper.ToObject(jsonString);
        bool status = (bool)jsonvale["status"];

        if (status)
        {
            PlayerPrefs.SetInt("unlocked", 1);

            this.gameObject.SetActive(false);
            issamMenuManagetScript.Btn_Load_AR();
            //			SceneManager.LoadScene ("us_issam_menu");
        }
        else
        {
            PlayerPrefs.SetInt("unlocked", 0);
            //			ScannerCanvas.SetActive (false);
            //			ScannerControllers.SetActive (false);
            //			mainCanvas.SetActive (true);
            //			mainCam.SetActive (true);
            //			Reset ();
            ShowAlert("Invalid Qr-Code! ", "Please scan a valid  iLike Dino QR-Code.");
            //			resetBtn.SetActive (true);
            //			HomeBtn.SetActive (true);

        }
    }

    public void Btn_Close()
    {
        //		Reset ();
        alertDialog.SetActive(false);
        this.gameObject.SetActive(false);

        mainCanvas.SetActive(true);
        mainCam.SetActive(true);
    }

    public void Btn_AlertOk()
    {
        //		Reset ();
        alertDialog.SetActive(false);
        Reset();
    }

    public void ShowAlert(string title, string content)
    {
        alertHeading.text = title;
        alertText.text = content;
        alertDialog.SetActive(true);
    }
}
