using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipData
{
	public static bool GetUnlocked (string key)
	{
		return TaskUtil.GetInt (key) == 0 ? false : true;
	}

	public static void SetUnlocked (string key, bool b)
	{
		Debug.Log (key);
		TaskUtil.SetInt (key, b ? 1 : 0);
	}

	public static int GetCount (string key)
	{
		return TaskUtil.GetInt ("CountEquip" + key);
	}

	public static void SetCount (string key, int newCount)
	{
		TaskUtil.SetInt ("CountEquip" + key, newCount);
	}

	public static int GetEquipTypeCurrentID (EquipType t)
	{
		return TaskUtil.GetInt ("EquipTypeCurrentID" + t.ToString ());
	}

	public static void SetEquipTypeCurrentID (EquipType t, int newID)
	{
		TaskUtil.SetInt ("EquipTypeCurrentID" + t.ToString (), newID);
	}

	public static string GetKey (EquipType equipType)
	{
		return equipType.ToString () + "EquipBoss";
	}
}
