using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinData : MonoBehaviour
{
	public const int MaxIDSkin = 10;
	public static bool bTry = false;
	public static int IdTry = 0;
	public static int SuggestId = 0;
	public static bool bPreview = false;

	public static string GetSkin {
		get { 
			string s = TaskUtil.GetString ("SkinData_GetSkin");
			if (s == "") {
				s = "0_None";
			}
			return s;
		}
	}

	public static void SetSkin (string s)
	{
		TaskUtil.SetString ("SkinData_GetSkin", s);
	}

	public static string GetSkinPreview {
		get { 
			string s = TaskUtil.GetString ("GetSkinPreview");
			if (s == "") {
				s = "0_None";
			}
			return s;
		}
	}

	public static void SetSkinPreview (string s)
	{
		TaskUtil.SetString ("GetSkinPreview", s);
	}


	public static string GetBike {
		get { 
			string s = TaskUtil.GetString ("SkinData_GetBike");
			if (s == "") {
				s = "0_Wheel";
			}
			return s;
		}
	}

	public static void SetBike (string s)
	{
		TaskUtil.SetString ("SkinData_GetBike", s);
	}

	public static int OderSkin (int idReal)
	{
		switch (idReal) {
		case 1:
			return 1;
		case 2:
			return 3;
		case 3:
			return 2;
		case 4:
			return 4;
		case 5:
			return 6;
		case 6:
			return 5;
		case 7:
			return 12;
		case 8:
			return 9;
		case 9:
			return 8;
		case 10:
			return 9;
		case 11:
			return 10;
		case 12:
			return 11;
		default:
			return 6;
		}
	}

	public static int GetPriceNext (int IDSkin)
	{
		int oder = OderSkin (IDSkin);
		if (oder <= 2) {
			return 0;
		} else {
			return 200;
		}
	}

	public static int IDSkin (int oder)
	{
		switch (oder) {
		case 1:
			return 1;
		case 2:
			return 3;
		case 3:
			return 2;
		case 4:
			return 4;
		case 5:
			return 6;
		case 6:
			return 5;
		case 7:
			return 8;
		case 8:
			return 9;
		case 9:
			return 10;
		case 10:
			return 11;
		case 11:
			return 12;
		default:
			return 6;
		}
	}

	public static int GetNext (int max)
	{
		int id = max + 1;
		id = Mathf.Clamp (id, 1, 6);
		return id;
	}

	public static string NameSkinNext (int IDSkinNext)
	{
		string s = "";
		switch (IDSkinNext) {
		case 0:
			s = "None";
			break;
		case 1:
			s = "Break";
			break;
		case 2:
			s = "Police";
			break;
		case 3:
			s = "Batman";
			break;
		case 4:
			s = "Hulk";
			break;
		case 5:
			s = "Iron";
			break;
		case 6:
			s = "Spider";
			break;
		case 7:
			s = "Cap";
			break;
		case 8:
			s = "Deadpool";
			break;
		case 9:
			s = "Goku";
			break;
		case 10:
			s = "Jocker";
			break;
		case 11:
			s = "Naruto";
			break;
		default :
			s = "" + IDSkinNext;
			break;
		}
		return s;
	}

	public static string SkinSmartThink (int level, string anti)
	{
		string data = "";
		string path = "PlayerSkin/SkinLevel" + level;
		SkinDataLevel skinDataLevel = Resources.Load<SkinDataLevel> (path);
		Debug.Log ("SkinSmartThink" + path);
		for (int i = 0; i < skinDataLevel.skinDatas.Count; i++) {
			string temp = skinDataLevel.skinDatas [i];
			string[] temps = temp.Split ('_');
			if (temps [0] != anti) {
				bool bUnlocked = EquipData.GetUnlocked (temps [0] + "EquipBoss_" + temps [1]);
				Debug.Log ("CC:" + temps [0] + "EquipBoss_" + temps [1] + "_" + bUnlocked);
				if (!bUnlocked) {
					data = temps [0] + "_" + temps [2];
					break;
				}
			}
		}
		return data;
	}
}
 