using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using Vuforia;

public class sc_issam_start_manager : MonoBehaviour
{
    private bool mVuforiaInitialized = false;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            InitializeVuforia();
        }
        else
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!mVuforiaInitialized && Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            InitializeVuforia();
        }
    }

    private void InitializeVuforia()
    {
        mVuforiaInitialized = true;
        VuforiaRuntime.Instance.InitVuforia();
        //GetComponent<VuforiaBehaviour>().enabled = true;
        SceneManager.LoadScene("us_issam_menu");
    }
}
