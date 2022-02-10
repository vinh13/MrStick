using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
	[SerializeField]float duration = 0.5F;
	[SerializeField]Transform target = null;
	bool bOn = false;

	void OnEnable ()
	{
		Play ();
	}

	void OnDisable ()
	{
		StopAllCoroutines ();
	}

	void Play ()
	{
		TaskUtil.Schedule (this, OnPlay, duration);
	}

	void OnPlay ()
	{
		bOn = !bOn;
		target.gameObject.SetActive (bOn);
		Play ();
	}
}
