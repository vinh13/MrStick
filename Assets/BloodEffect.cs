using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEffect : EffectControl
{
	[SerializeField]ParticleSystem parS = null;

	public override void Active (bool b = false)
	{
		this.gameObject.SetActive (b);
	}

	public override void SetColor (Color color)
	{
		var main = parS.main;
		main.startColor = color;
	}
}
