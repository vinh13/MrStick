using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class UIInventoryInfo : MonoBehaviour,Iui1
{
	[SerializeField]AnimatorPopUpScript animPop = null;
	[SerializeField]UIButton btnQuit = null;

	void Start ()
	{
		btnQuit.Register (Hide);
	}

	public void Show ()
	{
		animPop.show (OnShow);
	}

	void OnShow ()
	{
		
	}

	public void Hide ()
	{
		animPop.hide (OnHide);
	}

	void OnHide ()
	{
		gameObject.SetActive (false);
	}

	public void Register (System.Action<object> a)
	{
	}
}
