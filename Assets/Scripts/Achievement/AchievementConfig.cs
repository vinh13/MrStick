using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Target_Reward
{
	public string IndexRoman = "";
	public int target = 0;
	public TypeReward typeReward = TypeReward.Gem;
	public int reward = 0;
}

[System.Serializable]
public enum AchievementType
{
	None = 0,
	Warrior = 1,
	Perfection = 2,
	SmartPeople = 3,
	Generousity = 4,
	RichMan = 5,
	UnlockHero = 6,
	NewPlayer = 7,
	HardPlayer = 8,
	SrazyPlayer = 9,
	NighmatePlayer = 10,
	  
}
public class AchievementConfig : ScriptableObject
{
	public AchievementID ID = AchievementID.None;
	public AchievementType achievementType = AchievementType.None;
	public string AchievementName = "";
	public string DescribeBefore = "";
	public string DescribeAfter = "";
	[SerializeField]string stringValue = "";
	[SerializeField]string strinReward = "";
	public List<Target_Reward> listTarget_Reward = new List<Target_Reward> ();
	void OnValidate ()
	{
		listTarget_Reward.Clear ();
		if (stringValue != "" && strinReward != "") {
			GetValue ();
		}
	}

	void OnEnable ()
	{
		OnValidate ();
	}

	void GetValue ()
	{
		string[] values = stringValue.Split ('-');
		string[] rewards = strinReward.Split ('-');
		for (int i = 0; i < values.Length; i++) {
			Target_Reward tr = new Target_Reward ();
			tr.IndexRoman = intToXL (i + 1);
			tr.target = int.Parse (values [i].Trim ());
			tr.reward = int.Parse (rewards [i].Trim ());
			listTarget_Reward.Add (tr);
		}
	}

	string intToXL (int index)
	{
		switch (index) {
		case 1:
			return "I";
		case 2:
			return "II";
		case 3:
			return "III";
		case 4:
			return "IV";
		case 5:
			return "V";
		case 6:
			return "VI";
		case 7:
			return "VII";
		case 8:
			return "VIII";
		case 9:
			return "IX";
		case 10:
			return "X";
		default:
			return "Null";
		}
	}
}
