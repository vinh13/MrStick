using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILogMap : MonoBehaviour
{
	[SerializeField]Text textLap = null, textLapShadow = null;
	[SerializeField]AnimatorPopUpScript anim = null;

	public void Show (int indexLap)
	{
		this.gameObject.SetActive (true);
		anim.showII ();
		textLap.text = "Lap " + "<color=orange>" + indexLap + "</color>";
		textLapShadow.text = "Lap" + indexLap;
		OnShow ();
	}

	public void Show (string text)
	{
		this.gameObject.SetActive (true);
		anim.showII ();
		textLap.text = "<color=orange>" + text + "</color>";
		textLapShadow.text = "<color=orange>" + text + "</color>";
		OnShow ();
	}

	void OnShow ()
	{
		StopAllCoroutines ();
		TaskUtil.ScheduleWithTimeScale (this, Hide, 2F);
	}

	void Hide ()
	{
		anim.hide (OnHide);
	}

	void OnHide ()
	{
		this.gameObject.SetActive (false);
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.S)) {
			Show ("cccc");
		}
	}
}
