using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
	public static LeaderboardManager Instance = null;
	[SerializeField]UILeaderboard lb = null;
	List<RankConnect> listRankConnect = new List<RankConnect> ();
	[HideInInspector]public int numberPos = 0;
	[SerializeField]Sprite[] sprs;
	bool bPlayerEnd = false;
	void Awake ()
	{
		if (Instance == null)
			Instance = this;
	}

	#region Leaderboard

	public void RegisterLB (RankConnect rank, bool done)
	{
		listRankConnect.Add (rank);
		if (done) {
			_CreateData ();
		}
	}

	void _CreateData ()
	{
		listRankConnect.Add (PlayerControl.Instance.rankConnect (listRankConnect.Count));
		lb.Init (listRankConnect.ToArray(), StartSync, sprs);
		numberPos = listRankConnect.Count;
	}

	void StartSync ()
	{
		SyncData ();
		StartCoroutine (_StartSync ());
	}

	IEnumerator _StartSync ()
	{
		yield return new WaitForSeconds (1F);
		StartSync ();
	}

	void RefreshLap ()
	{
		RankConnect temp = null;
		for (int i = 0; i < numberPos - 1; i++) {
			int ilap = listRankConnect [i].iLap;
			for (int j = i + 1; j < numberPos; j++) {
				int jLap = listRankConnect [j].iLap;
				if (ilap < jLap) {
					temp = listRankConnect [i];
					listRankConnect [i] = listRankConnect [j];
					listRankConnect [j] = temp;
				} else if (ilap == jLap) {
					float iR = MapManager.Intance.GetRatioDistance (listRankConnect [i].GetPosX);
					float jR = MapManager.Intance.GetRatioDistance (listRankConnect [j].GetPosX);
					if (iR < jR) {
						temp = listRankConnect [i];
						listRankConnect [i] = listRankConnect [j];
						listRankConnect [j] = temp;
					}
				}


			}
		}
		for (int i = 0; i < numberPos; i++) {
			listRankConnect [i].iRank = i;
		}
	}

	public void NextLap (int ID)
	{
		if (bPlayerEnd)
			return;
		for (int i = 0; i < numberPos; i++) {
			if (listRankConnect [i].ID == ID) {
				listRankConnect [i].iLap += 1;
			}
		}
		StopAllCoroutines ();
		StartSync ();
	}

	public void SyncData ()
	{
		RefreshLap ();
		for (int i = 0; i < numberPos; i++) {
			lb.SetUpRank (listRankConnect [i].ID,
				listRankConnect [i].iRank,
				listRankConnect [i].iLap,
				listRankConnect [i].bDeath);
		}
	}

	#endregion

	#region RemoveSyncRank

	public void RemoveRank (int ID)
	{
		int iC = 0;
		for (int i = 0; i < numberPos; i++) {
			if (listRankConnect [i].ID == ID) {
				iC = i;
				break;
			}
		}
		GameObject go = new GameObject ();
		go.name = "Tdz";
		RankConnect rankNew = go.AddComponent<RankConnect> ();
		rankNew.bDeath = true;
		rankNew.ID = listRankConnect [iC].ID;
		rankNew.iLap = listRankConnect [iC].iLap;
		rankNew.iRank = listRankConnect [iC].iRank;
		go.transform.position = listRankConnect [iC].transform.position;
		listRankConnect [iC] = rankNew;
		StopAllCoroutines ();
		StartSync ();
	}

	#endregion

	#region UpdateRank

	public void UpdateRank (int ID, bool updateLate)
	{
		if (!updateLate) {
			lb.ActiveRank (ID);
		} else {
			lb.ActiveRank (ID);
			int iC = 0;
			for (int i = 0; i < numberPos; i++) {
				if (listRankConnect [i].ID == ID) {
					iC = i;
					break;
				}
			}
			listRankConnect [iC].iLap = MapManager.Intance.countLap;
		}
	}

	public int GetRankPlayer ()
	{
		bPlayerEnd = true;
		StopAllCoroutines ();
		SyncData ();
		int iC = 0;
		for (int i = 0; i < numberPos; i++) {
			if (listRankConnect [i].ID == numberPos - 1) {
				iC = i;
				break;
			}
		}
		return listRankConnect [iC].iRank;
	}

	public RankConnect GetRankPlayerConnect ()
	{
		StopAllCoroutines ();
		SyncData ();
		int iC = 0;
		for (int i = 0; i < numberPos; i++) {
			if (listRankConnect [i].ID == numberPos - 1) {
				iC = i;
				break;
			}
		}
		return listRankConnect [iC];
	}

	#endregion
}
