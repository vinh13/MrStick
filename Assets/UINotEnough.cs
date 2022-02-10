using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UINotEnough : MonoBehaviour
{
	[SerializeField]Text textCur = null, textNeed = null;
	[SerializeField]UIButton btnBuy = null, btnCancel = null;
	[SerializeField]Transform rectGold = null, rectGem = null;
	[SerializeField]UIButton btnVideo = null;
	[SerializeField]Text textVideo = null;
	bool gGem = false;
	int coinVideo = 0;
	int countVideo = 0;
	const int maxVideo = 3;

	public void _Start ()
	{
		btnBuy.Register (ClickBuy);
		btnCancel.Register (ClickCancel);
		btnVideo.Register (ClickVideo);
	}

	public void Show (TypePurchase t, string current, string need, int e)
	{
		gameObject.SetActive (true);
		switch (t) {
		case TypePurchase.Gem:
			rectGem.gameObject.SetActive (true);
			rectGold.gameObject.SetActive (false);
			gGem = true;
			btnVideo.Block (true);
			break;
		case TypePurchase.Coin:
			rectGem.gameObject.SetActive (false);
			rectGold.gameObject.SetActive (true);
			gGem = false;
			btnVideo.Block (true);
			countVideo = GameData.VideoNotEnoughCoin;
			if (countVideo < maxVideo) {
				int c = e;
				if (c <= 1000) {
					coinVideo = c;
					ShowVideo ();
				}
			}
			break;
		}
		textCur.text = current;
		textNeed.text = "+" + need;
	}

	void ShowVideo ()
	{
		textVideo.text = "+" + coinVideo;
		StartCoroutine (checkVideo ());
	}

	IEnumerator checkVideo ()
	{
		while (!AllInOne.Instance.CheckVideoReward ()) {
			yield return null;
		}
		btnVideo.Block (false);
	}

	void ClickVideo ()
	{
		btnBuy.Block (true);
		btnCancel.Block (true);
		FBManagerEvent.Instance.PostEventCustom ("NotEnoughCoin_AddCoin");
		btnVideo.Block (true);
		AllInOne.Instance.ShowVideoReward (CallbackVideo, "NotEnoughCoin_AddCoin",LevelData.IDLevel);
	}

	void CallbackVideo (bool b)
	{
		if (b) {
			CoinManager.Instance.PurchaseCoin (coinVideo);
			countVideo += 1;
			GameData.VideoNotEnoughCoin = countVideo;
		} else {
			StartCoroutine (checkVideo ());
		}
		btnBuy.Block (false);
		btnCancel.Block (false);
	}

	void ClickBuy ()
	{
		Manager.Instance.ShowShop (gGem);
		gameObject.SetActive (false);
	}

	void ClickCancel ()
	{
		gameObject.SetActive (false);
	}
}
