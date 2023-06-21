using UnityEngine;

public class ptera_cs : MonoBehaviour
{
	
	Transform Root,WingR,WingL,Neck0,Neck1,Neck2,Neck3,Neck4,Neck5,Neck6,Head,
	Tail0,Tail1,Tail2,Tail3,Tail4,Tail5,Tail6,Tail7,Tail8,Tail9,Tail10,Tail11,Arm1,Arm2;
	float spineYaw,spinePitch,spineRoll,balance,flyRoll,flyPitch,ang,velZ,velY,animcount;
	public AudioClip Smallstep,Smallsplash,Idlecarn,Bite,Sniff2,Wind,Bigstep,Largesplash,Ptera1,Ptera2;
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
		Root = transform.Find ("Ptera/root");
		WingR = transform.Find ("Ptera/root/spine0/spine1/spine2/right wing0");
		WingL = transform.Find ("Ptera/root/spine0/spine1/spine2/left wing0");
		Neck0 = transform.Find ("Ptera/root/spine0/spine1/spine2/neck0");
		Neck1 = transform.Find ("Ptera/root/spine0/spine1/spine2/neck0/neck1");
		Neck2 = transform.Find ("Ptera/root/spine0/spine1/spine2/neck0/neck1/neck2");
		Neck3 = transform.Find ("Ptera/root/spine0/spine1/spine2/neck0/neck1/neck2/neck3");
		Neck4  = transform.Find ("Ptera/root/spine0/spine1/spine2/neck0/neck1/neck2/neck3/neck4");
		Neck5  = transform.Find ("Ptera/root/spine0/spine1/spine2/neck0/neck1/neck2/neck3/neck4/neck5");
		Neck6  = transform.Find ("Ptera/root/spine0/spine1/spine2/neck0/neck1/neck2/neck3/neck4/neck5/neck6");
		Head   = transform.Find ("Ptera/root/spine0/spine1/spine2/neck0/neck1/neck2/neck3/neck4/neck5/neck6/head");

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
	//Check collisions
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

