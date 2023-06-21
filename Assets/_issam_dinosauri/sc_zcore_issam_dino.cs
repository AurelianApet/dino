using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class sc_zcore_issam_dino : MonoBehaviour
{

	public bool isFake;
	public string sceneToLoad;

	void Start ()
	{
		if (!isFake) {
            DontDestroyOnLoad(gameObject);
            //SceneManager.LoadScene(sceneToLoad);
        } else {
            GameObject zcore = (GameObject.Find("zcore"));
            if (zcore != null)
            {
                Destroy(gameObject);
            }

        }


	}


	void Update ()
	{
	
	}
}
