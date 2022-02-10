using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuTutorial : MonoBehaviour
{
	[SerializeField]Transform[] rects;
	[SerializeField]Transform panelBg = null;
	[SerializeField]Transform tBlock = null;
	UIGame uiGame = null;
	Transform tParent = null;
	Transform tCurrent = null;

	void Start ()
	{
		uiGame = MenuTutorial.Instance.uiGame;
		MenuTutorial.Instance.RegisterTutorial (TutorialID.Health, ShowHealth);
		MenuTutorial.Instance.RegisterTutorial (TutorialID.PlayGame, ShowPlay);
		MenuTutorial.Instance.RegisterTutorial (TutorialID.ClickUpgrade, ShowClickUpgrade);
		ShowClickUpgrade (true);
	}

	#region Show_Hide

	void ShowClickUpgrade (object ob)
	{
		bool b = (bool)ob;
		ShowRect (b, 0);
	}

	void ShowHealth (object ob)
	{
		bool b = (bool)ob;
		ShowRect (b, 1);
	}

	void ShowPlay (object ob)
	{
		bool b = (bool)ob;
		ShowRect (b, 2);
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
