using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffecter : MonoBehaviour
{
	[SerializeField]SpriteRenderer spr = null;
	[SerializeField]Fader fader = null;
	bool bOut = false;
	[SerializeField]byte[] a = new byte[2];
	[SerializeField]float duration = 0;
	Color32 color;

	void OnEnable ()
	{
		bOut = false;
		color = spr.color;
		Fade ();
	}

	void OnDisable ()
	{
		StopFade ();
	}

	void Fade ()
	{
		bOut = !bOut;
		if (bOut) {
			fader.Fade (Change, a [0], a [1], duration, FadeDone);
		} else
			fader.Fade (Change, a [1], a [0], duration, FadeDone);
	}

	void FadeDone ()
	{
		TaskUtil.Schedule (this, Fade, 0.5F);
	}

	void Change (byte b)
	{
		color.a = b;
		spr.color = color;

	}

	void StopFade ()
	{
		StopAllCoroutines ();
	}
}
