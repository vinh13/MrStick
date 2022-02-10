using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITutorialToATK : MonoBehaviour
{
	[SerializeField]Transform[] rects;
	[SerializeField]Transform panelBg = null;
	[SerializeField]Transform tBlock = null;
	UIGame uiGame = null;
	Transform tParent = null;
	Transform tCurrent = null;

	void Start ()
	{
		InfoTutorial.Instance.RegisterTutorial (TutorialID.ToATK, Show);
		uiGame = InfoTutorial.Instance.uiGame;
		Show (true);
	}

	void Show (object ob)
	{
		bool b = (bool)ob;
		ShowRect (b, 0);
		if (b) {
			TutorialData.bToATK = true;
		}
	}
	void ShowRect (bool b, int index)
	{
		rects [index].gameObject.SetActive (b);
		panelBg.gameObject.SetActive (b);
		if (b) {
			tCurrent = uiGame.GetRect (index);
			StartCoroutine (_ShowRect ());
		} else {
			tCurrent.SetParent (tParent);
		}
	}

	IEnumerator _ShowRect ()
	{
		yield return new WaitForEndOfFrame ();
		tParent = tCurrent.parent;
		tCurrent.SetParent (this.transform);
		tCurrent.SetAsLastSibling ();
		InfoTutorial.Instance.ActiveFocus (tCurrent);
	}
}
