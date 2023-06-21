using UnityEngine;

public class rap_cs_cele_old: MonoBehaviour
{
	Transform Spine0, Spine1, Spine2, Spine3, Spine4, Spine5, Neck0, Neck1, Neck2, Neck3, Head,
		Tail0, Tail1, Tail2, Tail3, Tail4, Tail5, Tail6, Tail7, Tail8, Tail9, Tail10, Tail11, Arm1, Arm2;
	float spineYaw, spinePitch, spineRoll, balance, ang, velY, velZ, animcount;
	public AudioClip Smallstep, Smallsplash, Swallow, Sniff1, Bite, Rap1, Rap2, Rap3, Rap4, Rap5, Rap6;
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
	
	//***************************************************************************************
	//Get components
	void Awake ()
	{
		Tail0 = transform.Find ("Rap/root/pelvis/tail0");
		Tail1 = transform.Find ("Rap/root/pelvis/tail0/tail1");
		Tail2 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2");
		Tail3 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2/tail3");
		Tail4 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2/tail3/tail4");
		Tail5 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5");
		Tail6 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6");
		Tail7 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7");
		Tail8 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7/tail8");
		Tail9 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7/tail8/tail9");
		Tail10 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7/tail8/tail9/tail10");
		Tail11 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7/tail8/tail9/tail10/tail11");
		Spine0 = transform.Find ("Rap/root/spine0");
		Spine1 = transform.Find ("Rap/root/spine0/spine1");
		Spine2 = transform.Find ("Rap/root/spine0/spine1/spine2");
		Spine3 = transform.Find ("Rap/root/spine0/spine1/spine2/spine3");
		Spine4 = transform.Find ("Rap/root/spine0/spine1/spine2/spine3/spine4");
		Spine5 = transform.Find ("Rap/root/spine0/spine1/spine2/spine3/spine4/spine5");
		Arm1 = transform.Find ("Rap/root/spine0/spine1/spine2/spine3/spine4/spine5/left arm0");
		Arm2 = transform.Find ("Rap/root/spine0/spine1/spine2/spine3/spine4/spine5/right arm0");
		Neck0 = transform.Find ("Rap/root/spine0/spine1/spine2/spine3/spine4/spine5/neck0");
		Neck1 = transform.Find ("Rap/root/spine0/spine1/spine2/spine3/spine4/spine5/neck0/neck1");
		Neck2 = transform.Find ("Rap/root/spine0/spine1/spine2/spine3/spine4/spine5/neck0/neck1/neck2");
		Neck3 = transform.Find ("Rap/root/spine0/spine1/spine2/spine3/spine4/spine5/neck0/neck1/neck2/neck3");
		Head = transform.Find ("Rap/root/spine0/spine1/spine2/spine3/spine4/spine5/neck0/neck1/neck2/neck3/head");
	
		source = GetComponent<AudioSource> ();
		anim = GetComponent<Animator> ();
		lods = GetComponent<LODGroup> ();
		rend = GetComponentsInChildren<SkinnedMeshRenderer> ();
		rg = GetComponent<Rigidbody> ();

		//Start texture
		foreach (SkinnedMeshRenderer element in rend) {
			element.materials [0].mainTexture = skin [BodySkin.GetHashCode ()];
			element.materials [1].mainTexture = eyes [EyesSkin.GetHashCode ()];
		}

		//Start Scale
		transform.localScale = new Vector3 (scale, scale, scale);

	}
	//***************************************************************************************
	//Check collisions
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
		if (AI) { //CPU

		} else { //Human
			//Moves
//			if (Input.GetKey (KeyCode.Space))
//				anim.SetInteger ("State", 2); //Jump
//			else if (Input.GetKey (KeyCode.LeftShift) && Input.GetKey (KeyCode.W))
//				anim.SetInteger ("State", 4); //Run
//			else if (Input.GetKey (KeyCode.LeftControl) && Input.GetKey (KeyCode.W))
//				anim.SetInteger ("State", 3); //Steps
//			else if (Input.GetKey (KeyCode.W))
//				anim.SetInteger ("State", 1); //Walk
//			else if (Input.GetKey (KeyCode.S))
//				anim.SetInteger ("State", -1); //Steps Back
//			else if (Input.GetKey (KeyCode.A))
//				anim.SetInteger ("State", 10); //Strafe+
//			else if (Input.GetKey (KeyCode.D))
//				anim.SetInteger ("State", -10); //Strafe-
//			else if (Input.GetKey (KeyCode.LeftControl))
//				anim.SetInteger ("State", -4); //Steps Idle
//			else
//				anim.SetInteger ("State", 0); //Idle



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
			

			//Turn
			if (Input.GetKey (KeyCode.A) && velZ != 0)
				ang = Mathf.Lerp (ang, -2.0f, 0.05f);
			else if (Input.GetKey (KeyCode.D) && velZ != 0)
				ang = Mathf.Lerp (ang, 2.0f, 0.05f);
			else
				ang = Mathf.Lerp (ang, 0.0f, 0.05f);
		
			//Attack
			if (Input.GetKey (KeyCode.Mouse0))
				anim.SetBool ("Attack", true);
			else
				anim.SetBool ("Attack", false);

			//Idles
//			if (Input.GetKey (KeyCode.Alpha1))
//				anim.SetInteger ("Idle", 1); //Idle looking
//			else if (Input.GetKey (KeyCode.Alpha2) || Input.GetKey (KeyCode.E))
//				anim.SetInteger ("Idle", 2); //Idle growl
//			else if (Input.GetKey (KeyCode.Alpha3))
//				anim.SetInteger ("Idle", 3); //Idle bark
//			else if (Input.GetKey (KeyCode.Alpha4))
//				anim.SetInteger ("Idle", 4); //Idle scrabble
//			else if (Input.GetKey (KeyCode.Alpha5))
//				anim.SetInteger ("Idle", 5); //Eat
//			else if (Input.GetKey (KeyCode.Alpha6))
//				anim.SetInteger ("Idle", 6); //Drink
//			else if (Input.GetKey (KeyCode.Alpha7))
//				anim.SetInteger ("Idle", 7); //Sleep
//			else if (Input.GetKey (KeyCode.Alpha8))
//				anim.SetInteger ("Idle", -1); //Kill
//			else if (anim.GetInteger ("Idle") != 100)
//				anim.SetInteger ("Idle", 0);



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



			//Reset spine position
			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|SleepLoop") ||
			    anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|Die") ||
			    anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|StandE") ||
			    anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|EatA") ||
			    anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|EatB") ||
			    anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|StandEat") ||
			    anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|GroundAttack"))
				reset = true;
			else
				reset = false;

			//Spine control
			if (Input.GetKey (KeyCode.Mouse1) && reset == false) {
				spineYaw += Input.GetAxis ("Mouse X") * 1.0F;
				spinePitch += Input.GetAxis ("Mouse Y") * 1.0F;
			} else {
				spineYaw = Mathf.Lerp (spineYaw, 0.0f, 0.05f);
				spinePitch = Mathf.Lerp (spinePitch, 0.0f, 0.05f);
			}
		}

