using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class ChestManager : MonoBehaviour
{
	[SerializeField]int[] randomRatio = { 36, 66, 85, 95, 100 };
	[SerializeField]UIChest uiChest = null;
	[SerializeField]Transform parent = null;
	[SerializeField]RollOption[] rollOptions;
	RollData roll = null;
	[SerializeField]ChestCantro chestCantro = null;
	[SerializeField]int idGif = 6;
	[SerializeField]RollConfig currentGif = null;
	int countRoll = 0;
	List<int> listData = new List<int> ();
	int _IDPACK = 0;

	void Awake ()
	{
		uiChest.Register (OpenChest, ResetRoll);
		LoadData ();
		SyncData ();
	}

	void TakeGif ()
	{
		idGif = 2;
		GetGif ();
	}

	void GetGif ()
	{
		int index = Random.Range (1, listData.Count + 1) - 1;
		int idPack = 0;
		if (index > randomRatio [3]) {
			idPack = 4;
		} else if (index > randomRatio [2]) {
			idPack = 3;
		} else if (index > randomRatio [1]) {
			idPack = 2;
		} else if (index > randomRatio [0]) {
			idPack = 1;
		} else {
			idPack = 0;
		}
		_IDPACK = idPack + 1;
		listData.RemoveAt (index);
		GameData.ChestData = NewData ();
		RollData temp = Resources.Load<RollData> ("Chest/" + gifType ());
		RollConfig rollCf = temp.rollsConfig [idPack];
		currentGif = rollCf;
		rollOptions [idGif].LoadReward (LoadReward (rollCf), idGif, rollCf.typeReward);

	}

	string gifType ()
	{
		int i = Random.Range (1, 9) - 1;
		string text = "";
		switch (i) {
		case 0:
			text = "Gold";
			break;
		case 1:
			text = "Gem";
			break;
		case 2:
			text = "Key";
			break;
		case 3:
			text = "Wheel";
			break;
		case 4:
			text = "Skin1";
			break;
		case 5:
			text = "Skin2";
			break;
		case 6:
			text = "Skin3";
			break;
		case 7:
			text = "Skin4";
			break;
		default:
			text = "Gold";
			break;
		}
		return text;
	}

	void SyncData ()
	{
		string data = GameData.ChestData;
		if (data.Equals ("")) {
			for (int i = 0; i < 100; i++) {
				listData.Add (i + 1);
				data = NewData ();
				GameData.ChestData = data;
			}
		} else {
			string[] datas = data.Split ('_');
			for (int i = 0; i < datas.Length; i++) {
				listData.Add (int.Parse (datas [i]));
				Debug.Log (listData.Count);
			}
		}
	}

	string NewData ()
	{
		if (listData.Count != 0) {
			string s = "" + listData [0];
			if (listData.Count == 1)
				return s;
			for (int i = 1; i < listData.Count; i++) {
				s += "_" + listData [i];
			}
			return s;
		} else {
			string data = "";
			for (int j = 0; j < 100; j++) {
				listData.Add (j + 1);
				data = NewData ();
				GameData.ChestData = data;
			}
			return data;
		}
	}

	void ResetRoll ()
	{
		chestCantro.ResetRoll ();
	}

	void Start ()
	{
		Invoke ("_Start", 0.1F);
		TakeGif ();
	}

	void _Start ()
	{
		uiChest.Show (true);
	}

	void OpenChest (object ob)
	{
		chestCantro.Roll (idGif, RollDone);
	}

	void RollDone (object ob)
	{
		int id = (int)ob;
		RewardGif (currentGif);
		TakeGif ();
	}


	void LoadData ()
	{
		int id = Random.Range (1, 3);
		roll = Resources.Load<RollData> ("Chest/C" + id);
		for (int i = 0; i < rollOptions.Length; i++) {
			rollOptions [i].LoadReward (LoadReward (roll.rollsConfig [i]), i, roll.rollsConfig [i].typeReward);
		}
	}

	void Update ()
	{
		if (Input.GetKey (KeyCode.L)) {
			RewardGif (roll.rollsConfig [idGif]);
		}
	}

	void RewardGif (RollConfig roll)
	{
		string text = roll._data;
		switch (roll.typeReward) {
		case TypeReward.Gold:
			GifManager.Instance.ExchangeGold (int.Parse (text), ExchangeType.GoldX2, text);
			break;
		case TypeReward.Gem:
			GifManager.Instance.x2Gem (int.Parse (text));
			break;
		case TypeReward.Key:
			int key = int.Parse (text);
			GifManager.Instance.x2Key (key);
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
		uiChest.OnGifDone ();
		FBManagerEvent.Instance.PostEventCustom ("Chest_done_Pack" + _IDPACK);
	}

	object LoadReward (RollConfig roll)
	{
		object temp = new object ();
		string text = roll._data;
		switch (roll.typeReward) {
		case TypeReward.Gold:
			temp = text;
			break;
		case TypeReward.Gem:
			temp = text;
			break;
		case TypeReward.Key:
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
