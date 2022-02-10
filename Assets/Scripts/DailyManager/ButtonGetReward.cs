using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ButtonGetReward : MonoBehaviour
{
	[SerializeField]Transform rectFocus = null, rectLock = null;
	[SerializeField]Text textTitle = null, textCoin = null, textTitle2 = null;
	[SerializeField]Button btnReward = null;
	[SerializeField]Image imgPreview = null;
	Action clickReward = null;
	bool BLocked = false;
	int _CoinBonus = 0;

	void Start ()
	{
		btnReward.onClick.AddListener (delegate() {
			ClickReward ();	
		});
	}

	public void SetUp (bool rewarded, string title, bool focus, bool block, Action cb, Daily daily)
	{

		clickReward = cb;
		//textTitle.text = "" + title;
		//textTitle2.text = "" + title;
		rectFocus.gameObject.SetActive (focus);
		rectLock.gameObject.SetActive (block);


		if (focus) {
			if (!rewarded) {
				btnReward.interactable = true;
			} else {
				btnReward.interactable = false;
				rectLock.gameObject.SetActive (true);
			}
		} else {
//			if (block) {
//			}
			btnReward.interactable = false;
		}


		if (daily.typeReward == TypeReward.Gold ||
		    daily.typeReward == TypeReward.Gem ||
		    daily.typeReward == TypeReward.Health ||
		    daily.typeReward == TypeReward.Shield) {
			string path = "";
			imgPreview.sprite = daily.spr;
			path = daily.typeReward.ToString ();
			textCoin.enabled = true;
			textCoin.text = "+" + daily.ValueReward;
		} else if (daily.typeReward == TypeReward.Skin || daily.typeReward == TypeReward.Wheels) {
			textCoin.text = daily.typeReward.ToString ();
			imgPreview.sprite = daily.equiConfig.spr;
		}
	}

	void ClickReward ()
	{
		rectLock.gameObject.SetActive (true);
		btnReward.interactable = false;
		clickReward.Invoke ();
	}
}
