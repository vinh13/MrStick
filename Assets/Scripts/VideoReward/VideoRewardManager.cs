using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VideoRewardManager : MonoBehaviour
{
	public static VideoRewardManager Instance = null;
	[SerializeField]Transform rectParent = null;
	public int countVideo = 0;
	[SerializeField]UIVideoReward[] rollOptions;
	int lateIDData = 0;
	RollData roll = null;
	[SerializeField]int maxReward = 0;

	void OnValidate ()
	{
		rollOptions = rectParent.GetComponentsInChildren<UIVideoReward> ();
		maxReward = rollOptions.Length;
	}

	void Awake ()
	{
		maxReward = rollOptions.Length;
		if (Instance == null)
			Instance = this;
		//VideoRewardData.ResetVideo ();
		countVideo = VideoRewardData.CountVideo;
		//test
		LoadData (1);
	}

	void Start ()
	{

	}

	public void ActiveReward ()
	{
		Debug.Log ("" + countVideo);
		if (countVideo <= maxReward - 1)
			rollOptions [countVideo].ActiveReward ();
	}

	void LoadData (int id)
	{
		roll = Resources.Load<RollData> ("Video/V" + id);
		for (int i = 0; i < rollOptions.Length; i++) {
			rollOptions [i].LoadReward (LoadReward (roll.rollsConfig [i]), i, roll.rollsConfig [i].typeReward, Availaible (i), Done (i));
		}
		lateIDData = id;
	}

	public bool Availaible (int indexVideo)
	{
		if (indexVideo == countVideo) {
			return true;
		} else {
			return false;
		}
	}

	public bool Done (int indexVideo)
	{
		if (indexVideo < countVideo) {
			return true;
		} else {
			return false;
		}
	}

	public void OnCompleteVideo (int indexVideo)
	{
		RewardGif (roll.rollsConfig [countVideo]);
		if (countVideo < maxReward - 1) {
			countVideo++;
			VideoRewardData.CountVideo = countVideo;
			//Reactive
			rollOptions [countVideo].ActiveReward ();
		} else {
			rollOptions [countVideo].BlockReward ();
			countVideo++;
			VideoRewardData.CountVideo = countVideo;
		}
		FBManagerEvent.Instance.PostEventCustom ("VideoReward_" + countVideo);
	}

	void RewardGif (RollConfig roll)
	{
		string text = roll._data;
		switch (roll.typeReward) {
		case TypeReward.Key:
			CoinManager.Instance.PurchaserKey (int.Parse (text));
			break;
		case TypeReward.Gem:
			//GifManager.Instance.ExchangeGold (int.Parse (text), ExchangeType.GoldX2, text);
			CoinManager.Instance.PurchaseGem (int.Parse (text), false);
			break;
		case TypeReward.Gold:
			GifManager.Instance.ExchangeGold (int.Parse (text), ExchangeType.GoldX2, text);
			break;
		case TypeReward.Skin:
			string[] skins = text.Split ('_');
			EquipConfig equipConfig = Resources.Load<EquipConfig> ("SkinData/" + skins [1] + "/" + skins [0]);
			GifManager.Instance.ShowSkin (equipConfig, null);
			break;
		case TypeReward.Wheels:
			string[] textsWheels = text.Split ('_');
			EquipConfig wheels = Resources.Load<EquipConfig> ("SkinData/" + textsWheels [1] + "/" + textsWheels [0]);
			GifManager.Instance.ShowSkin (wheels, null);
			break;
		case TypeReward.Health:
			int hp = int.Parse (text);
			ItemBoot h = new ItemBoot ();
			h.bootType = BootType.Health;
			h._valueBoot = hp;
			GifManager.Instance.X2Boot (h, null);
			break;
		case TypeReward.Shield:
			int shield = int.Parse (text);
			ItemBoot s = new ItemBoot ();
			s.bootType = BootType.Shield;
			s._valueBoot = shield;
			GifManager.Instance.X2Boot (s, null);
			break;
		}
	}

	object LoadReward (RollConfig roll)
	{
		object temp = new object ();
		string text = roll._data;
		switch (roll.typeReward) {
		case TypeReward.Key:
			temp = text;
			break;
		case TypeReward.Gem:
			temp = text;
			break;
		case TypeReward.Gold:
			temp = text;
			break;
		case TypeReward.Skin:
			string[] skins = text.Split ('_');
			EquipConfig equipConfig = Resources.Load<EquipConfig> ("SkinData/" + skins [1] + "/" + skins [0]);
			temp = equipConfig;
			break;
		case TypeReward.Wheels:
			string[] textsWheels = text.Split ('_');
			EquipConfig wheels = Resources.Load<EquipConfig> ("SkinData/" + textsWheels [1] + "/" + textsWheels [0]);
			temp = wheels;
			break;
		case TypeReward.Health:
			int hp = int.Parse (text);
			temp = hp;
			break;
		case TypeReward.Shield:
			int shield = int.Parse (text);
			temp = shield;
			break;
		}
		return temp;
	}

}
