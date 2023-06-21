using UnityEngine;

public class dime_cs : MonoBehaviour
{
	Transform Spine0,Spine1,Spine2,Spine3,Neck0, Neck1,Neck2,Head, Tail0,Tail1,Tail2,Tail3,Tail4,Tail5,Tail6,Tail7,Tail8;
	float spineYaw,spinePitch,spineRoll,balance,ang,velZ,animcount;
	public AudioClip Medstep,Medsplash,Sniff2,Bite,Swallow,Largestep,Largesplash,Idlecarn,Dime1,Dime2,Dime3;
	public Texture[] skin,eyes;
	
	bool reset,soundplayed,onwater,isdead;
	Animator anim;
	AudioSource source;
	SkinnedMeshRenderer[] rend;
	LODGroup lods;
	Rigidbody rg;
	
	[Header("---------------------------------------")]
	public float Health=100;
	public float scale=0.25f;
	public skinselect BodySkin;
	public eyesselect EyesSkin;
	public lodselect LodLevel=lodselect.Auto;
	[HideInInspector]public string infos;
	public bool AI=false;
	
//***************************************************************************************
//Get components
	void Awake ()
	{
		Tail0 = transform.Find ("Dime/root/pelvis/tail0");
		Tail1 = transform.Find ("Dime/root/pelvis/tail0/tail1");
		Tail2 = transform.Find ("Dime/root/pelvis/tail0/tail1/tail2");
		Tail3 = transform.Find ("Dime/root/pelvis/tail0/tail1/tail2/tail3");
		Tail4 = transform.Find ("Dime/root/pelvis/tail0/tail1/tail2/tail3/tail4");
		Tail5 = transform.Find ("Dime/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5");
		Tail6 = transform.Find ("Dime/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6");
		Tail7 = transform.Find ("Dime/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7");
		Tail8 = transform.Find ("Dime/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7/tail8");
		Spine0 = transform.Find ("Dime/root/spine0");
		Spine1 = transform.Find ("Dime/root/spine0/spine1");
		Spine2 = transform.Find ("Dime/root/spine0/spine1/spine2");
		Spine3 = transform.Find ("Dime/root/spine0/spine1/spine2/spine3");
		Neck0 = transform.Find ("Dime/root/spine0/spine1/spine2/spine3/spine4/neck0");
		Neck1 = transform.Find ("Dime/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1");
		Neck2 = transform.Find ("Dime/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2");
		Head = transform.Find ("Dime/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/head");

		source = GetComponent<AudioSource>();
		anim = GetComponent<Animator>();
		lods=GetComponent<LODGroup>();
		rend=GetComponentsInChildren<SkinnedMeshRenderer>();
		rg=GetComponent<Rigidbody>();
		
		foreach (SkinnedMeshRenderer element in rend)
		{
			element.materials[0].mainTexture = skin[BodySkin.GetHashCode()];
			element.materials[1].mainTexture = eyes[EyesSkin.GetHashCode()];
		}

		
		transform.localScale=new Vector3(scale,scale,scale);
	}
	//***************************************************************************************
	//Check what is colliding
	void OnTriggerStay(Collider coll)
	{
		if(coll.transform.name=="Water") { anim.speed=0.75f; onwater=true; } //Is on water ?
	}
	void OnTriggerExit(Collider coll)
	{
		if(coll.transform.name=="Water") { anim.speed=1.0f; onwater=false; }
	}
	//***************************************************************************************
	//Animation controller
	void Update ()
	{
		if(AI) //CPU
		{

		}
		else //Human
		{
			//Moves
			if (Input.GetKey (KeyCode.Space) && Input.GetKey (KeyCode.W)) anim.SetInteger ("State", 2); //Steps forward
			else if (Input.GetKey (KeyCode.LeftShift) && Input.GetKey (KeyCode.W)) anim.SetInteger ("State", 3); //Run
			else if (Input.GetKey (KeyCode.W))anim.SetInteger ("State", 1); //Walk
			else if (Input.GetKey (KeyCode.Space) && Input.GetKey (KeyCode.S)) anim.SetInteger ("State", -2); //Steps backward
			else if (Input.GetKey (KeyCode.S)) anim.SetInteger ("State", -1); //Walk backward
			else if (Input.GetKey (KeyCode.A)) anim.SetInteger ("State", 10); //Steps Strafe+
			else if (Input.GetKey (KeyCode.D)) anim.SetInteger ("State", -10); //Steps Strafe-
			else anim.SetInteger ("State", 0); //back to loop

			//Turn
			if(Input.GetKey(KeyCode.A) && velZ!=0) ang = Mathf.Lerp(ang,-1.5f,0.05f);
			else if(Input.GetKey(KeyCode.D) && velZ!=0) ang = Mathf.Lerp(ang,1.5f,0.05f);
			else ang = Mathf.Lerp(ang,0.0f,0.1f);

			//Attack
			if (Input.GetKey (KeyCode.Mouse0)) anim.SetBool ("Attack", true);
			else anim.SetBool ("Attack", false);

			//Idles
			if (Input.GetKey (KeyCode.Alpha1)||Input.GetKey(KeyCode.E)) anim.SetInteger ("Idle", 1); //Idle 1
			else if (Input.GetKey (KeyCode.Alpha2)) anim.SetInteger ("Idle", 2); //Idle 2
			else if (Input.GetKey (KeyCode.Alpha3)) anim.SetInteger ("Idle", 3); //Idle 3
			else if (Input.GetKey (KeyCode.Alpha4)) anim.SetInteger ("Idle", 4); //Eat
			else if (Input.GetKey (KeyCode.Alpha5)) anim.SetInteger ("Idle", 5); //Drink
			else if (Input.GetKey (KeyCode.Alpha6)) anim.SetInteger ("Idle", 6); //Sleep
			else if (Input.GetKey (KeyCode.Alpha7)) anim.SetInteger ("Idle", -1); //Die
			else anim.SetInteger ("Idle", 0);

			//Reset spine position
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|EatA") ||
			    anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|EatB") ||
			    anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|EatC") ||
			    anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Stand2C") ||
			    anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Stand2ToSleep") ||
			    anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|SleepLoop")
			    )
				reset = true; else reset = false;

			//Spine control
			if (Input.GetKey (KeyCode.Mouse1) && reset == false)
			{
				spineYaw += Input.GetAxis ("Mouse X") * 2.0F;
				spinePitch += Input.GetAxis ("Mouse Y") * 2.0F;
			}
			else
			{
				spineYaw = Mathf.Lerp(spineYaw,0.0f,0.05f);
				spinePitch = Mathf.Lerp(spinePitch,0.0f,0.05f);
			}

		}

