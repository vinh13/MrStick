using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertGem : MonoBehaviour
{
	[SerializeField]ButtonGetCoinInApp[] btns;
	[SerializeField]int[] coinsGet;
	[SerializeField]int[] coinsBonus;
	[SerializeField]float[] prices;
	[SerializeField]float[] rateFristPurchase;
	[SerializeField]int idRequest = 0;
	[SerializeField]bool bFirstPurchaseRequest = true;
	public const string keyPrefix = "PurchaseGoldGold";
	void _OnValidate ()
	{
		for (int i = 0; i < btns.Length; i++) {
			coinsBonus [i] = (int)(((float)coinsGet [i] * rateFristPurchase [i]) / 100F);
			btns [i].Setup (keyPrefix, coinsGet [i], prices [i], i, coinsBonus [i], false, rateFristPurchase [i]);
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
		idRequest = index;
		bFirstPurchaseRequest = bFirstPurchase;
		if (CoinManager.Instance.CheckGem ((int)prices [idRequest])) {
			if (!bFirstPurchaseRequest) {
				CoinManager.Instance.PurchaseCoin (coinsGet [idRequest] + coinsBonus [idRequest]);
				bFirstPurchaseRequest = true;
				btns [idRequest].DisableFirstPurchase (coinsGet [idRequest]);
			} else {
				CoinManager.Instance.PurchaseCoin (coinsGet [idRequest]);
			}
			CoinManager.Instance.PurchaseGem (-(int)prices [idRequest], false);
		}
	}
}
