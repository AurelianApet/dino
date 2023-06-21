using UnityEngine;

public class arge_cs_cele :  MonoBehaviour
{
	Transform Spine0, Spine1, Spine2, Head, Tail0, Tail1, Tail2, Tail3, Tail4, Tail5, Tail6, Tail7, Tail8,
		Neck0, Neck1, Neck2, Neck3, Neck4, Neck5, Neck6, Neck7, Neck8, Neck9, Neck10, Neck11, Neck12, Neck13, Neck14, Neck15, Neck16;
	float spineYaw, spinePitch, spineRoll, balance, ang, velZ, animcount;
	public AudioClip Largestep, Largesplash, Idleherb, Chew, Arge1, Arge2, Arge3, Arge4;
	public Texture[] skin, eyes;
	
	bool reset, soundplayed, onwater, isdead;
	Animator anim;
	AudioSource source;
	SkinnedMeshRenderer[] rend;
	LODGroup lods;
	Rigidbody rg;
	
	[Header ("---------------------------------------")]
	public float Health = 100;
	public float scale = 0.25f;
	public skinselect BodySkin;
	public eyesselect EyesSkin;
	public lodselect LodLevel = lodselect.Auto;
	[HideInInspector]public string infos;
	public bool AI = false;
	//Cele_inizio
	public Camera arCam;
    //cele***fine

    //***************************************************************************************
    void OnEnable()
    {
        transform.localScale = new Vector3(scale, scale, scale);
    }

    void OnDisable()
    {

    }

    //Get components
    void Start ()
	{
		Tail0 = transform.Find ("Arge/root/pelvis/tail0");
		Tail1 = transform.Find ("Arge/root/pelvis/tail0/tail1");
		Tail2 = transform.Find ("Arge/root/pelvis/tail0/tail1/tail2");
		Tail3 = transform.Find ("Arge/root/pelvis/tail0/tail1/tail2/tail3");
		Tail4 = transform.Find ("Arge/root/pelvis/tail0/tail1/tail2/tail3/tail4");
		Tail5 = transform.Find ("Arge/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5");
		Tail6 = transform.Find ("Arge/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6");
		Tail7 = transform.Find ("Arge/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7");
		Tail8 = transform.Find ("Arge/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7/tail8");
		Spine0 = transform.Find ("Arge/root/spine0");
		Spine1 = transform.Find ("Arge/root/spine0/spine1");
		Spine2 = transform.Find ("Arge/root/spine0/spine1/spine2");
		Neck0 = transform.Find ("Arge/root/spine0/spine1/spine2/spine3/spine4/neck0");
		Neck1 = transform.Find ("Arge/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1");
		Neck2 = transform.Find ("Arge/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2");
		Neck3 = transform.Find ("Arge/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3");
		Neck4 = transform.Find ("Arge/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4");
		Neck5 = transform.Find ("Arge/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5");
		Neck6 = transform.Find ("Arge/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6");
		Neck7 = transform.Find ("Arge/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7");
		Neck8 = transform.Find ("Arge/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8");
		Neck9 = transform.Find ("Arge/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9");
		Neck10 = transform.Find ("Arge/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9/neck10");
		Neck11 = transform.Find ("Arge/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9/neck10/neck11");
		Neck12 = transform.Find ("Arge/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9/neck10/neck11/neck12");
		Neck13 = transform.Find ("Arge/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9/neck10/neck11/neck12/neck13");
		Neck14 = transform.Find ("Arge/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9/neck10/neck11/neck12/neck13/neck14");
		Neck15 = transform.Find ("Arge/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9/neck10/neck11/neck12/neck13/neck14/neck15");
		Neck16 = transform.Find ("Arge/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9/neck10/neck11/neck12/neck13/neck14/neck15/neck16");
		Head = transform.Find ("Arge/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9/neck10/neck11/neck12/neck13/neck14/neck15/neck16/head");

		source = GetComponent<AudioSource> ();
		anim = GetComponent<Animator> ();
		lods = GetComponent<LODGroup> ();
		rend = GetComponentsInChildren<SkinnedMeshRenderer> ();
		rg = GetComponent<Rigidbody> ();



		foreach (SkinnedMeshRenderer element in rend) {
			element.materials [0].mainTexture = skin [BodySkin.GetHashCode ()];
			element.materials [1].mainTexture = eyes [EyesSkin.GetHashCode ()];
		}
        GameObject cameraObj = GameObject.Find("Camera");
        if (cameraObj == null)
            cameraObj = GameObject.Find("ARCamera");
        arCam = cameraObj.GetComponent<Camera>();
		transform.localScale = new Vector3 (scale, scale, scale);
	}
	//***************************************************************************************
	//Check what is colliding
	void OnTriggerStay (Collider coll)
	{
		if (coll.transform.name == "Water") {
			anim.speed = 0.75f;
			onwater = true;
		} //Is on water ?
	}

