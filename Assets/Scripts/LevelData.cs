using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
	#region CurrentLevel

	public static int IDLevel = 0;
	public static string keyLevel = "";
	public static MapID mapID = MapID.None;
	public static bool bNext = false;
	public static AIDataLevel aiDataLevel = AIDataLevel.None;

	#endregion

	public static int LevelCheckRate {
		get { 
			return TaskUtil.GetInt ("LevelCheckRate");
		}
		set { 
			TaskUtil.SetInt ("LevelCheckRate", value);
		}
	}

	public static bool GetUnlock (string key)
	{
		return TaskUtil.GetInt ("Unlock_" + key) == 0 ? false : true;
	}

	public static void SetUnlock (string key, bool b)
	{
		TaskUtil.SetInt ("Unlock_" + key, b ? 1 : 0);
	}

	public static int GetStar (string key)
	{
		return TaskUtil.GetInt ("Star_" + key);
	}

	public static void SetStar (string key, int star)
	{
		TaskUtil.SetInt ("Star_" + key, star);
	}
}
