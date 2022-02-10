using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIRegisterName : MonoBehaviour,Iui1
{
	[SerializeField]InputField textInput = null;
	[SerializeField]UIButton btnEnter = null;
	Action<object> register = null;

	public void Register (Action<object> a)
	{
		register = a;
	}

	public void Show ()
	{
		gameObject.SetActive (true);
	}

	public void Hide ()
	{
		gameObject.SetActive (false);
	}

	void Start ()
	{
		btnEnter.Register (ClickEnter);
	}

	void Update ()
	{
		CheckButton ();
	}

	void ClickEnter ()
	{
		string text = textInput.text;
		register.Invoke (text);
		Hide ();
	}

	void CheckButton ()
	{
		string text = textInput.text;
		bool b = false;
		char[] texts = text.ToCharArray ();
		if (texts.Length < 4) {
			b = false;
		} else {
			b = true;
		}
		btnEnter.Block (!b);
	}
}
