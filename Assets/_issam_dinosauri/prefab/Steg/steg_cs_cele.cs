using UnityEngine;

public class steg_cs_cele : MonoBehaviour
{
	Transform Spine0, Spine1, Spine2, Neck0, Neck1, Neck2, Neck3, Head, Tail0, Tail1, Tail2, Tail3, Tail4, Tail5, Tail6, Tail7, Tail8;
	float spineYaw, spinePitch, spineRoll, balance, ang, velZ, animcount;
	public AudioClip Medstep, Medsplash, Sniff2, Chew, Largestep, Largesplash, Idleherb, Steg1, Steg2, Steg3;
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
	public Camera arCam;
	
	//***************************************************************************************
	//Get components
	void Awake ()
	{
		Tail0 = transform.Find ("Steg/root/pelvis/tail0");
		Tail1 = transform.Find ("Steg/root/pelvis/tail0/tail1");
		Tail2 = transform.Find ("Steg/root/pelvis/tail0/tail1/tail2");
		Tail3 = transform.Find ("Steg/root/pelvis/tail0/tail1/tail2/tail3");
		Tail4 = transform.Find ("Steg/root/pelvis/tail0/tail1/tail2/tail3/tail4");
		Tail5 = transform.Find ("Steg/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5");
		Tail6 = transform.Find ("Steg/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6");
		Tail7 = transform.Find ("Steg/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7");
		Tail8 = transform.Find ("Steg/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7/tail8");
		Spine0 = transform.Find ("Steg/root/spine0");
		Spine1 = transform.Find ("Steg/root/spine0/spine1");
		Spine2 = transform.Find ("Steg/root/spine0/spine1/spine2");
		Neck0 = transform.Find ("Steg/root/spine0/spine1/spine2/spine3/spine4/neck0");
		Neck1 = transform.Find ("Steg/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1");
		Neck2 = transform.Find ("Steg/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2");
		Neck3 = transform.Find ("Steg/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3");
		Head = transform.Find ("Steg/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/head");

		source = GetComponent<AudioSource> ();
		anim = GetComponent<Animator> ();
		lods = GetComponent<LODGroup> ();
		rend = GetComponentsInChildren<SkinnedMeshRenderer> ();
		rg = GetComponent<Rigidbody> ();
		
		foreach (SkinnedMeshRenderer element in rend) {
			element.materials [0].mainTexture = skin [BodySkin.GetHashCode ()];
			element.materials [1].mainTexture = eyes [EyesSkin.GetHashCode ()];
		}

		transform.localScale = new Vector3 (scale, scale, scale);
        GameObject cameraObj = GameObject.Find("Camera");
        if (cameraObj == null)
            cameraObj = GameObject.Find("ARCamera");
        arCam = cameraObj.GetComponent<Camera>();
	}

    void OnEnable()
    {
        transform.localScale = new Vector3(scale, scale, scale);
    }

