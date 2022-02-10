using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UIHome : MonoBehaviour,IUI
{
	[SerializeField]AnimatorPopUpScript anim = null;
	[SerializeField]UIButton[] btns;

	void Start ()
	{
		btns [0].Register (ClickCampain);
		btns [1].Register (ClickPvp);
		btns [2].Register (ClickCharacter);
		btns [3].Register (ClickBike);
	}

	void ClickCampain ()
	{
		BeforeplayManager.Instance.Show ();
	}

	void ClickPvp ()
	{
	}

	void ClickCharacter ()
	{
		
	}

	void ClickBike ()
	{
		
	}

	public void Show ()
	{
		anim.show (OnShow);
	}

	void OnShow ()
	{
	
	}

	public void Hide ()
	{
		anim.hide (OnHide);
	}

	void OnHide ()
	{
		
	}
}
