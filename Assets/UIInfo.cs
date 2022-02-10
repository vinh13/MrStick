using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[System.Serializable]
public class LevelReward
{
	public int idLevel = 0;
	public int Exp = 0;
}

public class UIInfo : MonoBehaviour,UILogEndGame
{
	[SerializeField]AnimatorPopUpScript anim = null;
	//Text textPlaced = null, //textTotal = null,
	[SerializeField]Text[] textTotalCoin = new Text[2], textBonusCoin = new Text[2];
	//[SerializeField]Text[] textsInfo, textsTitle;
	[SerializeField]UIButton btnHome = null, btnContinue = null, btnX2 = null, btnRetry = null;
	[SerializeField]Stars stars = null;
	[SerializeField]GifDropDailyManager gifDrop = null;
	[SerializeField]Transform rectLock = null;
	[SerializeField]Text[] textExps = new Text[2];
	float coinPerKill = 10F;
	float coinPerDame = 0.1F;
	float coinPerTop = 50F;
	float coinPerSecond = 0.5F;
	float coinPerLap = 5F;
	int _totalCoin = 0;
	[SerializeField]Transform[] rectsWL = new Transform[2];
	[SerializeField]UIButton btnAddStar = null;
	string path = "PlayerLevel/ExpLevel";
	List<LevelReward> listReward = new List<LevelReward> ();

	void LoadData ()
	{
		TextAsset textAsset = Resources.Load<TextAsset> (path);
		string[] texts = textAsset.text.Split ('\n');
		listReward.Clear ();
		int index = 0;
		foreach (string text in texts) {
			string[] s = text.Split ('\t');
			LevelReward temp = new LevelReward ();
			temp.Exp = int.Parse (s [0]);
			temp.idLevel = index;
			listReward.Add (temp);
			index++;
		}
	}

	public void Show ()
	{
		MapManager.Intance.DestroyMap ();
		LoadData ();
		rectLock.gameObject.SetActive (true);
		bool b = UIManager.Instance.bWin;
		if (!b) {
			if (TutorialData.bUpgradeATKDirec) {
				if (TutorialData.bTutorialUpgradeATK) {
					this.gameObject.AddComponent<InfoTutorial> ();
					TutorialData.bUpgradeATKDirec = false;
				} else {
					TutorialData.bUpgradeATKDirec = false;
				}
			}
		}
		btnContinue.gameObject.SetActive (b);
		btnRetry.gameObject.SetActive (!b);

		anim.show (null);

		btnHome.Register (
			Home);
		//	btnReplay.Register (Replay);

		btnContinue.Register (Continue);
		btnRetry.Register (Continue);
		btnX2.Register (
			X2);
		rectsWL [0].gameObject.SetActive (!b);
		rectsWL [1].gameObject.SetActive (b);


		btnAddStar.Register (AddStarVideo);

		OnShow ();

	}

	void OnShow ()
	{
		bool bWin = UIManager.Instance.bWin;
		//CheckTutorial

		bool bWinWithKill = UIManager.Instance.bWinWithKillAll;
		RankConnect rankP = LeaderboardManager.Instance.GetRankPlayerConnect ();
		int numberEnemy = LeaderboardManager.Instance.numberPos;
		int numberKill = EnemyManager.Instance.GetKill;
		float damageDone = EnemyManager.Instance.DamageDone;
		int timeGame = UIManager.Instance.TimeGame;

		if (bWinWithKill) {
			rankP.iRank = 0;
			numberKill = numberEnemy - 1;
		}

		int coinTop = (int)(coinPerTop / (float)(rankP.iRank + 1));
		int coinKill = (int)(numberKill * coinPerKill);
		int coinDamage = (int)(damageDone * coinPerDame);
		int coinLap = (int)(rankP.iLap * coinPerLap);
		int coinTime = (int)(timeGame * coinPerSecond);

		int TotalCoin = coinKill + coinDamage + coinTop + coinTime + coinLap;
		_totalCoin = TotalCoin;

		textTotalCoin [0].text = "" + CoinManager.Instance.Convert (TotalCoin);
		textTotalCoin [1].text = textTotalCoin [0].text;
		textBonusCoin [0].text = "+" + CoinManager.Instance.Convert (TotalCoin * 2);
		textBonusCoin [1].text = textBonusCoin [0].text;
//		textPlaced.text = "#" + (rankP.iRank + 1);
//		textTotal.text = "/" + numberEnemy;	

	



		SetTitle (0, "Top", (rankP.iRank + 1));
		SetTitle (1, "Kill", numberKill);
		SetTitle (2, "Damage Done", (int)damageDone);
		SetTitle (3, "Lap", rankP.iLap);
		TimeSpan temp = TimeSpan.FromSeconds (timeGame);
		string s = string.Format ("{0:D2} : {1:D2}", temp.Minutes, temp.Seconds);
		//textsTitle [4].text = "Time Survival(" + s + ")";

		//SetCoin
		SetInfo (0, coinTop);
		SetInfo (1, coinKill);
		SetInfo (2, coinDamage);
		SetInfo (3, coinLap);
		SetInfo (4, coinTime);
		int star = 0;
		bool[] b = new bool[3];
		if (bWin) {
			star += 1;
			b [0] = true;
			float hp = PlayerControl.Instance.GetRHealth;
			if (bWinWithKill) {
				star += 1;
				b [1] = true;
			}
			if (hp >= 0.3F) {
				star += 1;
				b [2] = true;
			}
		} else {
			star = 0;
			for (int i = 0; i < 3; i++) {
				b [i] = false;
			}
			btnAddStar.Block (!AllInOne.Instance.CheckVideoReward ());
		}
		stars.SetStar (star, b, AddStar, GameData.GetDailyGifRewarded);
		MapManager.Intance.SetStar (star);
		AddExp (star);
	}

