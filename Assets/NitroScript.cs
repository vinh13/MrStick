using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class NitroScript : MonoBehaviour
{
	[SerializeField]SkeletonAnimation skeletonAnimation = null;
	[SerializeField]Material _ma = null;
	[SerializeField]ObjectHit _hit = null;
	[SerializeField]Transform tWp = null;
	Color32 color32 = Color.white;
	byte aMax = 200;
	byte temp = 0;
	bool bHit = false;

	void Start ()
	{
		color32 = _ma.color;
		_hit.Register (OnHit, TypeHit.Weapon);
	}

	public void Register (float dame)
	{
		_hit.damage = dame;
	}

	void OnHit (object ob)
	{
		TaskUtil.ScheduleWithTimeScale (this, _OnHit, 0.1F);
	}

	void _OnHit ()
	{
		_hit.hited = false;
	}

	public void PlayAnim (bool b)
	{
		if (b) {
			_hit.hited = false;
			temp = 0;
			color32.a = 0;
			_ma.color = color32;
			StopAllCoroutines ();
			StartCoroutine (_play ());
			skeletonAnimation.gameObject.SetActive (true);
			skeletonAnimation.state.ClearTrack (0);
			skeletonAnimation.state.SetAnimation (0, "animation", true);
			tWp.gameObject.SetActive (true);
			SFXManager.Instance.PlayNitro (true);
		} else {
			StopAllCoroutines ();
			StartCoroutine (_Stop ());
			tWp.gameObject.SetActive (false);
			SFXManager.Instance.PlayNitro (false);
		}
	}

	IEnumerator _play ()
	{
		bool done = false;
		while (!done) {
			if (temp < aMax) {
				temp += 20;
			} else {
				done = true;
			}
			temp = (byte)Mathf.Clamp (temp, 0, aMax);
			color32.a = temp;
			_ma.color = color32;
			yield return null;
		}
	}

	IEnumerator _Stop ()
	{
		bool done = false;
		while (!done) {
			if (temp > 0) {
				temp -= 20;
			} else {
				done = true;
			}
			temp = (byte)Mathf.Clamp (temp, 0, aMax);
			color32.a = temp;
			_ma.color = color32;
			yield return null;
		}
		skeletonAnimation.gameObject.SetActive (false);
	}
}
