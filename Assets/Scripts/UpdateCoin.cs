using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateCoin : MonoBehaviour
{
	[SerializeField]Text textCoin;
	[SerializeField]TypePurchase Type = TypePurchase.None;
	Scaler scaler = null;
	bool updateCoin = false;

	float timer = 0;
	float duration = 0.25F;
	int coinLate = 0, coinCur = 0;
	int amout = 0;

	void Awake ()
	{
		updateCoin = true;
		scaler = transform.GetChild (2).gameObject.AddComponent<Scaler> ();
	}

	void Start ()
	{
		CoinManager.Instance.RegisterUpdate (Type, UpdateStat);
		if (Type == TypePurchase.Coin)
			textCoin.text = CoinManager.Instance.Convert (CoinManager.Instance.coin);
		if (Type == TypePurchase.Gem)
			textCoin.text = CoinManager.Instance.Convert (CoinManager.Instance.Gem);
		if (Type == TypePurchase.Key)
			textCoin.text = CoinManager.Instance.Convert (CoinManager.Instance.Key);
	}

	void UpdateStat (object ob)
	{
		//textCoin.text = (string)ob;

		timer = duration;
		coinLate = coinCur;
		coinCur = (int)ob;
		updateCoin = false;

		float count = duration / Time.unscaledDeltaTime;

		amout = (int)(((float)Mathf.Abs (coinLate - coinCur)) / count);

		scaler.StopAllCoroutines ();
		scaler.Scale (1.2F, 0.5F, ToNormal);

	}

	void ToNormal ()
	{
		scaler.Scale (1F, 0.5F, null);
	}

	void Update ()
	{
		if (updateCoin)
			return;
		timer -= Time.unscaledDeltaTime;
		if (coinLate > coinCur) {
			coinLate -= amout;
		} else if (coinLate < coinCur) {
			coinLate += amout;
		} else {
			_EndUpdateCoin ();
			return;
		}
		if (timer <= 0) {
			_EndUpdateCoin ();
			return;
		}
		textCoin.text = "" + coinLate;
	}

	void _EndUpdateCoin ()
	{
		if (updateCoin)
			return;
		updateCoin = true;
		coinLate = coinCur;
		string text = coinLate.ToString ("N1");
		string[] texts = text.Split ('.');
		textCoin.text = "" + texts [0];
	}

	void OnDestroy ()
	{
		_EndUpdateCoin ();
		CoinManager.Instance.RemoveUpdate (Type);
	}
}
