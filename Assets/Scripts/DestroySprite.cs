using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DestroySprite : MonoBehaviour
{
	SpriteRenderer[] spr = null;
	Fader fader = null;
	Color32 _color;
	Action cb = null;

	void Start ()
	{
		fader = gameObject.AddComponent<Fader> ();
	}

	public void Setup (SpriteRenderer[] s)
	{
		spr = s;
	}

	public void DestroyNow (Action cb)
	{
		TaskUtil.ScheduleWithTimeScale (this, StartFade, 4F);
	}

	void StartFade ()
	{
		_color = spr [0].color;
		fader.Fade (Fade, 255, 0, 1F, Done, true);
	}

	void Fade (byte a)
	{
		_color.a = a;
		for (int i = 0; i < spr.Length; i++) {
			spr [i].color = _color;
		}
	}

	void Done ()
	{
		Destroy (gameObject);
	}
}
