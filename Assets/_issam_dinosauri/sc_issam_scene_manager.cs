using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;

public class sc_issam_scene_manager : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES

    public GameObject scanner;
	public Camera arCam;
	public GameObject main_mesh;
	public GameObject canvasFact;
    public GameObject panelCapturePhoto;
    public GameObject panelCaptureVideo;
    public GameObject btnCapturePhoto;
    public GameObject btnCaptureVideo;
    public GameObject btnNotification;

    public MeshRenderer[] sensors;
    public GameObject[] dinosauri;

	public AudioClip[] circleSound;
	public Material circleMeterial;
	public Sprite[] dinoSize;
	public Image i_dinoSize;
	public Scrollbar scrolbarrFacts;

    public GameObject dinoNow;
    public sc_tools toolz;

    public TextAsset dinoFactText;

    public Text name;
    public Text height;
    public Text lenght;
    public Text weight;
    public Text diet;
    public Text era;
    public Text facts;

    public string nome;
    public string path;
    public float delay;
    public RawImage capturedImage;
    public Texture2D imageloaded;

    public ReplayCam replayCam;

    public GameObject alertDialog;
    public Text alertText;
    public Text alertHeading;

    #endregion

    #region PRIVATE_MEMBER_VARIABLES

    AudioSource audioPlayer;
	bool atLeastOneSensorEnabled;
	int s;
    int indexType;
    int indexDino;
	DinoInfo[] dinoInfos;

    string[] objectTextLines;
    string[] objectTextParameter;
    string[] additionalInfoTemp;

    int lastId = 9999;

    Texture2D textureScreenshot;
    byte[] bytes;
    bool imageReady;

    AudioSource audio;

    bool statoCliccato;

    //private AndroidUtils androidUtils;
    #endregion

    public struct DinoInfo
	{
		public string name, size, height, width, weight, diet, era, additional;
	}

    #region UNITY_MONOBEHAVIOUR_METHODS

    void Awake ()
	{
		GameObject zcore = (GameObject.Find ("zcore"));
        if (zcore == null)
        {
            //Debug.LogError ("MENU MANAGER ERROR : ZCORE not found FAKE MODE");
            zcore = (GameObject.Find("zcore_FAKE"));
        }
        toolz = zcore.GetComponent<sc_tools>();
        audio = GetComponent<AudioSource>();

		LoadFact ();
		canvasFact.SetActive (false);
        panelCapturePhoto.SetActive(false);
        panelCaptureVideo.SetActive(false);
        btnCapturePhoto.SetActive(false);
        btnCaptureVideo.SetActive(false);
    }

    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        //DinoOn (0);
        
        if (alertDialog != null)
        {
            alertDialog.SetActive(false);
        }
        if (alertHeading != null)
        {
            alertHeading.text = string.Empty;
        }
        if (alertText != null)
        {
            alertText.text = string.Empty;
        }

        toolz.Deactivator();
        btnNotification.SetActive(true);
    }

    void OnEnable()
    {

    }

    void Update()
    {
        //for (s = 0; s < sensors.Length; s++)
        //{
        //    if (sensors[s].enabled)
        //    {
        //        atLeastOneSensorEnabled = true;
        //        //circleMeterial.color = circleColor [s];
                
        //        if (indexType != s)
        //        {
        //            statoCliccato = true;
        //            //	ButtonClicked (0);
        //        }
        //        indexType = s;

        //        Debug.Log("sensors_number :  " + s + ", last id : " + lastId);

        //        if (lastId != s)
        //        {
        //            DinoOn(indexType);
        //        }
        //        lastId = s;
        //    }
        //}

        //if (atLeastOneSensorEnabled)
        //{
        //    if (!main_mesh.activeSelf)
        //    {
        //        main_mesh.SetActive(true);
        //        if (dinoNow != null)
        //        {
        //            dinoNow.transform.position = Vector3.zero;
        //        }
        //        scanner.SetActive(false);
        //    }
        //}
        //else
        //{
        //    if (main_mesh.activeSelf)
        //    {
        //        main_mesh.SetActive(false);
        //        scanner.SetActive(true);
        //    }
        //}
        //atLeastOneSensorEnabled = false;

        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = arCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {

                if (hit.collider.transform.tag == "cliccabile")
                {
                    //CambiaCover ();
                    Debug.Log("^^^^^^RaycastHit blink");
                    hit.collider.transform.SendMessage("Click");

                }

            }


        }
    }


    void LateUpdate()
    {

    }

    #endregion

    #region PUBLIC_METHODS

    public void Reset()
    {
    }

    public void Btn_PhotoVideo()
    {
        if (toolz.IsActiva(710)) // Photo button
        {
            toolz.DeActiva(710);
        }
        else
        {
            toolz.Activa(710);
        }

        if (toolz.IsActiva(712)) // Video button
        {
            toolz.DeActiva(712);
        }
        else
        {
            toolz.Activa(712);
        }
    }

    public void Btn_Capture_Photo()
    {

        SaveScreenShotPath();
    }

    
    public void Btn_Facts()
    {
        FillFacts ();
        canvasFact.SetActive(true);
        scrolbarrFacts.value = 1;
    }

    public void FillFacts()
    {
        if (indexType < 0 || indexType >= dinoInfos.Length)
        {
            indexType = 4;
        }
        i_dinoSize.sprite = dinoSize[indexType];
        name.text = dinoInfos[indexType].name;
        height.text = dinoInfos[indexType].height;
        lenght.text = dinoInfos[indexType].width;
        weight.text = dinoInfos[indexType].weight;
        diet.text = dinoInfos[indexType].diet;
        era.text = dinoInfos[indexType].era;
        facts.text = dinoInfos[indexType].additional;
    }

    public void FillDinoFacts(int dinoIndex)
    {
        indexType = dinoIndex;

        //if (dinoIndex >= 0 && dinoIndex < dinoInfos.Length)
        //{
        //    i_dinoSize.sprite = dinoSize[dinoIndex];
        //    if (name != null) name.text = dinoInfos[dinoIndex].name;
        //    if (height != null) height.text = dinoInfos[dinoIndex].height;
        //    if (lenght != null) lenght.text = dinoInfos[dinoIndex].width;
        //    if (weight != null) weight.text = dinoInfos[dinoIndex].weight;
        //    if (diet != null) diet.text = dinoInfos[dinoIndex].diet;
        //    if (era != null) era.text = dinoInfos[dinoIndex].era;
        //    if (facts != null) facts.text = dinoInfos[dinoIndex].additional;
        //}
        //else
        //{
        //    i_dinoSize.sprite = dinoSize[0];
        //    if (name != null) name.text = string.Empty;
        //    if (height != null) height.text = string.Empty;
        //    if (lenght != null) lenght.text = string.Empty;
        //    if (weight != null) weight.text = string.Empty;
        //    if (diet != null) diet.text = string.Empty;
        //    if (era != null) era.text = string.Empty;
        //    if (facts != null) facts.text = string.Empty;
        //}
    }
    public void SaveScreenShotPath()
    {
        path = Application.persistentDataPath;
        StartCoroutine(SaveScreenshot_ReadPixelsAsynchPath(nome, path, delay));
    }

    public void Btn_Close_Share()
    {
        UI_Refresh();
    }

    public void Btn_SaveScreenshot()
    {
        var sshotName = string.Format("screenshot_{0}.png", System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff"));
        Debug.LogError(sshotName);
        Debug.LogError("SaveScreenshot result: " + NativeGallery.SaveImageToGallery(imageloaded, "iLikeDINO", sshotName));

        ShowAlert("Screen Capture", "Screen capture is succeeded, please check gallery in file manager.");

        //AndroidCamera.instance.SaveImageToGallery(imageloaded);
    }


    public void Btn_Share()
    {
        //AndroidSocialGate.StartShareIntent("", "Hey!! Look who visited me today", imageloaded);
    }


    public void ButtonPlus()
    {
        indexDino = indexDino + 1;
        if (indexDino > dinosauri.Length - 1)
        {
            indexDino = 0;
        }
        DinoOn(indexDino);
    }

    public void ButtonMinus()
    {
        indexDino = indexDino - 1;
        if (indexDino < 0)
        {
            indexDino = dinosauri.Length - 1;
        }
        DinoOn(indexDino);
    }


    public void Btn_Menu()
    {
        SceneManager.LoadScene("us_issam_menu");
    }

    public void Btn_Info()
    {

    }


    public void Btn_Play() //change dino animation
    {
        //	dinoNow.GetComponent<rex_cs_cele> ().AnimaNext ();
        dinoNow.SendMessage("AnimaNext");
    }


    public void Btn_Change_Skin() //change dino animation
    {
        //dinoNow.GetComponent<rex_cs_cele> ().ChangeSkin ();
        dinoNow.SendMessage("ChangeSkin");
    }

    public void ButtonClicked(int bIndex)
    {

    }

    public void UI_Refresh()
    {
        if (toolz != null)
        {
            toolz.Deactivator();
            toolz.Activa(711); // PhotoAndVideo
            toolz.Activa(7101);
            toolz.Activa(720);
            toolz.Activa(730);
            toolz.Activa(750);
            toolz.Activa(900); //joystick
        }
        alertDialog.SetActive(false);
    }

    // Video Capture
    public void Btn_Capture_Video()
    {
        UI_Refresh();

        toolz.DeActiva(711); // PhotoAndVideo button
        //toolz.DeActiva(750); // back to biscot button
        toolz.DeActiva(7101); // back to menu button
        toolz.DeActiva(730); // Dino facts button

        toolz.Activa(890);
        replayCam.CaptureStart();
    }

    public void Btn_Capture_Video_Save()
    {
        replayCam.SetSave(true);
        replayCam.CaptureStop();
    }

    public void Btn_Capture_Video_Preview()
    {
        //videoRecorderPanel.Preview();
    }

    public void Btn_Capture_Video_Share()
    {
        //videoRecorderPanel.SharePreview();
    }

    public void Btn_Close_VideoCapture()
    {
        replayCam.SetSave(false);
        replayCam.CaptureStop();
    }

    public void SaveCaptureVideo(string recordingPath)
    {
        var sshotName = string.Format("record_{0}.mp4", System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff"));
        Debug.LogError(sshotName);
        Debug.LogError("SaveScreenshot result: " + NativeGallery.SaveVideoToGallery(recordingPath, "iLikeDINO", sshotName));
        File.Delete(recordingPath);

        ShowAlert("VideoRecord", "Video record is succeeded, please check gallery in file manager.");
    }

    public void DiscardCaptureVideo(string recordingPath)
    {
        File.Delete(recordingPath);
        VideoCaptureClose();
    }

    public void VideoCaptureClose()
    {
        UI_Refresh();
    }

    public void ShowAlert(string title, string content)
    {
        alertHeading.text = title;
        alertText.text = content;
        alertDialog.SetActive(true);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////
    //public void ShowDinoContent(GameObject org, int dinoIndex)
    //{
    //    Debug.Log("ShowDinoContent:" + dinoIndex);
    //for (s = 0; s < dinosauri.Length; s++)
    //    dinosauri[s].SetActive(false);
    //    Destroy(dinoNow, 0);
    //    dinoNow = Instantiate(org, Vector3.zero, Quaternion.identity) as GameObject;
    //    dinoNow.transform.localScale = dinoNow.transform.localScale * 0.08f;
    //    dinoNow.transform.parent = main_mesh.transform;
    //    GetComponent<sc_finger_scale_object>().objectScalable = dinoNow.transform;

    //    // Fill data
    //    FillFacts(dinoIndex);
    //}
    public void ShowDinoContent(GameObject parent, GameObject org, int dinoIndex)
    {
        Debug.Log("ShowDinoContent:" + dinoIndex);
        dinoNow = org;
        //dinoNow = Instantiate(org, Vector3.zero, Quaternion.identity) as GameObject;
        //dinoNow.transform.parent = main_mesh.transform;

        GetComponent<sc_finger_scale_object>().objectScalable = dinoNow.transform;

        // Fill data
        FillDinoFacts(dinoIndex);
    }

    public void HideDinoContent(GameObject org)
    {
        //Destroy(dinoNow, 0);
        //dinoNow = null;
        org.SetActive(false);
    }

    public void Btn_Close_Notification()
    {
        btnNotification.SetActive(false);
        UI_Refresh();
    }

    #endregion

    #region PRIVATE_METHODS

    void LoadFact ()
	{
		string dino = dinoFactText.text;

		objectTextLines = dino.Split ('\n');
		dinoInfos = new DinoInfo[objectTextLines.Length - 1];
		for (int li = 1; li < objectTextLines.Length; li++) {
			objectTextParameter = objectTextLines [li].Split ('\t');
			dinoInfos [li - 1].name = (objectTextParameter [0]);
			//	dinoInfos [li - 1].size = (objectTextParameter [1]);
			dinoInfos [li - 1].height = (objectTextParameter [1]);
			dinoInfos [li - 1].width = (objectTextParameter [2]);

			dinoInfos [li - 1].weight = (objectTextParameter [3]);
			dinoInfos [li - 1].diet = (objectTextParameter [4]);
			dinoInfos [li - 1].era = (objectTextParameter [5]);

			//	dinoInfos [li - 1].additional = (objectTextParameter [5]);

			additionalInfoTemp = objectTextParameter [6].Split ('•');
			for (int n = 0; n < additionalInfoTemp.Length; n++) {
				if (n == 1 || n == 0) {
					dinoInfos [li - 1].additional = "•" + additionalInfoTemp [n];
				} else {
					dinoInfos [li - 1].additional = dinoInfos [li - 1].additional + "\n•" + additionalInfoTemp [n];
				}
			}
			Debug.Log (":::::::::::::" + dinoInfos [li - 1].additional);
			//dinoInfos [li - 1].additional = (objectTextParameter [5]);
		}
	}

    IEnumerator SaveScreenshot_ReadPixelsAsynchPath(string nome, string path, float delay)
    {
        //PlaySuono(0);
        audio.Play();
        if (toolz != null)
        {
            toolz.Deactivator();
            toolz.Activa(8);// attiva flash
        }

        yield return new WaitForEndOfFrame();

        if (toolz != null)
        {
            toolz.DeActiva(8);// disattiva flash
            toolz.Activa(750);// Attiva LOGO
                              //logo.SetActive (true);
                              //Wait for graphics to render
        }
        yield return new WaitForEndOfFrame();
        //Create a texture to pass to encoding
        textureScreenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        //Put buffer into texture
        textureScreenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        //Split the process up--ReadPixels() and the GetPixels() call inside of the encoder are both pretty heavy
        yield return 0;

        bytes = textureScreenshot.EncodeToPNG();
        // yield return new WaitForSeconds(3.0F);
        //Save our test image (could also upload to WWW)
#if !UNITY_WEBPLAYER
        System.IO.File.WriteAllBytes(path + "/" + nome + ".png", bytes);
        // File.WriteAllBytes(filePath, bytes);
#endif
        //yield return new WaitForSeconds (0.2f);
        yield return new WaitForEndOfFrame();
        //  canvas [groupId].SetActive (true);

        if (toolz != null)
        {
            toolz.Activa(790);
        }
        //sharePanel.SetActive (true);
        //capturedImage.texture = textureScreenshot;
        WWW www = new WWW("file://" + Application.persistentDataPath + "/" + nome + ".png");
        yield return www;
        imageloaded = www.texture;
        //testMaterial.mainTexture = imageloaded;
        capturedImage.texture = imageloaded;
        imageReady = true;
        //Tell unity to delete the texture, by default it seems to keep hold of it and memory crashes will occur after too many screenshots.
        // DestroyObject(texture);
    }

    //void DinoOn(int dinoindex)
    //{
    //    Destroy(dinoNow, 0);
    //    dinoNow = Instantiate(dinosauri[dinoindex], Vector3.zero, Quaternion.identity) as GameObject;
    //    dinoNow.transform.localScale = dinoNow.transform.localScale * 0.08f;
    //    dinoNow.transform.parent = main_mesh.transform;
    //    GetComponent<sc_finger_scale_object>().objectScalable = dinoNow.transform;
    //    Debug.Log("**** " + dinoindex);
    //    dinosauri[dinoindex].SetActive(true);
    //}

    void DinoOn(int dinoindex)
    {
        for (s = 0; s < dinosauri.Length; s++)
            dinosauri[s].SetActive(false);

        Debug.Log("ShowDinoContent:" + dinoindex);
        dinoNow = dinosauri[dinoindex];
        dinoNow.transform.position = Vector3.zero;

        GetComponent<sc_finger_scale_object>().objectScalable = dinoNow.transform;
        dinoNow.SetActive(true);
    }

    void HideButtons(int bIndexx)
    {

    }

    void PlaySuono(int bIndez)
    {
        audioPlayer.clip = circleSound[bIndez];
        audioPlayer.Play();
    }

    #endregion

    


}
