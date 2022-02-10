using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITutorialEnd : MonoBehaviour
{
	[SerializeField]Text textCoin = null;
	[SerializeField]UIButton btnAgian = null, btnNext = null;
	[SerializeField]AnimatorPopUpScript animPop = null;
	const int Coin = 1000;
	void Start ()
	{
		btnAgian.Register (ClickAgian);
		btnNext.Register (ClickNext);
		animPop.show (OnShow);
		textCoin.text = "+ " + CoinManager.Instance.Convert (Coin);
	}

	void OnShow ()
	{
		
	}

	void ClickNext ()
	{
		Manager.Instance.LoadScene (SceneName.Home, true);
		CoinManager.Instance.PurchaseCoin (Coin);
	}

	void ClickAgian ()
	{
		Manager.Instance.LoadScene (SceneName.Home, true);
		CoinManager.Instance.PurchaseCoin (Coin);
	}
}
