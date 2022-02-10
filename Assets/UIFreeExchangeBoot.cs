using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIFreeExchangeBoot : MonoBehaviour
{
	[SerializeField]RollItem cur = null, next = null;
	[SerializeField]UIButton btnNo = null, btnYes = null;
	[SerializeField]AnimatorPopUpScript animPop = null;
	Action<object> callback = null;
	string mess = "";

	void Start ()
	{
		btnNo.Register (No);
		btnYes.Register (Yes);
	}

	IEnumerator checkVideo ()
	{
		while (!AllInOne.Instance.CheckVideoReward ()) {
			yield return null;
		}
		btnYes.Block (false);
	}

	void Show (bool b)
	{
		if (b) {
			animPop.show (null);
		} else {
			animPop.hide (OnHide);
		}
	}

	void OnHide ()
	{
		Destroy (this.gameObject);
	}

	public void ExChangeX2 (ItemBoot c, ItemBoot n, Action<object> a)
	{
		callback = a;
		mess = c.bootType.ToString () + "_" + c._valueBoot + "_";
		cur.SetItem (c._valueBoot, c.bootType);
		next.SetItem (n._valueBoot, n.bootType);
		Show (true);
		btnYes.Block (true);
		btnNo.Block (false);
		StartCoroutine (checkVideo ());
	}

	void Yes ()
	{
		AllInOne.Instance.ShowVideoReward (CallbackYes,"FreeExhangeBoot",LevelData.IDLevel);	
	}

	void CallbackYes (bool b)
	{
		if (b) {
			mess += "1";
			callback.Invoke (mess);
			btnYes.Block (true);
			btnNo.Block (true);
			Show (false);
		} else {
			No ();
		}
	}

	void No ()
	{
		mess += "0";
		callback.Invoke (mess);
		btnYes.Block (true);
		btnNo.Block (true);
		Show (false);
	}
}
