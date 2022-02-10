using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using System;
using UnityEngine.UI;

public class ChestAnimation : MonoBehaviour
{
	[SerializeField]SkeletonGraphic skeletonGraphic = null;
	[SerializeField]Mask mask = null;

	public void PlayAnim (Action<object> a, Action<object> b)
	{
		mask.enabled = true;
		gameObject.SetActive (true);
		skeletonGraphic.AnimationState.ClearTrack (0);
		skeletonGraphic.AnimationState.SetAnimation (0, "animation", false);
		StopAllCoroutines ();
		StartCoroutine (Play (a, b));
	}

	IEnumerator Play (Action<object> cb, Action<object> b)
	{
		yield return new WaitForEndOfFrame ();
		mask.enabled = false;
		b.Invoke (false);
		yield return new WaitForSecondsRealtime (1.8F);
		cb.Invoke (true);
	}
}
