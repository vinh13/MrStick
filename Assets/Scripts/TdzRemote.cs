using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TdzRemote : MonoBehaviour
{
	public int defaultVersion = 0;

	public static bool UpdateVersion { get; private set; }

	public static string InfoUpdate{ get; private set; }


	public static bool RemoveSyncDone{ get; private set; }

	void Awake ()
	{
		if (Logic.GameStared)
			return;
		RemoveSyncDone = false;
		Logic.GameStared = true;
		defaultVersion = GetNumbleOnStringVS (Application.version);
//		#if UNITY_ANDROID
//		TimeResetADS = defaultTimeResetADS;
//		#elif UNITY_IOS
//		TimeResetADS = 60;
//		#endif
		//	ShowRate = DefaultShowRate;
		UpdateVersion = false;
		//	ShowZombie = DefaultShowZombie;
		RemoteSettings.Completed += Complete;
	}

	private void Complete (bool b, bool _b, int i)
	{
		//ShowZombie = RemoteSettings.GetBool ("ShowZombie", true);

		//ShowRate = RemoteSettings.GetInt ("ShowRate", 99) != 99 ? true : false;
		//Debug.Log ("Show Rate" + ShowRate);
		int currentVersion = 0;
		#if UNITY_ANDROID
		//TimeResetADS = RemoteSettings.GetInt ("TimeResetADS_AD", defaultTimeResetADS);
		currentVersion = GetNumbleOnStringVS (RemoteSettings.GetString ("UpdateVersion", Application.version));
		#elif UNITY_IPHONE
		//TimeResetADS = RemoteSettings.GetInt ("TimeResetADS", defaultTimeResetADS);
		currentVersion = GetNumbleOnStringVS (RemoteSettings.GetString ("UpdateVersion_IOS", Application.version));
		#endif

		if (currentVersion > defaultVersion) {
			UpdateVersion = true;
			#if UNITY_ANDROID
			InfoUpdate = RemoteSettings.GetString ("UpdateInfo", "New");
			#elif UNITY_IPHONE
			InfoUpdate = RemoteSettings.GetString ("UpdateInfo_IOS", "New");
			#endif
		} else {
			UpdateVersion = false;
		}
		RemoveSyncDone = true;
	}

	int GetNumbleOnStringVS (string nameVs)
	{
		string numbleupdate = "";
		int checkupdatevs = 0;
		for (int i = 0; i < nameVs.Length; i++) {
			if (nameVs [i].ToString () != ".") {
				numbleupdate += nameVs [i].ToString ();
			}
		}
		checkupdatevs = int.Parse (numbleupdate);
		return checkupdatevs;
	}
}
