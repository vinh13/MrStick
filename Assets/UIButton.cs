using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIButton : MonoBehaviour
{
	[SerializeField]Button btn = null;
	Action cb = null;
	Transform block = null;

	public void Register (Action a)
	{
		cb = a;
		block = this.transform.Find ("block");
		if (block != null) {
			block.gameObject.SetActive (false);
		}
	}

	public void Block (bool b)
	{
		btn.interactable = !b;
		if (block != null) {
			block.gameObject.SetActive (b);
		}
	}

	void OnValidate ()
	{
		if (btn == null)
			btn = gameObject.AddComponent<Button> ();
	}

	void Start ()
	{
		btn.onClick.AddListener (delegate() {
			Click ();
		});
	}

	public void Click ()
	{
		if (cb != null) {
			SFXManager.Instance.Play ("click");
			cb.Invoke ();
		}
	}

}
