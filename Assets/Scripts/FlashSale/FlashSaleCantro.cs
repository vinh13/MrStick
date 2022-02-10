using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

[System.Serializable]
public enum TypeSale
{
	FL = 0,
	Start = 1,
}

public class FlashSaleCantro : MonoBehaviour
{
	[SerializeField]ParticleSystem parS = null;
	[SerializeField]UIFlashSale uiFlashSale = null;
	[SerializeField]Text textDealine = null;
	[SerializeField]UIButton[] btn;
	[SerializeField]UIButton btnQuit = null;
	[SerializeField]int[] valueRewards = new int[3];
	[SerializeField]string keyTime = "";
	[SerializeField]string[] subs;
	[SerializeField]Offer offer = null;
	[SerializeField]int idPack = 6;
	private TimeSpan _startTime;
	private TimeSpan _endTime;
	private TimeSpan _remainingTime;
	float _value = 0;
	int CountClickExit = 0;
	bool hideing = false;
	[SerializeField]SceneName sceneName = SceneName.FlashSale;
	[SerializeField]TypeSale typeSale = TypeSale.FL;
	bool bUncloked = false;
	[SerializeField]int idSkin = 0;

	void OnValidate ()
	{
		offer.SetValue (valueRewards);
		offer.SetText (subs [0], subs [1], subs [2]);
	}

	void Awake ()
	{
		string path = "PlayerSkin/" + idSkin + "/" + idSkin;
		PartSkinData data = Resources.Load<PartSkinData> (path);
		string skindata = "";
		for (int i = 0; i < data.skinID.Length; i++) {
			skindata += idSkin + "_" + data.skinID [i].ToString () + ".";
		}
		SkinData.bPreview = true;
		SkinData.SetSkinPreview (skindata);
	}

	void Start ()
	{
		bUncloked = PlayerPrefs.GetInt (keyTime + "Unlocked", 0) == 0 ? false : true;
		bSameDay = TimeManager.Instance.bSameDay;
		bool b = false;
		switch (typeSale) {
		case TypeSale.FL:
			b = GameData.StartFlashSale;
			break;
		case TypeSale.Start:
			b = GameData.StartPackSale;
			break;
		}
		if (b) {
			PlayerPrefs.SetString (keyTime, TimeManager.Instance.System_getCurrentTimeNow ());
			PlayerPrefs.Save ();
		}
		_configTimerSettings (3);

		TaskUtil.Schedule (this, Show, 0.15F);
		parS.Play ();
		CountClickExit = GameData.CountClickExit;
		btn [0].Register (ClickPackI);
		//btn [1].Register (ClickPackII);
		btnQuit.Register (ClickExit);

		btn [0].Block (bUncloked);
	}

	void Show ()
	{
		uiFlashSale.Show (OnShow);
	}

	void OnShow ()
	{
		Manager.Instance.ShowWaitting (false);
	}

	void OnHide ()
	{
		Manager.Instance.ShowWaitting (false);
		SceneManager.UnloadSceneAsync (sceneName.GetHashCode ());
	}

	public void ClickExit ()
	{
		if (hideing)
			return;
		hideing = true;
		CountClickExit++;
		Manager.Instance.ShowWaitting (true);
		uiFlashSale.Hide (OnHide);
//		if (!GameData.BlockShowFlashSale) {
//			int countMax = GameData.PlayerLevel >= 14 ? 3 : 5;
//			if (CountClickExit >= countMax) {
//				CountClickExit = 0;
//				GameData.BlockShowFlashSale = true;
//			}
//		}
//		GameData.CountClickExit = CountClickExit;
		parS.Stop ();
	}

	public void ClickPackI ()
	{
		Manager.Instance.ShowWaitting (true);
		Purchaser.Instance.ReqestPurchase (CallbackPackI, idPack);
	}

	void CallbackPackI (object ob)
	{
		string text = (string)ob;
		string[] texts = text.Split ('_');
		if (texts [0] == "done") {
			TaskUtil.Schedule (this, _CallbackPackI, 0.15F);
		} else {
			Manager.Instance.ShowWaitting (false);
		}
	}

