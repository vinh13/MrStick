using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementData
{
	public static int GetCurrentLevel (AchievementType type)
	{
		return PlayerPrefs.GetInt ("AchievementData_CurrnetLevel_" + type.ToString (), 0);	
	}

	public static void SetCurrentLevel (AchievementType type, int newLevel)
	{
		PlayerPrefs.SetInt ("AchievementData_CurrnetLevel_" + type.ToString (), newLevel);
		PlayerPrefs.Save ();
	}

	public static int GetCurrentValue (AchievementType type, string s)
	{
		return PlayerPrefs.GetInt ("AchievementData_GetCurrentTarget_" + type.ToString () + s, 0);	
	}

	public static void SetCurrentValue (AchievementType type, string s, int newValue)
	{
		PlayerPrefs.SetInt ("AchievementData_GetCurrentTarget_" + type.ToString () + s, newValue);
		PlayerPrefs.Save ();
	}

	public static bool GetClaimAchievement (string key)
	{
		return PlayerPrefs.GetInt ("AchievementData_GetClaimAchievement" + key, 0) == 0 ? false : true;	
	}

	public static void SetClaimAchievement (string key, int tr)
	{
		PlayerPrefs.SetInt ("AchievementData_GetClaimAchievement" + key, tr);
		PlayerPrefs.Save ();
	}

	public static int TopKiller {
		get {
			return TaskUtil.GetInt ("TopKiller");
		}
		set { 
			TaskUtil.SetInt ("TopKiller", value);
		}
	}
}
