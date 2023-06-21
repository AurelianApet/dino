using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using System.IO;

//using System;
#endif

public class sc_version_manager : MonoBehaviour
{

		


	#if UNITY_EDITOR
	public int advFileCounter;
	bool aggiungiFile;
	#endif
	public bool checkAdvManager;
	public string advManagerFile;
// questo è il nome del fine che si trova su riomoko.com e contiene i settaggi di adv di questo game
	public string gameName;
	public string versionN;
	public int versionProgressive;
		
	public bool webcamPresent;
// se false non attiva la classifica a selfie
	public bool paid;
	public bool isNested;
	public string leaderboardName;
	//IL nome della leaderboard gamecenter vedere su itunes e crearne una se necessario
	public Platform platform;
	public Store store;
	public int platformId;
	public int storeId;
	public string[] platformName;
	public string[] storeName;
	//public string[] platform;
	public string[] storeLink;
	public string[] parametri;
		
	//	public string[] redirectPaid;
	//	public string[] redirectFree;
	public int parInterstitial;
// id convertito da file caricato
	public int parBanner;
// id convertito da file caricato
		
	public GameObject parametriRoot;
	public GameObject prefabParametriRoot;
	//public Transform photoScattata;
	//public Transform photoPrefabChart;
	//  ^^^^ sc_parametri parametriS;


	//in caso non ci sia il file di advmanege e quindi non sia possibile determinare l'interstizial mettere manualmente l'oggetto interstizial qui sotto
	public GameObject interstitialCommander;
// quello che viene deciso da remoto (o messo manualmente) e viene intercettato da ztoolz
	public GameObject bannerCommander;
// quello che viene deciso da remoto (o messo manualmente) e viene intercettato da ztoolz
	public GameObject moreGamesCommander;
// quello che viene deciso da remoto (o messo manualmente) e viene intercettato da ztoolz
	public string messagez;
	public GameObject revmob;
	public GameObject admob;
	public GameObject iAD;
	public GameObject chartBoost;


	public enum Platform
	{
		iOS,
		Andr,
		PC,
		MAC,
		BB,
		OUYA,
		WEB,
		VITA}
	;

	public enum Store
	{
		GEN,
		apple,
		Google,
		AMZ,
		SAM,
		OPR,
		SLME,
		MBG,
		CIN,
		FB,
		
	}
	;


	

	/* ------------------------------- cose da controllare prima della build

-verificare il bottone star per le review del main menù se punta al giusto store o se disattivato
-verificare se paid è checkato
 //********per GAMECENTER  !!!!!!!!!!!!!!!ricordarsi di mettere il nome giusto della classifica su ztools tipo grp.LoveKissAndBomb
 - prossdimamente organizzare un'array con tutti i nomi delle charts per automatizzare le versioni
 - per le versioni ios ricordarsi l'id di chartboost in CBUIManagerCele
 - per le versioni android /amazon ricordarsi l'id di chartboost in strings
 -verificare tutti i codici degli adv
 - verificare il corretto testo nello sharing per versioni paid e versioni free
 - verificare l'icona adeguata tra paid e free
 -per ios aggiungere queste librerie :
 twitter
 social(?)
 AdSupport
AudioToolbox
AVFoundation
già c'è CoreGraphics
CoreTelephony
MessageUI
StoreKit
SystemConfiguration
Add “-ObjC" to the Other Linker Flags of target build setting in Xcode.
qui ci sono dei doc utili
http://matrixbai.tumblr.com/post/56237858107/the-official-free-open-source-unity3d-admob-plugin
https://developers.google.com/mobile-ads-sdk/docs/#ios
 
-----------------------------------------------------------------



	 */


