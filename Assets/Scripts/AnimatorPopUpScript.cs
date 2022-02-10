using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class AnimatorPopUpScript : MonoBehaviour
{
	Animator _anim = null;

	Animator anim {
		get {
			if (_anim == null) {
				_anim = GetComponent<Animator> ();
				_anim.updateMode = AnimatorUpdateMode.UnscaledTime;
			}
			return _anim;
		}
	}

	Action callback = null;

	public void show (Action cb)
	{
		gameObject.SetActive (true);
		anim.enabled = true;
		anim.Rebind ();
		callback = cb;
	}

	public void showII ()
	{
		gameObject.SetActive (true);
		anim.enabled = true;
		anim.Rebind ();
		anim.SetTrigger ("showII");
	}

	public void hide (Action cb)
	{
		anim.enabled = true;
		anim.Rebind ();
		anim.SetTrigger ("hide");
		callback = cb;
	}

	public void hideII ()
	{
		anim.enabled = true;
		anim.Rebind ();
		anim.SetTrigger ("hideII");
	}

	public void disableAnim ()
	{
		if (callback != null) {
			callback.Invoke ();
			callback = null;
		}
		anim.enabled = false;
	}

	public void disablePanel ()
	{
		if (callback != null) {
			callback.Invoke ();
			callback = null;
		}
		gameObject.SetActive (false);
	}

	public void disableAnimManual (bool state)
	{
		anim.enabled = state;
	}

	public void DESTROYGAMEOBJECT ()
	{
		if (callback != null) {
			callback.Invoke ();
			callback = null;
		}
		DestroyObject (gameObject);
	}

	void Update ()
	{
		if (Time.deltaTime < 1F) {
			anim.Update (Time.unscaledDeltaTime);
		} else {
			anim.Update (Time.deltaTime);
		}
	}
}
