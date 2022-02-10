using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UISkinEquip : MonoBehaviour
{
	[SerializeField]Image imgPreview = null;
	[SerializeField]UIButton btnBack = null, btnEquip = null;
	[SerializeField]Text textTitle = null;
	[SerializeField]panelStars panelStar = null;
	[SerializeField]AnimatorPopUpScript animPop = null;
	Action<object> cb = null;
	string mess = "";

	public void ShowNewSkin (string Title, Sprite spr, Action<object> a, int id, int level)
	{
		imgPreview.sprite = spr;
		textTitle.text = "" + Title;
		cb = a;
		mess = Title + "_" + id;
		panelStar.SetStar (level);
		Show (true);
	}

	void Show (bool b)
	{
		if (b)
			animPop.show (null);
		else {
			animPop.hide (OnHide);
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
		mess += "_0";
		cb.Invoke (mess);
		Show (false);
	}

	void ClickEquip ()
	{
		mess += "_1";
		cb.Invoke (mess);
		Show (false);
	}
}
