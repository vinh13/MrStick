using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InAppManager : MonoBehaviour
{
	[SerializeField]ButtonGetCoinInApp[] btns;
	[SerializeField]int[] coinsGet, coinsGet_IOS;
	[SerializeField]int[] coinsBonus, coinsBonus_IOS;
	[SerializeField]float[] prices, prices_IOS;
	[SerializeField]float[] rateFristPurchase, rateFristPurchase_IOS;
	[SerializeField]int idRequest = 0;
	[SerializeField]bool bFirstPurchaseRequest = false;
	public const string keyPrefix = "PurchaseCoin";
	//[SerializeField]GemSpend gemSpend = null;
	void _OnValidate ()
	{
		for (int i = 0; i < btns.Length; i++) {

			#if UNITY_ANDROID
			coinsBonus [i] = (int)(((float)coinsGet [i] * rateFristPurchase [i]) / 100F);
			btns [i].Setup (keyPrefix, coinsGet [i], prices [i], i, coinsBonus [i], true, rateFristPurchase [i]);
			#elif UNITY_IPHONE
			coinsBonus_IOS [i] = (int)(((float)coinsGet_IOS [i] * rateFristPurchase_IOS [i]) / 100F);
			btns [i].Setup (keyPrefix, coinsGet_IOS [i], prices_IOS [i], i, coinsBonus_IOS [i], true, rateFristPurchase_IOS [i]);
			#endif
		}
	}

	void OnValidate ()
	{
		_OnValidate ();
	}

	void Awake ()
	{
		_OnValidate ();
		for (int i = 0; i < btns.Length; i++) {
			btns [i].Init (ClickGetCoin);
		}
	}

	public void ClickGetCoin (int index, bool bFirstPurchase)
	{
		if (Purchaser.Instance.IsInitialized ()) {
			idRequest = index;
			bFirstPurchaseRequest = bFirstPurchase;
			Manager.Instance.ShowWaitting (true);
			Purchaser.Instance.ReqestPurchase (AddCoin, index);
		} else {
			Purchaser.Instance.InitializePurchasing ();
			Manager.Instance.ShowWaitting (false);
//				TungDz.EventDispatcher.Instance.PostEvent (EventID.OnShowToast, "Purchase unavailaible, please check again");
		}
	}
	public void AddCoin (object ob)
	{
		string text = (string)ob;
		string[] texts = text.Split ('_');
		Manager.Instance.ShowWaitting (false);
		if (texts [0] == "done") {
			idRequest = int.Parse (texts [1]);
			//Add Gem
			int coin = 0;
			int coinBonus = 0;
			#if UNITY_ANDROID
			coin = coinsGet [idRequest];
			coinBonus = coinsBonus [idRequest];
			#elif UNITY_IPHONE
			coin = coinsGet_IOS [idRequest];
			coinBonus = coinsBonus_IOS [idRequest];
			#endif
			if (!bFirstPurchaseRequest) {
				CoinManager.Instance.PurchaseGem (coin + coinBonus, true);
				btns [idRequest].DisableFirstPurchase (coin);
				bFirstPurchaseRequest = true;
			} else {
				CoinManager.Instance.PurchaseGem (coin, true);
			}
//			#if UNITY_ANDROID
//			gemSpend.UpdateSpend (prices [idRequest]);
//			#elif UNITY_IPHONE
//			gemSpend.UpdateSpend (prices_IOS [idRequest]);
//			#endif
		} else {
			//	TungDz.EventDispatcher.Instance.PostEvent (EventID.OnShowToast, "Failed!");
		}
	}
}
