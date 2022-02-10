using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToastScript : MonoBehaviour
{
	[SerializeField]Text textValue = null;
	[SerializeField]Image imgPreview = null;
	Mover mover = null;
	Fader fader = null;
	Vector3 originalocalPos = Vector3.zero;
	Color32 colorOriginal = Color.white;

	public void Setup ()
	{
		originalocalPos = transform.localPosition;
		mover = gameObject.AddComponent<Mover> ();
		fader = gameObject.AddComponent<Fader> ();
	}

	public void Show (string text, float speed, float yPos)
	{

		fader.StopAllCoroutines ();
		mover.StopAllCoroutines ();

		colorOriginal.a = 255;
		imgPreview.color = colorOriginal;
		textValue.color = colorOriginal;
		transform.localPosition = originalocalPos;

		gameObject.SetActive (true);
		textValue.text = "" + text;
		Vector3 localTarget = originalocalPos;
		localTarget.y += yPos;
		mover._Move (localTarget, speed, OnShow);
	}

	void OnShow ()
	{
		fader.Fade (Fading, 255, 9, 0.5F, EndFade);
	}

	void Fading (byte a)
	{
		colorOriginal.a = a;
		imgPreview.color = colorOriginal;
		textValue.color = colorOriginal;
	}

	void EndFade ()
	{
		StopAllCoroutines ();
		gameObject.SetActive (false);
		colorOriginal.a = 255;
		imgPreview.color = colorOriginal;
		textValue.color = colorOriginal;
		transform.localPosition = originalocalPos;
	}
}
