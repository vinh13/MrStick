using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialHealth : MonoBehaviour
{
	[SerializeField]UIButton btn = null;
	[SerializeField]bool bH = false;

	void Start ()
	{
		if (bH) {
			btn.Register (CLickH);
		} else {
			btn.Register (CLickS);
		}
	}

	public void CLickH ()
	{
		PlayerControl.Instance.SkipHealth ();
		
	}

	public void CLickS ()
	{

		PlayerControl.Instance.SkipShield ();
	}
}
