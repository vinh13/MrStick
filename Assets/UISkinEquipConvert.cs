using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class UISkinEquipConvert : MonoBehaviour
{
	[SerializeField]Image imgPreview = null;
	[SerializeField]UIButton btnBack = null, btnEquip = null;
	[SerializeField]Text textTitle = null, textPrice = null;
	[SerializeField]AnimatorPopUpScript aimPop = null;
	Action<object> cb = null;
	string mess = "";
	int coinGet = 0;
	public void ShowNewSkin (string Title, Sprite spr, Action<object> a, int id, int level, int price)
	{
		imgPreview.sprite = spr;
		textTitle.text = "" + Title;
		textPrice.text = "+" + TaskUtil.Convert (price);
		cb = a;
		coinGet = price;
		mess = Title + "_" + id;
		Show (true);
	}

	void Show (bool b)
	{
		if (b) {
			aimPop.show (null);
		} else {
			aimPop.hide (OnHide);
		}
	}

	void OnHide ()
	{

		Destroy (this.gameObject);
	}

	void Start ()
	{
		btnBack.Register (ClickBack);
		btnEquip.Register (ClickEquip);
	}

	void ClickBack ()
	{
		Show (false);
		cb.Invoke (mess);
	}

	void ClickEquip ()
	{
		cb.Invoke (mess);
		btnEquip.Block (true);
		AllInOne.Instance.ShowVideoReward (CallbackDone,"ConvertSkin",LevelData.IDLevel);
	}

	void CallbackDone (bool b)
	{
		if (b) {
			CoinManager.Instance.PurchaseCoin (coinGet);
		} else {
			
		}
		TaskUtil.Schedule (this, this.Hide, 0.25F);
	}

	void Hide ()
	{
		Show (false);
	}
}
