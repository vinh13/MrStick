using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum VideoType
{
	None = 0,
	StaminaShop = 1,
	GoldShop = 2,
}

[System.Serializable]
public enum TypeReward
{
	None = 0,
	Gold = 1,
	Gem = 2,
	Skin = 3,
	Wheels = 4,
	Health = 5,
	Shield = 6,
	Key = 7,
}

public class RewardVideo : MonoBehaviour
{
	[SerializeField]TypeReward typeReward = TypeReward.None;
	[SerializeField]VideoType typeVideo = VideoType.None;
	[SerializeField]Text textValue = null, textCountVideo = null;
	[SerializeField]Transform rectLock = null, rectCount = null;
	[SerializeField]int iValue = 0;
	[SerializeField]Button btn = null;
	[SerializeField]int iCountVideo = 0;
	[SerializeField]int iMaxCount = 5;
	bool bRewarded = false;

	void OnValidate ()
	{
		textValue.text = "" + iValue;
	}

	void Start ()
	{
		if (typeVideo == VideoType.None) {
			iMaxCount = 1;
		}
		textValue.text = "" + iValue;

		iCountVideo = GameData.GetCountVideoReward (typeReward, typeVideo);

		textCountVideo.text = "" + (iMaxCount - iCountVideo);

		bRewarded = GameData.GetRewardVideo (typeReward, typeVideo);
		rectLock.gameObject.SetActive (bRewarded);
		rectCount.gameObject.SetActive (!bRewarded);


		btn.interactable = false;
		rectLock.gameObject.SetActive (true);
		rectCount.gameObject.SetActive (false);

		btn.onClick.AddListener (delegate() {
			Click ();
		});
		if (!bRewarded)
			StartCoroutine (checkVideo ());
	}

	void Click ()
	{
		AllInOne.Instance.ShowVideoReward (CallBack,"Reward_Video",LevelData.IDLevel);
		btn.interactable = false;
		rectLock.gameObject.SetActive (true);
		rectCount.gameObject.SetActive (false);
	}

	void CallBack (bool b)
	{
		if (b) {
			switch (typeReward) {
			case TypeReward.Gem:

				CoinManager.Instance.PurchaseGem (iValue, false);

				//FBManagerEvent.Instance.PostEventCustom ("RewardVideo_Gem");

				break;
			case TypeReward.Gold:


				CoinManager.Instance.PurchaseCoin (iValue);


			//	FBManagerEvent.Instance.PostEventCustom ("RewardVideo_Gold");

				break;
//			case TypeReward.Stamina:
//
//
//				StaminaManager.Instance.BuyStamina (iValue);
//
//
//				FBManagerEvent.Instance.PostEventCustom ("RewardVideo_Stamina");
//
//				break;
			}
			TaskUtil.Schedule (this, ReCheck, 0.25F);
		} else {
			rectLock.gameObject.SetActive (false);
			rectCount.gameObject.SetActive (true);
			btn.interactable = false;
			TaskUtil.Schedule (this, ResetCheck, 0.5F);
		}
	}

	void ResetCheck ()
	{
		btn.interactable = AllInOne.Instance.CheckVideoReward ();
		rectLock.gameObject.SetActive (btn.interactable);
		rectCount.gameObject.SetActive (!btn.interactable);
	}

	void ReCheck ()
	{
		iCountVideo++;
		if (iCountVideo == iMaxCount) {
			GameData.SetRewardVideo (typeReward, typeVideo);
		} else {
			GameData.SetCountVideoReward (typeReward, typeVideo, iCountVideo);
			StartCoroutine (checkVideo ());
		}

		textCountVideo.text = "" + (iMaxCount - iCountVideo);

	}

	IEnumerator checkVideo ()
	{
		while (!AllInOne.Instance.CheckVideoReward ()) {
			yield return null;
		}
		btn.interactable = true;
		rectLock.gameObject.SetActive (false);
		rectCount.gameObject.SetActive (true);

	}

	void OnDisable ()
	{
		StopAllCoroutines ();
	}
}
