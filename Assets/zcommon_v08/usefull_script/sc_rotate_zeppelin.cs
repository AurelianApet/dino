using UnityEngine;
using System.Collections;

public class sc_rotate_zeppelin : MonoBehaviour
{

	public Vector3 speedRot;
	void Start ()
	{
	
	}
	

	void Update ()
	{
		transform.Rotate (speedRot * Time.deltaTime);
	}
}
