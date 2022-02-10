using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ButtonGetCoinInApp : MonoBehaviour
{
	[SerializeField]Button btnGetCoin = null;
	[SerializeField]int index = 0;
	[SerializeField]Text textCoinGet = null, textPrice = null, textValue = null, textValueBonus = null, textRateBonus = null;
	Action<int,bool> purchase = null;
	[SerializeField]bool bFirstPurchase = false;
	[SerializeField]string keyPrefix = "";
	[SerializeField]Transform rectBonus = null;

	void Start ()
	{
		btnGetCoin.onClick.AddListener (delegate {
			_OnClick ();	
		});
	}

	public void Setup (string key, int coin, float priceTemp, int i, int bonus, bool bUSD, float rateBonus)
	{
		keyPrefix = key;
		if (bonus == 0) {
			DisableFirstPurchase (coin);
		} 
		index = i;
		bFirstPurchase = GameData.GetFirstPurchase (keyPrefix, index);
		transform.GetChild (1).gameObject.SetActive (!bFirstPurchase);
		textCoinGet.text = "" + TaskUtil.Convert (coin);
		if (!bFirstPurchase) {
			textValue.text = "" + TaskUtil.Convert (coin);
			textValueBonus.text = "+" + TaskUtil.Convert (bonus);
			textRateBonus.text = "Bonus " + rateBonus + "%";
			rectBonus.gameObject.SetActive (true);
		} else {
			rectBonus.gameObject.SetActive (false);
		}
		if (bUSD)
			textPrice.text = "" + priceTemp + " $";
		else
			textPrice.text = TaskUtil.Convert ((int)priceTemp);
	}

	public void Init (Action<int,bool> cb)
	{
		purchase = cb;
	}

	public void _OnClick ()
	{
		purchase.Invoke (index, bFirstPurchase);
	}

	public void DisableFirstPurchase (int coin)
	{
		bFirstPurchase = true;
		textCoinGet.text = "" + TaskUtil.Convert ((int)coin);
		transform.GetChild (1).gameObject.SetActive (false);
		rectBonus.gameObject.SetActive (false);
		GameData.SetFirstPurchase (keyPrefix, index);
	}
}
