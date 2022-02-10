using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum UpgradeType
{
	None = 0,
	Health = 1,
	Speed = 2,
	ATK = 3,
}

public class UIUpgrade : IUpgrade
{
	[SerializeField]UpgradeType upgradeType = UpgradeType.None;
	[SerializeField]UIButton btnPlus = null, btnEquip = null;
	[SerializeField]Transform[] rects = new Transform[2];
	[SerializeField]Text textValue = null, textPrice = null, textLevel = null;
	[SerializeField]PanelLevel panelLevel = null;
	[SerializeField]PanelInfoPlayer panelInfo = null;
	int currentLevel = 0;
	int currentLevelBonus = 0;
	float price = 500;
	bool bFull = false;
	bool bTutorial = false;
	float maxLevel = 20F;

	public override void SetColor (Color32 color)
	{
//		bars [1].SetColor (color);
//		bars [2].SetColor (color);
//		color.a = 150;
//		bars [0].SetColor (color);
//		bars [3].SetColor (color);
	}

	public override void UpdateData ()
	{
		currentLevel = UpgradeData.GetUpgradeLevel (upgradeType);
		if (currentLevel == 0) {
			currentLevel = 1;
			UpgradeData.SetUpgradeLevel (upgradeType, 1);
		}
		currentLevelBonus = UpgradeData.GetUpgradeLevelBonus (upgradeType);
		Check ();
		SyncUI ();
		//bars [3].Active (false);
		panelInfo.Preview (0, 0, false);
	}

	public override void ClearPreview ()
	{
		panelInfo.Preview (0, 0, false);
	}

	public override void UpdateBonus (int level)
	{
		currentLevelBonus = level;
		UpgradeData.SetUpgradeLevelBonus (upgradeType, level);
		SyncLevelBonus ();
		SyncValue ();
	}

	public override void Preview (float ratio, bool b)
	{
//		bars [3].Active (b);
//		bars [3].Change (ratio);
		float r = currentLevel / maxLevel;
//		Debug.Log (ratio);
		panelInfo.Preview (r, ratio, b);
	}

	void SyncUI ()
	{
		SyncLevel ();
		SyncLevelBonus ();
		SyncPrice ();
		SyncValue ();

	}

	void SyncLevel ()
	{
		float r = currentLevel / maxLevel;
		panelLevel.SetCurLevel (currentLevel);
		UpdateLevel ();
		panelInfo.SyncReal (r);
//		bars [0].Change (r + 0.1F);
//		bars [1].Change (r);
	}

	void SyncLevelBonus ()
	{
		if (currentLevelBonus != 0) {
			//bars [2].Change (r);
			float r0 = currentLevel / maxLevel;
			float r1 = currentLevelBonus / maxLevel;
			panelInfo.SyncBounusI (r1, r0);
		} else {
			//bars [2].Change (0);
			panelInfo.SyncBounusI (0, 0);
		}
	}

	void UpdateLevel ()
	{
		textLevel.text = "" + currentLevel;
	}

	void SyncPrice ()
	{
		if (!bFull) {
			textPrice.text = "" + CoinManager.Instance.Convert ((int)price);
		} else {
			textPrice.enabled = false;
		}
	}

	void SyncValue ()
	{
		float hp = UpgradeManager.Instance.GetConfig (upgradeType, currentLevel);
		float maxValue = UpgradeManager.Instance.GetConfig (upgradeType, (int)maxLevel);
		float minValue = UpgradeManager.Instance.GetConfig (upgradeType, 1);
		float bonusValue = (maxValue - minValue) * ((float)currentLevelBonus / maxLevel);

		string text = "";

		switch (upgradeType) {
		case UpgradeType.ATK:
			text = "ATK:";
			break;
		case UpgradeType.Speed:
			text = "NT:";
			break;
		case UpgradeType.Health:
			text = "HP:";
			break;
		}
		textValue.text = text + hp;
	}

	void Start ()
	{
		bTutorial = TutorialData.bMenuTutorial;
		btnPlus.Register (ClickPlus);
		btnEquip.Register (CickEquip);
		UpdateData ();
	}

	public void ClickPlus ()
	{
		if (currentLevel >= maxLevel) {
			return;
		}
		if (CoinManager.Instance.CheckCoin ((int)price)) {
			CoinManager.Instance.PurchaseCoin (-(int)price);
			currentLevel++;

			UpgradeManager.Instance.Play (upgradeType.GetHashCode () - 1);

			UpgradeData.SetUpgradeLevel (upgradeType, currentLevel);
			Check ();
			SyncUI ();
			CheckTutorial ();

			float b = UpgradeManager.Instance.GetConfig (upgradeType, currentLevel);
			float l = UpgradeManager.Instance.GetConfig (upgradeType, currentLevel - 1);
			UpgradeManager.Instance.PlayText (upgradeType, "+" + (b - l));
			UpdateLevel ();
		} else {
			CheckTutorial ();
		}
	}

	void CheckTutorial ()
	{
		if (upgradeType == UpgradeType.Health) {
			if (bTutorial) {
				Debug.Log ("cc");
				MenuTutorial.Instance.HideTutorial (TutorialID.Health);
				ShowTutorial ();
				bTutorial = false;
			}
		} else if (upgradeType == UpgradeType.ATK) {
			if (TutorialData.bTutorialUpgradeATK) {
				TutorialData.bTutorialUpgradeATK = false;
			}
			if (TutorialData.bToATK) {
				TutorialData.bToATK = false;
				MenuTutorial.Instance.HideTutorial (TutorialID.UpATK);
			}
		}
	}

	void ShowTutorial ()
	{
		MenuTutorial.Instance.ShowTutorial (TutorialID.PlayGame);
	}

	public void CickEquip ()
	{
		//HomeManager.Instance.ShowEquip (upgradeType);
	}

	void Check ()
	{
		if (currentLevel >= maxLevel) {
			//bars [0].Active (false);
			bFull = true;
			textPrice.enabled = false;
			btnPlus.Block (true);
		} else {
			price = UpgradeManager.Instance.GetPrice (upgradeType, currentLevel + 1);
			if (CoinManager.Instance.coin >= price) {
				//bars [0].Active (true);
				panelLevel.SetNextLevel (currentLevel + 1, true);
				ActiveButton (true);
			} else {
				//bars [0].Active (false);
				panelLevel.SetNextLevel (currentLevel + 1, false);
				ActiveButton (false);
			}
			bFull = false;
			btnPlus.Block (false);
		}
	}

	void ActiveButton (bool b)
	{
		rects [0].gameObject.SetActive (b);
		rects [1].gameObject.SetActive (!b);
	}
}
