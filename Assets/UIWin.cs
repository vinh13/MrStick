using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIWin : MonoBehaviour,UILogEndGame
{
	[SerializeField]AnimatorPopUpScript anim = null;
	Action cb = null;

	public void Show ()
	{
		anim.show (OnShow);
	}

	void OnShow ()
	{
		MusicManager.Instance.Stop ("");
		if (UIManager.Instance.bWin) {
			SFXManager.Instance.Play ("victory");
		} else {
			SFXManager.Instance.Play ("defeat");
		}
	}

	public void Hide (Action a)
	{
		cb = a;
		anim.hide (OnHide);
	}

	void OnHide ()
	{
		if (cb != null)
			cb.Invoke ();
		Destroy (this.gameObject);
	}
}
