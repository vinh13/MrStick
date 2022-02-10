using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievement : MonoBehaviour
{

	string[] textNameMap = { "DEATH GRAVAYARD", "BLOODY ARENA", "HELL PRISON", "HELL CAVE", "CHEMICAL FACTORY" };
	string[] textTitleHard = { "Normal", "Hard", "Crazy Hard", "Nightmare" };
	[SerializeField]AchievementID ID = AchievementID.None;
	[SerializeField]Transform rectActive = null, rectLockBtn = null, rectDone = null;
	[SerializeField]Text textName = null, textSub = null, textProcess = null, textPrice = null;
	[SerializeField]Image imgBar = null;
	[SerializeField]Button btnClaim = null;
	AchievementConfig achievementConfig = null;
	int currentLevelAchievement = 0;
	bool bClaimed = false;
	bool bCompleted = false;
	[SerializeField]string keyPrefs = "";
	[SerializeField]string keyMore = "";
	int currentValue = 0;
	int targetValue = 0;
	int maxLevel = 0;
	int iReward = 0;
	string textPerfection = "";

	void OnValidate ()
	{
		//Awake ();
	}

	void Awake ()
	{
		if (ID == AchievementID.None)
			return;
		LoadData ();


		RefreshData ();

	}

	void RefreshData ()
	{

		currentLevelAchievement = AchievementData.GetCurrentLevel (achievementConfig.achievementType);




		//Get key more
		if (achievementConfig.achievementType == AchievementType.Perfection) {
			string v = "" + achievementConfig.listTarget_Reward [currentLevelAchievement].target;
			char[] values = v.ToCharArray ();
			textPerfection = textNameMap [int.Parse (values [0].ToString ())] + " mode " + textTitleHard [int.Parse (values [1].ToString ())];
			keyMore = v;
		} else {
			keyMore = "";
		}

		Display ();


		currentValue = AchievementData.GetCurrentValue (achievementConfig.achievementType, keyMore);



		// Key Claim
		keyPrefs = "" + achievementConfig.achievementType.ToString () + "_" + currentLevelAchievement.ToString ();


		bCompleted = (currentValue >= targetValue);
		if (bCompleted) {
			bClaimed = !AchievementData.GetClaimAchievement (keyPrefs);
		} else {
			bClaimed = false;
		}

		Processing ();


		if (bClaimed)
			Active ();
		else
			Disable ();

		if (currentLevelAchievement == (maxLevel - 1)) {
			if (!bClaimed) {
				btnClaim.gameObject.SetActive (false);
				rectDone.gameObject.SetActive (true);
			} else {
				btnClaim.gameObject.SetActive (true);
				rectDone.gameObject.SetActive (false);
			}
			
		} else {
			btnClaim.gameObject.SetActive (true);
			rectDone.gameObject.SetActive (false);
		}
	}

	void Start ()
	{
		btnClaim.onClick.AddListener (delegate {
			OnClickButton ();	
		});
	}

	void OnClickButton ()
	{
		AchievementData.SetClaimAchievement (keyPrefs, 1);
		CoinManager.Instance.PurchaseGem (iReward,false);
		Disable ();
		UpgradeAchievement ();
	}

	void LoadData ()
	{
		string path = "AchievementConfig/" + ID.ToString ();
		achievementConfig = Resources.Load<AchievementConfig> (path);
		maxLevel = achievementConfig.listTarget_Reward.Count;

	}

	void Display ()
	{

		Target_Reward tr = new Target_Reward ();
		tr = achievementConfig.listTarget_Reward [currentLevelAchievement];
		textName.text = "" + achievementConfig.AchievementName + " " + tr.IndexRoman;

		if (achievementConfig.achievementType == AchievementType.Perfection) {
			textSub.text = achievementConfig.DescribeBefore + textPerfection;
		} else {
			textSub.text = achievementConfig.DescribeBefore + " " + tr.target + " " + achievementConfig.DescribeAfter;
		}


		iReward = tr.reward;

		textPrice.text = CoinManager.Instance.Convert (iReward);


		if (achievementConfig.achievementType != AchievementType.Perfection) {
			targetValue = tr.target;
		} else {
			targetValue = 3 * 15;
		}

	}

	void Active ()
	{
		rectActive.gameObject.SetActive (true);
		btnClaim.interactable = true;
		rectLockBtn.gameObject.SetActive (false);
	}

	void Disable ()
	{
		rectActive.gameObject.SetActive (false);
		btnClaim.interactable = false;
		rectLockBtn.gameObject.SetActive (true);
	}

	void Processing ()
	{
		if (currentValue != 0) {
			float ratio = (float)currentValue / (float)targetValue;
			ratio = Mathf.Clamp (ratio, 0, 1);
			imgBar.fillAmount = ratio;
			textProcess.text = "" + (int)(ratio * 100) + "%";
		} else {
			imgBar.fillAmount = 0;
			textProcess.text = "0%";
		}
	}

	void UpgradeAchievement ()
	{
		if (currentLevelAchievement < (maxLevel - 1)) {
			currentLevelAchievement += 1;

			AchievementData.SetCurrentLevel (achievementConfig.achievementType, currentLevelAchievement);

			RefreshData ();
		} else {
			//Max Level
			rectActive.gameObject.SetActive (true);
			btnClaim.gameObject.SetActive (false);
			rectDone.gameObject.SetActive (true);
		}
	}
}
