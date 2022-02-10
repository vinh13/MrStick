using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

[System.Serializable]
public class ItemBoot
{
	public BootType bootType = BootType.None;
	public int _valueBoot = 0;
}

public class RollManager : MonoBehaviour
{
	[SerializeField]Transform parent = null;
	[SerializeField]RollOption[] rollOptions;
	RollData roll = null;
	[SerializeField]UIButton btnRoll = null, btnFreeRoll = null;
	[SerializeField]RollCantro rollCantro = null;
	[SerializeField]int idGif = 6;
	int lateIDData = 0;
	bool bFree = false;
	bool bRoll = false;
	Action<object> _SyncReward = null;
	int countRoll = 0;
	#if UNITY_EDITOR
	void OnValidate ()
	{
		rollOptions = parent.GetComponentsInChildren<RollOption> ();
	}
	#endif
	void Awake ()
	{
		lateIDData = 100;
		LoadData (1, true);
		btnRoll.Register (Roll);
		btnFreeRoll.Register (FreeRoll);
	}

	void TakeGif ()
	{
		countRoll = GameData.CountRoll;
		if (countRoll < 2) {
			idGif = RandomID (3);
		} else if (countRoll < 4) {
			idGif = RandomID (4);
		} else if (countRoll < 6) {
			idGif = RandomID (5);
		} else if (countRoll < 8) {
			idGif = RandomID (6);
		} else if (countRoll < 10) {
			idGif = RandomID (10);
		}
		idGif = Mathf.Clamp (idGif, 0, 6);
	}

	void LoadData (int id, bool preview)
	{
		TakeGif ();
		bRoll = GameData.GetRollAction (id, "gRoll");
		bFree = GameData.GetRollAction (id, "gFree");
		if (lateIDData != id) {
			roll = Resources.Load<RollData> ("Roll/R" + id);
			for (int i = 0; i < rollOptions.Length; i++) {
				rollOptions [i].LoadReward (LoadReward (roll.rollsConfig [i]), i, roll.rollsConfig [i].typeReward);
			}
			lateIDData = id;
		} else {
			
		}
		if (!preview) {
			btnRoll.Block (bRoll);
			if (!bFree) {
				btnFreeRoll.Block (true);
				StopAllCoroutines ();
				StartCoroutine (checkVideoBtn ());
			} else {
				btnFreeRoll.Block (true);
			}
		} else {
			btnRoll.Block (true);
			btnFreeRoll.Block (true);
		}
	}

	int RandomID (int range)
	{
		return Random.Range (1, range) - 1;
	}

	public void Roll ()
	{
		if (bRoll)
			return;
		bRoll = true;
		FBManagerEvent.Instance.PostEventCustom ("Roll_Free");
		GameData.SetRollAction (lateIDData, "gRoll", bRoll);
		Callback ();
		btnRoll.Block (true);
		btnFreeRoll.Block (true);
		StopAllCoroutines ();
	}

	void SyncReward (object ob)
	{
		_SyncReward.Invoke (ob);
	}

	public void FreeRoll ()
	{
		if (bFree)
			return;
		bFree = true;
		FBManagerEvent.Instance.PostEventCustom ("Roll_Video");
		GameData.SetRollAction (lateIDData, "gFree", bFree);
		AllInOne.Instance.ShowVideoReward (Free, "Roll_Video",LevelData.IDLevel);
		btnFreeRoll.Block (true);
		btnRoll.Block (true);
	}

	public void ShowRoll (int id, bool auto, bool bReward, Action<object> a)
	{
		_SyncReward = a;
		rollCantro.Show (true);
		rollCantro.BlockExit (!auto);
		btnRoll.Block (true);
		btnFreeRoll.Block (true);
		//GetData
		if (bReward) {
			LoadData (id + 1, true);
		} else {
			LoadData (id + 1, false);
		}
	}

	public void OffRoll ()
	{
		GifSync gifSync = new GifSync ();
		gifSync.ID = lateIDData - 1;
		gifSync.bRewarded = Bconvert ();
		SyncReward (gifSync);
		rollCantro.Show (false);
	}

	public void ShowRollAuto ()
	{
		rollCantro.Show (true);
		rollCantro.BlockExit (true);
	}

	void Callback ()
	{
		rollCantro.Roll (idGif, RollDone);
		GifSync gifSync = new GifSync ();
		gifSync.ID = lateIDData - 1;
		gifSync.bRewarded = Bconvert ();
		SyncReward (gifSync);
	}

	bool Bconvert ()
	{
		bool temp = false;
		if (bRoll && bFree) {
			temp = true;
		} else {
			temp = false;
		}
		return temp;
	}

	void Free (bool b)
	{
		if (b) {
			Callback ();
		} else {
			StartCoroutine (checkVideoBtn ());
		}
	}

	void RollDone (object ob)
	{
		if (!bFree)
			StartCoroutine (checkVideoBtn ());
		if (!bRoll)
			btnRoll.Block (false);


		countRoll += 1;
		GameData.CountRoll = countRoll;

		SFXManager.Instance.Play ("unlock");

		int id = (int)ob;
		RewardGif (roll.rollsConfig [id]);
	}

	void Update ()
	{
		if (Input.GetKey (KeyCode.I)) {
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

	IEnumerator checkVideoBtn ()
	{
		while (!AllInOne.Instance.CheckVideoReward ()) {
			yield return null;
		}
		btnFreeRoll.Block (false);
	}
}
