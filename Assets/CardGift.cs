using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CardGift : MonoBehaviour
{
	[SerializeField]Animator anim = null;
	[SerializeField]UIButton[] btnOpen = null;
	[SerializeField]UIButton btnVideo = null;
	[SerializeField]Transform rectGift = null;
	[SerializeField]Transform rectBoder = null;
	Action<int> cbOpen = null, cbVideo = null;
	Action<int,bool> cbDone = null;
	[SerializeField]int idCard = 0;
	[SerializeField]Text textCd = null;
	bool bOpen = false;
	bool bVideo = false;
	bool bPreview = false;

	public void UpdateCd (string text)
	{
		textCd.text = "" + text;
	}

	public void UpdateColor (Color32 c)
	{
		textCd.color = c;
	}

	public Transform getRect {
		get{ return rectGift; }
	}

	public void Register (int id, Action<int> a, Action<int> b, Action<int,bool> c)
	{
		cbOpen = a;
		idCard = id;
		cbVideo = b;
		cbDone = c;
		btnOpen [0].Register (ClickOpen);
		btnOpen [1].Register (ClickOpen);
		btnVideo.Register (ClickVideo);
	}

	public void Show (bool b)
	{
		if (bOpen)
			return;
		bVideo = b;
		bOpen = true;
		textCd.enabled = false;
		anim.Rebind ();
		anim.enabled = true;
	}

	public void OnShow ()
	{
		anim.enabled = false;
		if (!bPreview)
			cbDone.Invoke (idCard, bVideo);
	}

	void ClickOpen ()
	{
		rectBoder.gameObject.SetActive (true);
		Show (false);
		cbOpen.Invoke (idCard);
		btnOpen [0].Block (true);
		btnOpen [1].Block (true);
	}

	void ClickVideo ()
	{
		cbVideo.Invoke (idCard);
		rectBoder.gameObject.SetActive (true);
		btnVideo.Block (true);
		AllInOne.Instance.ShowVideoReward (callbackVideo,"OpenCardGif",LevelData.IDLevel);
	}

	void callbackVideo (bool b)
	{
		if (b) {
			Show (true);
			ShowVideo (false);
		} else {
			StartCoroutine (CheckVideo ());
		}
	}

	public void BlockVideo (bool b)
	{
		btnVideo.Block (b);
	}

	public void BlockCard (bool b)
	{
		btnOpen [0].Block (b);
		btnOpen [1].Block (b);
	}

	public void ShowVideo (bool b)
	{
		if (b) {
			if (!bOpen) {
				btnVideo.gameObject.SetActive (true);
				StartCoroutine (CheckVideo ());
			}
		} else {
			btnVideo.gameObject.SetActive (false);
		}
	}

	IEnumerator CheckVideo ()
	{
		while (!AllInOne.Instance.CheckVideoReward ()) {
			yield return null;
		}
		btnVideo.Block (false);
	}

	public void ShowOpen (bool b)
	{
		btnOpen [0].gameObject.SetActive (b);
		btnOpen [1].Block (!b);
	}

	public void ShowAllReward ()
	{
		bPreview = true;
		Show (false);
		ShowOpen (false);
		ShowVideo (false);
	}

	public void ResetAllData ()
	{
		bPreview = false;
		bOpen = false;
		anim.Rebind ();
		anim.enabled = false;
		rectBoder.gameObject.SetActive (false);
		textCd.text = "";
		textCd.enabled = true;
	}

	void Update ()
	{
		if (!anim.enabled)
			return;
		if (Time.timeScale < 1) {
			anim.Update (Time.unscaledDeltaTime);
		}
	}
}
