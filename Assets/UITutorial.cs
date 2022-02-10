using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITutorial : MonoBehaviour
{
	[SerializeField]Transform panelBg = null;
	[SerializeField]Transform[] rects;
	[SerializeField]Transform tBlock = null;
	UIGame uiGame = null;
	Transform tParent = null;
	Transform tCurrent = null;

	void Start ()
	{
		TutorialManager.Instance.RegisterTutorial (TutorialID.Slide, ShowSlide);
		TutorialManager.Instance.RegisterTutorial (TutorialID.Jump, ShowJump);
		TutorialManager.Instance.RegisterTutorial (TutorialID.Shot, ShowShot);
		TutorialManager.Instance.RegisterTutorial (TutorialID.UpSpeed, ShowUpSpeed);
		TutorialManager.Instance.RegisterTutorial (TutorialID.JumpMove, ShowJumpMove);
		uiGame = TutorialManager.Instance.uiGame;
	}

	#region Show_Hide

	void ShowSlide (object ob)
	{
		bool b = (bool)ob;
		ShowRect (b, 0);
	}

	void ShowJump (object ob)
	{
		bool b = (bool)ob;
		ShowRect (b, 1);
	}

	void ShowShot (object ob)
	{
		bool b = (bool)ob;
		ShowRect (b, 2);
		if (!b) {
			rects [5].gameObject.SetActive (true);
			tBlock.gameObject.SetActive (false);
			rects [6].gameObject.SetActive (false);
			TutorialData.bTutorialStart = false;
			TaskUtil.Schedule (this, ShowNote, 2F);
		}
	}

	void ShowNote ()
	{
		rects [5].gameObject.SetActive (false);
		Destroy (gameObject);
	}

	void ShowUpSpeed (object ob)
	{
		bool b = (bool)ob;
		ShowRect (b, 3);
		if (b) {
			EnemyLogic.pause = true;
		} else {
			EnemyLogic.pause = false;
		}
	}

	void ShowJumpMove (object ob)
	{
		bool b = (bool)ob;
		ShowRect (b, 4);
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
		TutorialManager.Instance.ActiveFocus (tCurrent);
	}

	#endregion
}

