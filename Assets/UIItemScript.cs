using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIItemScript : MonoBehaviour
{
	[SerializeField]Image imgFill = null, imgAvatar = null;
	[SerializeField]ObjectType uiType = ObjectType.None;
	[SerializeField]UIButton btnReject = null;
	float duration = 0;
	float timer = 0;
	Action<ObjectType> cb = null;
	float step = 1F;
	WeaponDir wpdir = WeaponDir.None;
	Action<int> RejectObject = null;
	int IndexOb = 0;

	public void Setup (Action<int> a, int id = 0)
	{
		IndexOb = id;
		RejectObject = a;
		btnReject.Register (ClickReject);
	}

	public void ClickReject ()
	{
		RejectObject.Invoke (IndexOb);
	}

	void EnableReject (bool b)
	{
		btnReject.gameObject.SetActive (b);
	}

	public void Fill (ObjectType type, float t, Action<ObjectType> a, WeaponDir dir, Sprite spr)
	{
		StopAllCoroutines ();
		EnableReject (true);
		wpdir = dir;
		imgAvatar.enabled = true;
		imgAvatar.sprite = spr;
		uiType = type;
		imgFill.fillAmount = 1;
		duration = t;
		timer = t;
		cb = a;
		StartCoroutine (Task ());
	}

	public void Eject ()
	{
		StopAllCoroutines ();
		CallEject ();
	}

	void CallEject ()
	{
		EnableReject (false);
		UIObjectManager.Instance.DisableAttack (wpdir);
		imgFill.fillAmount = 0;
		imgAvatar.enabled = false;
		if (cb != null)
			cb.Invoke (uiType);
	}

	IEnumerator Task ()
	{
		yield return new WaitForSeconds (step);
		if (Logic.bStarted) {
			duration -= step;
		}
		if (duration > 0) {
			float ratio = duration / timer;
			imgFill.fillAmount = ratio;
			StartCoroutine (Task ());
		} else {
			EnableReject (false);
			CallEject ();
		}
	}

	void OnDisable ()
	{
		StopAllCoroutines ();
	}
}

