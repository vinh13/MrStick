using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankScript : Rank
{
	[SerializeField]Text textRank = null, textLap = null;
	[SerializeField]Image imgAvatar = null;
	[SerializeField]Transform tDeath = null;
	[SerializeField]Transform tYou = null;
	[SerializeField]Image imgPanel = null;
	bool bEnd = false;
	bool bActive = false;

	public void ActiveYou (bool b)
	{
		tYou.gameObject.SetActive (b);
	}

	void Start ()
	{
		tDeath.gameObject.SetActive (false);
	}

	public override void Init (Sprite sp, int rank, int lap)
	{
		imgAvatar.sprite = sp;
		//textRank.text = "R: " + rank;
		if (bActive) {
			textLap.text = "L: " + lap;
		} else {
			textLap.text = "--";
		}
	}

	public override void SetRank (int rank, int lap, Color color)
	{
		//textRank.text = "R: " + rank;
		if (bActive) {
			textLap.text = "Lap: " + lap;
			imgPanel.color = color;
		} else {
			imgPanel.color = Color.gray;
			textLap.text = "--";
		}
	}

	public override void Active (bool b)
	{
		bActive = b;
	}

	public override void ToDeath ()
	{
		if (bEnd)
			return;
		bEnd = true;
		tDeath.gameObject.SetActive (true);
		imgPanel.color = Color.gray;
	}
}
