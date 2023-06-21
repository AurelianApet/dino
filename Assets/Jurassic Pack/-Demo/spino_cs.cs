using UnityEngine;

public class spino_cs : MonoBehaviour
{
	Transform Spine0,Spine1,Spine2,Neck0,Neck1,Neck2,Head,Tail1,Tail2,Tail3,Tail4,Tail5,Tail6;
	float spineYaw,spinePitch,spineRoll,balance,ang,velZ,animcount;
	public AudioClip Bigstep,Largesplash,Idlecarn,Carngrowl1,Carngrowl2,Bite,Swallow,Sniff1,Spino1,Spino2;
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
		Tail1 = transform.Find ("Spino/root/tail0/tail1");
		Tail2 = transform.Find ("Spino/root/tail0/tail1/tail2");
		Tail3 = transform.Find ("Spino/root/tail0/tail1/tail2/tail3");
		Tail4 = transform.Find ("Spino/root/tail0/tail1/tail2/tail3/tail4");
		Tail5 = transform.Find ("Spino/root/tail0/tail1/tail2/tail3/tail4/tail5");
		Tail6 = transform.Find ("Spino/root/tail0/tail1/tail2/tail3/tail4/tail5/tail6");
		Spine0 = transform.Find ("Spino/root/spine0");
		Spine1 = transform.Find ("Spino/root/spine0/spine1");
		Spine2 = transform.Find ("Spino/root/spine0/spine1/spine2");
		Neck0 = transform.Find ("Spino/root/spine0/spine1/spine2/spine3/neck0");
		Neck1 = transform.Find ("Spino/root/spine0/spine1/spine2/spine3/neck0/neck1");
		Neck2 = transform.Find ("Spino/root/spine0/spine1/spine2/spine3/neck0/neck1/neck2");
		Head = transform.Find ("Spino/root/spine0/spine1/spine2/spine3/neck0/neck1/neck2/head");
		
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
			if (Input.GetKey(KeyCode.Space)) anim.SetInteger("State", 2); //Steps
			else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W)) anim.SetInteger("State", 3); //Run
			else if (Input.GetKey(KeyCode.W)) anim.SetInteger("State", 1); //Walk
			else if (Input.GetKey(KeyCode.S)) anim.SetInteger("State", -2); //Steps backward
			else if (Input.GetKey(KeyCode.A)) anim.SetInteger("State", 10); //Steps Strafe+
			else if (Input.GetKey(KeyCode.D)) anim.SetInteger("State", -10); //Steps Strafe-
			else anim.SetInteger("State", 0); //Idle
			
			//Attak
			if (Input.GetKey(KeyCode.Mouse0)) anim.SetBool("Attack", true);
			else anim.SetBool("Attack", false);
			
			//Turn
			if(Input.GetKey(KeyCode.A)&& velZ!=0)ang = Mathf.Lerp(ang,-2.0f,0.05f);
			else if(Input.GetKey(KeyCode.D)&& velZ!=0)ang = Mathf.Lerp(ang,2.0f,0.05f);
			else ang = Mathf.Lerp(ang,0.0f,0.05f);
			
			//Idles
			if (Input.GetKey (KeyCode.Alpha1) || Input.GetKey (KeyCode.E)) anim.SetInteger ("Idle", 1); //Idle 1
			else if (Input.GetKey (KeyCode.Alpha2)) anim.SetInteger ("Idle", 2); //Idle 2
			else if (Input.GetKey (KeyCode.Alpha3)) anim.SetInteger ("Idle", 3); //Idle 3
			else if (Input.GetKey (KeyCode.Alpha4)) anim.SetInteger ("Idle", 4); //Idle 4
			else if (Input.GetKey (KeyCode.Alpha5)) anim.SetInteger ("Idle", 5); //Eat
			else if (Input.GetKey (KeyCode.Alpha6)) anim.SetInteger ("Idle", 6); //Drink
			else if (Input.GetKey (KeyCode.Alpha7)) anim.SetInteger ("Idle", 7); //Sleep
			else if (Input.GetKey (KeyCode.Alpha8)) anim.SetInteger ("Idle", -1); //Die
			else anim.SetInteger ("Idle", 0);
			
			//Spine control
			if (Input.GetKey(KeyCode.Mouse1) && reset == false)
			{
				spineYaw += Input.GetAxis ("Mouse X") * 1.0F;
				spinePitch += -Input.GetAxis ("Mouse Y") * 1.0F;
			}
			else
			{
				spineYaw = Mathf.Lerp(spineYaw,0.0f,0.05f);
				spinePitch = Mathf.Lerp(spinePitch,0.0f,0.05f);
			}
			
			//Reset spine position
			if (anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|EatA") ||
			    anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|EatB") ||
			    anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|EatC") ||
			    anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Die1") ||
			    anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Die2") ||
			    anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|SleepLoop") ||
			    anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Stand2B") ||
			    anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Stand2D"))
				reset = true; else reset = false;
		}
		
		//***************************************************************************************
		//Motions code
		
		//Walking
		if (anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Walk") ||
		    anim.GetNextAnimatorStateInfo (0).IsName("Spino|Walk") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|WalkGrowl") ||
		    anim.GetNextAnimatorStateInfo (0).IsName("Spino|WalkGrowl") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|WalkToStand2") ||
		    anim.GetNextAnimatorStateInfo (0).IsName("Spino|WalkToStand2") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Stand1ToWalk") ||
		    anim.GetNextAnimatorStateInfo (0).IsName("Spino|Stand1ToWalk") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Stand2ToWalk") ||
		    anim.GetNextAnimatorStateInfo (0).IsName("Spino|Stand2ToWalk"))
		{
			if(!anim.GetNextAnimatorStateInfo (0).IsName("Spino|Stand1A") &&
			   !anim.GetNextAnimatorStateInfo (0).IsName("Spino|Stand2A"))
				transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			
			if (velZ < 0.3F)
				velZ = velZ + (Time.deltaTime * 0.5F);
			else if (velZ > 0.3F)
				velZ = velZ - (Time.deltaTime * 0.5F);
			
			if (anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|WalkToStand2") &&
			    anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.6)
				velZ =0.0F;
			
			
			if (anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|RunToStand1") &&
			    anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.7)
				velZ =0.0F;
			else if (anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|RunToStand1") && (velZ > 0.0F))
				velZ = velZ - (Time.deltaTime * 0.25F);
			
			if (anim.GetNextAnimatorStateInfo (0).IsName("Spino|EatA"))
				velZ =0.0F;
			
			transform.Translate (0, 0, velZ*scale*anim.speed);
		}
		
		//Backward steps
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Step1-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Step2-") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|Step2-") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|Step1-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Step1ToStand2B") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|Step1ToStand2B") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Step1ToStand2D") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|Step1ToStand2D") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Step1ToSleep") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|Step1ToSleep"))
		{
			if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.3 && anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.9)
				transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, -1, 0));
			
			if (velZ > -0.35F && anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.4 &&
			    anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.9)
				velZ = velZ - (Time.deltaTime * 0.5F);
			else velZ =0.0F;
			
			transform.Translate (0, 0, velZ*scale*anim.speed);
		}
		
		//Forward steps
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Step1+") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Step2+") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|Step2+") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|Step1+") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Step2ToStand1C") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|Step2ToStand1C") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Step2ToEatA") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|Step2ToEatA") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Step2ToEatC") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|Step2ToEatC"))
		{
			if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.6)
				transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			
			if (velZ < 0.35 && anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.55)
				velZ = velZ + (Time.deltaTime * 0.5F);
			else velZ =0.0F;
			
			transform.Translate (0, 0, velZ*scale*anim.speed);
		}
		
		//Attack Steps 
		else if (anim.GetNextAnimatorStateInfo (0).IsName("Spino|Step1Attack") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Step1Attack") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|Step2Attack") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Step2Attack"))
		{
			if (velZ < 0.5F) velZ = velZ + (Time.deltaTime * 1.5F);
			
			if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.6) velZ = 0.0F;
			
			transform.Translate (0, 0, velZ*scale*anim.speed);
		}
		
		//Strafe+
		else if (anim.GetNextAnimatorStateInfo (0).IsName("Spino|Strafe1+") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Strafe1+") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|Strafe2-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Strafe2-"))
		{
			velZ=0.1f;
			transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			transform.Translate (-velZ*scale*anim.speed,0,0);
		}
		
		//Strafe-
		else if (anim.GetNextAnimatorStateInfo (0).IsName("Spino|Strafe1-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Strafe1-") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|Strafe2+") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Strafe2+"))
		{
			velZ=0.1f;
			transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			transform.Translate (velZ*scale*anim.speed,0,0);
		}
		
		//Running
		else if (anim.GetNextAnimatorStateInfo (0).IsName("Spino|Run") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Run") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|RunAttackA") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|RunAttackA") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|RunAttackB") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|RunAttackB") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|WalkAttackA") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|WalkAttackA") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|WalkAttackB") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|WalkAttackB") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|RunGrowl") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|RunGrowl") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|Stand1ToRun") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Stand1ToRun") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|Stand2ToRun") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Stand2ToRun"))
		{
			transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			
			if (velZ > 0.6F && (anim.GetNextAnimatorStateInfo (0).IsName("Spino|RunGrowl") ||
			                    anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|RunGrowl")))
				velZ = velZ - (Time.deltaTime * 0.5F);
			
			if (velZ < 0.75F) velZ = velZ + (Time.deltaTime * 1.5F);
			
			transform.Translate (0, 0, velZ*scale*anim.speed);
		}
		
		//Stop
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Stand1A") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Stand2A"))
		{
			velZ =0.0F;
			transform.Translate (0, 0, 0);
		}
		
		//***************************************************************************************
		//Sound Fx code
		
		//Get current animation point
		animcount = (anim.GetCurrentAnimatorStateInfo (0).normalizedTime) % 1.0F;
		if(anim.GetAnimatorTransitionInfo(0).normalizedTime!=0.0F) animcount=0.0F;
		animcount = Mathf.Round(animcount * 30);
		
		if (anim.GetNextAnimatorStateInfo (0).IsName("Spino|Stand1B"))
		{
			if(animcount==0 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Spino1,1.0f);
				soundplayed=true;
			}
			else if(animcount!=0) soundplayed=false;
		}
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Stand1C"))
		{
			if(animcount==10 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Sniff1,0.4f);
				soundplayed=true;
			}
			if(animcount==15 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Sniff1,0.5f);
				soundplayed=true;
			}
			if(animcount==20 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Sniff1,0.6f);
				soundplayed=true;
			}
			else if(animcount!=10 && animcount!=15 && animcount!=20) soundplayed=false;
		}
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Stand2B"))
		{
			if(animcount==5 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Bite,0.4f);
				soundplayed=true;
			}
			if(animcount==10 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Bite,0.4f);
				soundplayed=true;
			}
			if(animcount==15 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Bite,0.4f);
				soundplayed=true;
			}
			else if(animcount!=5 && animcount!=10 && animcount!=15 ) soundplayed=false;
		}
		
		else if (anim.GetNextAnimatorStateInfo (0).IsName("Spino|Stand2C"))
		{
			if(animcount==0 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Spino2,1.0f);
				soundplayed=true;
			}
			else if(animcount!=0) soundplayed=false;
		}
		
		else if (anim.GetNextAnimatorStateInfo (0).IsName("Spino|Stand2D"))
		{
			if(animcount==0 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Carngrowl1,0.5f);
				soundplayed=true;
			}
			else if(animcount!=0) soundplayed=false;
		}
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|SleepLoop") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|EatA") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|EatA")
		         )
		{
			if (anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|EatA"))
			{
				if(animcount==5 && soundplayed==false)
				{
					source.pitch=Random.Range(1.0F, 1.25F);
					source.PlayOneShot(Carngrowl2,1.0F);
					soundplayed=true;
				}
			}
			
			if(animcount==15 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Idlecarn,0.5f);
				soundplayed=true;
			}
			else if(animcount!=5 && animcount!=15) soundplayed=false;
		}
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Strafe1+") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Strafe1-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Strafe2+") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Strafe2-"))
		{
			if(animcount==13 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(onwater?Largesplash:Bigstep,0.5f);
				soundplayed=true;
			}
			else if(animcount==25 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(onwater?Largesplash:Bigstep,0.5f);
				soundplayed=true;
			}
			else if(animcount!=13 && animcount!=25) soundplayed=false;
		}
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Stand1Attack") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Stand2Attack"))
		{
			if(animcount==13 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(onwater?Largesplash:Bigstep,0.5f);
				soundplayed=true;
			}
			else if(animcount==15 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Bite,1.0f);
				soundplayed=true;
			}
			else if(animcount==21 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(onwater?Largesplash:Bigstep,0.5f);
				soundplayed=true;
			}
			else if(animcount!=13 && animcount!=15 && animcount!=21) soundplayed=false;
		}
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Step1+")||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Step2+")||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Step2ToStand1C") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Step2ToEatA") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Step2ToEatC") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Step1Attack")||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Step2Attack"))
		{
			if (animcount==3 && soundplayed==false && (
				anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Step1Attack")||
				anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Step2Attack")))
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Carngrowl1,1.0f);
				soundplayed=true;
			}
			else if(animcount==15 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(onwater?Largesplash:Bigstep,0.5f);
				
				if (anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Step1Attack")||		  
				    anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Step2Attack"))
				{
					source.pitch=Random.Range(1.0F, 1.25F);
					source.PlayOneShot(Bite,0.75f);
				}
				soundplayed=true;
			}
			else if(animcount!=3 && animcount!=15 ) soundplayed=false;
		}
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Step1-")||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Step2-"))
		{
			if(animcount==25 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(onwater?Largesplash:Bigstep,0.5f);
				soundplayed=true;
			}
			else if(animcount!=25) soundplayed=false;
		}
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Stand1ToWalk"))
		{
			if(animcount==15 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(onwater?Largesplash:Bigstep,0.5f);
				soundplayed=true;
			}
			
			else if(animcount==27 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(onwater?Largesplash:Bigstep,0.5f);
				soundplayed=true;
			}
			
			else if(animcount!=15 && animcount!=27) soundplayed=false;
		}
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Stand2ToWalk"))
		{
			if(animcount==20 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(onwater?Largesplash:Bigstep,0.5f);
				soundplayed=true;
			}
			
			if(animcount!=20) soundplayed=false;
		}
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|WalkToStand2"))
		{
			if(animcount==15 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(onwater?Largesplash:Bigstep,0.5f);
				soundplayed=true;
			}
			
			else if(animcount!=15) soundplayed=false;
		}
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Stand1ToRun"))
		{
			if(animcount==13 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(onwater?Largesplash:Bigstep,0.5f);
				soundplayed=true;
			}
			else if(animcount==25 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(onwater?Largesplash:Bigstep,0.5f);
				soundplayed=true;
			}
			
			else if(animcount!=13&&animcount!=25) soundplayed=false;
		}
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Walk")||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|Walk")||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Run") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|Run") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|RunGrowl") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|RunGrowl") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|WalkGrowl") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|WalkGrowl") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|WalkAttackA") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|WalkAttackA") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|WalkAttackB") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|WalkAttackB") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|RunAttackA") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|RunAttackA") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|RunAttackB") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Spino|RunAttackB"))
		{
			if(animcount==0 && soundplayed==false)
			{
				source.PlayOneShot(onwater?Largesplash:Bigstep,0.75f);
				
				if(anim.GetNextAnimatorStateInfo (0).IsName("Spino|WalkGrowl"))
				{
					source.PlayOneShot(Spino1,1.0f);
				}
				
				else if(anim.GetNextAnimatorStateInfo (0).IsName("Spino|RunGrowl"))
				{
					source.PlayOneShot(Spino2,1.0f);
				}
				else if(anim.GetNextAnimatorStateInfo (0).IsName("Spino|WalkAttackA") ||
				        anim.GetNextAnimatorStateInfo (0).IsName("Spino|WalkAttackB") ||
				        anim.GetNextAnimatorStateInfo (0).IsName("Spino|RunAttackA") ||
				        anim.GetNextAnimatorStateInfo (0).IsName("Spino|RunAttackB"))
				{
					source.PlayOneShot(Carngrowl1,1.0f);
				}
				soundplayed=true;
			}
			
			else if(animcount==15 && soundplayed==false)
			{
				source.pitch=Random.Range (1.0F,1.25F);
				source.PlayOneShot(onwater?Largesplash:Bigstep,0.75f);
				soundplayed=true;
			}
			else if(animcount==20 && soundplayed==false && (
				anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|WalkAttackA") ||
				anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|WalkAttackB") ||
				anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|RunAttackA") ||
				anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|RunAttackB")))
			{
				source.pitch=Random.Range (1.0F,1.25F);
				source.PlayOneShot(Bite,0.75f);
				soundplayed=true;
			}
			
			else if(animcount!=0 && animcount!=15 && animcount!=20) soundplayed=false;
		}
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|EatB"))
		{
			if(animcount==1 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Swallow,0.5f);
				soundplayed=true;
			}
			else if(animcount!=1) soundplayed=false;
		}
		
		else if (!isdead && (anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Die1") ||
		                     anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Die2")))
		{
			if(animcount==0 && soundplayed==false)
			{
				source.pitch=Random.Range(0.5F, 0.5F);
				source.PlayOneShot(Carngrowl1,1.0f);
				soundplayed=true;
			}
			else if(animcount==20 && soundplayed==false)
			{
				source.pitch=Random.Range(0.9F, 0.9F);
				source.PlayOneShot(onwater?Largesplash:Bigstep,1.0f);
				soundplayed=true;
			}
			else if(animcount!=0 && animcount!=20 ) soundplayed=false;
			if (animcount>20) isdead=true;
		}
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Rise1") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Rise2"))
		{
			isdead=false;
			if(animcount==2 && soundplayed==false)
			{
				
				source.pitch=Random.Range(0.75F, 0.75F);
				source.PlayOneShot(Carngrowl2,1.0f);
				soundplayed=true;
			}
			else if(animcount!=2) soundplayed=false;
		}
	}
	
	//***************************************************************************************
	//Additionals bone rotations
	void LateUpdate()
	{
		spineRoll = spineYaw*spinePitch/24;
		balance = Mathf.Lerp(balance,ang*8,0.05f);
		
		spineYaw = Mathf.Clamp(spineYaw, -25.0F, 25.0F);
		spinePitch = Mathf.Clamp(spinePitch, -12.0F, 12.0F);
		
		//Spine and neck
		Spine0.transform.rotation *= Quaternion.Euler(-spinePitch, 0, 0);
		Spine1.transform.rotation *= Quaternion.Euler(-spinePitch, 0, -spineYaw-balance);
		Spine2.transform.rotation *= Quaternion.Euler(-spinePitch, 0, -spineYaw-balance);
		Neck0.transform.rotation *= Quaternion.Euler(-spinePitch, spineRoll, -spineYaw-balance);
		Neck1.transform.rotation *= Quaternion.Euler(-spinePitch, spineRoll, -spineYaw-balance);
		Neck2.transform.rotation *= Quaternion.Euler(-spinePitch, spineRoll, -spineYaw-balance);
		Head.transform.rotation *= Quaternion.Euler(-spinePitch, spineRoll, -spineYaw-balance);
		
		//Tail
		Tail1.transform.rotation *= Quaternion.Euler(0, 0, balance);
		Tail2.transform.rotation *= Quaternion.Euler(0, 0, balance);
		Tail3.transform.rotation *= Quaternion.Euler(0, 0, balance);
		Tail4.transform.rotation *= Quaternion.Euler(0, 0, balance);
		Tail5.transform.rotation *= Quaternion.Euler(0, 0, balance);
		Tail6.transform.rotation *= Quaternion.Euler(0, 0, balance);
		
		//Disable collision and freeze position
		if (anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Die1")||
		    anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Die2")||
		    anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Rise1")||
		    anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Rise2")||
		    anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|Stand2ToSleep")||
		    anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|SleepToStand2")||
		    anim.GetCurrentAnimatorStateInfo (0).IsName("Spino|SleepLoop")) rg.isKinematic=true; else rg.isKinematic=false;
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
			if(Mathf.DeltaAngle(transform.eulerAngles.x,0.0f)>10.0f||Mathf.DeltaAngle(transform.eulerAngles.x,0.0f)<-10.0f||
			   Mathf.DeltaAngle(transform.eulerAngles.z,0.0f)>10.0f||Mathf.DeltaAngle(transform.eulerAngles.z,0.0f)<-10.0f)
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
		rg.mass = 10.0f/0.5f*scale;
	}
	
}