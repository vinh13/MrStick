using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Offer : MonoBehaviour
{
	[SerializeField]Text textRate = null, textOld = null, textNew, textGold = null, textGem = null, textKey = null;

	public void SetValue (int[] vs)
	{
		textGold.text = "" + vs [0];
		textGem.text = "" + vs [1];
		textKey.text = "" + vs [2];

	}

	public void SetText (string rate, string old, string tnew)
	{
		textRate.text = "" + rate;
		textOld.text = "" + old;
		textNew.text = "" + tnew;
	}
}
