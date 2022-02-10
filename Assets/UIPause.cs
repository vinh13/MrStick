using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIPause : MonoBehaviour,UILogEndGame
{
	[SerializeField]Button[] btns;
	[SerializeField]AnimatorPopUpScript animPop = null;
	Action callbackResume = null;

	void Start ()
	{
		btns [0].onClick.AddListener (delegate() {
			ClickHome ();	
		});
		btns [1].onClick.AddListener (delegate() {
			ClickRetry ();	
		});
		btns [2].onClick.AddListener (delegate() {
			ClickResume ();	
		});
	}

	public void Show ()
	{
		animPop.show (null);
	}

	public void Hide (System.Action a)
	{
		callbackResume = a;
	}

	void ClickHome ()
	{
		Logic.bShowAds = true;
		Manager.Instance.LoadScene (SceneName.Home, true);
		Destroy (gameObject);
		//AllInOne.Instance.ShowAdmobFULL ();
	}

	void ClickRetry ()
	{
		Logic.bShowAds = true;
		Manager.Instance.LoadScene (SceneName.Main, true);
		Destroy (gameObject);
		//AllInOne.Instance.ShowAdmobFULL ();
	}

	void ClickResume ()
	{
		callbackResume.Invoke ();
		Destroy (gameObject);

	}
}
