using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSprite : ObFollow
{
	[SerializeField]SpriteRenderer spr = null;

	public override void SetColor (Color color)
	{
		spr.color = color;
	}

	public override void Active (bool b = false)
	{
		this.gameObject.SetActive (b);
		if (!b)
			StopAllCoroutines ();
	}

	public override void Break ()
	{
		this.transform.SetParent (null);
		this.gameObject.SetActive (false);
		StopAllCoroutines ();
	}

	public override void SetUp (Transform parent, Transform sync)
	{
		this.transform.SetParent (parent);
		this.transform.position = sync.position;
		this.transform.rotation = sync.rotation;
	}

	public override void Disable (float timer = 0)
	{
		StartCoroutine (_waitDisable (timer));
	}

	IEnumerator _waitDisable (float time)
	{
		yield return new WaitForSeconds (time);
		Active (false);
	}
}
