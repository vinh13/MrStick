using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public enum PlayerSettingID
{
	None = 0,
	ChangeName = 1,
	ChangeExp = 2,
	ChangeAvatar = 3,
}

[System.Serializable]
public class PlayerLevel
{
	public int Level = 0;
	public int Exp = 0;
}

[System.Serializable]
public class ExpLevelChange
{
	public int level = 0;
	public int exp = 0;
	public int maxExp = 0;
	public float ratioFill = 0;
}

public class PlayerManagerTdz : MonoBehaviour
{
	#region Singleton

	private static PlayerManagerTdz instance;

	public static PlayerManagerTdz Instance {
		get { 

			if (instance == null) {
				GameObject singletonObject = Instantiate (Resources.Load<GameObject> ("Manager/PlayerManagerTdz"));
				instance = singletonObject.GetComponent<PlayerManagerTdz> ();
				singletonObject.name = "Singleton - PlayerManagerTdz";
				//singletonObject.hideFlags = HideFlags.HideInHierarchy;
			}
			return instance;
		}
	}

	public static bool HasInstance ()
	{
		return instance != null;
	}



	#endregion

	#region RegisterName

	Iui1 CreateGo (string path, Transform parent)
	{
		return Instantiate (Resources.Load<GameObject> (path), parent).GetComponent<Iui1> ();
	}

	public void ShowRegisterName (Transform rectHUD)
	{
		Iui1 register = CreateGo ("UI/UIRegisterName", rectHUD);
		register.Register (RegisterName);
		register.Show ();
	}

	void RegisterName (object ob)
	{
		string t = (string)ob;
		GameData.PlayerName = t;
		Call (PlayerSettingID.ChangeName, "");
	}

	#endregion

	#region Listener

	Dictionary<PlayerSettingID,Action<object>> Listeners = new Dictionary<PlayerSettingID, Action<object>> ();

	public void Register (PlayerSettingID ID, Action<object> cb)
	{
		if (!Listeners.ContainsKey (ID)) {
			Listeners.Add (ID, cb);
		}
	}

	public void Remove (PlayerSettingID ID)
	{
		if (Listeners.ContainsKey (ID)) {
			Listeners.Remove (ID);
		} 
	}

	public void Call (PlayerSettingID ID, object ob)
	{
		if (Listeners.ContainsKey (ID)) {
			Listeners [ID].Invoke (ob);
		}
	}

	#endregion

	public int currentExp = 0;
	public int currentLevel = 0;
	public int IDAvatar = 0;
	List<PlayerLevel> listLevelExp = new List<PlayerLevel> ();
	string path = "PlayerLevel/PlayerLevel";

	void Awake ()
	{
		if (instance != null && instance.GetInstanceID () != this.GetInstanceID ()) {
			Destroy (gameObject);
			return;
		} else {
			instance = this as PlayerManagerTdz;
			DontDestroyOnLoad (gameObject);
		}
		currentExp = GameData.PlayerExp;
		currentLevel = GameData.PlayerLevel;
		if (currentLevel == 0) {
			currentLevel = 1;
			GameData.PlayerLevel = currentLevel;
		}
		LoadData ();
	}

	void LoadData ()
	{
		TextAsset textAsset = Resources.Load<TextAsset> (path);
		string[] texts = textAsset.text.Split ('\n');
		listLevelExp.Clear ();
		int lateExp = 0;
		int index = 0;
		foreach (string text in texts) {
			string[] s = text.Split ('\t');
			PlayerLevel temp = new PlayerLevel ();
			temp.Level = int.Parse (s [0]);
			if (index > 0)
				lateExp = listLevelExp [index - 1].Exp;
			temp.Exp = int.Parse (s [1]) + lateExp;
			listLevelExp.Add (temp);
			index++;
		}
	}

	#region Level_Exp

	public void UpdateExp (int exp)
	{
		currentExp += exp;
		GameData.PlayerExp = currentExp;
		int level = currentLevel;
		for (int i = currentLevel - 1; i < listLevelExp.Count; i++) {
			if (currentExp >= listLevelExp [i].Exp) {
				level = i + 1;
			}
		}
		currentLevel = level;
		float fillAmout = 0;
		ExpLevelChange exp_p_level = new ExpLevelChange ();
		if (level < listLevelExp.Count) {
			float amout = (float)listLevelExp [level].Exp - (float)currentExp;
			float lateExp = (float)listLevelExp [level - 1].Exp;
			float maxAmount = (float)listLevelExp [level].Exp - lateExp;

			fillAmout = amout / maxAmount;

			fillAmout = 1 - fillAmout;

			fillAmout = Mathf.Clamp (fillAmout, 0, 1F);
			exp_p_level.level = level;
			exp_p_level.exp = (int)(maxAmount - amout);
			exp_p_level.maxExp = (int)maxAmount;
			exp_p_level.ratioFill = fillAmout;
		} else {
			fillAmout = 1F;
			exp_p_level.level = level;
			exp_p_level.exp = 0;
			exp_p_level.maxExp = 0;
			exp_p_level.ratioFill = fillAmout;
		}
		GameData.PlayerLevel = currentLevel;
		Call (PlayerSettingID.ChangeExp, exp_p_level);
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.U)) {
			UpdateExp (100);
		}
	}

	#endregion
}

