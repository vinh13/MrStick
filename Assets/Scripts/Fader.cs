using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fader : MonoBehaviour
{
	Action Done = null;
	bool withTimeScale = false;

	public void Fade (Action<byte> cb, float _from, float to, float duration)
	{
		withTimeScale = false;
		StopAllCoroutines ();
		StartCoroutine (_Fade (cb, _from, to, duration));
	}

	public void Fade (Action<byte> cb, float _from, float to, float duration, Action done)
	{
		withTimeScale = false;
		Done = done;
		StopAllCoroutines ();
		StartCoroutine (_Fade (cb, _from, to, duration));
	}

	public void Fade (Action<byte> cb, float _from, float to, float duration, Action done, bool b)
	{
		withTimeScale = b;
		Done = done;
		StopAllCoroutines ();
		StartCoroutine (_Fade (cb, _from, to, duration));
	}


	IEnumerator _Fade (Action<byte> cb, float _from, float to, float duration)
	{	
		int dir = _from > to ? -1 : 1;

		to = to - _from;

		to = Math.Abs (to);


		bool done = false;

		float dur = duration;

		while (!done) {

			if (!withTimeScale) {
				duration -= Time.unscaledDeltaTime;
			} else {
				duration -= Time.deltaTime;
			}

			if (duration <= 0) {
				done = true;
				duration = 0;
			}

			float ratio = (dur - duration) / dur;

			float value = _from + dir * to * ratio;

			cb.Invoke ((byte)value);

			yield return null;
		}
		if (Done != null) {
			Done.Invoke ();
			Done = null;
		}
	}

}
