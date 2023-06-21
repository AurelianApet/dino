using UnityEngine;
using System.Collections;

public class sc_autodistruggi : MonoBehaviour {

	public float autodistruggiTime;

	void Start () {
	StartCoroutine(	Autodistruggi());
	}
	
	IEnumerator Autodistruggi()
	{
		yield return new WaitForSeconds(autodistruggiTime);
		//yield return new WaitForSeconds(autodistruggiTime);
		Destroy(gameObject) ;
	}
}