	void AddExp (int star)
	{
		LevelReward levelReward = listReward [MapManager.Intance.lateLevel - 1];
		float exp = (float)levelReward.Exp;
		if (star == 0) {
			exp = 0;
		} else {
			exp /= (float)star;
			exp = (int)exp;
		}
		textExps [0].text = "+" + exp;
		textExps [1].text = "+" + exp;
		PlayerManagerTdz.Instance.UpdateExp ((int)exp);
	}


	void AddStar (object ob)
	{
		AddStarS ad = (AddStarS)ob;
		gifDrop.SetStar (ad);
		if (ad.bLate) {
			rectLock.gameObject.SetActive (false);
		}
	}

	void SetTitle (int i, string textPrefix, int number)
	{
		//textsTitle [i].text = textPrefix + "(" + number + ")";
	}

	void SetInfo (int i, int number)
	{
		//textsInfo [i].text = "+" + number;
	}

	public void Hide (Action a)
	{
		anim.hide (a);
	}

	void CheckX2Coin ()
	{
		bool b = AllInOne.Instance.CheckVideoReward ();
		if (b) {
			btnX2.Block (false);
		} else {
			btnX2.Block (true);
			CoinManager.Instance.PurchaseCoin (_totalCoin);
			_totalCoin = 0;
		}
	}

	public void AddCoin ()
	{
		if (_totalCoin != 0) {
			CoinManager.Instance.PurchaseCoin (_totalCoin);
			_totalCoin = 0;
		}
	}

	#region Click

	void Home ()
	{
		AddCoin ();
		TaskUtil.Schedule (this, _Home, 0.2F);
	}

	void _Home ()
	{
		//AllInOne.Instance.ShowAdmobFULL ();
		Logic.bShowAds = true;
		Manager.Instance.LoadScene (SceneName.Home, true);
	}

	//	void Replay ()
	//	{
	//		AddCoin ();
	//		AllInOne.Instance.ShowAdmobFULL ();
	//		Manager.Instance.LoadScene (SceneName.Main, false);
	//	}

	void Continue ()
	{
		AddCoin ();
		//AllInOne.Instance.ShowAdmobFULL ();
		Logic.bShowAds = true;
		Manager.Instance.LoadScene (SceneName.Level, true);

	}

	void X2 ()
	{
		btnX2.Block (true);
		//
		AllInOne.Instance.ShowVideoReward (CallbackX2,"X2",LevelData.IDLevel);
	}

	void CallbackX2 (bool b)
	{
		if (b) {
			_totalCoin *= 2;
			CoinManager.Instance.PurchaseCoin (_totalCoin);
			_totalCoin = 0;
			FBManagerEvent.Instance.PostEventCustom ("x2Coin");
		} else {
			CheckX2Coin ();
		}
	}

	#endregion

	#region AddStar
	void AddStarVideo ()
	{
		btnAddStar.Block (true);
		AllInOne.Instance.ShowVideoReward (cbAddStarVideo,"addStar",LevelData.IDLevel);
	}
	void cbAddStarVideo (bool b)
	{
		if (b) {
			AddStarS temp = new AddStarS ();
			temp.star = 1;
			temp.bLate = true;
			gifDrop.SetStar (temp);
			FBManagerEvent.Instance.PostEventCustom ("AddStar_Video");
		} else {
			btnAddStar.Block (!AllInOne.Instance.CheckVideoReward ());
		}	
	}

	#endregion
}