	void Awake ()
	{




		//	Debug.Log ("platform : " + platform.ToString ());

		if (platform == Platform.iOS) {
			platformId = 0;
		}
		if (platform == Platform.Andr) {	
			platformId = 1;
		}
		if (platform == Platform.PC) {
			platformId = 2;
		}
		if (platform == Platform.MAC) {
			platformId = 3;
		}
		if (platform == Platform.BB) {
			platformId = 4;
		}
		if (platform == Platform.OUYA) {
			platformId = 5;
		}
		if (platform == Platform.WEB) {
			platformId = 6;
		}
		if (platform == Platform.VITA) {
			platformId = 7;
		}

	



		if (store == Store.GEN) {
			storeId = 0;
		}
		if (store == Store.apple) {
			storeId = 1;
		}
		if (store == Store.Google) {
			storeId = 2;
		}
		if (store == Store.AMZ) {
			storeId = 3;
		}
		if (store == Store.SAM) {
			storeId = 4;
		}
		if (store == Store.OPR) {
			storeId = 5;
		}
		if (store == Store.SLME) {
			storeId = 6;
		}
		if (store == Store.MBG) {
			storeId = 7;
		}
		if (store == Store.CIN) {
			storeId = 8;
		}
		if (store == Store.FB) {
			storeId = 9;
		}


				
				
		//^^^^^^^^^^^^^^	parametriRoot = Instantiate (prefabParametriRoot, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
		//^^^^^ parametriS = parametriRoot.GetComponent<sc_parametri> ();
		storeName = new string[10];// ricordarsi di aggiornare questi quando si aggiunge piattaforma
		platformName = new string[8];// ricordarsi di aggiornare questi quando si aggiunge piattaforma
		storeLink = new string[10];

		platformName [0] = "iOS";
		platformName [1] = "Andr";
		platformName [2] = "PC";
		platformName [3] = "MAC";
		platformName [4] = "BB";//BLACK BERRY
		platformName [5] = "OUYA";
		platformName [6] = "WEB";
		platformName [7] = "VITA";

		storeName [0] = "GEN";
		storeName [1] = "apple";
		storeName [2] = "Google";
		storeName [3] = "AMZ";
		storeName [4] = "SAM";
		storeName [5] = "OPR";
		storeName [6] = "SLME";
		storeName [7] = "MBG";
		storeName [8] = "CIN";
		storeName [9] = "FB";

	
			


		storeLink [0] = "http://riomoko.com";//"GEN";
		storeLink [1] = "itms-apps://itunes.apple.com/us/artist/blisscomedia/id537083315#";// "iOS"; http://appstore.com/blisscomedia
		storeLink [2] = "https://play.google.com/store/apps/developer?id=Blisscomedia%20s.r.l.&hl=it";//"GOG";
		storeLink [3] = "http://www.amazon.com/s/ref=bl_sr_mobile-apps?_encoding=UTF8&field-brandtextbin=Blisscomedia%20srl&node=2350149011";//"AMZ";
		storeLink [4] = "http://riomoko.com";//"SAM"; ^^^^^^^^^^^^^^^^^^ qui inserire lo store adeguat
		storeLink [5] = "http://apps.opera.com/en_us/catalog.php?vendor_id=52193";//"OPR";
		storeLink [6] = "http://slideme.org/user/riomoko";//"SLME";
		storeLink [7] = "http://riomoko.com";//"MBG";
		storeLink [8] = "http://riomoko.com";//"CIN";
		storeLink [9] = "http://riomoko.com";//"FB";



		//  link a landin page senza redirect  http://bit.ly/1kckphd 
		//  link a landin e all free ios e google play http://bit.ly/NJ8cT2
				
			
		
		
				 
	
				

				



		/*
				if (paid) {
						stateMachine.GetComponent<sc_state_machine_tile_crush> ().redirectLink = redirectPaid [platformId];
				} else {
						stateMachine.GetComponent<sc_state_machine_tile_crush> ().redirectLink = redirectFree [platformId];
				}
stateMachine.GetComponent<sc_state_machine_tile_crush> ().riomokoStoreLink = storeLink [platformId];
				stateMachine.GetComponent<sc_state_machine_tile_crush> ().paid = paid;
*/
				


				
//^^^^^^^^^^^^^				chartBoost.GetComponent<CBUIManagerCele> ().paid = paid;
//^^^^^^^^^^^^^				revmob.GetComponent<sc_revmob_cele_v2> ().paid = paid;
		string paidz = "";
		if (paid) {
			paidz = "_P";
		}
		advManagerFile = gameName + platformName [platformId] + paidz + ".txt";
		gameName = gameName + paidz;
		if (checkAdvManager) {
			StartCoroutine (CheckWWW ());// in pratica il cuore di version manager si occupa di andare a leggere i files di configurazione su riomoko.com
		}

		#if UNITY_EDITOR  
		// questa parte funziona solo sotto editor e si preoccupa di capire se il file adv è già presente o meno, se non è presente in adv_big_list lo appende e crea un file
		//APPEND my file and create default
		//System.IO.File.WriteAllText ("/Users/riomoko/Dropbox/_adv_manager/" + nomeFile [0] + ".txt", "nceleazzzzzooooooe");
		//	Debug.Log ("leggo_file");
		#if !UNITY_WEBPLAYER
//		string p = File.ReadAllText ("/Users/riomoko/Dropbox/advmanager/_adv_big_list.txt");
//		string[] lines = p.Split ('\n');
//		for (int n=0; n<lines.Length; ++n) {
//			advFileCounter++;
//			if (lines [n] == advManagerFile) {
//				aggiungiFile = true;
//			}
//		}
//
//		if (!aggiungiFile) {
//
//			System.IO.File.AppendAllText ("/Users/riomoko/Dropbox/advmanager/_adv_big_list.txt", "\n" + advManagerFile);
//		}
		#endif
		/*	foreach (string line in File.ReadLines("/Users/riomoko/Dropbox/_adv_manager/adv_big_list.txt")) {
						++advFileCounter;
				}*/
		#endif
	}




