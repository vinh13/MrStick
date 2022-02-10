using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBoss : MonoBehaviour
{
	[SerializeField]HPBarUI hpBar = null;
	[SerializeField]Image imgAvatar = null;
	[SerializeField]Image imgBoss = null;
	[SerializeField]Image imgDraken = null;
	[SerializeField]Animator anim = null;
	[SerializeField]Transform tWarning = null;

	void OnValidate ()
	{
		this.transform.GetChild (0).gameObject.SetActive (false);
	}

	public void IntroBoss ()
	{
		imgDraken.enabled = true;
		tWarning.gameObject.SetActive (true);
		if (enabled)
			StartCoroutine (_introBoss ());
	}

	IEnumerator _introBoss ()
	{
		imgBoss.enabled = true;
		yield return new WaitForSeconds (.1F);
		imgBoss.enabled = false;
		yield return new WaitForSeconds (.1F);
		imgBoss.enabled = true;
		yield return new WaitForSeconds (.1F);
		imgBoss.enabled = false;
		yield return new WaitForSeconds (1F);
		imgDraken.enabled = false;
		tWarning.gameObject.SetActive (false);
	}

	public void ShowBoss (bool b)
	{
		if (b) {
			this.transform.GetChild (0).gameObject.SetActive (true);
			anim.enabled = true;
			anim.SetTrigger ("show");
		} else {
			Debug.Log ("hide boss");
			anim.enabled = true;
			anim.SetTrigger ("hide");
		}
	}

	public void OnShow ()
	{
		anim.enabled = false;
		timer = 0;
		StartCoroutine (checkBoss ());
	}

	public void OnHide ()
	{
		anim.enabled = false;
		this.transform.GetChild (0).gameObject.SetActive (false);
	}

	public HPBar GetHpBar {
		get {
			return hpBar;
		}
	}

	float timer = 0;

	IEnumerator checkBoss ()
	{
		bool done = false;
		while (!done) {
			timer += Time.deltaTime;
			if (timer >= 0.5F) {
				timer = 0;
				done = hpBar.Ratio () <= 0;
			}
			yield return null;
		}
		ShowBoss (false);
	}


	void OnDisable ()
	{
		StopAllCoroutines ();
	}
}