	void OnTriggerExit (Collider coll)
	{
		if (coll.transform.name == "Water") {
			anim.speed = 1.0f;
			onwater = false;
		}
	}

	//***************************************************************************************
	//Animation controller
	void Update ()
	{
		scale = transform.localScale.x;
		if (AI) {//CPU

		} else {//Human
			//Moves
//			if (Input.GetKey (KeyCode.LeftShift) && Input.GetKey (KeyCode.W))
//				anim.SetInteger ("State", 3); //Run
//			else if (Input.GetKey (KeyCode.W))
//				anim.SetInteger ("State", 1); //Walk
//			else if (Input.GetKey (KeyCode.S))
//				anim.SetInteger ("State", -1); //Walk
//			else if (Input.GetKey (KeyCode.A))
//				anim.SetInteger ("State", 10); // Strafe+
//			else if (Input.GetKey (KeyCode.D))
//				anim.SetInteger ("State", -10); // Strafe-
//			else
//				anim.SetInteger ("State", 0); //Idle



			//Moves  By Cele  ***inizio
			if (Input.GetKey (KeyCode.Space)) {
				anim.SetInteger ("State", 2); //Steps
			} else if (ETCInput.GetAxis ("Vertical") > 0.9f) {
				anim.SetInteger ("State", 3); //Run
			} else if (ETCInput.GetAxis ("Vertical") > 0.2f & ETCInput.GetAxis ("Vertical") < 0.9f) {
				anim.SetInteger ("State", 1); //Walk
			} else if (ETCInput.GetAxis ("Vertical") < -0.2f) {
				anim.SetInteger ("State", -2); //Steps backward
			} else if (ETCInput.GetAxis ("Horizontal") > 0.2f) {
				anim.SetInteger ("State", 10); //Steps Strafe+
			} else if (ETCInput.GetAxis ("Horizontal") < -0.2f) {
				anim.SetInteger ("State", -10); //Steps Strafe-
			} else
				anim.SetInteger ("State", 0); //Idle
			//Moves  By Cele  ***fino

			//CELE inizio



			//Turn
//			if (Input.GetKey (KeyCode.A) && velZ != 0)
//				ang = Mathf.Lerp (ang, -0.5f, 0.025f);
//			else if (Input.GetKey (KeyCode.D) && velZ != 0)
//				ang = Mathf.Lerp (ang, 0.5f, 0.025f);
//			else
//				ang = Mathf.Lerp (ang, 0.0f, 0.025f);

			//Turn by Cele ETC  inizio
			if (ETCInput.GetAxis ("Horizontal") > 0.2f && velZ != 0)
				ang = Mathf.Lerp (ang, -2.0f, 0.05f);
			else if (ETCInput.GetAxis ("Horizontal") < -0.2f && velZ != 0)
				ang = Mathf.Lerp (ang, 2.0f, 0.05f);
			else
				ang = Mathf.Lerp (ang, 0.0f, 0.05f);
			//Turn by Cele ETC  fine

			//CELE********* inizio

			//Idles
//			if (Input.GetKey (KeyCode.Alpha1) || Input.GetKey (KeyCode.E))
//				anim.SetInteger ("Idle", 1); //growl
//			else if (Input.GetKey (KeyCode.Alpha2))
//				anim.SetInteger ("Idle", 2); //Eat
//			else if (Input.GetKey (KeyCode.Alpha3))
//				anim.SetInteger ("Idle", 3); //Drink
//			else if (Input.GetKey (KeyCode.Alpha4) || Input.GetKey (KeyCode.Space))
//				anim.SetInteger ("Idle", 4); //Rise
//			else if (Input.GetKey (KeyCode.Alpha5))
//				anim.SetInteger ("Idle", 5); //Sit/sleep
//			else if (Input.GetKey (KeyCode.Alpha6))
//				anim.SetInteger ("Idle", -1); //Die
//			else
//				anim.SetInteger ("Idle", 0);


			//CELE********* inizio

			if (changeAnim) {
				changeAnim = false;
				switch (idAnim) {
				case 1:
					anim.SetInteger ("Idle", 1); //Idle 1
					break;

				case 2:
					anim.SetInteger ("Idle", 2); //Idle 2
					break;

				case 3:
					anim.SetInteger ("Idle", 3); //Idle 3
					break;


				case 5:
					anim.SetInteger ("Idle", 4); //Idle 4
					break;

				case 4:
					anim.SetInteger ("Idle", 5); //Eat
					break;


				//				case 4:
				//					anim.SetInteger ("Idle", 6); //Drink
				//					break;
				//
				//
				//				case 4:
				//					anim.SetInteger ("Idle", 7); //Sleep
				//					break;


				}
			} else {
				anim.SetInteger ("Idle", 0);
			}

			//CELE********* fine


			//Spine control
//			if (Input.GetKey (KeyCode.Mouse1) && reset == false) {
//				spineYaw += Input.GetAxis ("Mouse X") * 1.0F;
//				spinePitch -= Input.GetAxis ("Mouse Y") * 1.0F;
//			} else {
//				spineYaw = Mathf.Lerp (spineYaw, 0.0f, 0.05f);
//				spinePitch = Mathf.Lerp (spinePitch, 0.0f, 0.05f);
//			}

			//Reset spine position
			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|EatB"))
				reset = true;
			else
				reset = false;
		}

//***************************************************************************************
//Motions code

		//Walking
		if (anim.GetNextAnimatorStateInfo (0).IsName ("Arge|Walk") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|Walk") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Arge|WalkGrowl") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|WalkGrowl")) {
			if (velZ < 0.075f)
				velZ = velZ + (Time.deltaTime * 0.1f);
			else if (velZ > 0.075f)
				velZ = velZ - (Time.deltaTime * 0.5f);

			if (anim.GetNextAnimatorStateInfo (0).IsName ("Arge|StandA"))
				velZ = velZ - (Time.deltaTime * 0.25f);

			transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			transform.Translate (0, 0, velZ * scale * anim.speed);
		}
		