    void OnDisable()
    {

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
		//cele inizio
		scale = transform.localScale.x;
		//cele fine
		if (AI) { //CPU

		} else { //Human
//			//Moves
//			if (Input.GetKey (KeyCode.Space) && Input.GetKey (KeyCode.W)) anim.SetInteger ("State", 2); //Steps forward
//			else if (Input.GetKey (KeyCode.LeftShift) && Input.GetKey (KeyCode.W)) anim.SetInteger ("State", 3); //Run
//			else if (Input.GetKey (KeyCode.W))anim.SetInteger ("State", 1); //Walk
//			else if (Input.GetKey (KeyCode.Space) && Input.GetKey (KeyCode.S)) anim.SetInteger ("State", -2); //Steps backward
//			else if (Input.GetKey (KeyCode.S)) anim.SetInteger ("State", -1); //Walk backward
//			else if (Input.GetKey (KeyCode.A)) anim.SetInteger ("State", 10); //Strafe+
//			else if (Input.GetKey (KeyCode.D)) anim.SetInteger ("State", -10); //Strafe-
//			else if (Input.GetKey (KeyCode.Space)) anim.SetInteger ("State", 100); //Steps
//			else if (Input.GetKey (KeyCode.LeftControl)) anim.SetInteger ("State", -100); //Attack  pose
//			else anim.SetInteger ("State", 0); //back to loop

			//Moves  By Cele
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

//			//Turn
//			if(Input.GetKey(KeyCode.A)&& velZ!=0) ang = Mathf.Lerp(ang,-1.0f,0.05f);
//			else if(Input.GetKey(KeyCode.D)&& velZ!=0) ang = Mathf.Lerp(ang,1.0f,0.05f);
//			else ang = Mathf.Lerp(ang,0.0f,0.05f);

			//Turn by Cele ETC
			if (ETCInput.GetAxis ("Horizontal") > 0.2f && velZ != 0)
				ang = Mathf.Lerp (ang, -2.0f, 0.05f);
			else if (ETCInput.GetAxis ("Horizontal") < -0.2f && velZ != 0)
				ang = Mathf.Lerp (ang, 2.0f, 0.05f);
			else
				ang = Mathf.Lerp (ang, 0.0f, 0.05f);

			//Attack
//			if (Input.GetKey (KeyCode.Mouse0))
//				anim.SetBool ("Attack", true);
//			else
//				anim.SetBool ("Attack", false);
			
//			//Idles
//			if (Input.GetKey (KeyCode.Alpha1) || Input.GetKey (KeyCode.E)) anim.SetInteger ("Idle", 1); //Idle 1
//			else if (Input.GetKey (KeyCode.Alpha2)) anim.SetInteger ("Idle", 2); //Idle 2
//			else if (Input.GetKey (KeyCode.Alpha3)) anim.SetInteger ("Idle", 3); //Idle 3
//			else if (Input.GetKey (KeyCode.Alpha4)) anim.SetInteger ("Idle", 4); //Eat
//			else if (Input.GetKey (KeyCode.Alpha5)) anim.SetInteger ("Idle", 5); //Drink
//			else if (Input.GetKey (KeyCode.Alpha6)) anim.SetInteger ("Idle", 6); //Sleep
//			else if (Input.GetKey (KeyCode.Alpha7)) anim.SetInteger ("Idle", -1); //Die
//			else anim.SetInteger ("Idle", 0);

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
//
			//Spine control
//			if (Input.GetKey (KeyCode.Mouse1) && reset == false) {
//				spineYaw += Input.GetAxis ("Mouse X") * 2.0F;
//				spinePitch += Input.GetAxis ("Mouse Y") * 2.0F;
//			} else {
//				spineYaw = Mathf.Lerp (spineYaw, 0.0f, 0.05f);
//				spinePitch = Mathf.Lerp (spinePitch, 0.0f, 0.05f);
//			}

			//Reset spine
			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|EatC"))
				reset = true;
			else
				reset = false;
		}

		//***************************************************************************************
		//Motions code
		
		//Walking
		if (anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step1") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step1") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2ToWalk") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2ToWalk") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Walk") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Walk") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|WalkGrowl") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|WalkGrowl") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2ToEatA") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2ToEatA") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2ToEatC") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2ToEatC") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2ToStand1C") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2ToStand1C")) {	
			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step1") ||
			    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2")) {
				if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.8)
					transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			} else
				transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			
			if (velZ < 0.06F)
				velZ = velZ + (Time.deltaTime * 0.5F);
			else if (velZ > 0.06F)
				velZ = velZ - (Time.deltaTime * 1.0F);
			
			
			if (anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Stand1A") ||
			    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Stand2A") ||
			    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|EatC"))
				velZ = 0.0f;
			
			transform.Translate (0, 0, velZ * scale * anim.speed);
		}

		//Backward walk
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step1-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step1-") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2-") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step1ToWalk-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step1ToWalk-") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Walk-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Walk-") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2-ToSit") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2-ToSit") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2-ToSit") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2-ToStand2C") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2-ToStand2C")) {
			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step1-") ||
			    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2-")) {
				if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.8)
					transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, -1, 0));
			} else
				transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, -1, 0));
			
			if (velZ > -0.06F)
				velZ = velZ - (Time.deltaTime * 0.5F);
			if (anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Stand1A") ||
			    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Stand2A"))
				velZ = velZ + (Time.deltaTime * 0.6F);
			
			transform.Translate (0, 0, velZ * scale * anim.speed);
		}
		
		//Strafe-
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Strafe1+") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Strafe1+") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Strafe2-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Strafe2-")) {
			velZ = 0.01f;
			transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			transform.Translate (-velZ * scale * anim.speed, 0, 0);
		}

		//Strafe+
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Strafe1-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Strafe1-") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Strafe2+") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Strafe2+")) {
			velZ = 0.01f;
			transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			transform.Translate (velZ * scale * anim.speed, 0, 0);
		}

		//Running
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Run") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Run") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2ToRun") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2ToRun") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Steg|RunGrowl") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|RunGrowl")) {
			if (velZ < 0.25F)
				velZ = velZ + (Time.deltaTime * 1.0F);

			transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			transform.Translate (0, 0, velZ * scale * anim.speed);
		}

		//Attack
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand1ToAttackA") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Stand1ToAttackA") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand1ToAttackB") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Stand1ToAttackB")) {
			if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.8) {
				velZ = velZ + (Time.deltaTime * 0.3F);
				transform.rotation *= Quaternion.AngleAxis (2.75F, new Vector3 (0, -1, 0));
			} else
				velZ = 0.0F;
			transform.Translate (0, 0, velZ * scale * anim.speed);
		}

		//Stop
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand1A") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Stand2A") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Steg|AttackLoop")) {
			velZ = 0.0F;
			
			transform.Translate (0, 0, velZ * scale * anim.speed);
		}


		//***************************************************************************************
		//Sound Fx code
		
		//Get current animation point
		animcount = (anim.GetCurrentAnimatorStateInfo (0).normalizedTime) % 1.0F;
		if (anim.GetAnimatorTransitionInfo (0).normalizedTime != 0.0F)
			animcount = 0.0F;
		animcount = Mathf.Round (animcount * 30);

		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Walk") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Walk") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Walk-") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Walk-") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2ToWalk") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2ToWalk") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step1ToWalk-") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step1ToWalk-") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|WalkGrowl") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|WalkGrowl") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Strafe1+") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Strafe1+") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Strafe1-") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Strafe1-") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Strafe2+") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Strafe2+") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Strafe2-") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Strafe2-")) {
			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|WalkGrowl") ||
			    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|WalkGrowl")) {
				if (animcount == 4 && soundplayed == false) {
					source.pitch = Random.Range (1.0F, 1.25F);
					source.PlayOneShot (Steg2, 1.0f);
					soundplayed = true;
				}
			}
			if (soundplayed == false && (animcount == 8 || animcount == 22)) {
				source.pitch = Random.Range (0.8F, 1.0F);
				source.PlayOneShot (onwater ? Medsplash : Medstep, 0.5f);
				soundplayed = true;
			} else if (animcount != 4 && animcount != 8 && animcount != 22)
				soundplayed = false;
		} else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step1") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step1") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step1-") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step1-") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2-") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2-") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2ToEatA") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2ToEatA") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2ToEatC") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2ToEatC") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2ToAttackB") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2ToAttackB") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2-ToSit") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2-ToSit")) {
			if (animcount == 10 && soundplayed == false) {
				source.pitch = Random.Range (0.75F, 1.0F);
				source.PlayOneShot (onwater ? Medsplash : Medstep, 0.5f);
				soundplayed = true;
			} else if (animcount != 10)
				soundplayed = false;
		} else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Run") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Run") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|RunGrowl") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Steg|RunGrowl")) {
			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|RunGrowl") ||
			    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|RunGrowl")) {
				if (animcount == 4 && soundplayed == false) {
					source.pitch = Random.Range (0.8F, 1.25F);
					source.PlayOneShot (Steg3, 1.0f);
					soundplayed = true;
				}
			} else if (animcount == 3 && soundplayed == false) {
				source.pitch = Random.Range (0.8F, 1.0F);
				source.PlayOneShot (Sniff2, 0.5f);
				soundplayed = true;
			}
			if (soundplayed == false && (animcount == 8 || animcount == 22)) {
				source.pitch = Random.Range (0.8F, 1.0F);
				source.PlayOneShot (onwater ? Medsplash : Medstep, 0.75f);
				soundplayed = true;
			} else if (animcount != 3 && animcount != 4 && animcount != 8 && animcount != 22)
				soundplayed = false;
		} else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand1A") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand2A") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|SitLoop") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|SleepLoop") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|EatA") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|AttackLoop")) {
			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|EatA") &&
			    animcount == 20 && soundplayed == false) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (Chew, 0.5f);
				soundplayed = true;
			}
			if (animcount == 15 && soundplayed == false) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (Idleherb, 0.5f);
				soundplayed = true;
			} else if (animcount != 15 && animcount != 20)
				soundplayed = false;
		} else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|EatB")) {
			if (soundplayed == false &&
			    (animcount == 0 || animcount == 10 || animcount == 20)) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (Chew, 0.5f);
				soundplayed = true;
			} else if (animcount != 0 && animcount != 10 && animcount != 20)
				soundplayed = false;
		} else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand1B") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand2B")) {
			if (animcount == 5 && soundplayed == false) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (Steg1, 1.0f);
				soundplayed = true;
			} else if (animcount != 5)
				soundplayed = false;
		} else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand1C")) {
			if (animcount == 5 && soundplayed == false) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (Steg3, 1.0f);
				soundplayed = true;
			} else if (animcount != 5)
				soundplayed = false;
		} else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand2C")) {
			if (animcount == 5 && soundplayed == false) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (Sniff2, 0.75f);
				soundplayed = true;
			} else if (animcount != 5)
				soundplayed = false;
		} else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|SitGrowl")) {
			if (animcount == 1 && soundplayed == false) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (Steg1, 1.0f);
				soundplayed = true;
			} else if (animcount != 1)
				soundplayed = false;
		} else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|AttackC")) {
			if (animcount == 4 && soundplayed == false) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (Steg1, 1.0f);
				soundplayed = true;
			} else if (animcount == 5 && soundplayed == false) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (Sniff2, 0.75f);
				soundplayed = true;
			} else if (animcount != 4 && animcount != 5)
				soundplayed = false;
		} else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand1ToAttackA") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand2ToAttack")) {
			if (animcount == 4 && soundplayed == false) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (Steg1, 1.0f);
				soundplayed = true;
			} else if (animcount != 4)
				soundplayed = false;
		} else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand1ToAttackB")) {
			if (animcount == 3 && soundplayed == false) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (Steg2, 1.0f);
				soundplayed = true;
			} else if (animcount == 5 && soundplayed == false) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (Sniff2, 0.75f);
				soundplayed = true;
			} else if (animcount != 3 && animcount != 5)
				soundplayed = false;
		} else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|AttackLoopGrowl")) {
			if (animcount == 3 && soundplayed == false) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (Steg3, 1.0f);
				soundplayed = true;
			} else if (animcount == 10 && soundplayed == false) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (onwater ? Largesplash : Largestep, 1.0f);
				soundplayed = true;
			} else if (animcount != 3 && animcount != 10)
				soundplayed = false;
		} else if (!isdead && (
		               anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Die1") ||
		               anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Die2"))) {
			if (animcount == 5 && soundplayed == false) {
				source.pitch = Random.Range (0.5F, 0.75F);
				source.PlayOneShot (Steg3, 1.0f);
				soundplayed = true;
			} else if (animcount == 20 && soundplayed == false) {
				source.pitch = Random.Range (0.75F, 1.0F);
				source.PlayOneShot (onwater ? Largesplash : Largestep, 1.0f);
				soundplayed = true;
			} else if (animcount != 5 && animcount != 20)
				soundplayed = false;

			if (animcount > 20)
				isdead = true;
		} else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Rise1") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Rise2")) {
			if (animcount == 5 && soundplayed == false) {
				source.pitch = Random.Range (0.75F, 1.25F);
				source.PlayOneShot (Steg1, 1.0f);
				soundplayed = true;
			} else if (animcount != 5)
				soundplayed = false;
			isdead = false;
		}

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
					anim.SetBool ("Attack", true);
				}

			}

		} else {
			anim.SetBool ("Attack", false);
		}


	}

	//***************************************************************************************
	//Additionals bone rotationstions
	void LateUpdate ()
	{
		spineYaw = Mathf.Clamp (spineYaw, -30.0F, 30.0F);
		spinePitch = Mathf.Clamp (spinePitch, -10.0F, 15.0F);

		balance = Mathf.Lerp (balance, -ang * 8, 0.05f);
		spineRoll = spineYaw * spinePitch / 32;

		//Neck and head
		Neck0.transform.rotation *= Quaternion.Euler (spinePitch, -spineRoll, -spineYaw + balance);
		Neck1.transform.rotation *= Quaternion.Euler (spinePitch, -spineRoll, -spineYaw + balance);
		Neck2.transform.rotation *= Quaternion.Euler (spinePitch, -spineRoll, -spineYaw + balance);
		Neck3.transform.rotation *= Quaternion.Euler (spinePitch, -spineRoll, -spineYaw + balance);
		Head.transform.rotation *= Quaternion.Euler (spinePitch, -spineRoll, -spineYaw + balance);
		
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
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Die1") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Die2") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Rise1") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Rise2") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Sitting+") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Sitting-") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|SitLoop") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|SitGrowl") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|SleepLoop"))
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
		//transform.localScale = new Vector3 (scale, scale, scale);
		rg.mass = 10.0f / 0.5f * scale;
	}


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

	int skinId = 0;

	public void ChangeSkin ()
	{

		skinId = skinId + 1;
		if (skinId >= skin.Length) {
			skinId = 0;
		}
		Debug.Log ("Change Screen id: " + skinId.ToString ());

		//		foreach (SkinnedMeshRenderer element in rend) {
		//			if (element.isVisible)
		//				infos = element.sharedMesh.triangles.Length / 3 + " triangles";
		//			element.materials [0].mainTexture = skin [skinId];
		//			element.materials [1].mainTexture = eyes [skinId];
		//			lods.ForceLOD (LodLevel.GetHashCode ());
		//		}

		//skinMaterial.mainTexture = skin [skinId];
		foreach (SkinnedMeshRenderer element in rend) {
			Debug.Log ("element name : " + element.name);
			element.materials [0].mainTexture = skin [skinId];
			//	element.materials [1].mainTexture = eyes [EyesSkin.GetHashCode ()];
		}
	}

}




