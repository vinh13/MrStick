using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
	public static HomeManager Instance = null;
	[SerializeField]UIButton btnUpdate = null, btnUpgrade = null, btnInventory = null, btnPvP = null, btnCam = null, btnBuy = null, btnRank = null,
		btnRateUS = null, btnKey = null, btnFacebook = null, btnVideo = null, btnChievement = null, btnFlashSale = null, btnStartSale = null, btnGiftCode = null;
	bool bTutorial = false;
	[SerializeField]Transform rectHUD = null;
	[SerializeField]UIButton btnGold = null, btnGem = null;
	[SerializeField]panelRight _pRight = null, _pLeft = null;
	bool bShowChar = false;
	bool bShowInven = false;
	bool bTutorialClickUpgrade = false;
	bool bTutorialATK = false;
	[SerializeField]UIButton btnKey2 = null;

	void Awake ()
	{
		if (Instance == null)
			Instance = this;
		bTutorial = TutorialData.bMenuTutorial;
		AllInOne.Instance.Init ();
		FBManagerEvent.Instance.Init ();
		btnGold.Register (ClickGold);
		btnGem.Register (ClickGem);
		btnGiftCode.Register (ShowGiftCode);
		if (!TutorialData.bToATK) {
			bTutorialClickUpgrade = TutorialData.bMenuTutorial;
			bTutorialATK = false;
		} else {
			bTutorialClickUpgrade = false;
			bTutorialATK = TutorialData.bTutorialUpgradeATK;
		}
	}

	void ShowGiftCode ()
	{
		Instantiate (Resources.Load<GameObject> ("UI/UIGiftCode"), rectHUD);
	}

	void ClickGold ()
	{
		Manager.Instance.ShowShop (false);
	}

	void ClickGem ()
	{
		Manager.Instance.ShowShop (true);
	}

	void Start ()
	{
		btnInventory.Register (ClickInventory);
		//btnBike.Register (ClickBike);
		btnPvP.Register (ClickPvP);
		btnCam.Register (ClickCam);
		btnBuy.Register (ClickBuy);
		btnRank.Register (ClickRank);
		btnRateUS.Register (RateUS);
		btnFacebook.Register (ConnectFacebook);
		btnVideo.Register (ShowVideo);
		btnChievement.Register (ShowAchievement);
		btnFlashSale.Register (ShowFL);
		btnUpgrade.Register (ClickUpgrade);
		btnStartSale.Register (StartSale);
		btnKey.Register (ClickKey);
		btnKey2.Register (ClickKey);
		btnUpdate.Register (ClickUpgrade);
		btnVideo.Block (true);

		Invoke ("_Start", 0.1F);
	}

	IEnumerator checkVideo ()
	{
		while (!AllInOne.Instance.CheckVideoReward ()) {
			yield return null;
		}
		btnVideo.Block (false);
	}

	void ClickKey ()
	{
		Manager.Instance.ShowWaitting (true);
		SceneManager.LoadSceneAsync (SceneName.Chest.GetHashCode (), LoadSceneMode.Additive);
	}

	void StartSale ()
	{
		Manager.Instance.ShowWaitting (true);
		SceneManager.LoadSceneAsync (SceneName.StartOffer.GetHashCode (), LoadSceneMode.Additive);
	}

	void RateUS ()
	{
		#if UNITY_ANDROID
		Application.OpenURL ("market://details?id=stickman.ragdoll.happy.wheel");
		#elif UNITY_IOS
		Application.OpenURL ("market://details?id=stickman.ragdoll.happy.wheel");
		#endif
	}

	public void ConnectFacebook ()
	{
		Application.OpenURL ("https://www.facebook.com/mrstickepic/");
	}


	void _Start ()
	{
		MusicManager.Instance.Play ("Menu");
		StartCoroutine (checkPlayer ());
		StartCoroutine (checkVideo ());
	}

	IEnumerator checkPlayer ()
	{
		while (!Logic.bPlayerLoadDone) {
			yield return null;
		}
	}

	public void ClickInventory ()
	{
		if (bShowInven)
			return;
		if (bShowChar) {
			HideChar ();
		}
		_pRight.Show (false);
		_pLeft.Show (false);
		bShowInven = true;
		EquipCantro.Instance.ShowPage (true);
		btnInventory.Block (true);

	}

	public void HideInventory ()
	{
		if (!bShowInven)
			return;
		_pRight.Show (true);
		_pLeft.Show (true);
		bShowInven = false;
		btnInventory.Block (false);
		EquipCantro.Instance.ShowPage (false);
		SkinEditor.Instance.ClearPreview ();
		UpgradeManager.Instance.ClearPreview ();
	}

	public void ClickUpgrade ()
	{
		if (bShowChar)
			return;
		if (bShowInven) {
			HideInventory ();
		}
		_pRight.Show (false);
		_pLeft.Show (false);
		bShowChar = true;
		UpgradeManager.Instance.ShowUpgrade (true);
		btnUpgrade.Block (true);
		if (bTutorialClickUpgrade) {
			bTutorialClickUpgrade = false;
			MenuTutorial.Instance.HideTutorial (TutorialID.ClickUpgrade);
		}
		if (bTutorialATK) {
			bTutorialATK = false;
			MenuTutorial.Instance.HideTutorial (TutorialID.ClickUpgrade);
		}
	}

	public void HideChar ()
	{
		if (!bShowChar)
			return;
		_pRight.Show (true);
		_pLeft.Show (true);
		bShowChar = false;
		UpgradeManager.Instance.ShowUpgrade (false);
		btnUpgrade.Block (false);
	}

	public void ClickPvP ()
	{
		
	}

	public void ClickRank ()
	{
		AllInOne.Instance._OnShowLB ();
	}

	public void ClickBuy ()
	{
		Manager.Instance.ShowShop (true);
	}

	public void ClickCam ()
	{
		if (bTutorial) {
			bTutorial = false;
			TutorialData.bMenuTutorial = false;
		}
		Manager.Instance.LoadScene (SceneName.Level, true);
		MusicManager.Instance.Stop ("Menu");
	}

	public void ShowEquip (UpgradeType t)
	{
		switch (t) {
		case UpgradeType.Health:
//			ClickCharacter ();
			break;
		case UpgradeType.Speed:
//			ClickBike ();
			break;
		case UpgradeType.ATK:
			break;
		}
	}

	public void ShowAchievement ()
	{
		Manager.Instance.ShowWaitting (true);
		SceneManager.LoadSceneAsync (SceneName.Achievement.GetHashCode (), LoadSceneMode.Additive);
	}

	public void ShowFL ()
	{
		Manager.Instance.ShowWaitting (true);
		SceneManager.LoadSceneAsync (SceneName.FlashSale.GetHashCode (), LoadSceneMode.Additive);
	}

	public void ShowVideo ()
	{
		Manager.Instance.ShowWaitting (true);
		SceneManager.LoadSceneAsync (SceneName.VideoReward.GetHashCode (), LoadSceneMode.Additive);
	}
}
