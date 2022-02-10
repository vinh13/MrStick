using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[SelectionBase]
public class ButtonLevel : ILevel
{
	[SerializeField]GameObject[] rects = new GameObject[2];
	[SerializeField]Text[] texts = new Text[2];
	[SerializeField]UIButton btn = null;
	[SerializeField]Transform[] stars = new Transform[3];
	[SerializeField]Transform rectFocus = null;
	[SerializeField]Text textStar = null;
	string key = "";
	int ID = 0;
	bool bUnlocked = false;
	int star = 0;
	Action<int> clickLevel = null;

	public override bool Unlocked ()
	{
		return bUnlocked;
	}

	public override int GetStar ()
	{
		return star;
	}

	public override void ShowTarget ()
	{
		rectFocus.gameObject.SetActive (true);
	}

	public override bool Setup (int id, string idLevel, Action<int> a)
	{
		key = idLevel;
		ID = id;
		bUnlocked = LevelData.GetUnlock (key);
		texts [0].text = "" + (ID + 1);
		texts [1].text = texts [0].text;
		texts [2].text = texts [0].text;
		if (bUnlocked) {
			btn.Register (Click);
			clickLevel = a;
			star = LevelData.GetStar (key);
			SetStar ();
		}
		CheckUnlock ();
		textStar.text = "" + star + "/3";
		return bUnlocked;
	}

	public override void Select ()
	{
		rects [2].gameObject.SetActive (true);
	}

	public void SetStar ()
	{
		for (int i = 0; i < star; i++) {
			stars [i].gameObject.SetActive (true);
		}
	}

	void CheckUnlock ()
	{
		rects [0].SetActive (bUnlocked);
		rects [1].SetActive (!bUnlocked);
	}

	public void Click ()
	{
		clickLevel.Invoke (ID);
	}
}
