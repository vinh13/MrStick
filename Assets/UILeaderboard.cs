using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UILeaderboard : MonoBehaviour
{
	Rank[] ranks;
	Vector3[] rectPosition;
	[SerializeField]Color[] colorsRank;

	public void Init (RankConnect[] listRankConnect, Action a, Sprite[] sprs)
	{
		int numberEnemy = listRankConnect.Length;
		ranks = new Rank[numberEnemy];
		rectPosition = new Vector3[numberEnemy];
		for (int i = 0; i < numberEnemy; i++) {
			ranks [i] = CreatePanel.GetComponent<Rank> ();
			int indexSpr = listRankConnect [i].aiLevel.GetHashCode ();
			ranks [i].Init (sprs [indexSpr], 0, 1);
			if (i == numberEnemy - 1) {
				RankScript rs = (RankScript)ranks [i];
				rs.ActiveYou (true);
				rs.Active (true);
			}
		}
		StartCoroutine (_GetPos (a));
	}

	IEnumerator _GetPos (Action a)
	{
		yield return new WaitForSeconds (1F);
		for (int i = 0; i < rectPosition.Length; i++) {
			rectPosition [i] = ranks [i].transform.localPosition;
		}
		if (a != null)
			a.Invoke ();
	}

	GameObject CreatePanel {
		get { 
			return Instantiate (Resources.Load<GameObject> ("UI/panelRank"), transform.GetChild (0));
		}
	}

	public void InitRank (Sprite sp, int i, int _rank, int lap)
	{
		ranks [i].Init (sp, _rank, lap);
	}

	public void ActiveRank (int i)
	{
		ranks [i].Active (true);
	}

	public void  SetUpRank (int i, int _rank, int lap, bool bDeath)
	{
		if (!bDeath) {
			ranks [i].SetRank (_rank, lap, _rank < 3 ? colorsRank [_rank] : Color.gray);
		} else {
			ranks [i].ToDeath ();
		}
		ranks [i].transform.localPosition = rectPosition [_rank];
	}

	public void RemoveRank (int i)
	{
		ranks [i].gameObject.SetActive (false);	
	}
}
