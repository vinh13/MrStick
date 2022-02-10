using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TypeTutorial
{
	None = 0,
	Play = 1,
	Attack = 2,
}

public class TutorialSimple : MonoBehaviour
{
	[SerializeField]Transform[] rects;
	[SerializeField]Transform panelBg = null;
	[SerializeField]Transform tBlock = null;
	[SerializeField]TutorialID ID = TutorialID.None;
	UIGame uiGame = null;
	Transform tParent = null;
	Transform tCurrent = null;
	[SerializeField]int indexUI = 0;
	[SerializeField]bool bTest = false;
	[SerializeField]TypeTutorial typeTut = TypeTutorial.Play;

	void Start ()
	{
		switch (typeTut) {
		case TypeTutorial.Play:
			PlayTutorial.Instance.RegisterTutorial (ID, Show);
			uiGame = PlayTutorial.Instance.uiGame;
			this.gameObject.SetActive (false);
			break;
		case TypeTutorial.Attack:
			TutorialAttack.Instance.RegisterTutorial (ID, Show);
			uiGame = TutorialAttack.Instance.uiGame;
			TutorialAttack.Instance.ShowTutorial (ID);
			break;
		}
	}

	void Show (object ob)
	{
		bool b = (bool)ob;
		if (b)
			this.gameObject.SetActive (true);
		ShowRect (b, 0);
	
	}

	void ShowRect (bool b, int index)
	{
		rects [index].gameObject.SetActive (b);
		panelBg.gameObject.SetActive (b);
		if (b) {
			tCurrent = uiGame.GetRect (indexUI);
			StartCoroutine (_ShowRect ());
		} else {
			if (tCurrent != null) {
				tCurrent.SetParent (tParent);
			}
			this.gameObject.SetActive (false);
		}
	}

	IEnumerator _ShowRect ()
	{
		yield return new WaitForEndOfFrame ();
		tParent = tCurrent.parent;
		tCurrent.SetParent (this.transform);
		tCurrent.SetAsLastSibling ();
		switch (typeTut) {
		case TypeTutorial.Play:
			PlayTutorial.Instance.ActiveFocus (tCurrent);
			break;
		case TypeTutorial.Attack:
			TutorialAttack.Instance.ActiveFocus (tCurrent);
			break;
		}
	}
}