		//Backward
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Arge|Walk-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|Walk-") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Arge|WalkGrowl-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|WalkGrowl-")) {
			if (velZ > -0.075f)
				velZ = velZ - (Time.deltaTime * 0.1f);
	
			if (anim.GetNextAnimatorStateInfo (0).IsName ("Arge|StandA"))
				velZ = velZ + (Time.deltaTime * 0.25f);

			transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, -1, 0));
			transform.Translate (0, 0, velZ * scale * anim.speed);
		}
		
		//Strafe+
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Arge|Strafe+") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|Strafe+")) {
			velZ = 0.01f;

			transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			transform.Translate (velZ * scale * anim.speed, 0, 0);
		}
		
		
		//Strafe-
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Arge|Strafe-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|Strafe-")) {
			velZ = 0.01f;

			transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			transform.Translate (-velZ * scale * anim.speed, 0, 0);
		}
		
		
		//Running
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Arge|Run") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|Run") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Arge|RunGrowl") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|RunGrowl")) {
			if (velZ < 0.225f)
				velZ = velZ + (Time.deltaTime * 0.25F);

			transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			transform.Translate (0, 0, velZ * scale * anim.speed);
		}

		//Stop
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|StandA") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|Die")) {
			velZ = 0.0f;
			transform.Translate (0, 0, velZ);
		}


