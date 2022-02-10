using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIReady : MonoBehaviour
{
	[SerializeField] AnimatorPopUpScript anim;
	[SerializeField] Text textCount = null, textEffect = null;
	int count = 3;
	void Start ()
	{
		TaskUtil.Schedule (this, _Start, 0.1F);
	}

	void _Start ()
	{
		SFXManager.Instance.Play ("321go");
		Play ();
	}

	public void Play ()
	{
		if (!UIManager.Instance.bIgnoreReady) {
			anim.hide (CallBack);
			textCount.text = "" + count;
			textEffect.text = "" + count;
			count--;
		} else {
			Open ();
			Destroy (gameObject);
		}
	}

	void CallBack ()
	{
		if (count > 0) {
			TaskUtil.Schedule (this, Delay, 0.01F);
		} else {
			textCount.enabled = false;
			textEffect.enabled = false;
			TaskUtil.Schedule (this, Fight, 0.1F);
		}
	}

	void Delay ()
	{
		Play ();
	}

	void Fight ()
	{
		anim.showII ();
	}

	void Open ()
	{
		Logic.bStarted = true;
		Logic.bReady = true;
		MusicManager.Instance.PlayMusic ();
		UIManager.Instance.StartTime ();
		MapManager.Intance._Start ();
		TungDz.EventDispatcher.Instance.PostEvent (EventID.StartRace);
	}

	void OnDisable ()
	{
		Open ();
		Destroy (gameObject);
	}
}
