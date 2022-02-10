using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DailyManager : MonoBehaviour
{
	[SerializeField]AnimatorPopUpScript uiReward = null;
	[SerializeField]RectTransform rectParent = null;
	[SerializeField]ButtonGetReward[] btnsReward;
	int countDay = 0;
	bool bRewarded = false;
	int dayReal = 0;
	DailyConfig dailyConfig = null;
	[SerializeField]int iDaytest = 0;
	[SerializeField]UIButton btnClaim = null, btnX2 = null;
	bool bx2 = false;

	void OnValidate ()
	{
		btnsReward = rectParent.GetComponentsInChildren<ButtonGetReward> ();
	}

	void ClickClaim ()
	{
		OnReward ();
		btnClaim.Block (true);
	}


	void ClickX2 ()
	{
		AllInOne.Instance.ShowVideoReward (Callback,"x2_DailyReward",LevelData.IDLevel);
		btnX2.Block (true);
	}

	void Callback (bool b)
	{
		if (b) {
			bx2 = true;
		} else {
			bx2 = false;
		}
		OnReward ();
	}


	void Start ()
	{
		countDay = GameData.DayCount;
		bRewarded = GameData.GetDayRewarded ("day" + countDay);
		int n = 0;
		//countDay = iDaytest;
		if (countDay <= 7) {
			n = 0;
		} else {
			n = countDay / 7;
		}
		dayReal = countDay - (7 * n);
		if (dayReal == 0)
			dayReal = 7;
		//dayReal = 7;
		//	bRewarded = false;
		string path = "W";
		if (n >= 3)
			n = 3;
		path += ("" + (n + 1));
		dailyConfig = Resources.Load<DailyConfig> ("DailyReward/" + path);
		SyncData ();
		TaskUtil.Schedule (this, Show, 0.25F);
		if (!bRewarded) {
			btnClaim.Register (ClickClaim);
			btnX2.Register (ClickX2);
			//btnX2.Block (true);
			//StartCoroutine (CheckVideo ());
			checkX2Button ();
		} else {
			btnClaim.Block (true);
			btnX2.Block (true);
		}
	}

	IEnumerator CheckVideo ()
	{
		while (!AllInOne.Instance.CheckVideoReward ()) {
			yield return null;
		}
		btnX2.Block (false);
	}

	void SyncData ()
	{
		bool focus = false;
		bool block = false;
		for (int i = 1; i <= btnsReward.Length; i++) {
			string title = "DAY " + i;
			focus = dayReal == i ? true : false;
			block = dayReal > i ? true : false;
			btnsReward [i - 1].SetUp (bRewarded, title, focus, block, OnReward, dailyConfig.listDaily [i - 1]);
		}
	}

	void checkX2Button ()
	{
		Daily daily = new Daily ();
		daily = dailyConfig.listDaily [dayReal - 1];
		bool b = false;
		switch (daily.typeReward) {
		case TypeReward.Gold:
			b = true;
			break;
		case TypeReward.Gem:
			b = true;
			break;
		case TypeReward.Skin:
			b = false;
			break;
		case TypeReward.Wheels:
			b = false;
			break;
		case TypeReward.Shield:
			b = true;
			break;
		case TypeReward.Health:
			b = true;
			break;
		}
		if (b) {
			btnX2.Block (true);
			StartCoroutine (CheckVideo ());
		} else {
			btnX2.Block (true);
		}
	}

	void OnReward ()
	{
		if (bRewarded)
			return;
		bRewarded = true;
		Manager.Instance.ShowWaitting (true);
		TaskUtil.Schedule (this, AddReward, 0.25F);
	}

	void AddReward ()
	{
		GameData.SetDayRewarded ("day" + countDay, bRewarded);
		Daily daily = new Daily ();
		daily = dailyConfig.listDaily [dayReal - 1];
//		int VipRank = GameData.UserVip;
//
//	int _CoinBonus = (int)((float)daily.ValueReward * (VipMemberData.rateDailyLogin [VipRank] - 1));

		switch (daily.typeReward) {
		case TypeReward.Gold:
			if (bx2) {
				int _CoinBonus = bx2 ? daily.ValueReward : 0;
				CoinManager.Instance.PurchaseCoin (daily.ValueReward + _CoinBonus);
			} else {
				GifManager.Instance.ExchangeGold (daily.ValueReward, ExchangeType.GoldX2, daily.ValueReward);
			}
			break;
		case TypeReward.Gem:
			int _gemBonus = bx2 ? daily.ValueReward : 0;
			CoinManager.Instance.PurchaseGem (daily.ValueReward + _gemBonus, false);
			break;
		case TypeReward.Skin:
			//StaminaManager.Instance.BuyStamina (daily.ValueReward + _CoinBonus);
			GifManager.Instance.ShowSkin (daily.equiConfig, null);
			break;
		case TypeReward.Wheels:
			GifManager.Instance.ShowSkin (daily.equiConfig, null);
			//CardSelectCharacterManager.Instance.UnlockCharacterDaily (daily.ValueReward);
			break;
		case TypeReward.Shield:
			if (bx2) {
				int sx2 = bx2 ? daily.ValueReward : 0;
				CharacterData.SetBoot (BootType.Shield, daily.ValueReward + sx2);
				Manager.Instance.ShowToas (daily.ValueReward + sx2, TypeReward.Shield, true);
			} else {
				ItemBoot se = new ItemBoot ();
				se.bootType = BootType.Shield;
				se._valueBoot = daily.ValueReward;
				GifManager.Instance.X2Boot (se, null);
			}
			break;
		case TypeReward.Health:
			if (bx2) {
				int sx2 = bx2 ? daily.ValueReward : 0;
				CharacterData.SetBoot (BootType.Health, daily.ValueReward + sx2);
				Manager.Instance.ShowToas (daily.ValueReward + sx2, TypeReward.Health, true);
			} else {
				ItemBoot he = new ItemBoot ();
				he.bootType = BootType.Health;
				he._valueBoot = daily.ValueReward;
				GifManager.Instance.X2Boot (he, null);
			}
			break;
		}
		bx2 = false;
		TaskUtil.Schedule (this, CallExit, 0.05F);
	}

	void CallExit ()
	{
		uiReward.hide (ExitScene);
		Manager.Instance.ShowWaitting (true);
	}

	void ExitScene ()
	{
		SceneManager.UnloadSceneAsync (SceneName.DailyReward.ToString ());
		Manager.Instance.ShowWaitting (false);
	}

	public void ClickExit ()
	{

		//AudioUIManager.Instance.Play ("UI_Back");
		Manager.Instance.ShowWaitting (true);
		uiReward.hide (ExitScene);
	}

	void Show ()
	{
		uiReward.show (OnShow);
	}

	void OnShow ()
	{
		SFXManager.Instance.Play ("popup_open");
		Manager.Instance.ShowWaitting (false);
	}
}
