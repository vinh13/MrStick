using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILog_Kill : MonoBehaviour
{
	[SerializeField]Animator anim = null;
	[SerializeField]Text textTitle = null;

	public void Show (string text)
	{
		StopAllCoroutines ();
		textTitle.text = text;
		gameObject.SetActive (true);
		anim.enabled = true;
		anim.SetTrigger ("show");
	}

	public void onShow ()
	{
		anim.enabled = false;
		StartCoroutine (_Show ());
	}

	public void onHide ()
	{
		anim.enabled = false;
		gameObject.SetActive (false);
	}

	IEnumerator _Show ()
	{
		yield return new WaitForSeconds (2F);
		anim.enabled = true;
		anim.SetTrigger ("hide");
	}

	public void ShowBoss ()
	{
		anim.enabled = false;
		anim.Rebind ();
		StopAllCoroutines ();
		this.gameObject.SetActive (false);
	}
}
