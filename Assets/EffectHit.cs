using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectHit : Effect
{
	[SerializeField]ParticleSystem parS = null;
	[SerializeField]Collider2DManager colls = null;
	[SerializeField]ObjectHit objectHit = null;
	[SerializeField]float ratioDame = 10;
	bool bPlay = false;
	int countDame = 0;

	public override void Init ()
	{
		colls.InitCollider2D ();
		objectHit.Register (OnHit, TypeHit.Weapon);
		colls.Active (false);
	}

	public override void Active (bool b = false)
	{
		gameObject.SetActive (b);
	}

	public override void Attack (Vector2 pos, bool isP = false, float damage = 0)
	{
		if (bPlay)
			return;
		bPlay = true;
		objectHit.damage = damage * ratioDame;
		if (isP) {
			colls.ChangeLayer (LayerConfig._layerPWP);
		} else {
			colls.ChangeLayer (LayerConfig._layerEWP);
		}
		transform.position = pos;
		parS.Play ();
		colls.Active (true);
		StartCoroutine (_Attack ());
	}

	void OnHit (object ob = null)
	{
		countDame++;
		if (countDame >= 5) {
			colls.Active (false);
			return;
		}
	}

	void DisableEffect ()
	{
		bPlay = false;
		gameObject.SetActive (false);
		colls.Active (false);
	}

	IEnumerator _Attack ()
	{
		yield return new WaitForSeconds (0.2F);
		DisableEffect ();
	}
}
