using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{

	#region CardGift

	public static bool GetLucky (string key)
	{
		return TaskUtil.GetInt ("GameDataLucky_bGetLucky") == 0 ? false : true;
	}

	public static void SetLucky (string key, bool b)
	{
		TaskUtil.SetInt ("GameDataLucky_bGetLucky", b ? 1 : 0);
	}


	public static int Lucky {
		get { 
			return TaskUtil.GetInt ("GameDataLucky");
		}
		set { 
			TaskUtil.SetInt ("GameDataLucky", value);
		}
	}

	public static bool bFirstTypeI {
		get {
			return TaskUtil.GetInt ("GameDataLucky_bFirstTypeI") == 0 ? false : true;
		}
		set { 
			TaskUtil.SetInt ("GameDataLucky_bFirstTypeI", value ? 1 : 0);
		}
	}

	public static bool bFirstTypeII {
		get {
			return TaskUtil.GetInt ("GameDataLucky_bFirstTypeII") == 0 ? false : true;
		}
		set { 
			TaskUtil.SetInt ("GameDataLucky_bFirstTypeII", value ? 1 : 0);
		}
	}

	public static bool bFirstTypeIII {
		get {
			return TaskUtil.GetInt ("GameDataLucky_bFirstTypeIII") == 0 ? false : true;
		}
		set { 
			TaskUtil.SetInt ("GameDataLucky_bFirstTypeIII", value ? 1 : 0);
		}
	}



	#endregion

	#region FirstPurchase

	public static bool GetFirstPurchase (string keyPrefix, int index)
	{
		return TaskUtil.GetInt ("FirstPurchase_" + keyPrefix + "_" + index) == 0 ? false : true;
	}

	public static void SetFirstPurchase (string keyPrefix, int index)
	{
		TaskUtil.SetInt ("FirstPurchase_" + keyPrefix + "_" + index, 1);
	}

	#endregion

	#region RateUS

	public static bool RateUSNow {
		get { 
			return TaskUtil.GetInt ("RateUSNow_tnh") == 0 ? false : true;
		}
		set { 
			TaskUtil.SetInt ("RateUSNow_tnh", value ? 1 : 0);
		}
	}

	#endregion

	#region RewardVideo

	public static int GetCountVideoReward (TypeReward typeReward, VideoType videoType)
	{
		return TaskUtil.GetInt (typeReward.ToString () + videoType.ToString () + "_Count");
	}

	public static void SetCountVideoReward (TypeReward typeReward, VideoType videoType, int newCount)
	{
		TaskUtil.SetInt (typeReward.ToString () + videoType.ToString () + "_Count", newCount);
	}

	public static bool GetRewardVideo (TypeReward typeReward, VideoType videoType)
	{
		return TaskUtil.GetInt (typeReward.ToString () + videoType.ToString () + "_RewardVideo") == 0 ? false : true;
	}

	public static void SetRewardVideo (TypeReward typeReward, VideoType videoType)
	{
		TaskUtil.SetInt (typeReward.ToString () + videoType.ToString () + "_RewardVideo", 1);
	}

	public static void ResetAllRewardVideo ()
	{
		TaskUtil.SetInt (TypeReward.Gold.ToString () + VideoType.GoldShop.ToString () + "_RewardVideo", 0);

		SetCountVideoReward (TypeReward.Gold, VideoType.GoldShop, 0); 

		TaskUtil.SetInt (TypeReward.Gem.ToString () + VideoType.None.ToString () + "_RewardVideo", 0);

		SetCountVideoReward (TypeReward.Gem, VideoType.GoldShop, 0); 

	}

	#endregion

	#region Time

	public static int DayCount {
		get {
			return TaskUtil.GetInt ("DayCount");
		}set {
			TaskUtil.SetInt ("DayCount", value);
		}
	}

	public static bool GetDayRewarded (string day)
	{
		return TaskUtil.GetInt ("DayRewarded_" + day) == 0 ? false : true;
	}

	public static void SetDayRewarded (string day, bool rewarded)
	{
		TaskUtil.SetInt ("DayRewarded_" + day, rewarded ? 1 : 0);
	}

	#endregion

	#region StarGif


	public static int GetStarGif {
		get {
			return TaskUtil.GetInt ("StarGif_Roll");
		}
		set { 
			TaskUtil.SetInt ("StarGif_Roll", value);
		}
	}

	public static bool GetDailyGifRewarded {
		get {
			return TaskUtil.GetInt ("GetDailyGifRewarded") == 0 ? false : true;
		}
		set { 
			TaskUtil.SetInt ("GetDailyGifRewarded", value ? 1 : 0);
		}
	}

	public static bool GetGifRewarded (string key)
	{
		return TaskUtil.GetInt ("GifRewarded_" + key) == 0 ? false : true;
	}

	public static void SetGifRewarded (string key, bool b)
	{
		TaskUtil.SetInt ("GifRewarded_" + key, b ? 1 : 0);
	}

	public static bool GetRollAction (int idRoll, string text)
	{
		return TaskUtil.GetInt ("GetRollAction" + idRoll + "_" + text) == 0 ? false : true;
	}

	public static void SetRollAction (int idRoll, string text, bool b)
	{
		TaskUtil.SetInt ("GetRollAction" + idRoll + "_" + text, b ? 1 : 0);
	}

	public static void ClearRoll ()
	{
		if (GameData.GetDailyGifRewarded) {
			//Clear
			GameData.GetDailyGifRewarded = false;
			GameData.GetStarGif = 0;
			for (int i = 0; i < 10; i++) {
				GameData.SetGifRewarded ("tdz" + i, false);
			}
			for (int j = 1; j < 11; j++) {
				GameData.SetRollAction (j, "gRoll", false);
				GameData.SetRollAction (j, "gFree", false);
			}
		} else {
			int temp = 0;
			for (int j = 1; j < 11; j++) {
				bool b = GameData.GetRollAction (j, "gRoll");
				if (b)
					temp++;
			}
			if (temp >= 2) {
				GameData.GetDailyGifRewarded = false;
				GameData.GetStarGif = 0;
				for (int i = 0; i < 10; i++) {
					GameData.SetGifRewarded ("tdz" + i, false);
				}
				for (int j = 1; j < 11; j++) {
					GameData.SetRollAction (j, "gRoll", false);
					GameData.SetRollAction (j, "gFree", false);
				}
			}

		}
		GameData.CheckUpdate = false;
	}

	public static int CountRoll {
		get {
			return TaskUtil.GetInt ("Gif_CountRoll");
		}
		set {
			TaskUtil.SetInt ("Gif_CountRoll", value);
		}
	}

	#endregion

	#region Update

	public static bool CheckUpdate {
		get {
			return TaskUtil.GetInt ("CheckUpdate_Tdz") == 0 ? false : true;
		}
		set {
			TaskUtil.SetInt ("CheckUpdate_Tdz", value ? 1 : 0);
		}
	}

	#endregion

	#region Setting

	public static string PlayerName {
		get { 
			return TaskUtil.GetString ("pName");
		}
		set { 
			TaskUtil.SetString ("pName", value);
		}
	}

	public static int PlayerLevel {
		get { 
			return TaskUtil.GetInt ("PlayerLevel");
		}
		set { 
			TaskUtil.SetInt ("PlayerLevel", value);
		}
	}

	public static int PlayerExp {
		get { 
			return TaskUtil.GetInt ("PlayerExp");
		}
		set { 
			TaskUtil.SetInt ("PlayerExp", value);
		}
	}

	#endregion

	#region FlashSale

	public static bool StartFlashSale {
		get {
			bool temp = TaskUtil.GetInt ("StartFlashSale") == 0 ? true : false;
			if (temp) {
				TaskUtil.SetInt ("StartFlashSale", 1);
			}
			return temp;
		}
	}

	public static bool StartPackSale {
		get {
			bool temp = TaskUtil.GetInt ("StartPackSale") == 0 ? true : false;
			if (temp) {
				TaskUtil.SetInt ("StartPackSale", 1);
			}
			return temp;
		}
	}


	public static bool BlockShowFlashSale {
		get {
			return TaskUtil.GetInt ("BlockShowFlashSale") == 0 ? false : true;
		}
		set {
			TaskUtil.SetInt ("BlockShowFlashSale", value ? 1 : 0);

		}
	}

	public static int CountClickExit {
		get {
			return TaskUtil.GetInt ("CountClickExit");
		}
		set {
			TaskUtil.SetInt ("BlockShowFlashSale", value);
		}
	}

	#endregion

	#region LevelGif

	public static bool GetLevelGif (string key)
	{
		return TaskUtil.GetInt ("bGetLevelGif_" + key) == 0 ? false : true;
	}

	public static void SetLevelGif (string key, bool value)
	{
		TaskUtil.SetInt ("bGetLevelGif_" + key, value ? 1 : 0);
	}

	#endregion

	#region Gifcode

	public static bool GifCode (string key)
	{
		bool temp = GetGifCode (key);
		if (!temp) {
			//SetGifCode (key, true);
		}
		return temp;
	}

	private static bool GetGifCode (string key)
	{
		return TaskUtil.GetInt ("GetGifCode_" + key) == 0 ? false : true; 
	}

	private static void SetGifCode (string key, bool b)
	{
		TaskUtil.SetInt ("GetGifCode_" + key, b ? 1 : 0);
	}

	#endregion

	#region ChestData

	public static string ChestData {
		get { 
			return TaskUtil.GetString ("GameDataChestData");
		}
		set { 
			TaskUtil.SetString ("GameDataChestData", value);
		}
	}

	#endregion

	#region VideoNotEnoughCoin

	public static int VideoNotEnoughCoin {
		get {
			return TaskUtil.GetInt ("VideoNotEnoughCoin");
		}
		set {
			TaskUtil.SetInt ("VideoNotEnoughCoin", value);

		}
	}

	#endregion
}
