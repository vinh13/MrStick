using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TriggerCheckEnemy : MonoBehaviour
{
	bool bSend = false;
	void OnTriggerEnter2D (Collider2D coll)
	{
		if (bSend)
			return;
		int collLayer = coll.gameObject.layer;
		if (collLayer == 13) {
			TutorialManager.Instance.ShowTutorial (TutorialID.Jump);
			TutorialManager.Instance.JumpMove ();
			bSend = true;
			gameObject.SetActive (false);
		}
	}
}
