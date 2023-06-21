using UnityEngine;
using System.Collections;

public class sc_camera_orbit_cele : MonoBehaviour
{
	public GameObject cameraOrbit;
	Camera cam;
	public float speedRotation;
	public float speedTilt;
	public float tiltRotMin;
	public float tiltRotMax;
	public float zoomMin;
	public float zoomMax;
	public float speedZoom;
	int fingerCount;
	Vector2 deltaOne;
	Vector2 deltaTwo;
	float fieldOfViewZ;
	bool twoTouchReady;
	float lastDistance;
	float distance ;
	public float inertiaRotation;
	Vector3 rotationSlide;
	Vector3 lastrotationSlide;


	void Start ()
	{
		cam = cameraOrbit.GetComponent<Camera> ();
	}

	void LateUpdate ()
	{
		fingerCount = 0;
		foreach (Touch touch in Input.touches) {
			if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
				fingerCount++;
			
		}

		if (fingerCount == 0) {

			//transform.eulerAngles = transform.eulerAngles + new Vector3 (0, (lastRotationDelta * inertiaRotation), 0);
			// transform.rotation = transform.rotation * Quaternion.Euler (0, (lastRotationDelta * inertiaRotation * Time.deltaTime), 0);
			//	Debug.Log ("inertia " + (lastRotationDelta * inertiaRotation * Time.deltaTime));


			if (lastrotationSlide.magnitude > 0.1f) {
				rotationSlide = new Vector3 (0, lastrotationSlide.y * inertiaRotation, 0);
				transform.Rotate (rotationSlide, Space.World);


				lastrotationSlide = rotationSlide;
			} else {
				lastrotationSlide = Vector3.zero;
			}
			CheckTilt ();
			CheckZ ();

		}
		

		if (fingerCount == 1) {
			//Debug.Log ("unitouch");
			deltaOne = Input.touches [0].deltaPosition;
			rotationSlide = new Vector3 (-deltaOne.y * Time.deltaTime * speedTilt, deltaOne.x * Time.deltaTime * speedRotation, 0);
			transform.Rotate (rotationSlide, Space.Self);

			lastrotationSlide = rotationSlide;
			//	transform.eulerAngles = transform.eulerAngles + new Vector3 (-deltaOne.y * Time.deltaTime * speedTilt, deltaOne.x * Time.deltaTime * speedRotation, 0);
			//transform.rotation = transform.rotation * Quaternion.Euler (-deltaOne.y * Time.deltaTime * speedTilt, deltaOne.x * Time.deltaTime * speedRotation, 0);

			CheckTilt ();
			CheckZ ();
		}



		if (fingerCount == 2) {

//			deltaOne = Input.touches [0].Position;
//			deltaTwo = Input.touches [1].Position;
			//fieldOfViewZ = cam.fieldOfView + (deltaOne - deltaTwo).magnitude * speedZoom;
			if (twoTouchReady) {
				distance = Vector2.Distance (Input.touches [0].position, Input.touches [1].position);
				fieldOfViewZ = cam.fieldOfView + (lastDistance - distance) * speedZoom * Time.deltaTime;
				CheckZoom ();
				cam.fieldOfView = fieldOfViewZ;
				//Debug.Log ("bi touch" + fieldOfView);
				//^^ Debug.Log ("magnitude" + (deltaOne - deltaTwo).magnitude.ToString () + "field : " + fieldOfViewZ + " inguacchio" + (deltaOne - deltaTwo).magnitude * speedZoom);
				//	Debug.Log ("field" + fieldOfView);
			} else {
				twoTouchReady = true;
				lastDistance = Vector2.Distance (Input.touches [0].position, Input.touches [1].position);
			}

		} else {
			twoTouchReady = false;
		}
		//transform.rotation = transform.rotation * Quaternion.Euler (0, (lastRotationDelta * inertiaRotation * Time.deltaTime), 0);
		//rotation = transform.eulerAngles.y;
//		rotationq = transform.rotation;
//		lastRotationDeltaq = rotationq - lastRotationq;
//		lastRotationq = rotationq;
	}


	public  void ZoomSpeed (float speedZoomZ)
	{
		speedZoom = speedZoomZ;
	}

	public  void CheckTilt ()
	{
		if (transform.eulerAngles.x > tiltRotMax) {
			transform.eulerAngles = new Vector3 (tiltRotMax, transform.eulerAngles.y, transform.eulerAngles.z);
		}
		if (transform.eulerAngles.x < tiltRotMin) {
			transform.eulerAngles = new Vector3 (tiltRotMin, transform.eulerAngles.y, transform.eulerAngles.z);
		}
	}

	public  void CheckZoom ()
	{
		if (fieldOfViewZ > zoomMax) {
			fieldOfViewZ = zoomMax;
		}
		if (fieldOfViewZ < zoomMin) {
			fieldOfViewZ = zoomMin;
		}
	}

	public  void CheckZ ()
	{
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, 0);
	}
}
