using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TungDz;

[System.Serializable]
public enum TypePurchase
{
	None = 0,
	Coin = 1,
	Gem = 2,
	Video = 3,
	Key = 4,
}

public class CoinManager : Singleton<CoinManager>
{
	Dictionary<TypePurchase,Action<object>> Listeners = new Dictionary<TypePurchase, Action<object>> ();
	public int coinPerVideo = 1000;
	public int priceBoot = 1000;

	protected CoinManager ()
	{
		//Tdz	
	}

	public void RegisterUpdate (TypePurchase ID, Action<object> cb)
	{
		if (!Listeners.ContainsKey (ID)) {
			Listeners.Add (ID, cb);
		}
	}

	public void RemoveUpdate (TypePurchase ID)
	{
		if (Listeners.ContainsKey (ID)) {
			Listeners.Remove (ID);
		} 
	}

	public int coin {
		get {
			if (_coin == 0) {
				_coin = TaskUtil.GetInt ("COIN");	
			}
			return _coin;
		}
	}

	private int _coin = 0;

	public int Gem {
		get {
			if (gem == 0) {
				gem = TaskUtil.GetInt ("GEM");
			}
			return gem;
		}
	}

	private int gem = 0;


	private int _key = 0;

	public int Key {
		get {
			if (_key == 0) {
				_key = TaskUtil.GetInt ("KEY_KEY_KEY");
			}
			return _key;
		}
	}

	public bool CheckCoin (int price)
	{
		if (price <= _coin) {
			return true;
		} else {
			// Show Not EnoughCoin
			Debug.Log ("K du coin");

			int enough = price - _coin;

			string text = "GOLD_" + Convert (_coin) + "_+" + Convert (enough);

			//EventDispatcher.Instance.PostEvent (EventID.NotEnoughCoin, text);
			Manager.Instance.ShowNotEnough (TypePurchase.Coin, Convert (_coin), Convert (enough), enough);

			return false;
		}
	}

	public bool CheckKey (int k)
	{
		if (k <= Key) {
			return true;
		} else {
			return false;
		}
	}

	public void PurchaserKey (int value)
	{
		_key += value;
		TaskUtil.SetInt ("KEY_KEY_KEY", _key);
		if (Listeners.ContainsKey (TypePurchase.Key))
			Listeners [TypePurchase.Key].Invoke (_key);
		Manager.Instance.ShowToas (Mathf.Abs (value), TypeReward.Key, value > 0);
	}


	public void PurchaseCoin (int value)
	{
		_coin += value;

		if (_coin < 0)
			_coin = Mathf.Abs (_coin);
		else {
		}

		TaskUtil.SetInt ("COIN", _coin);

		if (Listeners.ContainsKey (TypePurchase.Coin))
			Listeners [TypePurchase.Coin].Invoke (_coin);

		Manager.Instance.ShowToas (Mathf.Abs (value), TypeReward.Gold, value > 0);

		SFXManager.Instance.Play ("addgold");

//		Manager.Instance.ShowToast_TypeReward (TypeReward.Gold, Convert (Mathf.Abs (value)), value >= 0);

		//EventDispatcher.Instance.PostEvent (EventID.UpdateAllCoin);

//		if (value < 0) {
//			//Set Data
//			int lateValue = GameData.GoldSpend;
//			lateValue += Mathf.Abs (value);
//			GameData.GoldSpend = lateValue;
//		}
	}

	public string Convert (int value)
	{
		string text = value.ToString ("N1");
		string[] texts = text.Split ('.');
		return texts [0];
	}


	public bool CheckGem (int price)
	{
		if (price <= gem) {
			return true;
		} else {

			// Show Not EnoughCoin

			Debug.Log ("K du gem");

			int enough = price - gem;

			string text = "GEM_" + gem + "_+" + enough;

			Manager.Instance.ShowNotEnough (TypePurchase.Gem, Convert (gem), Convert (enough), enough);

			return false;
		}
	}

	public void PurchaseGem (int value, bool IAP)
	{
		gem += value;

		if (gem < 0)
			gem = Mathf.Abs (gem);

		TaskUtil.SetInt ("GEM", gem);

		Manager.Instance.ShowToas (Mathf.Abs (value), TypeReward.Gem, value > 0);

		if (Listeners.ContainsKey (TypePurchase.Gem))
			Listeners [TypePurchase.Gem].Invoke (gem);
		//EventDispatcher.Instance.PostEvent (EventID.UpdateAllCoin);

//		AudioUIManager.Instance.Play ("UI_AddGem");
//
//		Manager.Instance.ShowToast_TypeReward (TypeReward.Gem, Convert (Mathf.Abs (value)), value >= 0);
//
//		if (value < 0) {
//			//Set Data
//			int lateValue = GameData.GemSpend;
//			lateValue += Mathf.Abs (value);
//			GameData.GemSpend = lateValue;
//		} else {
//			if (IAP) {
//				int lateTotal = GameData.TotalGem;
//				lateTotal += value;
//				GameData.TotalGem = lateTotal;
//				EventDispatcher.Instance.PostEvent (EventID.UpdateVip);
//			}
//		}

	}

}
