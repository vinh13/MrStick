using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[System.Serializable]
public enum TutorialID
{
	None = 0,
	Slide = 1,
	Jump = 2,
	Shot = 3,
	JumpMove = 4,
	UpSpeed = 5,
	Health = 6,
	PlayGame = 7,
	Level = 8,
	BeforePlay = 9,
	ToATK = 10,
	UpATK = 11,
	UseHealth = 12,
	UseShield = 13,
	UseWeapon = 14,
	Attack = 15,
	ClickUpgrade = 16,
}

public class TutorialManager : MonoBehaviour
{
	Dictionary<TutorialID,Action<object>> Listeners = new Dictionary<TutorialID, Action<object>> ();
	public static TutorialManager Instance = null;
	[SerializeField]Transform HUD = null;
	[SerializeField]ParticleSystem parFocus = null;
	[SerializeField]UIGame uiBot = null;
	bool bJumpMove = false;
	[SerializeField]bool bTest = false;

	public void ActiveFocus (Transform target)
	{
		parFocus.gameObject.SetActive (true);
		parFocus.transform.position = target.position;
		parFocus.Play ();
	}

	#region Listeners

	public void RegisterTutorial (TutorialID ID, Action<object> cb)
	{
		if (!Listeners.ContainsKey (ID)) {
			Listeners.Add (ID, cb);
		}
	}

	public void RemoveTutorial (TutorialID ID)
	{
		if (Listeners.ContainsKey (ID)) {
			Listeners.Remove (ID);
		} 
	}

	public void ShowTutorial (TutorialID ID)
	{
		if (Listeners.ContainsKey (ID)) {
			Logic.PAUSE ();
			Listeners [ID].Invoke (true);
		}
	}

	public void HideTutorial (TutorialID ID)
	{
		if (Listeners.ContainsKey (ID)) {
			Logic.UNPAUSE ();
			parFocus.gameObject.SetActive (false);
			Listeners [ID].Invoke (false);
		}
	}

	#endregion

	public UIGame uiGame {
		get { 
			return HUD.GetComponentInChildren<UIGame> ();
		}
	}

	void Awake ()
	{
		if (bTest)
			TutorialData.bTutorialStart = false;
		if (TutorialData.bTutorialStart) {
			//Create Tutorial
			GameObject go = Instantiate (Resources.Load<GameObject> ("UI/UITutorial"), HUD);
			if (Instance == null)
				Instance = this;
			Application.targetFrameRate = 60;
		} else {
			if (TutorialData.bDonePlay) {
				GameObject playDone = new GameObject ();
				playDone.name = "PlayTutorial";
				parFocus.transform.SetParent (null);
				playDone.AddComponent<PlayTutorial> ().Register (HUD, uiBot, parFocus);
			}
			Destroy (this.gameObject);
		}
	}

	public void SendTutorial (TutorialID ID)
	{
		HideTutorial (ID);
		if (bJumpMove) {
			if (ID == TutorialID.Jump) {
				TaskUtil.Schedule (this, _JumpMove, 0.5F);
				bJumpMove = false;
			}
		}
	}

	public void JumpMove ()
	{
		bJumpMove = true;
	}

	void _JumpMove ()
	{
		ShowTutorial (TutorialID.JumpMove);
	}
}
