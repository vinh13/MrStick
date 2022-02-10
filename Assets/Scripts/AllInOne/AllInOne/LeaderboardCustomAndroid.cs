#if UNITY_ANDROID
using UnityEngine;
using System.Collections;
//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System;
public class LeaderboardCustomAndroid : MonoBehaviour
{
	void Awake ()
	{
		//PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder ().Build ();
		//PlayGamesPlatform.InitializeInstance (config);
		//PlayGamesPlatform.Activate ();
	}

	public void LoginLB ()
	{
		Social.localUser.Authenticate ((bool success) => {
			if (success) {
				Debug.Log ("Login Sucess");
				AllInOne.Instance.isLeaderboardLogin = true;
				AllInOne.Instance.submitScore ();
			} else {
				Debug.Log ("Login failed");
				AllInOne.Instance.isLeaderboardLogin = false;
			}
		});
	}

	public void OnLogOut ()
	{
		//((PlayGamesPlatform)Social.Active).SignOut ();
	}

	public void OnShowLeaderBoard ()
	{
		if (Social.localUser.authenticated)
		{
			//((PlayGamesPlatform)Social.Active).ShowLeaderboardUI ();
		}
		else
		{
			this.LoginLB();
		}
	}

	public void SummitScores (string idlb, string keyPrefs)
	{
		int value = TaskUtil.GetInt (keyPrefs);
		if (Social.localUser.authenticated) {
			Social.ReportScore (value, idlb, (bool success) => {
				if (success) {
					Debug.Log ("Update Score Success");
				} else {
					Debug.Log ("Update Score Fail");
				}
			});
		}
	}

}
#endif