			if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.W)) anim.SetInteger("State", 2); //fly up
			else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W)) anim.SetInteger("State", 3); //Run 
			else if (Input.GetKey(KeyCode.LeftShift))anim.SetInteger("State", -3);//fly down
			else if (Input.GetKey(KeyCode.Space))anim.SetInteger("State", 2);//fly up
			else if (Input.GetKey(KeyCode.W)) anim.SetInteger("State", 1); //Walk
			else if (Input.GetKey(KeyCode.S)) anim.SetInteger("State", -1); //Walk backward
			else if (Input.GetKey(KeyCode.A))anim.SetInteger("State", 10); // Strafe+ 
			else if (Input.GetKey(KeyCode.D))anim.SetInteger("State", -10); // Strafe-
			else anim.SetInteger("State", 0); //Idle

			//Turn
			if(Input.GetKey(KeyCode.A)&& velZ!=0) ang = Mathf.Lerp(ang,-2.0f,0.05f);
			else if(Input.GetKey(KeyCode.D)&& velZ!=0) ang = Mathf.Lerp(ang,2.0f,0.05f);
			else ang = Mathf.Lerp(ang,0.0f,0.05f);

			//Attack
			if (Input.GetKey(KeyCode.Mouse0)) anim.SetBool ("Attack", true);
			else anim.SetBool ("Attack", false);
			
			if (Input.GetKey (KeyCode.Alpha1)) anim.SetInteger ("Idle", 1); //Idle 1
			else if (Input.GetKey (KeyCode.Alpha2)) anim.SetInteger ("Idle", 2); //Idle 2
			else if (Input.GetKey (KeyCode.Alpha3)|| Input.GetKey (KeyCode.E)) anim.SetInteger ("Idle", 3); //Idle 3
			else if (Input.GetKey (KeyCode.Alpha4)) anim.SetInteger ("Idle", 4); //Eat
			else if (Input.GetKey (KeyCode.Alpha5)) anim.SetInteger ("Idle", 5); //Drink
			else if (Input.GetKey (KeyCode.Alpha6)) anim.SetInteger ("Idle", 6); //Sleep
			else if (Input.GetKey (KeyCode.Alpha7)) anim.SetInteger ("Idle", -1); //Die
			else anim.SetInteger ("Idle", 0);

			//Spine control
			if (Input.GetKey (KeyCode.Mouse1) && reset == false)
			{
				spineYaw += Input.GetAxis ("Mouse X") * 1.0F;
				spinePitch += Input.GetAxis ("Mouse Y") * 1.0F;
			}
			else
			{
				spineYaw = Mathf.Lerp(spineYaw,0.0f,0.05f);
				spinePitch = Mathf.Lerp(spinePitch,0.0f,0.05f);
			}

			//Reset Spine
			if (anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|StandB"))
				reset = true; else reset = false;
		}


		//***************************************************************************************
		//Motions code

		//Walking
		if (anim.GetNextAnimatorStateInfo (0).IsName("Ptera|Walk") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Walk"))
		{
			if (velZ < 0.1F) velZ = velZ + (Time.deltaTime * 1.0F);
			else if (velZ > 0.1F) velZ = velZ - (Time.deltaTime * 1.0F);

			transform.rotation *= Quaternion.AngleAxis (ang/2, new Vector3 (0, 1, 0));
			transform.Translate (0, 0, velZ*scale*anim.speed);
		}
		
		//Backward
		else if (anim.GetNextAnimatorStateInfo (0).IsName("Ptera|Walk-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Walk-"))
		{
			if (velZ > -0.075F) velZ = velZ - (Time.deltaTime * 1.0F);

			transform.rotation *= Quaternion.AngleAxis (ang/2, new Vector3 (0, -1, 0));
			transform.Translate (0, 0, velZ*scale*anim.speed);
		}
	
		//Strafe+
		else if (anim.GetNextAnimatorStateInfo (0).IsName("Ptera|Strafe+") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Strafe+"))
		{
			velZ=0.01f;
			transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			transform.Translate (-velZ*scale*anim.speed, 0, 0);
		}

		//Strafe-
		else if (anim.GetNextAnimatorStateInfo (0).IsName("Ptera|Strafe-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Strafe-"))
		{
			velZ=0.01f;
			transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			transform.Translate (velZ*scale*anim.speed, 0, 0);
		}

		//Running
		else if (anim.GetNextAnimatorStateInfo (0).IsName("Ptera|Run") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Run"))
		{
			if (velZ < 0.5F) velZ = velZ + (Time.deltaTime * 1.0F);

			transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			transform.Translate (0, 0, velZ*scale*anim.speed);
		}

		//Running to Fly
		else if (anim.GetNextAnimatorStateInfo (0).IsName("Ptera|RunToFlight") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|RunToFlight"))
		{
			velZ = 0.10f;
			velY = velY + (Time.deltaTime * 0.05F);
			transform.Translate (0, velY, velZ);
		}
		
		//Fly to Run
		else if (anim.GetNextAnimatorStateInfo (0).IsName("Ptera|FlightToRun") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|FlightToRun"))
		{
			flyRoll = Mathf.Lerp(flyRoll,0.0f,0.1f);
			flyPitch = Mathf.Lerp(flyPitch,0.0f,0.1f);

			if (velZ > 0.0f) velZ = velZ - (Time.deltaTime * 0.1F);
			velY = 0.0F;

			transform.Translate (0, velY, velZ);
		}

		//Stand
		else if (anim.GetNextAnimatorStateInfo (0).IsName("Ptera|StandA") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|StandA") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Ptera|Landing") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Landing"))
		{
			flyRoll = Mathf.Lerp(flyRoll,0.0f,0.1f);
			flyPitch = Mathf.Lerp(flyPitch,0.0f,0.1f);
			velZ = 0.0f;
			velY = 0.0f;

			transform.Translate (0, velY, velZ);
		}

		//Takeoff
		else if (anim.GetNextAnimatorStateInfo (0).IsName("Ptera|Takeoff") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Takeoff"))
		{
			velY = velY + (Time.deltaTime * 0.05F);
			transform.Translate (0, velY, velZ*scale*anim.speed);
		}

		//Fly - Stationary
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Stationary") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|StationaryUp") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|StationaryGrowl") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|AttackFly"))
		{
			flyRoll = Mathf.Lerp(flyRoll,ang*16,0.1f);
			flyPitch = Mathf.Lerp(flyPitch,0.0f,0.1f);

			if(anim.GetInteger("State") == 2) //fly up
				velY = velY + (Time.deltaTime * 0.1f); 
			else if(anim.GetInteger("State") == -3) //fly down
				velY = velY - (Time.deltaTime * 0.1f);
			else if(anim.GetInteger("State") == -1 && velZ>-0.1f) //fly backward
				velZ = velZ - (Time.deltaTime * 0.1f); 
			else if(anim.GetInteger("State")== 1) //fly forward
				velZ = velZ + (Time.deltaTime * 0.1f); 
			else velY = Mathf.Lerp(velY,0.0f,0.05f);

			velZ= Mathf.Lerp(velZ,0.0f,0.01f);

			transform.rotation *= Quaternion.AngleAxis (ang, new Vector3 (0, 1, 0));
			transform.Translate (0, velY, velZ);
		}

		//Fly
		else if (anim.GetNextAnimatorStateInfo (0).IsName("Ptera|Dive") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Dive") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Ptera|Flight") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Flight") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Ptera|FlightGrowl") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|FlightGrowl") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Ptera|Glide") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Glide") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Ptera|GlideGrowl") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|GlideGrowl"))
		{
			flyRoll = Mathf.Lerp(flyRoll,ang*16,0.1f); // flight roll
			flyPitch = Mathf.Lerp(flyPitch,velY*-256,0.1f); // flight pitch

			if(anim.GetInteger("State") == 2)
				velY = velY + (Time.deltaTime * 0.1f); //fly up
			else if(anim.GetInteger("State") == -3)
				velY = velY - (Time.deltaTime * 0.1f); //fly down
			else velY = Mathf.Lerp(velY,0.0f,0.05f);

			if (velZ < 0.15f) velZ = velZ + (Time.deltaTime * 2.0F);

			transform.rotation *= Quaternion.AngleAxis (ang/2, new Vector3 (0, 1, 0));
			transform.Translate (0, velY, velZ);
		}

		//Die
		else if (anim.GetNextAnimatorStateInfo (0).IsName("Ptera|Fall") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Fall"))
		{
			flyRoll = Mathf.Lerp(flyRoll,0.0f,0.01f);
			flyPitch = Mathf.Lerp(flyPitch,0.0f,0.01f);
			velY = Mathf.Lerp(velY,-0.5f,0.1f);
			velZ = Mathf.Lerp(velZ,0.0f,0.01f);

			transform.Translate (0, velY, velZ);
		}

		//***************************************************************************************
		//Sound Fx code
		
		//Get current animation point
		animcount = (anim.GetCurrentAnimatorStateInfo (0).normalizedTime) % 1.0F;
		if(anim.GetAnimatorTransitionInfo(0).normalizedTime!=0.0F) animcount=0.0F;
		animcount = Mathf.Round(animcount * 30);
		
		if (anim.GetNextAnimatorStateInfo (0).IsName("Ptera|Stationary") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Stationary") ||
		    anim.GetNextAnimatorStateInfo (0).IsName("Ptera|StationaryUp") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|StationaryUp") ||
		    anim.GetNextAnimatorStateInfo (0).IsName("Ptera|StationaryGrowl") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|StationaryGrowl") ||
		    anim.GetNextAnimatorStateInfo (0).IsName("Ptera|Landing") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Landing") ||
		    anim.GetNextAnimatorStateInfo (0).IsName("Ptera|Takeoff") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Takeoff"))
		{
			if (anim.GetNextAnimatorStateInfo (0).IsName("Ptera|Landing") ||
			    anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Landing"))
			{
				if(soundplayed==false &&(animcount==9))
				{
					source.pitch=Random.Range(0.9F, 1.0F);
					source.PlayOneShot(onwater?Smallsplash:Smallstep,1.0f);
					soundplayed=true;
				}
				
			}
			else if (anim.GetNextAnimatorStateInfo (0).IsName("Ptera|StationaryGrowl") ||
			         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|StationaryGrowl"))
			{
				if(soundplayed==false &&(animcount==5))
				{
					source.pitch=Random.Range(1.0F, 1.1F);
					source.PlayOneShot(Ptera2,1.0f);
					soundplayed=true;
				}
			}
			if(soundplayed==false &&(animcount==10))
			{
				source.pitch=Random.Range(1.0F, 1.1F);
				source.PlayOneShot(Sniff2,0.5f);
				soundplayed=true;
			}
			else if(animcount!=5 && animcount!=9 && animcount!=10) soundplayed=false;
		}
		
		else if (anim.GetNextAnimatorStateInfo (0).IsName("Ptera|Flight") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Flight") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Ptera|Glide") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Glide") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Ptera|GlideGrowl") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|GlideGrowl") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Ptera|FlightGrowl") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|FlightGrowl") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Ptera|Dive") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Dive"))
		{
			if (anim.GetNextAnimatorStateInfo (0).IsName("Ptera|Flight") ||
			    anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Flight"))
			{
				if(soundplayed==false &&(animcount==10))
				{
					source.pitch=Random.Range(1.0F, 1.1F);
					source.PlayOneShot(Sniff2,0.5f);
					soundplayed=true;
				}
			}
			
			else if (anim.GetNextAnimatorStateInfo (0).IsName("Ptera|GlideGrowl") ||
			         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|GlideGrowl") ||
			         anim.GetNextAnimatorStateInfo (0).IsName("Ptera|FlightGrowl") ||
			         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|FlightGrowl"))
			{
				if(soundplayed==false &&(animcount==5))
				{
					source.pitch=Random.Range(0.9F, 1.1F);
					source.PlayOneShot(Ptera2,1.0f);
					soundplayed=true;
				}
			}
			
			if(soundplayed==false &&(animcount==1))
			{
				source.pitch=Random.Range(1.0F, 1.1F);
				source.PlayOneShot(Wind,1.0f);
				soundplayed=true;
			}
			else if(animcount!=1 && animcount!=5 && animcount!=10) soundplayed=false;
		}
		
		else if (anim.GetNextAnimatorStateInfo (0).IsName("Ptera|Walk") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Walk") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Ptera|Strafe+") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Strafe+") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Ptera|Strafe-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Strafe-") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Ptera|Walk-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Walk-"))
		{
			if(soundplayed==false &&(animcount==10))
			{
				source.pitch=Random.Range(1.1F, 1.25F);
				source.PlayOneShot(onwater?Smallsplash:Smallstep,0.5F);
				soundplayed=true;
			}
			else if(soundplayed==false &&(animcount==25))
			{
				source.pitch=Random.Range(1.1F, 1.25F);
				source.PlayOneShot(onwater?Smallsplash:Smallstep,0.5f);
				soundplayed=true;
			}
			else if(animcount!=10 && animcount!=25) soundplayed=false;
		}

		else if (anim.GetNextAnimatorStateInfo (0).IsName("Ptera|Run") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Run") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Ptera|RunToFlight") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|RunToFlight") ||
		         anim.GetNextAnimatorStateInfo (0).IsName("Ptera|FlightToRun") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|FlightToRun"))
		{
			if(soundplayed==false &&(animcount==20))
			{
				source.pitch=Random.Range(1.1F, 1.25F);
				source.PlayOneShot(onwater?Smallsplash:Smallstep,0.75f);
				soundplayed=true;
			}
			else if(soundplayed==false &&(animcount==25))
			{
				source.pitch=Random.Range(1.1F, 1.25F);
				source.PlayOneShot(Sniff2,0.5f);
				soundplayed=true;
			}
			else if(animcount!=20 && animcount!=25) soundplayed=false;
		}

		else if (anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|StandGrowl"))
		{
			if(soundplayed==false &&(animcount==3))
			{
				source.pitch=Random.Range(0.9F, 1.25F);
				source.PlayOneShot(Ptera1,1.0f);
				soundplayed=true;
			}
			
			if(animcount!=3) soundplayed=false;
		}
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|AttackFly"))
		{
			if(soundplayed==false &&(animcount==10))
			{
				source.pitch=Random.Range(1.5F, 1.8F);
				source.PlayOneShot(Bite,1.0f);
				soundplayed=true;
			}
			
			if(animcount !=10) soundplayed=false;
		}
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|StandC"))
		{
			if(soundplayed==false &&(animcount==5))
			{
				source.pitch=Random.Range(0.9F, 1.25F);
				source.PlayOneShot(Ptera1,1.0f);
				soundplayed=true;
			}
			else if(soundplayed==false &&(animcount==8))
			{
				source.pitch=Random.Range(1.0F, 1.1F);
				source.PlayOneShot(Sniff2,0.5f);
				soundplayed=true;
			}
			if(animcount!=5 && animcount!=8) soundplayed=false;
		}
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|SleepLoop"))
		{
			if(soundplayed==false &&(animcount==15))
			{
				source.pitch=Random.Range(1.5F, 1.6F);
				source.PlayOneShot(Idlecarn,0.5f);
				soundplayed=true;
			}
			
			if(animcount!=15) soundplayed=false;
		}

		else if (anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Fall"))
		{
			
			if(soundplayed==false &&(animcount==3))
			{
				source.pitch=Random.Range(1.0F, 1.6F);
				source.PlayOneShot(Ptera2,1.0f);
				soundplayed=true;
			}
			
			if(animcount!=3) soundplayed=false;
		}

		else if (!isdead && anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Die1"))
		{
			
			if(soundplayed==false &&(animcount==3))
			{
				source.pitch=Random.Range(1.5F, 1.6F);
				source.PlayOneShot(Ptera1,1.0f);
				soundplayed=true;
			}
			
			if(soundplayed==false &&(animcount==25))
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(onwater?Largesplash:Bigstep,0.5f);
				soundplayed=true;
				isdead = true;
			}
			
			if(animcount!=3 && animcount!=25) soundplayed=false;
		}

		else if (!isdead && anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Die2"))
		{
			if(soundplayed==false &&(animcount==5))
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(onwater?Largesplash:Bigstep,1.0f);
				soundplayed=true;
				isdead = true;
			}
			
			if(animcount!=5) soundplayed=false;
		}

		else if (anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Rise"))
		{
			isdead = false;
			if(soundplayed==false &&(animcount==5))
			{
				source.pitch=Random.Range(1.0F, 1.1F);
				source.PlayOneShot(Ptera1,1.0f);
				soundplayed=true;
			}
			
			if(animcount!=5) soundplayed=false;
		}
		
	}

	//***************************************************************************************
	//Additionals bone rotations
	void LateUpdate()
	{
		spineRoll = spineYaw*spinePitch/24;
		balance = Mathf.Lerp(balance,ang*16,0.05f);
		flyPitch = Mathf.Clamp(flyPitch, -25.0F, 90.0F);
		balance = Mathf.Clamp(balance, -10.0F, 10.0F);
		spinePitch = Mathf.Clamp(spinePitch, -12.0F, 12.0F);
		spineYaw = Mathf.Clamp (spineYaw, -15.0F, 15.0F);

		//Root
		Root.transform.rotation *= Quaternion.Euler(flyRoll, flyPitch, 0);

		//Wings
		WingR.transform.rotation *= Quaternion.Euler(0, balance*1.5f, 0);
		WingL.transform.rotation *= Quaternion.Euler(0, -balance*1.5f, 0);
		
		//Neck and head
		Neck0.transform.rotation *= Quaternion.Euler(-spineRoll, -spinePitch, -spineYaw-balance);
		Neck1.transform.rotation *= Quaternion.Euler(-spineRoll, -spinePitch, -spineYaw-balance);
		Neck2.transform.rotation *= Quaternion.Euler(-spineRoll, -spinePitch, -spineYaw-balance);
		Neck3.transform.rotation *= Quaternion.Euler(-spineRoll, -spinePitch, -spineYaw-balance);
		Neck4.transform.rotation *= Quaternion.Euler(-spineRoll, -spinePitch, -spineYaw-balance);
		Neck5.transform.rotation *= Quaternion.Euler(-spineRoll, -spinePitch, -spineYaw-balance);
		Neck6.transform.rotation *= Quaternion.Euler(-spineRoll, -spinePitch, -spineYaw-balance);
		Head.transform.rotation *= Quaternion.Euler(-spineRoll, -spinePitch, -spineYaw-balance);

		//Disable collision and freeze position
		if (anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Die1")||
		    anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Die2")||
		    anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|StandToSleep")||
		    anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|SleepToStand")||
		    anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Rise")||
		    anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|SleepLoop")) rg.isKinematic=true; else rg.isKinematic=false;
		rg.velocity=Vector3.zero; rg.freezeRotation=true;

		//Stick and slip on terrain
		RaycastHit hit; int terrainlayer=1<<8; //terrain layer only
		if (Physics.Raycast(transform.position+transform.up, -transform.up, out hit, Mathf.Infinity,terrainlayer))
		{
			//Fly, disable stick to the terrain
			if(!anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Takeoff")&&
			   !anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Landing")&&
			   !anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|RunToFlight")&&
			   !anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|StationaryUp")&&
			   !anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Stationary")&&
			   !anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|StationaryGrowl")&&
			   !anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Dive")&&
			   !anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Glide")&&
			   !anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|GlideGrowl")&&
			   !anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Flight")&&
			   !anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|FlightGrowl")&&
			   !anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|AttackFly")&&
			   !anim.GetCurrentAnimatorStateInfo (0).IsName("Ptera|Fall"))
				transform.position=new Vector3(transform.position.x,Mathf.Lerp(transform.position.y,hit.point.y,0.1f),transform.position.z);
			
			//is on ground ?
			if(Mathf.Round(transform.position.y*10-hit.point.y*10)<=0)
			{
				anim.SetBool("Onground",true); 
				transform.rotation=Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(Vector3.Cross(transform.right, hit.normal), hit.normal), 0.1f);
			}
			else
			{
				anim.SetBool("Onground",false);
				transform.rotation=Quaternion.Lerp(transform.rotation ,Quaternion.Euler(0,transform.eulerAngles.y,0), 0.1f);
			}
			
			//slip on sloped terrain and avoid
			float xs=0,zs=0;
			if(Mathf.DeltaAngle(transform.eulerAngles.x,0.0f)>25.0f||Mathf.DeltaAngle(transform.eulerAngles.x,0.0f)<-25.0f||
			   Mathf.DeltaAngle(transform.eulerAngles.z,0.0f)>25.0f||Mathf.DeltaAngle(transform.eulerAngles.z,0.0f)<-25.0f)
			{
				xs=xs+(Time.deltaTime * -(Mathf.DeltaAngle(transform.eulerAngles.x,0.0f)/5));
				zs=zs+(Time.deltaTime * (Mathf.DeltaAngle(transform.eulerAngles.z,0.0f)/5));
				if(zs>0)ang = Mathf.Lerp(ang,2.0f,0.5f); else ang = Mathf.Lerp(ang,-2.0f,0.5f);
				transform.Translate(zs ,0,xs);
			}
		}
		
		//In game switch skin and lod
		foreach (SkinnedMeshRenderer element in rend)
		{
			if(element.isVisible) infos = element.sharedMesh.triangles.Length/3+" triangles";
			element.materials[0].mainTexture = skin[BodySkin.GetHashCode()];
			element.materials[1].mainTexture = eyes[EyesSkin.GetHashCode()];
			lods.ForceLOD(LodLevel.GetHashCode());
		}
		transform.localScale=new Vector3(scale,scale,scale);
		rg.mass = 1.0f/0.5f*scale;
	}
}