		//***************************************************************************************
		//Motions code
		
		//Walking
		if (anim.GetNextAnimatorStateInfo(0).IsName("Dime|Step1") ||
		    anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Step1") ||
		    anim.GetNextAnimatorStateInfo(0).IsName("Dime|Step2") ||
		    anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Step2") ||
		    anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Step2ToStand1") ||
		    anim.GetNextAnimatorStateInfo(0).IsName("Dime|Step2ToStand1")  ||
		    anim.GetNextAnimatorStateInfo(0).IsName("Dime|Step2ToWalk") ||
		    anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Step2ToWalk") ||
		    anim.GetNextAnimatorStateInfo(0).IsName("Dime|Walk") ||
		    anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Walk") ||
		    anim.GetNextAnimatorStateInfo(0).IsName("Dime|WalkGrowl") ||
		    anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|WalkGrowl"))
		{
			if (velZ < 0.1F) velZ = velZ + (Time.deltaTime * 0.5F);
			
			else if (velZ > 0.1F) velZ = velZ - (Time.deltaTime * 1.0F);

			if (anim.GetNextAnimatorStateInfo(0).IsName("Dime|Stand1A") ||
			    anim.GetNextAnimatorStateInfo(0).IsName("Dime|Stand2A"))
				velZ = velZ - (Time.deltaTime * 0.9F);

			if( anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Step1") ||
			   anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Step2"))
			{
				if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8)
					transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			}
			else transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));