	void _CallbackPackI ()
	{
		CoinManager.Instance.PurchaseCoin (valueRewards [0]);
		CoinManager.Instance.PurchaseGem (valueRewards [1], true);
		CoinManager.Instance.PurchaserKey (valueRewards [2]);
		Manager.Instance.ShowWaitting (false);


		EquipConfig[] equipsConfig = new EquipConfig[3];

		equipsConfig [0] = Resources.Load<EquipConfig> ("SkinData/" + idSkin + "/Head");
		equipsConfig [1] = Resources.Load<EquipConfig> ("SkinData/" + idSkin + "/Body");
		equipsConfig [2] = Resources.Load<EquipConfig> ("SkinData/" + idSkin + "/Arm");

		GifData[] gifData = new GifData[3];
		for (int i = 0; i < gifData.Length; i++) {
			gifData [i] = new GifData ();
			gifData [i].gifType = GifType.Skin;
			gifData [i].Ob = equipsConfig [i];
		}
		GifManager.Instance.GifNow (gifData);
		bUncloked = true;
		PlayerPrefs.SetInt (keyTime + "Unlocked", 1);
		PlayerPrefs.Save ();
		btn [0].Block (bUncloked);
	}

	//	public void ClickPackII ()
	//	{
	//		Manager.Instance.ShowWaitting (true);
	//		Purchaser.Instance.ReqestPurchase (CallbackPackII, 7);
	//	}
	//
	//	void CallbackPackII (object ob)
	//	{
	//		string text = (string)ob;
	//		string[] texts = text.Split ('_');
	//		if (texts [0] == "done") {
	//			TaskUtil.Schedule (this, _CallbackPackII, 0.15F);
	//		} else {
	//			Manager.Instance.ShowWaitting (false);
	//		}
	//	}
	//
	//	void _CallbackPackII ()
	//	{
	//		int gemReward = 3500;
	//		int coinReward = 150000;
	//		Manager.Instance.ShowWaitting (false);
	//		CoinManager.Instance.PurchaseGem (gemReward, true);
	//		CoinManager.Instance.PurchaseCoin (coinReward);
	//	}

	bool bSameDay = false;
	bool _timerComplete = false;

	private void _configTimerSettings (int hour)
	{
		_startTime = TimeSpan.Parse (PlayerPrefs.GetString (keyTime));
		_endTime = TimeSpan.Parse ("" + hour + ":" + 0 + ":" + 0);
		TimeSpan temp = TimeSpan.Parse (TimeManager.Instance.System_getCurrentTimeNow ());
		TimeSpan diff = temp.Subtract (_startTime);
		_remainingTime = _endTime.Subtract (diff);
		float ah = 1f / (float)_endTime.TotalSeconds;
		float bh = 1f / (float)_remainingTime.TotalSeconds;
		_value = ah / bh;
		if (bSameDay) {
			if (diff > _endTime) {
				//da xong
				_timerComplete = true;

				TimeSpan range = diff - _endTime;

				int count = (int)(range.TotalMinutes / _endTime.TotalMinutes);

				//UpdateStamina (count);


			} else {
				// chua xong
				_timerComplete = false;
			}
		} else {
			bSameDay = true;
		}
	}

	public string GetRemainingTime (double x)
	{
		TimeSpan tempB = TimeSpan.FromMilliseconds (x);
		return string.Format ("{0:D2}:{1:D2}:{2:D2}", tempB.Hours, tempB.Minutes, tempB.Seconds);
	}

	#region Update

	void Update ()
	{
		if (!_timerComplete) {
			_value -= Time.unscaledDeltaTime * 1f / (float)_endTime.TotalSeconds;

			double x = _value * _endTime.TotalMilliseconds;

			textDealine.text = GetRemainingTime (x);


			if (_value <= 0) {
				_timerComplete = true;
				ResetTime ();
			}
		}
	}

	void ResetTime ()
	{
		PlayerPrefs.SetString (keyTime, TimeManager.Instance.System_getCurrentTimeNow ());
		PlayerPrefs.Save ();
		_configTimerSettings (3);
	}

	#endregion
}
