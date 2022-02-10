using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILevelTutorial : MonoBehaviour
{
	[SerializeField]Transform panelBg = null;
	[SerializeField]Transform[] rects;
	[SerializeField]Transform tBlock = null;
	UIGame uiGame = null;
	Transform tParent = null;
	Transform tCurrent = null;

	void Start ()
	{
		uiGame = MenuTutorial.Instance.uiGame;
		MenuTutorial.Instance.RegisterTutorial (TutorialID.Level, ShowLevel);
		ShowLevel (true);
	}

	#region Show_Hide

	void ShowLevel (object ob)
	{
		bool b = (bool)ob;
		ShowRect (b, 0);
		if (!b) {
			tBlock.gameObject.SetActive (false);
		}
	}

	#endregion

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
		MenuTutorial.Instance.ActiveFocus (tCurrent);
	}

}
