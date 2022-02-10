using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToast : MonoBehaviour
{
	[SerializeField]ToastScript[] Toasts;
	[SerializeField]float fSpeed = 0;
	[SerializeField]float yPos = 0;

	void Awake ()
	{
		for (int i = 0; i < Toasts.Length; i++) {
			Toasts [i].Setup ();
		}
	}

	public void AddCoin (string text, bool bPlus)
	{
		ShowNow (0, text, bPlus);
	}

	public void AddGem (string text, bool bPlus)
	{
		ShowNow (3, text, bPlus);
	}

	public void AddKey (string text, bool bPlus)
	{
		ShowNow (4, text, bPlus);
	}

	public void AddShield (string text, bool bPlus)
	{
		ShowNow (1, text, bPlus);
	}

	public void AddHealth (string text, bool bPlus)
	{
		ShowNow (2, text, bPlus);
	}


	private void ShowNow (int index, string text, bool bPlus)
	{
		float y = bPlus ? yPos : -1 * yPos;
		string p = bPlus ? "+ " : "- ";
		Toasts [index].Show (p + text, fSpeed, y);
	}
}
