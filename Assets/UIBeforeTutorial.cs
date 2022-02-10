using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBeforeTutorial : MonoBehaviour
{
	[SerializeField]Transform[] rects;
	[SerializeField]Transform panelBg = null;
	[SerializeField]Transform tBlock = null;
	UIGame uiGame = null;
	Transform tParent = null;
	Transform tCurrent = null;

	void Start ()
	{
		uiGame = BeforeTutorial.Instance.uiGame;
		BeforeTutorial.Instance.RegisterTutorial (TutorialID.BeforePlay, Show);
		Show (true);
	}

	#region Show_Hide

	void Show (object ob)
	{
		bool b = (bool)ob;
		ShowRect (b, 0);
		if (!b) {
			Destroy (this.gameObject);
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
		BeforeTutorial.Instance.ActiveFocus (tCurrent);
	}

}
