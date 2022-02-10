using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelLevel : MonoBehaviour
{
	[SerializeField]Transform[] rectsLevel;
	[SerializeField]GameObject nextLevel = null;

	public void SetCurLevel (int level)
	{
		for (int i = 0; i < rectsLevel.Length; i++) {
			if (i < level) {
				rectsLevel [i].gameObject.SetActive (true);
			} else {
				rectsLevel [i].gameObject.SetActive (false);
			}
		}
	}

	public void SetNextLevel (int level, bool b)
	{
		nextLevel.transform.localPosition = rectsLevel [level - 1].localPosition;
		nextLevel.gameObject.SetActive (b);
	}
}
