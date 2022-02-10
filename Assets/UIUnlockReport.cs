using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIUnlockReport : MonoBehaviour
{
	[SerializeField]UIButton btnBack = null;
	[SerializeField]Text textStar = null;
	Action cb = null;
	public void ShowUnlock (int need, Action a)
	{
		textStar.text = "Need more: <color=yellow> +" + need + "</color>";
		cb = a;
		Show (true);
	}

	public void Show (bool b)
	{
		this.gameObject.SetActive (b);
	}

	void Start ()
	{
		btnBack.Register (ClickBack);
	}

	void ClickBack ()
	{
		cb.Invoke ();
		Show (false);	
	}
}