			transform.Translate (0, 0, velZ*scale*anim.speed);
		}
		
		//Backward walk
		else if (anim.GetNextAnimatorStateInfo(0).IsName("Dime|Step1-") ||
		         anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Step1-") ||
		         anim.GetNextAnimatorStateInfo(0).IsName("Dime|Step2-") ||
		         anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Step2-") ||
		         anim.GetNextAnimatorStateInfo(0).IsName("Dime|Step1ToWalk-") ||
		         anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Step1ToWalk-") ||
		         anim.GetNextAnimatorStateInfo(0).IsName("Dime|Step2ToWalk-") ||
		         anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Step2ToWalk-") ||
		         anim.GetNextAnimatorStateInfo(0).IsName("Dime|Step1ToSleep") ||
		         anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Step1ToSleep") ||
		         anim.GetNextAnimatorStateInfo(0).IsName("Dime|Step1ToStand2") ||
		         anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Step1ToStand2") ||
		         anim.GetNextAnimatorStateInfo(0).IsName("Dime|Step2ToEatA") ||
		         anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Step2ToEatA") ||
		         anim.GetNextAnimatorStateInfo(0).IsName("Dime|Step2ToEatC") ||
		         anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Step2ToEatC") ||
		         anim.GetNextAnimatorStateInfo(0).IsName("Dime|Walk-") ||
		         anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Walk-"))
		{
			if (velZ > -0.05F) velZ = velZ - (Time.deltaTime * 0.5F);
			
			if (anim.GetNextAnimatorStateInfo(0).IsName("Dime|Stand1A") ||
			    anim.GetNextAnimatorStateInfo(0).IsName("Dime|Stand2A"))
				velZ = velZ + (Time.deltaTime * 0.6F); //deceleration

			if( anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Step1-") ||
			   anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Step2-"))
			{
				if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8)
					transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, -1, 0));
			}
			else transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, -1, 0));
			
			transform.Translate (0, 0, velZ*scale*anim.speed);
		}

		//Strafe+
		else if (anim.GetNextAnimatorStateInfo(0).IsName("Dime|Strafe1-") ||
		         anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Strafe1-")||
		         anim.GetNextAnimatorStateInfo(0).IsName("Dime|Strafe2+") ||
		         anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Strafe2+"))
		{
			velZ =0.01f;
			transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			transform.Translate (velZ*scale*anim.speed,0,0);
		}
		//Strafe-
		else if (anim.GetNextAnimatorStateInfo(0).IsName("Dime|Strafe1+") ||
		         anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Strafe1+") ||
		         anim.GetNextAnimatorStateInfo(0).IsName("Dime|Strafe2-") ||
		         anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Strafe2-")
		         )
		{
			velZ =0.01f;
			transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			transform.Translate (-velZ*scale*anim.speed,0,0);
		}
		
		//Running
		else if (anim.GetNextAnimatorStateInfo(0).IsName("Dime|Run") ||
		         anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Run") ||
		         anim.GetNextAnimatorStateInfo(0).IsName("Dime|WalkAttack") ||
		         anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|WalkAttack") ||
		         anim.GetNextAnimatorStateInfo(0).IsName("Dime|Step2ToRun") ||
		         anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Step2ToRun") ||
		         anim.GetNextAnimatorStateInfo(0).IsName("Dime|RunGrowl") ||
		         anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|RunGrowl"))
		{
			if (velZ < 0.3F) velZ = velZ + (Time.deltaTime * 1.5F);

			transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			transform.Translate (0, 0, velZ*scale*anim.speed);
		}

		//Step Attack
		else if (anim.GetNextAnimatorStateInfo(0).IsName("Dime|StepAttack1") ||
		         anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|StepAttack1") ||
		         anim.GetNextAnimatorStateInfo(0).IsName("Dime|StepAttack2") ||
		         anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|StepAttack2"))
		{
			if (velZ < 0.15F) velZ = velZ + (Time.deltaTime * 2.5F);
	
			if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5) velZ = 0.0F;

			transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			transform.Translate (0, 0, velZ*scale*anim.speed);
		}

		//Stop
		else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Stand1A") ||
		         anim.GetNextAnimatorStateInfo(0).IsName("Dime|Stand2A"))
		{
			velZ =0.0F;
			
			transform.Translate (0, 0, velZ*scale*anim.speed);
		}

		//***************************************************************************************
		//Sound Fx code
		
		//Get current animation point
		animcount = (anim.GetCurrentAnimatorStateInfo(0).normalizedTime) % 1.0F;
		if(anim.GetAnimatorTransitionInfo(0).normalizedTime!=0.0F) animcount=0.0F;
		animcount = Mathf.Round(animcount * 30);

		if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Walk") ||
		    anim.GetNextAnimatorStateInfo(0).IsName("Dime|Walk") ||
		    anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Walk-") ||
		    anim.GetNextAnimatorStateInfo(0).IsName("Dime|Walk-") ||
		    anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Step2ToWalk") ||
		    anim.GetNextAnimatorStateInfo(0).IsName("Dime|Step2ToWalk") ||
		    anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Step1ToWalk-") ||
		    anim.GetNextAnimatorStateInfo(0).IsName("Dime|Step1ToWalk-") ||
		    anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|WalkGrowl") ||
		    anim.GetNextAnimatorStateInfo(0).IsName("Dime|WalkGrowl") ||
		    anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Strafe1+") ||
		    anim.GetNextAnimatorStateInfo(0).IsName("Dime|Strafe1+") ||
		    anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Strafe1-") ||
		    anim.GetNextAnimatorStateInfo(0).IsName("Dime|Strafe1-") ||
		    anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Strafe2+") ||
		    anim.GetNextAnimatorStateInfo(0).IsName("Dime|Strafe2+") ||
		    anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Strafe2-") ||
		    anim.GetNextAnimatorStateInfo(0).IsName("Dime|Strafe2-"))
		{
			if(anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|WalkGrowl") ||
			   anim.GetNextAnimatorStateInfo(0).IsName("Dime|WalkGrowl"))
			{
				if(animcount==4 && soundplayed==false)
				{
					source.pitch=Random.Range(1.0F, 1.25F);
					source.PlayOneShot(Dime3,1.0f);
					soundplayed=true;
				}
			}
			if(soundplayed==false &&(animcount==8 || animcount==22))
			{
				source.pitch=Random.Range(0.8F, 1.0F);
				source.PlayOneShot(onwater?Medsplash:Medstep,0.75f);
				soundplayed=true;
			}
			else if(animcount!=4 && animcount!=8 && animcount!=22) soundplayed=false;
		}

		else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Step1") ||
		    anim.GetNextAnimatorStateInfo(0).IsName("Dime|Step1") ||
		    anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Step1-") ||
		    anim.GetNextAnimatorStateInfo(0).IsName("Dime|Step1-") ||
		    anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Step2") ||
		    anim.GetNextAnimatorStateInfo(0).IsName("Dime|Step2") ||
		    anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Step2-") ||
		    anim.GetNextAnimatorStateInfo(0).IsName("Dime|Step2-"))
		{
			if(animcount==20 && soundplayed==false)
			{
				source.pitch=Random.Range(0.75F, 1.0F);
				source.PlayOneShot(onwater?Medsplash:Medstep,0.5f);
				soundplayed=true;
			}
			else if( animcount!=20) soundplayed=false;
		}

		else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Run") ||
		         anim.GetNextAnimatorStateInfo(0).IsName("Dime|Run") ||
		         anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|RunGrowl") ||
		         anim.GetNextAnimatorStateInfo(0).IsName("Dime|RunGrowl"))
		{
			if(anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|RunGrowl") ||
			   anim.GetNextAnimatorStateInfo(0).IsName("Dime|RunGrowl"))
			{
				if(animcount==5 && soundplayed==false)
				{
					source.PlayOneShot(Dime2,1.0f);
					soundplayed=true;
				}
			}
			if(animcount==3 && soundplayed==false)
			{
				source.pitch=Random.Range(0.8F, 1.0F);
				source.PlayOneShot(Sniff2,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			if(soundplayed==false &&(animcount==8 || animcount==22))
			{
				source.PlayOneShot(onwater?Medsplash:Medstep,1.0f);
				soundplayed=true;
			}
			else if(animcount!=3 && animcount!=5 && animcount!=8 && animcount!=22) soundplayed=false;
		}

		else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|StepAttack1") ||
				anim.GetNextAnimatorStateInfo(0).IsName("Dime|StepAttack1") ||
				anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|StepAttack2") ||
				anim.GetNextAnimatorStateInfo(0).IsName("Dime|StepAttack2"))
		{
			if(animcount==4 && soundplayed==false)
			{
				source.pitch=Random.Range(0.75F, 1.0F);
				source.PlayOneShot(Dime2,1.0f);
				soundplayed=true;
			}
			if(animcount==11 && soundplayed==false)
			{
				source.PlayOneShot(Bite,1.0f);
				soundplayed=true;
			}
			else if(animcount!=4 && animcount!=11) soundplayed=false;
		}

		else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|WalkAttack") ||
		         anim.GetNextAnimatorStateInfo(0).IsName("Dime|WalkAttack"))
		{
			if(animcount==3 && soundplayed==false)
			{
				source.pitch=Random.Range(0.75F, 1.0F);
				source.PlayOneShot(Dime2,1.0f);
				soundplayed=true;
			}
			
			if(animcount==11 && soundplayed==false)
			{
				source.PlayOneShot(Bite,1.0f);
				soundplayed=true;
			}
			
			else if(animcount!=3 && animcount!=11) soundplayed=false;
		}

		else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|SleepLoop"))
		{
			if(animcount==5 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Idlecarn,0.5f);
				soundplayed=true;
			}
			else if(animcount!=5) soundplayed=false;
		}

		else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Stand1B") ||
		         anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Stand2B"))
		{
			if(animcount==5 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Dime1,1.0f);
				soundplayed=true;
			}
			else if(animcount!=5) soundplayed=false;
		}

		else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Stand1C"))
		{
			if(animcount==10 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.5F);
				source.PlayOneShot(Sniff2,0.5f);
				soundplayed=true;
			}
			if(animcount==15 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.5F);
				source.PlayOneShot(Sniff2,0.5f);
				soundplayed=true;
			}
			if(animcount==20&& soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.5F);
				source.PlayOneShot(Sniff2,0.5f);
				soundplayed=true;
			}
			else if(animcount!=10 && animcount!=15 && animcount!=20) soundplayed=false;
		}

		else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Stand2C"))
		{
			if(animcount==3 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.5F);
				source.PlayOneShot(Sniff2,0.5f);
				soundplayed=true;
			}
			else if(animcount!=3) soundplayed=false;
		}

		else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|EatA") ||
		         anim.GetNextAnimatorStateInfo(0).IsName("Dime|EatA"))
		{
			if(animcount==5 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.5F);
				source.PlayOneShot(Swallow,0.75f);
				soundplayed=true;
			}
			else if(animcount!=5) soundplayed=false;
		}

		else if (!isdead && (
				 anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Die1") ||
		         anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Die2")))
		{
			if(animcount==2 && soundplayed==false)
			{
				source.pitch=Random.Range(0.75F, 1.0F);
				source.PlayOneShot(Dime1,1.0f);
				soundplayed=true;
			}
			else if(animcount==22 && soundplayed==false)
			{
				source.PlayOneShot(onwater?Largesplash:Largestep,1.0f);
				soundplayed=true;
			}
			else if(animcount!=2 && animcount!=22) soundplayed=false;

			if(animcount>25) isdead=true;
		}


		else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Rise1") ||
			anim.GetCurrentAnimatorStateInfo(0).IsName("Dime|Rise2"))
		{
			if(animcount==5 && soundplayed==false)
			{
				source.pitch=Random.Range(0.75F,1.25F);
				source.PlayOneShot(Dime3,1.0f);
				soundplayed=true;
			}

			else if(animcount!=5) soundplayed=false;
			isdead=false;
		}

	}

	//***************************************************************************************
	//Additionals bone rotations
	void LateUpdate()
	{
		spineYaw = Mathf.Clamp (spineYaw, -30.0F, 30.0F);
		spinePitch = Mathf.Clamp(spinePitch, -15.0F, 20.0F);

		balance = Mathf.Lerp(balance,-ang*4,0.05f);
		spineRoll = spineYaw*spinePitch/32;
		
		//Neck and head
		Neck0.transform.rotation *= Quaternion.Euler(spinePitch, -spineRoll, -spineYaw+balance);
		Neck1.transform.rotation *= Quaternion.Euler(spinePitch, -spineRoll, -spineYaw+balance);
		Neck2.transform.rotation *= Quaternion.Euler(spinePitch, -spineRoll, -spineYaw+balance);
		Head.transform.rotation *= Quaternion.Euler(spinePitch, -spineRoll, -spineYaw+balance);
		
		//Spine and tail
		Spine0.transform.rotation *= Quaternion.Euler(0, 0, balance);
		Spine1.transform.rotation *= Quaternion.Euler(0, 0, balance);
		Spine2.transform.rotation *= Quaternion.Euler(0, 0, balance);
		Spine3.transform.rotation *= Quaternion.Euler(0, 0, balance);
		Tail0.transform.rotation *= Quaternion.Euler(0, 0, -balance);
		Tail1.transform.rotation *= Quaternion.Euler(0, 0, -balance);
		Tail2.transform.rotation *= Quaternion.Euler(0, 0, -balance);
		Tail3.transform.rotation *= Quaternion.Euler(0, 0, -balance);
		Tail4.transform.rotation *= Quaternion.Euler(0, 0, -balance);
		Tail5.transform.rotation *= Quaternion.Euler(0, 0, -balance);
		Tail6.transform.rotation *= Quaternion.Euler(0, 0, -balance);
		Tail7.transform.rotation *= Quaternion.Euler(0, 0, -balance);
		Tail8.transform.rotation *= Quaternion.Euler(0, 0, -balance);

		//Disable collision and freeze position
		if (anim.GetCurrentAnimatorStateInfo (0).IsName("Dime|Die1")||
		    anim.GetCurrentAnimatorStateInfo (0).IsName("Dime|Die2")||
		    anim.GetCurrentAnimatorStateInfo (0).IsName("Dime|Rise1")||
		    anim.GetCurrentAnimatorStateInfo (0).IsName("Dime|Rise2")||
		    anim.GetCurrentAnimatorStateInfo (0).IsName("Dime|Stand2ToSleep")||
		    anim.GetCurrentAnimatorStateInfo (0).IsName("Dime|Stand2ToSleep-")||
		    anim.GetCurrentAnimatorStateInfo (0).IsName("Dime|SleepLoop")) rg.isKinematic=true; else rg.isKinematic=false;
		rg.velocity=Vector3.zero; rg.freezeRotation=true;
		
		//Stick and slip on terrain
		rg.velocity=Vector3.zero;
		RaycastHit hit; int terrainlayer=1<<8; //terrain layer only
		if (Physics.Raycast(transform.position+transform.up, -transform.up, out hit, Mathf.Infinity,terrainlayer))
		{
			//stick to the terrain
			transform.position=new Vector3(transform.position.x,Mathf.Lerp(transform.position.y,hit.point.y,1.0f),transform.position.z);
			transform.rotation=Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(Vector3.Cross(transform.right, hit.normal), hit.normal), 0.025f);
			
			//slip on sloped terrain and avoid
			float xs=0,zs=0;
			if(Mathf.DeltaAngle(transform.eulerAngles.x,0.0f)>15.0f||Mathf.DeltaAngle(transform.eulerAngles.x,0.0f)<-15.0f||
			   Mathf.DeltaAngle(transform.eulerAngles.z,0.0f)>15.0f||Mathf.DeltaAngle(transform.eulerAngles.z,0.0f)<-15.0f)
			{
				xs=xs+(Time.deltaTime * -(Mathf.DeltaAngle(transform.eulerAngles.x,0.0f)/5));
				zs=zs+(Time.deltaTime * (Mathf.DeltaAngle(transform.eulerAngles.z,0.0f)/5));
				if(zs>0)ang = Mathf.Lerp(ang,2.0f,0.025f); else ang = Mathf.Lerp(ang,-2.0f,0.025f);
				transform.Translate(zs ,0,xs);
			}
		}
		
		//In game switch skin lod
		foreach (SkinnedMeshRenderer element in rend)
		{
			if(element.isVisible) infos = element.sharedMesh.triangles.Length/3+" triangles";
			element.materials[0].mainTexture = skin[BodySkin.GetHashCode()];
			element.materials[1].mainTexture = eyes[EyesSkin.GetHashCode()];
			lods.ForceLOD(LodLevel.GetHashCode());
		}
		transform.localScale=new Vector3(scale,scale,scale);
		rg.mass = 3.0f/0.5f*scale;
	}
}



