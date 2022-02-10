using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ButtonEquipAll : MonoBehaviour
{
	[SerializeField]Image imgPreview = null;
	[SerializeField]Transform tBlock = null, tSelect = null, tPreiview = null;
	[SerializeField]Transform tStarUnlock = null;
	[SerializeField]panelStars panelStar = null;
	[SerializeField]Image imgRank = null;
	Action<int> _cb = null;
	int ID = 0;
	[SerializeField] float fScale = 0.3F;

	public bool Register (int id, Action<int> a, Sprite spr, EquipLevel level)
	{
		if (level.GetHashCode () != 0)
			imgRank.sprite = Resources.Load<Sprite> ("Image/Inven/" + level.GetHashCode ());
		else
			imgRank.sprite = Resources.Load<Sprite> ("Image/Inven/" + "6");
		panelStar.SetStar (level.GetHashCode ());
		imgPreview.sprite = spr;
		imgPreview.SetNativeSize ();
		imgPreview.transform.localScale = new Vector3 (fScale, fScale, fScale);
		ID = id;
		_cb = a;
		Block (false);
		UpdateStar (true);
		return true;
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
		_cb.Invoke (ID);
	}

}
