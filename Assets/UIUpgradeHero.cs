using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpgradeHero : MonoBehaviour
{
	[SerializeField]Mover mover = null;
	[SerializeField]float fSpeed = 0;
	[SerializeField]Vector3 localTarget = Vector3.zero;
	Vector3 originalPos = Vector3.zero;
	[SerializeField]Transform posTarget = null;
	[SerializeField]UIButton btnQuit = null;
	bool bTutorialHP = false;
	bool bTutorialATK = false;

	void Start ()
	{
		originalPos = this.posTarget.localPosition;
		btnQuit.Register (ClickQuit);
		bTutorialHP = TutorialData.bMenuTutorial;
		if (TutorialData.bToATK) {
			bTutorialATK = TutorialData.bTutorialUpgradeATK;
		} else {
			bTutorialATK = false;
		}
	}

	void ClickQuit ()
	{
		HomeManager.Instance.HideChar ();
	}

	public void Show (bool b)
	{
		if (b) {
			this.transform.localPosition = localTarget;
			this.gameObject.SetActive (true);
			mover._Move (originalPos, fSpeed, OnShow);
		} else {
			this.transform.localPosition = originalPos;
			mover._Move (localTarget, fSpeed, OnHide);
		}
		this.gameObject.SetActive (b);
	}

	void OnShow ()
	{
		if (bTutorialHP) {
			MenuTutorial.Instance.ShowTutorial (TutorialID.Health);
			bTutorialHP = false;
		}
		if (bTutorialATK) {
			MenuTutorial.Instance.ShowTutorial (TutorialID.UpATK);
			bTutorialATK = false;
		}
	}

	void OnHide ()
	{
		this.gameObject.SetActive (false);	
	}
}
