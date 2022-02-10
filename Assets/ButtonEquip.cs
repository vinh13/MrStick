using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ButtonEquip : MonoBehaviour
{
	[SerializeField]Image imgPreview = null;
	[SerializeField]Transform tBlock = null, tSelect = null, tPreiview = null;
	[SerializeField]Text textPrice = null;
	[SerializeField]Transform tStarUnlock = null;
	[SerializeField]panelStars panelStar = null;
	[SerializeField]Image imgRank = null;
	[SerializeField]Text textPro = null;
	public int ID = 0;
	Action<int,bool> _cb = null;
	string key = "";
	bool bUnlocked = false;
	int price = 0;
	Transform parent = null;
	int indexSib = 0;

	public void Reset ()
	{
		this.transform.SetParent (parent);
		this.transform.SetSiblingIndex (indexSib);
	}

	public bool Register (int id, Action<int,bool> a, string k, int _price, Sprite spr, EquipLevel level, bool bScale, EquipType eqT)
	{
		parent = this.transform.parent;
		indexSib = this.transform.GetSiblingIndex ();
		if (level.GetHashCode () != 0)
			imgRank.sprite = Resources.Load<Sprite> ("Image/Inven/" + level.GetHashCode ());
		else
			imgRank.sprite = Resources.Load<Sprite> ("Image/Inven/" + "6");
		panelStar.SetStar (level.GetHashCode ());
		imgPreview.sprite = spr;
		imgPreview.SetNativeSize ();
		if (bScale) {
			imgPreview.transform.localScale = new Vector3 (0.15F, 0.15F, 0.15F);
		} else {
			imgPreview.transform.localScale = new Vector3 (0.3F, 0.3F, 0.3F);
		}
		if (eqT != EquipType.Wheels) {
			textPro.text = "+ " + (level.GetHashCode () * 10) + "% HP";
		} else {
			textPro.enabled = false;
		}

		ID = id;
		_cb = a;
		if (id != 0) {
			price = _price;
			key = k;
			bUnlocked = EquipData.GetUnlocked (key);
			Block (!bUnlocked);

			UpdateStar (bUnlocked);

			if (!bUnlocked)
				textPrice.text = "" + CoinManager.Instance.Convert (price);
			else
				panelStar.transform.localPosition = tStarUnlock.localPosition;
			return bUnlocked;


		} else {
			bUnlocked = true;
			panelStar.transform.localPosition = tStarUnlock.localPosition;
			Block (false);

			UpdateStar (bUnlocked);
			return true;
		}
	}

	void UpdateStar (bool b)
	{
		panelStar.SetStar (b);
	}

	public void Select (bool b)
	{
		tSelect.gameObject.SetActive (b);
	}

	public void Preiview (bool b)
	{
		tPreiview.gameObject.SetActive (b);
	}

	public void Block (bool b)
	{
		tBlock.gameObject.SetActive (b);
	}

	public void Click ()
	{
		if (!bUnlocked) {
			_cb.Invoke (ID, false);
		} else {
			_cb.Invoke (ID, true);
		}
	}

	public void ClickAll ()
	{
		if (!bUnlocked) {
			_cb.Invoke (ID, false);
		} else {
			_cb.Invoke (ID, true);
		}
	}

	public void ClickUnlock ()
	{
		if (CoinManager.Instance.CheckCoin (price)) {
			CoinManager.Instance.PurchaseCoin (-price);
			Unlock (true);
		}
	}

	public void Unlock (bool b)
	{
		bUnlocked = true;
		if (b)
			_cb.Invoke (ID, bUnlocked);
		EquipData.SetUnlocked (key, true);
		Block (false);
		Preiview (false);
		UpdateStar (bUnlocked);
	}

	public bool GetUnlocked {
		get { 
			return bUnlocked;
		}
	}
}
