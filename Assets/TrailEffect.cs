using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailEffect : EffectControl
{
	[SerializeField]TrailRenderer trailRenderer = null;
	void Start ()
	{
		trailRenderer = GetComponent<TrailRenderer> ();
	}
	public override void Active (bool b = false)
	{
		trailRenderer.Clear ();
		if (!b) {
			trailRenderer.enabled = false;
		} else {
			trailRenderer.enabled = true;
		}
	}

	public override void SetColor (Color color)
	{
		trailRenderer.startColor = color;
		Color32 temp = color;
		temp.a = 0;
		trailRenderer.endColor = temp;
	}
}
