using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardGiftCantro : MonoBehaviour
{
	[SerializeField]AnimatorPopUpScript animPop = null;
	[SerializeField]CardGift[] cardGifts;
	[SerializeField]GiftOption[] rollOptions;
	[SerializeField]RollConfig[] typeGifI;
	[SerializeField]RollConfig[] typeGifII;
	[SerializeField]RollConfig[] typeGifIII;
	[SerializeField]RollConfig[] startCard;
	[SerializeField]UIButton btnQuit = null;
	[SerializeField]Color32[] colors = new Color32[2];
	bool[] bPickeds;
	bool[] bGifteds;
	int countLucky = 0;
	int countTime = 0;
	const int maxTime = 8;

	#region SmartThink

	int idTygif (RollConfig[] rolls, bool b)
	{
		List<int> idGifs = new List<int> ();
		for (int i = 0; i < rolls.Length; i++) {
			string key = rolls [i].typeReward.ToString () + "_" + rolls [i]._data;
			if (!GameData.GetLucky (key)) {
				idGifs.Add (i);
			}
		}
		int cc = 0;
		if (idGifs.Count > 1) {
			cc = Random.Range (1, idGifs.Count + 1) - 1;
			string s = rolls [idGifs [cc]].typeReward.ToString () + "_" + rolls [idGifs [cc]]._data;
			if (b)
				GameData.SetLucky (s, true);
		} else if (idGifs.Count == 1) {
			cc = 0;
			string s = rolls [idGifs [cc]].typeReward.ToString () + "_" + rolls [idGifs [cc]]._data;
			if (b)
				GameData.SetLucky (s, true);
		} else {
			cc = RandomIndex (rolls.Length + 1);
		}
		return cc;
	}

	void ClearGifts (RollConfig[] rolls)
	{
		for (int i = 0; i < rolls.Length; i++) {
			string key = rolls [i].typeReward.ToString () + "_" + rolls [i]._data;
			GameData.SetLucky (key, false);
		}
	}

	#endregion

	#region Timing

	IEnumerator _Timing = null;

	void UpdateText ()
	{
		for (int i = 0; i < cardGifts.Length; i++) {
			cardGifts [i].UpdateCd ("" + countTime);
		}
	}

	void UpdateColor (Color32 color)
	{
		for (int i = 0; i < cardGifts.Length; i++) {
			cardGifts [i].UpdateColor (color);
		}
	}

	void StartTime ()
	{
		countTime = maxTime;
		PauseTime (false);
		UpdateColor (colors [0]);
		UpdateText ();
	}

	void StopTime ()
	{
		countTime = 0;
		SyncRewardFake ();
		for (int i = 0; i < cardGifts.Length; i++) {
			cardGifts [i].ShowAllReward ();
		}
		ActiveQuit (true);
	}

	void ActiveQuit (bool b)
	{
		btnQuit.Block (!b);
	}

	void PauseTime (bool b)
	{
		if (b) {
			StopCoroutine (_Timing);
		} else {
			_Timing = _CountTime ();
			StartCoroutine (_Timing);
		}
	}

	IEnumerator _CountTime ()
	{
		yield return new WaitForSecondsRealtime (1F);
		countTime -= 1;
		countTime = Mathf.Clamp (countTime, 0, maxTime);
		if (countTime == 0) {
			StopTime ();
		} else {
			PauseTime (false);
		}
		UpdateText ();
	}

	#endregion

	#if UNITY_EDITOR
	void OnValidate ()
	{
		cardGifts = GetComponentsInChildren<CardGift> ();
		rollOptions = GetComponentsInChildren<GiftOption> ();
	}
	#endif
	int TakeGif (int id, bool bVideo)
	{
		int typeGiftId = 1;
		if (id < 3) {
			if (bVideo) {
				typeGiftId = 2;
			} else {
				typeGiftId = 1;
			}
		} else if (id < 7) {
			if (bVideo) {
				typeGiftId = 1;
			} else {
				typeGiftId = 2;
			}
		} else if (id < 10) {
			if (bVideo) {
				typeGiftId = 2;
			} else {
				typeGiftId = 1;
			}
		} else if (id < 12) {
			if (bVideo) {
				typeGiftId = 3;
			} else {
				typeGiftId = 2;
			}
		} else {
			typeGiftId = 3;
		}
		if (typeGiftId == 3) {
			countLucky = 0;
			GameData.Lucky = 0;
		}
		return typeGiftId;
	}

	bool bInited = false;

	void Awake ()
	{
		this._Awake ();
	}

	void _Awake ()
	{

		if (!bInited) {
			bInited = true;
			for (int i = 0; i < cardGifts.Length; i++) {
				cardGifts [i].Register (i, PickCard, PickCardVideo, OnGiftDone);
	
			}
			bPickeds = new bool[3];
			bGifteds = new bool[3];
			countLucky = GameData.Lucky;
			btnQuit.Register (ClickQuit);
			ActiveQuit (false);

		}
	}

	void ResetAllData ()
	{
		for (int i = 0; i < 3; i++) {
			bPickeds [i] = false;
			bGifteds [i] = false;
			cardGifts [i].ResetAllData ();
		}
	}

	void ClickQuit ()
	{
		ActiveQuit (false);
		Show (false);
	}

	public void Show (bool b)
	{
		if (b) {
			this._Awake ();
			animPop.show (OnShow);
			Manager.Instance.ShowPlayer (true);
		} else {
			animPop.hide (OnHide);
			Manager.Instance.ShowPlayer (false);
		}
	}

	void OnShow ()
	{
		ActiveQuit (false);
		for (int i = 0; i < cardGifts.Length; i++) {
			cardGifts [i].ShowVideo (false);
			cardGifts [i].ShowOpen (true);
			cardGifts [i].BlockCard (false);
		}
		StartTime ();
	}

	void OnHide ()
	{
		ResetAllData ();
	}

	public void PickCard (int id)
	{
		PauseTime (true);
		countLucky++;
		GameData.Lucky = countLucky;
		SyncGift (false, id);
		for (int i = 0; i < cardGifts.Length; i++) {
			cardGifts [i].BlockCard (true);
			cardGifts [i].ShowOpen (false);
		}
		FBManagerEvent.Instance.PostEventCustom ("OpenCard_Free");
	}

	void OnGiftDone (int id, bool video)
	{
		if (video) {
			SyncRewardFake ();
			for (int i = 0; i < cardGifts.Length; i++) {
				if (i != id)
					cardGifts [i].ShowAllReward ();
			}
			//Debug.Log (currentRollConfig.typeReward);
			RewardGif (currentRollConfig, true);
			ActiveQuit (true);
		} else {
			RewardGif (currentRollConfig, false);
		}
	}

	public void SyncRewardFake ()
	{
		for (int i = 0; i < bPickeds.Length; i++) {
			if (!bPickeds [i]) {
				bPickeds [i] = true;
				CreateFake (i);
			}
		}
	}

	void CreateFake (int i)
	{
		for (int j = 0; j < bGifteds.Length; j++) {
			if (!bGifteds [j]) {
				bGifteds [j] = true;
				CreateCC (j, i);
				break;
			}
		}
	}

	void CreateCC (int j, int idGifSelect)
	{
		int index = 0;
		switch (j) {
		case 0:
			index = idTygif (typeGifI, false);
			rollOptions [idGifSelect].LoadReward (LoadReward (typeGifI [index], false), idGifSelect, typeGifI [index].typeReward);
			break;
		case 1:
			index = idTygif (typeGifII, false);
			rollOptions [idGifSelect].LoadReward (LoadReward (typeGifII [index], false), idGifSelect, typeGifII [index].typeReward);
			break;
		case 2:
			index = idTygif (typeGifIII, false);
			rollOptions [idGifSelect].LoadReward (LoadReward (typeGifIII [index], false), idGifSelect, typeGifIII [index].typeReward);
			break;
		}
	}

	public void PickCardVideo (int id)
	{
		PauseTime (true);
		countLucky++;
		GameData.Lucky = countLucky;
		SyncGift (true, id);
		for (int i = 0; i < cardGifts.Length; i++) {
			if (i != id)
				cardGifts [i].ShowVideo (false);
		}
		FBManagerEvent.Instance.PostEventCustom ("OpenCard_Video");
	}

	#region SyncGift

	void SyncGift (bool bVideo, int i)
	{
		int idtypeGif = TakeGif (countLucky, bVideo);
		int index = 0;
		int idGifSelect = i;
		switch (idtypeGif) {
		case 1:
			if (!bPickeds [idGifSelect])
				bPickeds [idGifSelect] = true;
			if (!GameData.bFirstTypeI) {
				GameData.bFirstTypeI = true;
				if (!bGifteds [2]) {
					bGifteds [2] = true;
				}
				rollOptions [idGifSelect].LoadReward (LoadReward (startCard [0], true), idGifSelect, startCard [0].typeReward);
			} else {
				if (!bGifteds [0]) {
					bGifteds [0] = true;
				}
				index = idTygif (typeGifI, true);
				rollOptions [idGifSelect].LoadReward (LoadReward (typeGifI [index], true), idGifSelect, typeGifI [index].typeReward);
			}
			break;
		case 2:
			if (!bPickeds [idGifSelect])
				bPickeds [idGifSelect] = true;
			if (!GameData.bFirstTypeII) {
				GameData.bFirstTypeII = true;
				if (!bGifteds [1]) {
					bGifteds [1] = true;
				}
				rollOptions [idGifSelect].LoadReward (LoadReward (startCard [1], true), idGifSelect, startCard [1].typeReward);
			} else {
				if (!bGifteds [1]) {
					bGifteds [1] = true;
				}
				index = idTygif (typeGifII, true);
				rollOptions [idGifSelect].LoadReward (LoadReward (typeGifII [index], true), idGifSelect, typeGifII [index].typeReward);
			}
			break;
		case 3:
			if (!bPickeds [idGifSelect])
				bPickeds [idGifSelect] = true;
		
			if (!GameData.bFirstTypeIII) {
				if (!bGifteds [2]) {
					bGifteds [2] = true;
				}
				GameData.bFirstTypeIII = true;
				rollOptions [idGifSelect].LoadReward (LoadReward (startCard [2], true), idGifSelect, startCard [2].typeReward);
			} else {
				if (!bGifteds [2]) {
					bGifteds [2] = true;
				}
				index = idTygif (typeGifIII, true);
				rollOptions [idGifSelect].LoadReward (LoadReward (typeGifIII [index], true), idGifSelect, typeGifIII [index].typeReward);
			}
			ClearGifts (typeGifI);
			ClearGifts (typeGifII);
			ClearGifts (typeGifIII);
			break;
		}
	}

	int RandomIndex (int range)
	{
		return Random.Range (1, range) - 1;
	}

	#endregion

	void RewardGif (RollConfig roll, bool bExchange)
	{
		string text = roll._data;
		switch (roll.typeReward) {
		case TypeReward.Gold:
			if (bExchange) {
				GifManager.Instance.ExchangeGold (int.Parse (text), ExchangeType.GoldX2, text);
			} else {
				CoinManager.Instance.PurchaseCoin (int.Parse (text));
				ContinueTime ();
			}
			break;
		case TypeReward.Gem:
			if (bExchange) {
				GifManager.Instance.x2Gem (int.Parse (text));
			} else {
				CoinManager.Instance.PurchaseGem (int.Parse (text), false);
				ContinueTime ();
			}
			break;
		case TypeReward.Key:
			if (bExchange) {
				int key = int.Parse (text);
				GifManager.Instance.x2Key (key);
			} else {
				CoinManager.Instance.PurchaserKey (int.Parse (text));
				ContinueTime ();
			}
			break;
		case TypeReward.Skin:
			string[] skins = text.Split ('_');
			EquipConfig equipConfig = Resources.Load<EquipConfig> ("SkinData/" + skins [1] + "/" + skins [0]);
			GifManager.Instance.ShowSkin (equipConfig, ContinueTime);
			break;
		case TypeReward.Wheels:
			string[] textsWheels = text.Split ('_');
			EquipConfig wheels = Resources.Load<EquipConfig> ("SkinData/" + textsWheels [1] + "/" + textsWheels [0]);
			GifManager.Instance.ShowSkin (wheels, ContinueTime);
			break;
		case TypeReward.Health:
			int hp = int.Parse (text);
			ItemBoot h = new ItemBoot ();
			h.bootType = BootType.Health;
			h._valueBoot = hp;
			GifManager.Instance.X2Boot (h, ContinueTime);
			break;
		case TypeReward.Shield:
			int shield = int.Parse (text);
			ItemBoot s = new ItemBoot ();
			s.bootType = BootType.Shield;
			s._valueBoot = shield;
			GifManager.Instance.X2Boot (s, ContinueTime);
			break;
		}
		//uiChest.OnGifDone ();
		//FBManagerEvent.Instance.PostEventCustom ("Chest_done_Pack" + _IDPACK);
	}

	void ContinueTime ()
	{
		countTime = 4;
		UpdateColor (colors [1]);
		UpdateText ();
		PauseTime (false);
		for (int i = 0; i < cardGifts.Length; i++) {
			cardGifts [i].BlockVideo (true);
			cardGifts [i].ShowVideo (true);
		}
		ActiveQuit (true);
	}

	RollConfig currentRollConfig = null;

	object LoadReward (RollConfig roll, bool bTakeGif)
	{
		if (bTakeGif) {
			currentRollConfig = new RollConfig ();
		}
		object temp = new object ();
		string text = roll._data;
		switch (roll.typeReward) {
		case TypeReward.Gold:
			temp = text;
			if (bTakeGif) {
				currentRollConfig.typeReward = TypeReward.Gold;
				currentRollConfig._data = text;
			}
			break;
		case TypeReward.Gem:
			temp = text;
			if (bTakeGif) {
				currentRollConfig.typeReward = TypeReward.Gem;
				currentRollConfig._data = text;
			}
			break;
		case TypeReward.Key:
			temp = text;
			if (bTakeGif) {
				currentRollConfig.typeReward = TypeReward.Key;
				currentRollConfig._data = text;
			}
			break;
		case TypeReward.Skin:
			string[] skins = text.Split ('_');
			if (skins [0] != "Skin") {
				EquipConfig equipConfig = Resources.Load<EquipConfig> ("SkinData/" + skins [1] + "/" + skins [0]);
				temp = equipConfig;
				if (bTakeGif) {
					currentRollConfig.typeReward = TypeReward.Skin;
					currentRollConfig._data = text;
				}
			} else {
				int levelSkin = int.Parse (skins [1]);
				if (levelSkin != 0) {
					string dataSkin = SkinData.SkinSmartThink (levelSkin, "Head");
					if (dataSkin != null) {
						string[] skinss = dataSkin.Split ('_');
						if (skinss.Length > 1) {
							EquipConfig equipConfig = Resources.Load<EquipConfig> ("SkinData/" + skinss [1] + "/" + skinss [0]);
							if (equipConfig != null) {
								temp = equipConfig;
								if (bTakeGif) {
									currentRollConfig.typeReward = TypeReward.Skin;
									currentRollConfig._data = dataSkin;
								}
							} else {
								if (bTakeGif) {
									currentRollConfig.typeReward = TypeReward.Key;
									currentRollConfig._data = "" + levelSkin;
								}
							}
						} else {
							if (bTakeGif) {
								currentRollConfig.typeReward = TypeReward.Key;
								currentRollConfig._data = "" + levelSkin;
							}
						}
					} else {
						if (bTakeGif) {
							currentRollConfig.typeReward = TypeReward.Key;
							currentRollConfig._data = "" + levelSkin;
						}
					}
				} 
			}
			break;
		case TypeReward.Wheels:
			string[] textsWheels = text.Split ('_');
			EquipConfig wheels = Resources.Load<EquipConfig> ("SkinData/" + textsWheels [1] + "/" + textsWheels [0]);
			temp = wheels;
			if (bTakeGif) {
				currentRollConfig.typeReward = TypeReward.Wheels;
				currentRollConfig._data = text;
			}
			break;
		case TypeReward.Health:
			int hp = int.Parse (text);
			temp = hp;
			if (bTakeGif) {
				currentRollConfig.typeReward = TypeReward.Health;
				currentRollConfig._data = text;
			}
			break;
		case TypeReward.Shield:
			int shield = int.Parse (text);
			temp = shield;
			if (bTakeGif) {
				currentRollConfig.typeReward = TypeReward.Shield;
				currentRollConfig._data = text;
			}
			break;
		}
		return temp;
	}

}
