using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using System;
public class LeaderboardCustomIOS : MonoBehaviour
{
	public void LoginLB ()
	{
		Social.localUser.Authenticate ((bool success) => {
			if (success) {
				Debug.Log ("Login Sucess");
				AllInOne.Instance.isLeaderboardLogin = true;
				AllInOne.Instance.submitScore ();

			} else {
				AllInOne.Instance.isLeaderboardLogin = false;

				Debug.Log ("Login failed");
			}
		});
	}

	public void showLeaderBoardsUI ()
	{		
		Social.ShowLeaderboardUI ();
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