//***************************************************************************************
//Sound Fx code

		//Get current animation point
		animcount = (anim.GetCurrentAnimatorStateInfo (0).normalizedTime) % 1.0F;
		if (anim.GetAnimatorTransitionInfo (0).normalizedTime != 0.0F)
			animcount = 0.0F;
		animcount = Mathf.Round (animcount * 30);

		if (anim.GetNextAnimatorStateInfo (0).IsName ("Arge|StandA") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|StandA") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Arge|EatB") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|EatB") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Arge|RiseStand") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|RiseStand") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Arge|SitA") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|SitA") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Arge|Sleep") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|Sleep")) {
			if (soundplayed == false && (animcount == 5)) {
				source.pitch = Random.Range (0.5F, 1.0F);
				source.PlayOneShot (Idleherb, 0.5f);
				soundplayed = true;
			} else if (animcount != 5)
				soundplayed = false;
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Arge|StandB") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|StandB") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Arge|RiseGrowl") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|RiseGrowl")) {
			if (soundplayed == false && (animcount == 5)) {
				source.pitch = Random.Range (0.75F, 1.0F);
				source.PlayOneShot (Arge4, 0.75f);
				soundplayed = true;
			} else if (animcount != 5)
				soundplayed = false;
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Arge|EatA") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|EatA")) {
			if (soundplayed == false && (animcount == 11)) {
				source.pitch = Random.Range (0.75F, 1.0F);
				source.PlayOneShot (Chew, 0.1f);
				soundplayed = true;
			} else if (animcount != 11)
				soundplayed = false;
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Arge|EatC") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|EatC")) {
			if (soundplayed == false && (animcount == 5 || animcount == 20)) {
				source.pitch = Random.Range (0.75F, 1.0F);
				source.PlayOneShot (Chew, 0.1f);
				soundplayed = true;
			} else if (animcount != 5 && animcount != 20)
				soundplayed = false;
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Arge|Sitting") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|Sitting") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Arge|Rising") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|Rising")) {
			if (soundplayed == false && (animcount == 5)) {
				source.pitch = Random.Range (0.75F, 1.0F);
				source.PlayOneShot (Arge1, 0.5f);
				soundplayed = true;
			} else if (animcount != 5)
				soundplayed = false;
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Arge|SitB") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|SitB")) {
			if (soundplayed == false && (animcount == 5)) {
				source.pitch = Random.Range (0.75F, 1.0F);
				source.PlayOneShot (Arge1, 0.75f);
				soundplayed = true;
			} else if (animcount != 5)
				soundplayed = false;
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Arge|Rise") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|Rise")) {
			if (soundplayed == false && (animcount == 9)) {
				source.pitch = Random.Range (0.75F, 1.0F);
				source.PlayOneShot (Arge3, 1.0f);
				soundplayed = true;
			} else if (soundplayed == false && (animcount == 15)) {
				source.PlayOneShot (Arge3, 1.0f);
				soundplayed = true;
			} else if (animcount != 9 && animcount != 15)
				soundplayed = false;
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Arge|Rise-") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|Rise-")) {
			if (soundplayed == false && (animcount == 3)) {
				source.pitch = Random.Range (0.75F, 1.0F);
				source.PlayOneShot (Arge1, 1.0f);
				soundplayed = true;
			}
			if (soundplayed == false && (animcount == 10)) {
				source.PlayOneShot (onwater ? Largesplash : Largestep, 1.0f);
				soundplayed = true;
			} else if (animcount != 3 && animcount != 10)
				soundplayed = false;
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Arge|Walk") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|Walk") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Arge|Walk-") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|Walk-") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Arge|WalkGrowl") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|WalkGrowl") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Arge|WalkGrowl-") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|WalkGrowl-") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Arge|Strafe-") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|Strafe-") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Arge|Strafe+") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|Strafe+") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Arge|Run") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|Run") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Arge|RunGrowl") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|RunGrowl")) {
			
			if (anim.GetNextAnimatorStateInfo (0).IsName ("Arge|WalkGrowl") ||
			    anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|WalkGrowl") ||
			    anim.GetNextAnimatorStateInfo (0).IsName ("Arge|WalkGrowl-") ||
			    anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|WalkGrowl-")) {
				if (soundplayed == false && (animcount == 5)) {
					source.pitch = Random.Range (0.8F, 1.2F);
					source.PlayOneShot (Arge4, 1.0f);
					soundplayed = true;
				} else if (animcount != 5)
					soundplayed = false;
			} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Arge|RunGrowl") ||
			           anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|RunGrowl")) {
				if (soundplayed == false && (animcount == 5)) {
					source.pitch = Random.Range (0.8F, 1.2F);
					source.PlayOneShot (Arge2, 1.0f);
					soundplayed = true;
				} else if (animcount != 5)
					soundplayed = false;
			} else if (soundplayed == false && (
			               animcount == 10 || animcount == 25)) {
				source.pitch = Random.Range (0.8F, 1.0F);
				source.PlayOneShot (onwater ? Largesplash : Largestep, 0.5f);
				soundplayed = true;
			} else if (animcount != 10 && animcount != 25 && animcount != 30)
				soundplayed = false;
		} else if (!isdead && (
		               anim.GetNextAnimatorStateInfo (0).IsName ("Arge|Die") ||
		               anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|Die"))) {
			if (soundplayed == false && (animcount == 4)) {
				source.pitch = Random.Range (0.75F, 1.0F);
				source.PlayOneShot (Arge2, 1.0f);
				soundplayed = true;
			}
			if (soundplayed == false && (animcount == 20)) {
				source.pitch = Random.Range (0.75F, 0.75F);
				source.PlayOneShot (onwater ? Largesplash : Largestep, 2.0f);
				soundplayed = true;
			} else if (animcount != 4 && animcount != 20)
				soundplayed = false;

			if (animcount > 20)
				isdead = true;
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Arge|Die-") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|Die-")) {

			if (soundplayed == false && (animcount == 2)) {
				source.pitch = Random.Range (0.75F, 1.0F);
				source.PlayOneShot (Arge3, 1.0f);
				soundplayed = true;
			} else if (animcount != 2)
				soundplayed = false;
			isdead = false;
		}
		//CELE********* inizio

		if (Input.GetMouseButtonDown (0)) {

			Ray ray = arCam.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 100)) {

				//				if (hit.collider.transform.tag == "cliccabile") {
				//					//CambiaCover ();
				//					Debug.Log ("^^^^^^RaycastHit blik");
				//					//	objectSelectedT = hit.collider.transform;
				//					//	objectSelectedT.SendMessage ("Click");
				//
				//				}

				if (hit.collider.transform == transform) {

					Debug.Log ("attak!!!!!!!!!!!!!!!!");
					//anim.SetBool ("Attack", true);
					anim.SetInteger ("Idle", 1); //Idle 1

				}

			}

		} else {
			//	anim.SetBool ("Attack", false);


		}

		//CELE********* fine
	}

	//***************************************************************************************
	//Bone rotations, model modification and stick to the terrain

	void LateUpdate ()
	{
		spineRoll = spineYaw * spinePitch / 32;
		balance = Mathf.Lerp (balance, -ang * 16, 0.01f);

		spineYaw = Mathf.Clamp (spineYaw, -12.0F, 12.0F);
		spinePitch = Mathf.Clamp (spinePitch, -10.0F, 5.0F);

		//Neck and head
		Neck0.transform.rotation *= Quaternion.Euler (-spinePitch / 16, spineRoll, -spineYaw / 8);
		Neck1.transform.rotation *= Quaternion.Euler (-spinePitch / 14, spineRoll, -spineYaw / 7);
		Neck2.transform.rotation *= Quaternion.Euler (-spinePitch / 12, spineRoll, -spineYaw / 6);
		Neck3.transform.rotation *= Quaternion.Euler (-spinePitch / 10, spineRoll, -spineYaw / 5);
		Neck4.transform.rotation *= Quaternion.Euler (-spinePitch / 8, spineRoll, -spineYaw / 4);

		Neck5.transform.rotation *= Quaternion.Euler (-spinePitch / 6, spineRoll, -spineYaw / 3);
		Neck6.transform.rotation *= Quaternion.Euler (-spinePitch / 4, spineRoll, -spineYaw / 2);
		Neck7.transform.rotation *= Quaternion.Euler (-spinePitch / 2, spineRoll, -spineYaw + balance);
		Neck8.transform.rotation *= Quaternion.Euler (-spinePitch, spineRoll, -spineYaw + balance);
		Neck9.transform.rotation *= Quaternion.Euler (-spinePitch, spineRoll, -spineYaw + balance);
		Neck10.transform.rotation *= Quaternion.Euler (-spinePitch, spineRoll, -spineYaw + balance);
		Neck11.transform.rotation *= Quaternion.Euler (-spinePitch, 0, -spineYaw + balance);
		Neck12.transform.rotation *= Quaternion.Euler (-spinePitch, 0, -spineYaw + balance);
		Neck13.transform.rotation *= Quaternion.Euler (-spinePitch, 0, -spineYaw + balance);

		Neck14.transform.rotation *= Quaternion.Euler (-spinePitch, 0, -spineYaw + balance);
		Neck15.transform.rotation *= Quaternion.Euler (-spinePitch, 0, -spineYaw + balance);
		Neck16.transform.rotation *= Quaternion.Euler (-spinePitch, 0, -spineYaw + balance);
		Head.transform.rotation *= Quaternion.Euler (-spinePitch, 0, -spineYaw + balance);

		//Spine and tail
		Spine0.transform.rotation *= Quaternion.Euler (0, 0, balance);
		Spine1.transform.rotation *= Quaternion.Euler (0, 0, balance);
		Spine2.transform.rotation *= Quaternion.Euler (0, 0, balance);
		Tail0.transform.rotation *= Quaternion.Euler (0, 0, -balance);
		Tail1.transform.rotation *= Quaternion.Euler (0, 0, -balance);
		Tail2.transform.rotation *= Quaternion.Euler (0, 0, -balance);
		Tail3.transform.rotation *= Quaternion.Euler (0, 0, -balance);
		Tail4.transform.rotation *= Quaternion.Euler (0, 0, -balance);
		Tail5.transform.rotation *= Quaternion.Euler (0, 0, -balance);
		Tail6.transform.rotation *= Quaternion.Euler (0, 0, -balance);
		Tail7.transform.rotation *= Quaternion.Euler (0, 0, -balance);
		Tail8.transform.rotation *= Quaternion.Euler (0, 0, -balance);


		//Disable collision and freeze position
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|Sitting") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|Rising") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|SitA") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|SitB") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|Sleep") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|Die") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Arge|Die-"))
			rg.isKinematic = true;
		else
			rg.isKinematic = false;
		rg.velocity = Vector3.zero;
		rg.freezeRotation = true;

		//Stick and slip on terrain
		rg.velocity = Vector3.zero;
		RaycastHit hit;
		int terrainlayer = 1 << 8; //terrain layer only
		if (Physics.Raycast (transform.position + transform.up, -transform.up, out hit, Mathf.Infinity, terrainlayer)) {
			//stick to the terrain
			transform.position = new Vector3 (transform.position.x, Mathf.Lerp (transform.position.y, hit.point.y, 1.0f), transform.position.z);
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation (Vector3.Cross (transform.right, hit.normal), hit.normal), 0.025f);
			
			//slip on sloped terrain and avoid
			float xs = 0, zs = 0;
			if (Mathf.DeltaAngle (transform.eulerAngles.x, 0.0f) > 10.0f || Mathf.DeltaAngle (transform.eulerAngles.x, 0.0f) < -10.0f ||
			    Mathf.DeltaAngle (transform.eulerAngles.z, 0.0f) > 10.0f || Mathf.DeltaAngle (transform.eulerAngles.z, 0.0f) < -10.0f) {
				xs = xs + (Time.deltaTime * -(Mathf.DeltaAngle (transform.eulerAngles.x, 0.0f) / 5));
				zs = zs + (Time.deltaTime * (Mathf.DeltaAngle (transform.eulerAngles.z, 0.0f) / 5));
				if (zs > 0)
					ang = Mathf.Lerp (ang, 2.0f, 0.025f);
				else
					ang = Mathf.Lerp (ang, -2.0f, 0.025f);
				transform.Translate (zs, 0, xs);
			}
		}
	
		//In game switch skin lod
		foreach (SkinnedMeshRenderer element in rend) {
			if (element.isVisible)
				infos = element.sharedMesh.triangles.Length / 3 + " triangles";
			element.materials [0].mainTexture = skin [BodySkin.GetHashCode ()];
			element.materials [1].mainTexture = eyes [EyesSkin.GetHashCode ()];
			lods.ForceLOD (LodLevel.GetHashCode ());
		}
		
		//Rescale model
		//	transform.localScale = new Vector3 (scale, scale, scale);
		//Mass based on scale
		rg.mass = 10.0f / 0.5f * scale;
	}

	//cele******inizio
	int idAnim;
	bool changeAnim;

	public void AnimaNext ()
	{

		idAnim = idAnim + 1;
		if (idAnim > 5) {
			idAnim = 1;
		}
		Debug.Log ("change Animation " + idAnim);
		changeAnim = true;

		//			anim.SetInteger ("Idle", 1); //Idle 1
		//
		//			anim.SetInteger ("Idle", 2); //Idle 2
		//		else if (Input.GetKey (KeyCode.Alpha3))
		//			anim.SetInteger ("Idle", 3); //Idle 3
		//		else if (Input.GetKey (KeyCode.Alpha4))
		//			anim.SetInteger ("Idle", 4); //Idle 4
		//		else if (Input.GetKey (KeyCode.Alpha5))
		//			anim.SetInteger ("Idle", 5); //Eat
		//		else if (Input.GetKey (KeyCode.Alpha6))
		//			anim.SetInteger ("Idle", 6); //Drink
		//		else if (Input.GetKey (KeyCode.Alpha7))
		//			anim.SetInteger ("Idle", 7); //Sleep
		//		else if (Input.GetKey (KeyCode.Alpha8))
		//			anim.SetInteger ("Idle", -1); //Die
		//		else
		//			anim.SetInteger ("Idle", 0);


	}

}