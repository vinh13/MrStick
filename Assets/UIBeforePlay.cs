using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBeforePlay : MonoBehaviour
{
	[SerializeField]AnimatorPopUpScript animPop = null;

	public void ClickRace ()
	{
		this.gameObject.SetActive (false);
		BeforeplayManager.Instance.ClickPlay ();
	}

	public void ClickBack ()
	{
		animPop.hide (null);
	}

	public void Show ()
	{
		animPop.show (OnShow);
	}
	void OnShow ()
	{
		if (TutorialData.bBeforePlay) {
			BeforeplayManager.Instance.CreateTutorial ();
		}
	}
}