	IEnumerator CheckWWW ()
	{
	
		WWW zzz = new WWW ("http://www.riomoko.com/advmanager/" + advManagerFile);
			
		yield return zzz;
		if (zzz.text == null) {
			messagez = "  *                               ";
			//	Debug.Log ("NUL WWW cele");
		} else {
			messagez = zzz.text;
		}
		if (messagez == "") {
			messagez = "  *                               ";
		} 
		parametri = messagez.Split ('\n');
		//parametri di default************_______________________
		parInterstitial = 1;
		#if UNITY_ANDROID
		parBanner = 2;
		#endif
		#if UNITY_IPHONE
				parBanner = 3;
		#endif
		//********************__________________________________

		// questa parte bisogna riprogettarla meglio 
//				for (int pa=0; pa<parametri.Length; pa++) {
//						if (pa == 0) {
//								for (int paUno=0; paUno<parametriS.parametro01.Length; ++paUno) {
//										if (parametriS.parametro01 [paUno] == parametri [pa]) {
//												parInterstitial = paUno;
//												Debug.Log ("interstitial: " + parametri [pa]);
//										}
//								}
//
//						}
//
//						if (pa == 1) {
//								for (int paDue=0; paDue<parametriS.parametro01.Length; ++paDue) {
//										if (parametriS.parametro02 [paDue] == parametri [pa]) {
//												parBanner = paDue;
//												Debug.Log ("banner: " + parametri [pa]);
//										}
//								}
//				
//						}
//				}
		/*	Debug.Log (messagez);
				// banner è la seconda parte di 8, interst la prima parte di 8
				Debug.Log ("lunga = " + messagez.Length.ToString ());
				string interst = messagez.Remove (8, messagez.Length - 8);
				Debug.Log ("interst = " + interst);
				string banner = messagez.Remove (0, 8);
				banner = banner.Remove (8, messagez.Length - 16);
				Debug.Log ("banner = " + banner);
				Debug.Log ("PPPPP = " + messagez);*/
		/*^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
				switch (parInterstitial) {
				case 0://none
						interstitial = revmob;
						revmob.GetComponent<sc_revmob_cele_v2> ().interstitialAttivo = true;
						revmob.active = true;
						interstitial = revmob;
						break;
				case 1://revmob
						interstitial = revmob;
						revmob.GetComponent<sc_revmob_cele_v2> ().interstitialAttivo = true;
						revmob.active = true;
						interstitial = revmob;
						break;
				case 2://chartboost !!!! non implementata in android
			#if UNITY_IPHONE
						interstitial = chartBoost;
						chartBoost.GetComponent<CBUIManagerCele> ().interstitialAttivo = true;
						chartBoost.active = true;
#endif
#if UNITY_ANDROID
						interstitial = revmob;
						revmob.GetComponent<sc_revmob_cele_v2> ().interstitialAttivo = true;
						revmob.active = true;
						interstitial = revmob;
#endif
						
						break;
				case 3://riomoko
						interstitial = revmob;
						revmob.GetComponent<sc_revmob_cele_v2> ().interstitialAttivo = true;
						revmob.active = true;
						interstitial = revmob;
						break;
				}

				switch (parBanner) {
				case 0://none
						admob.active = true;
						break;
				case 1://revmob
						revmob.active = true;
						revmob.GetComponent<sc_revmob_cele_v2> ().bannerAttivo = true;
						if (!paid) {
								revmob.GetComponent<sc_revmob_cele_v2> ().AttivaBanner ();
						}
						break;
				case 2://admob
						admob.active = true;
						
						break;
				case 3://iAd
#if UNITY_IPHONE
						
						if (!paid) {
								iAD.GetComponent<sc_iAd_cele> ().bannerSubitoAttivo = true;
								//admob.GetComponent<sc_iAd_cele> ().BannerDisplay (true);
						} else {
								iAD.GetComponent<sc_iAd_cele> ().bannerSubitoAttivo = false;
						}
						iAD.active = true;
#endif
			#if UNITY_ANDROID
						admob.active = true;
			#endif
						break;
				case 4://Riomoko
						admob.active = true;
						break;
				}

		*/
				
	}



		
}