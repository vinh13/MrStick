using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UIFreeExchangeGem : MonoBehaviour
{
	[SerializeField]UIButton btnBack = null, btnEquip = null;
	string mess = "";
	int Gem = 0;
	[SerializeField]RollGold rollGold = null;
	[SerializeField]AnimatorPopUpScript animPop = null;
	Action<object> cb = null;
	[SerializeField]Text textPrice = null;
	void Start ()
	{
		btnBack.Register (ClickBack);
		btnEquip.Register (ClickEquip);
	}

	public void ShowExchangeGEM (Action<object> a, int coin)
	{
		rollGold.SetGold ("" + TaskUtil.Convert (coin * 2));
		Gem = coin;
		cb = a;
		mess = "Gem_" + Gem + "_";
		textPrice.text = TaskUtil.Convert (Gem);
		Show (true);
	}

	void Show (bool b)
	{
		if (b) {
			animPop.show (null);
			btnBack.Block (false);
			btnEquip.Block (false);
			StartCoroutine (checkVideo ());
		} else {
			animPop.hide (OnHide);
		}
	}

	void OnHide ()
	{
		Destroy (this.gameObject);
	}

	IEnumerator checkVideo ()
	{
		while (!AllInOne.Instance.CheckVideoReward ()) {
			yield return null;
		}
		btnEquip.Block (false);
	}

	void ClickBack ()
	{
		mess += "0";
		btnBack.Block (true);
		btnEquip.Block (true);
		cb.Invoke (mess);
		Show (false);
	}

	void ClickEquip ()
	{
		btnBack.Block (true);
		btnEquip.Block (true);
		AllInOne.Instance.ShowVideoReward(cbVideo, "FreeExchange_gem",LevelData.IDLevel);
	}

	void cbVideo (bool b)
	{
		if (b) {
			mess += "1";
			btnBack.Block (true);
			btnEquip.Block (true);
			cb.Invoke (mess);
			Show (false);
		} else {
			ClickBack ();
		}
	}

}
