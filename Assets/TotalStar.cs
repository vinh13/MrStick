using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalStar : MonoBehaviour
{
	[SerializeField]Image imgFill = null;
	[SerializeField]Text textCurrent = null, textTotal = null;
	[SerializeField]Image imgEffect = null;

	public void UpdateStar (float ratio, string cur, string total, bool bEffect)
	{
		imgFill.fillAmount = ratio;
		imgEffect.fillAmount = ratio;
		textCurrent.text = cur;
		textTotal.text = "/" + total;
		if (bEffect) {
			StopAllCoroutines ();
			StartCoroutine (effect ());
		}
	}
	IEnumerator effect ()
	{
		imgEffect.enabled = true;
		yield return new WaitForSecondsRealtime (0.1F);
		imgEffect.enabled = false;
	}
}