		//***************************************************************************************
		//Motions code

		//Walking
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|Walk") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Rap|Walk") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|WalkGrowl") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Rap|WalkGrowl") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|WalkToStand") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Rap|RunToStand") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|RunToStand") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Rap|WalkToStand") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|StandToWalk") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Rap|StandToWalk")) {
			if (velZ < 0.2F)
				velZ = velZ + (Time.deltaTime * 0.25F); //acceleration
			else if (velZ > 0.2F)
				velZ = velZ - (Time.deltaTime * 2.0F); //deceleration

			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|WalkToStand") && velZ > 0.0F)
				velZ = velZ - (Time.deltaTime * 0.5F); //deceleration

			transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			transform.Translate (0, 0, velZ * scale * anim.speed);
		}

		//Forward steps
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|Steps+") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|StepsGrowl+")) {
			if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.25F && anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.45F)
				velZ = 0.0F;
			else if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.7F)
				velZ = 0.0F;
			else {
				velZ = 0.1F;
				transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			}
			
			transform.Translate (0, 0, velZ * scale * anim.speed);
		}
		
		//Backward steps
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|Steps-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|StepsGrowl-")) {
			if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.2F)
				velZ = 0.0F;
			else if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.5F &&
			         anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.75F)
				velZ = 0.0F;
			else {
				velZ = -0.1F;
				transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, -1, 0));
			}
			
			transform.Translate (0, 0, velZ * scale * anim.speed);
		}
		
		//Strafe-/Turn
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Rap|Strafe-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|Strafe-")) {
			//if (velZ < 0.075F) velZ = velZ + (Time.deltaTime * 0.5F); //acceleration
			//else if (velZ > 0.075F) velZ = velZ - (Time.deltaTime * 0.5F); //acceleration
			velZ = 0.01f;
			transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			transform.Translate (velZ * scale * anim.speed, velY, 0);
		}
		
		//Strafe+/Turn
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Rap|Strafe+") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|Strafe+")) {
			//if (velZ < 0.075F) velZ = velZ + (Time.deltaTime * 0.5F); //acceleration
			//else if (velZ > 0.075F)velZ = velZ - (Time.deltaTime * 0.5F); //acceleration
			velZ = 0.01f;
			transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			transform.Translate (-velZ * scale * anim.speed, velY, 0);
		}

		//Running
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Rap|Run") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|Run") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Rap|RunGrowl") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|RunGrowl")) {
			if (velZ < 0.75F)
				velZ = velZ + (Time.deltaTime * 1.0F);

			transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			transform.Translate (0, 0, velZ * scale * anim.speed);
		}

		//Running attack
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|RunAttackB") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Rap|RunAttackB")) {
			if (velZ < 0.5F)
				velZ = velZ + (Time.deltaTime * 1.0F);

			transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			transform.Translate (0, 0, velZ * scale * anim.speed);
		}

		//Jump Attack
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|JumpAttack") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Rap|JumpAttack") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|RunAttackA") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Rap|RunAttackA") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|OnDinoStand") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Rap|OnDinoStand") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|OnDinoAttack") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Rap|OnDinoAttack")) {

			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|RunAttackA")) {
				if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.8)
					velZ = 0.0F;
				else if (velZ < 0.75F)
					velZ = velZ + (Time.deltaTime * 1.0F);

				transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
				transform.Translate (0, 0, velZ * scale * anim.speed);
			} else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|JumpAttack")) {
				if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.4 && anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.9)
					velZ = velZ + (Time.deltaTime * 1.0F);
				else
					velZ = 0.0F;

				transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
				transform.Translate (0, 0, velZ * scale * anim.speed);
			}
		}

		//Stand jump up
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|StandJumpUp") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Rap|StandJumpUp")) {
			velZ = Mathf.Lerp (velZ, 0.0f, 0.01f);
			
			if (anim.GetInteger ("State") == 1 && velZ < 0.5f)
				velZ = velZ + (Time.deltaTime * 2.5F);
			if (anim.GetInteger ("State") == -1 && velZ > 0.5f)
				velZ = velZ - (Time.deltaTime * 2.0F);

			if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.5 && velY <= 0.5f)
				velY = velY + (Time.deltaTime * 10.0F);
			
			transform.Translate (0, velY * scale, velZ * scale);
		}

		//Running jump up
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|RunJumpUp") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Rap|RunJumpUp")) {
			if (anim.GetInteger ("State") == 1 && velZ < 0.75f)
				velZ = velZ + (Time.deltaTime * 2.5F);
			if (anim.GetInteger ("State") == -1 && velZ > 0.0f)
				velZ = velZ - (Time.deltaTime * 2.5F);

			if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.5 && velY <= 0.5f)
				velY = velY + (Time.deltaTime * 10.0F);
			
			transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			transform.Translate (0, velY * scale, velZ * scale * anim.speed);
		}

		//Jump loop
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|JumpLoop") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Rap|JumpLoop") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|JumpLoopAttack") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Rap|JumpLoopAttack")) {
			velY = Mathf.Lerp (velY, -2.0f, 0.025f);

			if (anim.GetInteger ("State") == 1 && velZ < 0.75f)
				velZ = velZ + (Time.deltaTime * 2.5F);
			if (anim.GetInteger ("State") == -1 && velZ > 0.0f)
				velZ = velZ - (Time.deltaTime * 2.5F);

			transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			transform.Translate (0, velY * scale, velZ * scale * anim.speed);
		}

		//Jump landing
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|StandJumpDown") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Rap|StandJumpDown") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|RunJumpDown") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Rap|RunJumpDown")) {
			velY = 0;

			if (velZ < 0.75F &&
			    anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|RunJumpDown") ||
			    anim.GetNextAnimatorStateInfo (0).IsName ("Rap|RunJumpDown"))
				velZ = velZ + (Time.deltaTime * 5.0F);
			else
				velZ = 0.0F;
			
			transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			transform.Translate (0, velY, velZ * scale * anim.speed);
		}


		//Stop
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|StandA") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|StepsStand")) {
			velZ = 0.0F;
			transform.Translate (0, 0, 0);
		}


		//***************************************************************************************
		//Sound Fx code
		
		//Get current animation point
		animcount = (anim.GetCurrentAnimatorStateInfo (0).normalizedTime) % 1.0F;
		if (anim.GetAnimatorTransitionInfo (0).normalizedTime != 0.0F)
			animcount = 0.0F;
		animcount = Mathf.Round (animcount * 30);

		if (anim.GetNextAnimatorStateInfo (0).IsName ("Rap|StandB") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|StandB")) {
			if (soundplayed == false && animcount == 15) {
				source.pitch = Random.Range (1.1F, 1.25F);
				source.PlayOneShot (Sniff1, 0.1F);
				soundplayed = true;
			} else if (animcount != 15)
				soundplayed = false;
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Rap|StandC") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|StandC")) {
			if (soundplayed == false && animcount == 2) {
				source.pitch = Random.Range (0.9F, 1.1F);
				source.PlayOneShot (Rap5, 0.75F);
				soundplayed = true;
			} else if (animcount != 2)
				soundplayed = false;
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Rap|StandD") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|StandD")) {
			if (soundplayed == false && (animcount == 1 || animcount == 8 || animcount == 16)) {
				source.pitch = Random.Range (0.9F, 1.1F);
				source.PlayOneShot (Rap4, 0.75F);
				soundplayed = true;
			} else if (animcount != 1 && animcount != 8 && animcount != 16)
				soundplayed = false;
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Rap|StandE") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|StandE")) {
			if (soundplayed == false && (animcount == 6 || animcount == 12 || animcount == 18)) {
				source.pitch = Random.Range (0.9F, 1.1F);
				source.PlayOneShot (Bite, 0.25F);
				soundplayed = true;
			} else if (animcount != 6 && animcount != 12 && animcount != 18)
				soundplayed = false;
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Rap|EatA") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|EatA")) {
			if (soundplayed == false && animcount == 16) {
				source.pitch = Random.Range (1.0F, 1.5F);
				source.PlayOneShot (Bite, 0.75F);
				soundplayed = true;
			} else if (animcount != 16)
				soundplayed = false;
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Rap|StandEat") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|StandEat")) {
			if (soundplayed == false && animcount == 3) {
				source.pitch = Random.Range (2.0F, 2.5F);
				source.PlayOneShot (Swallow, 0.5F);
				soundplayed = true;
			} else if (animcount != 3)
				soundplayed = false;
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Rap|AttackA") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|AttackA") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Rap|AttackB") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|AttackB")) {
			if (soundplayed == false && (animcount == 2)) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (Rap1, 0.75F);
				soundplayed = true;
			} else if (soundplayed == false && (animcount == 12)) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (Bite, 0.75F);
				soundplayed = true;
			} else if (animcount != 2 && animcount != 12)
				soundplayed = false;
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Rap|RunAttackA") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|RunAttackA")) {
			if (soundplayed == false && (animcount == 2)) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (Rap2, 0.75F);
				soundplayed = true;
			} else if (soundplayed == false && (animcount == 15)) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (Bite, 0.75F);
				soundplayed = true;
			} else if (animcount != 2 && animcount != 15)
				soundplayed = false;
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Rap|RunAttackB") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|RunAttackB")) {
			if (soundplayed == false && (animcount == 3)) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (Rap1, 0.75F);
				soundplayed = true;
			} else if (soundplayed == false && (animcount == 15)) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (Bite, 0.75F);
				soundplayed = true;
			} else if (animcount != 3 && animcount != 15)
				soundplayed = false;
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Rap|JumpLoopAttack") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|JumpLoopAttack")) {
			if (soundplayed == false && (animcount == 15)) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (Bite, 0.5F);
				soundplayed = true;
			} else if (animcount != 15)
				soundplayed = false;
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Rap|JumpAttack") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|JumpAttack")) {
			if (soundplayed == false && (animcount == 3)) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (Rap3, 0.75F);
				soundplayed = true;
			} else if (animcount != 3)
				soundplayed = false;
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Rap|OnDinoAttack") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|OnDinoAttack") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Rap|GroundAttack") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|GroundAttack")) {
			if (soundplayed == false && (animcount == 3)) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (Rap1, 0.75F);
				soundplayed = true;
			} else if (soundplayed == false && (animcount == 5)) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (Bite, 0.75F);
				soundplayed = true;
			} else if (animcount != 3 && animcount != 5)
				soundplayed = false;

		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Rap|Walk") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|Walk") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Rap|WalkGrowl") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|WalkGrowl")) {

			if (soundplayed == false && animcount == 2 && (
			        anim.GetNextAnimatorStateInfo (0).IsName ("Rap|WalkGrowl") ||
			        anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|WalkGrowl"))) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (Rap6, 0.75F);
				soundplayed = true;
			}


			if (soundplayed == false && (animcount == 10 || animcount == 25)) {
				source.pitch = Random.Range (1.1F, 1.25F);
				source.PlayOneShot (onwater ? Smallsplash : Smallstep, 0.25F);
				soundplayed = true;
			} else if (animcount != 2 && animcount != 10 && animcount != 25)
				soundplayed = false;
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Rap|WalkToStand") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|WalkToStand") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Rap|StandToWalk") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|StandToWalk") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Rap|RunToStand") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|RunToStand")) {
			if (soundplayed == false && (animcount == 15 || animcount == 25)) {
				source.pitch = Random.Range (1.1F, 1.25F);
				source.PlayOneShot (onwater ? Smallsplash : Smallstep, 0.25F);
				soundplayed = true;
			} else if (animcount != 15 && animcount != 25)
				soundplayed = false;

		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Rap|Steps-") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|Steps-")) {
			if (soundplayed == false && (animcount == 12 || animcount == 26)) {
				source.pitch = Random.Range (1.1F, 1.25F);
				source.PlayOneShot (onwater ? Smallsplash : Smallstep, 0.1F);
				soundplayed = true;
			} else if (animcount != 12 && animcount != 26)
				soundplayed = false;
			
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Rap|Steps+") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|Steps+")) {
			if (soundplayed == false && (animcount == 5 || animcount == 20)) {
				source.pitch = Random.Range (1.1F, 1.25F);
				source.PlayOneShot (onwater ? Smallsplash : Smallstep, 0.1F);
				soundplayed = true;
			} else if (animcount != 2 && animcount != 5 && animcount != 20)
				soundplayed = false;
			
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Rap|Strafe-") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|Strafe-") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Rap|Strafe+") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|Strafe+")) {
			if (soundplayed == false && (animcount == 12 || animcount == 26)) {
				source.pitch = Random.Range (1.1F, 1.25F);
				source.PlayOneShot (onwater ? Smallsplash : Smallstep, 0.25F);
				soundplayed = true;
			} else if (animcount != 12 && animcount != 26)
				soundplayed = false;
			
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Rap|StepsGrowl+") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|StepsGrowl+") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Rap|StepsGrowl-") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|StepsGrowl-")) {
			if (soundplayed == false && animcount == 4) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (Rap5, 0.75F);
				soundplayed = true;
			} else if (animcount != 4)
				soundplayed = false;
			
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Rap|Run") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|Run") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Rap|RunGrowl") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|RunGrowl")) {
			
			if (soundplayed == false && animcount == 2 && (
			        anim.GetNextAnimatorStateInfo (0).IsName ("Rap|RunGrowl") ||
			        anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|RunGrowl"))) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (Rap6, 0.75F);
				soundplayed = true;
			} 
	
			if (soundplayed == false && (animcount == 10 || animcount == 25)) {
				source.pitch = Random.Range (1.1F, 1.25F);
				source.PlayOneShot (onwater ? Smallsplash : Smallstep, 0.5F);
				soundplayed = true;
			} else if (animcount != 2 && animcount != 10 && animcount != 25)
				soundplayed = false;
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Rap|StandJumpUp") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|StandJumpUp") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Rap|RunJumpUp") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|RunJumpUp")) {
			if (soundplayed == false && animcount == 4) {
				source.pitch = Random.Range (1.5F, 2.0F);
				source.PlayOneShot (Sniff1, 0.5F);
				soundplayed = true;
			} else if (animcount != 4)
				soundplayed = false;
			
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Rap|StandJumpDown") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|StandJumpDown") ||
		           anim.GetNextAnimatorStateInfo (0).IsName ("Rap|RunJumpDown") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|RunJumpDown")) {
			if (soundplayed == false && animcount == 4) {
				source.pitch = Random.Range (0.75F, 1.0F);
				source.PlayOneShot (onwater ? Smallsplash : Smallstep, 0.75F);
				soundplayed = true;
			} else if (animcount != 4)
				soundplayed = false;
			
		} else if (!isdead && (anim.GetNextAnimatorStateInfo (0).IsName ("Rap|Die") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|Die"))) {
			if (soundplayed == false && animcount == 4) {
				source.pitch = Random.Range (0.75F, 1.0F);
				source.PlayOneShot (Rap6, 0.75F);
				soundplayed = true;
			}
			if (soundplayed == false && animcount == 20) {
				source.PlayOneShot (onwater ? Smallsplash : Smallstep, 0.75F);
				soundplayed = true;
			} else if (animcount != 4 && animcount != 20)
				soundplayed = false;

			if (animcount > 20)
				isdead = true;
		} else if (anim.GetNextAnimatorStateInfo (0).IsName ("Rap|Rise") ||
		           anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|Rise")) {
			isdead = false;

			if (soundplayed == false && animcount == 1) {
				source.pitch = Random.Range (1.0F, 1.25F);
				source.PlayOneShot (Rap2, 0.75F);
				soundplayed = true;
			} else if (animcount != 1)
				soundplayed = false;
		}

	}


	int idAnim;
	bool changeAnim;





	//***************************************************************************************
	//Bone rotations, model modification and stick to the terrain
	void LateUpdate ()
	{
		spineYaw = Mathf.Clamp (spineYaw, -16.0F, 16.0F);
		spinePitch = Mathf.Clamp (spinePitch, -9.0F, 9.0F);

		balance = Mathf.Lerp (balance, -ang * 4, 0.05f);
		spineRoll = spineYaw * spinePitch / 24;


		//Spine/neck/head rotations
		Spine0.transform.rotation *= Quaternion.Euler (-spinePitch, spineRoll, -spineYaw + balance);
		Spine1.transform.rotation *= Quaternion.Euler (-spinePitch, spineRoll, -spineYaw + balance);
		Spine2.transform.rotation *= Quaternion.Euler (-spinePitch, spineRoll, -spineYaw + balance);
		Spine3.transform.rotation *= Quaternion.Euler (-spinePitch, spineRoll, -spineYaw + balance);
		Spine4.transform.rotation *= Quaternion.Euler (-spinePitch, spineRoll, -spineYaw + balance);
		Spine5.transform.rotation *= Quaternion.Euler (-spinePitch, spineRoll, -spineYaw + balance);
			
		Neck0.transform.rotation *= Quaternion.Euler (-spinePitch, spineRoll, -spineYaw + balance);
		Neck1.transform.rotation *= Quaternion.Euler (-spinePitch, spineRoll, -spineYaw + balance);
		Neck2.transform.rotation *= Quaternion.Euler (-spinePitch, spineRoll, -spineYaw + balance);
		Neck3.transform.rotation *= Quaternion.Euler (-spinePitch, spineRoll, -spineYaw + balance);
		Head.transform.rotation *= Quaternion.Euler (-spinePitch, spineRoll, -spineYaw + balance);

		//Tail rotations
		Tail0.transform.rotation *= Quaternion.Euler (0, 0, -balance);
		Tail1.transform.rotation *= Quaternion.Euler (0, 0, -balance);
		Tail2.transform.rotation *= Quaternion.Euler (0, 0, -balance);
		Tail3.transform.rotation *= Quaternion.Euler (0, 0, -balance);
		Tail4.transform.rotation *= Quaternion.Euler (0, 0, -balance);
		Tail5.transform.rotation *= Quaternion.Euler (0, 0, -balance);
		Tail6.transform.rotation *= Quaternion.Euler (0, 0, -balance);
		Tail7.transform.rotation *= Quaternion.Euler (0, 0, -balance);
		Tail8.transform.rotation *= Quaternion.Euler (0, 0, -balance);
		Tail9.transform.rotation *= Quaternion.Euler (0, 0, -balance);
		Tail10.transform.rotation *= Quaternion.Euler (0, 0, -balance);
		Tail11.transform.rotation *= Quaternion.Euler (0, 0, -balance);

		//Arms balance
		Arm1.transform.rotation *= Quaternion.Euler (spinePitch * 8, 0, 0);
		Arm2.transform.rotation *= Quaternion.Euler (0, spinePitch * 8, 0);

		//Disable collision and freeze position
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|SleepLoop") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|Sleep+") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|Sleep-") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|Die"))
			rg.isKinematic = true;
		else
			rg.isKinematic = false;
		rg.velocity = Vector3.zero;
		rg.freezeRotation = true;

		//Stick and slip on terrain
		RaycastHit hit;
		int terrainlayer = 1 << 8; //terrain layer only
		if (anim.GetInteger ("Idle") != 100 && Physics.Raycast (transform.position + transform.up, -transform.up, out hit, Mathf.Infinity, terrainlayer)) {
			//jump, disable stick to the terrain
			if (!anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|StandJumpUp") &&
			    !anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|RunJumpUp") &&
			    !anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|JumpLoop") &&
			    !anim.GetCurrentAnimatorStateInfo (0).IsName ("Rap|JumpLoopAttack"))
				transform.position = new Vector3 (transform.position.x, Mathf.Lerp (transform.position.y, hit.point.y, 0.1f), transform.position.z);
			
			//is on ground ?
			if (Mathf.Round (transform.position.y * 10 - hit.point.y * 10) <= 0) {
				anim.SetBool ("Onground", true); 
				transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation (Vector3.Cross (transform.right, hit.normal), hit.normal), 0.1f);
			} else {
				anim.SetBool ("Onground", false);
				transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (0, transform.eulerAngles.y, 0), 0.1f);
			}
			
			//slip on sloped terrain and avoid
			float xs = 0, zs = 0;
			if (Mathf.DeltaAngle (transform.eulerAngles.x, 0.0f) > 25.0f || Mathf.DeltaAngle (transform.eulerAngles.x, 0.0f) < -25.0f ||
			    Mathf.DeltaAngle (transform.eulerAngles.z, 0.0f) > 25.0f || Mathf.DeltaAngle (transform.eulerAngles.z, 0.0f) < -25.0f) {
				xs = xs + (Time.deltaTime * -(Mathf.DeltaAngle (transform.eulerAngles.x, 0.0f) / 5));
				zs = zs + (Time.deltaTime * (Mathf.DeltaAngle (transform.eulerAngles.z, 0.0f) / 5));
				if (zs > 0)
					ang = Mathf.Lerp (ang, 2.0f, 0.5f);
				else
					ang = Mathf.Lerp (ang, -2.0f, 0.5f);
				transform.Translate (zs, 0, xs);
			}
		}

		//In game switch skin and lod
		foreach (SkinnedMeshRenderer element in rend) {
			if (element.isVisible)
				infos = element.sharedMesh.triangles.Length / 3 + " triangles";
			element.materials [0].mainTexture = skin [BodySkin.GetHashCode ()];
			element.materials [1].mainTexture = eyes [EyesSkin.GetHashCode ()];
			lods.ForceLOD (LodLevel.GetHashCode ());
		}

		//Rescale model
		transform.localScale = new Vector3 (scale, scale, scale);
		//Mass based on scale
		rg.mass = 4.0f / 0.5f * scale;
	}

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