using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpgradeATK : MonoBehaviour
{
	[SerializeField]Transform[] rects;
	[SerializeField]Transform panelBg = null;
	[SerializeField]Transform tBlock = null;
	UIGame uiGame = null;
	Transform tParent = null;
	Transform tCurrent = null;

	void Start ()
	{
		MenuTutorial.Instance.RegisterTutorial (TutorialID.UpATK, Show);
		MenuTutorial.Instance.RegisterTutorial (TutorialID.ClickUpgrade, ShowClickUpgrade);
		uiGame = transform.root.GetComponent<UIGame> ();
		ShowClickUpgrade (true);
	}

	void ShowClickUpgrade (object ob)
	{
		bool b = (bool)ob;
		ShowRect (b, 0);
	}

	void Show (object ob)
	{
		bool b = (bool)ob;
		ShowRect (b, 1);
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
			if (index == 1) {
				gameObject.SetActive (false);
			}
		}
	}

	IEnumerator _ShowRect ()
	{
		yield return new WaitForEndOfFrame ();
		tParent = tCurrent.parent;
		tCurrent.SetParent (this.transform);
		tCurrent.SetAsLastSibling ();
		MenuTutorial.Instance.ActiveFocus (tCurrent);
	}
}
