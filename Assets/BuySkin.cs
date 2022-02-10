using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BuySkin : MonoBehaviour
{
	Action<bool> bBuy = null;
	Action<bool> bTry = null;
	[SerializeField]UIButton btnBuy = null;
	[SerializeField]UIButton btnTry = null;
	[SerializeField]Text textPrice = null;
	int curPrice = 0;

	public void Show (bool b)
	{
		gameObject.SetActive (b);
	}

	public void Register (Action<bool> b, Action<bool> t)
	{
		bBuy = b;
		bTry = t;
		btnBuy.Register (ClickBuy);
		btnTry.Register (ClickTry);
		btnBuy.Block (true);
		btnTry.Block (true);
	}

	void ClickBuy ()
	{
		if (CoinManager.Instance.CheckGem (curPrice)) {
			CoinManager.Instance.PurchaseGem (-curPrice, false);
			bBuy.Invoke (true);
			btnBuy.Block (true);
			btnTry.Block (true);
		}
	}

	void ClickTry ()
	{
		
	}

	public void Refesh (int price, bool b)
	{
		StopAllCoroutines ();
		curPrice = price;
		textPrice.text = "-" + CoinManager.Instance.Convert (price);
		btnBuy.Block (b);
		if (!b) {
			btnTry.Block (true);
			StartCoroutine (checkVideo ());
		} else {
			btnTry.Block (true);
		}
	}

	IEnumerator checkVideo ()
	{
		while (!AllInOne.Instance.CheckVideoReward ()) {
			yield return null;
		}
		btnTry.Block (false);
	}

}
