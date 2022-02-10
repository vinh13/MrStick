using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTutorial : MonoBehaviour
{
	[SerializeField]TutorialID tutorialID = TutorialID.None;
	bool bSend = false;
	void OnTriggerEnter2D (Collider2D coll)
	{
		if (bSend)
			return;
		int collLayer = coll.gameObject.layer;
		if (collLayer == 8) {
			TutorialManager.Instance.ShowTutorial (tutorialID);
			bSend = true;
			gameObject.SetActive (false);
		}
	}
}
