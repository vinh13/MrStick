using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectTimer : MonoBehaviour
{
	[SerializeField]ObjectType uiType = ObjectType.None;
	[SerializeField]Sprite[] sprs;
	float duration = 0;
	Action<ObjectType> cb = null;
	Action<WeaponDir> disableAttack = null;
	WeaponDir wpdir = WeaponDir.None;

	public void RegisterDisableAttack (Action<WeaponDir> a)
	{
		disableAttack = a;
	}

	public void Fill (ObjectType type, float t, Action<ObjectType> a, WeaponDir dir)
	{
		StopAllCoroutines ();
		if (disableAttack != null)
			disableAttack.Invoke (wpdir);
		wpdir = dir;
		uiType = type;
		duration = t;
		cb = a;
		StartCoroutine (Task ());
	}

	public void Eject ()
	{
		if (cb != null)
			cb.Invoke (uiType);
		disableAttack.Invoke (wpdir);
		StopAllCoroutines ();
	}

	IEnumerator Task ()
	{
		yield return new WaitForSeconds (duration);
		if (cb != null)
			cb.Invoke (uiType);
		disableAttack.Invoke (wpdir);
	}

	void OnDisable ()
	{
		StopAllCoroutines ();
	}
}
