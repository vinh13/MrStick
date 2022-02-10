using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TutorialAttack : MonoBehaviour
{
	Dictionary<TutorialID,Action<object>> Listeners = new Dictionary<TutorialID, Action<object>> ();
	public static TutorialAttack Instance = null;
	ParticleSystem parFocus = null;
	[HideInInspector]
	public UIGame uiGame = null;
	Transform HUD = null;

	#region Listener

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
			switch (ID) {
			case TutorialID.Attack:
				UIObjectManager.Instance.CompleteAttack2nd ();
				break;
			}
		}
	}


	#endregion

	public void Register (Transform t, UIGame ui)
	{
		if (Instance == null) {
			Instance = this;
		}
		HUD = t;
		uiGame = ui;
		parFocus = Instantiate (Resources.Load<GameObject> ("Tutorial/focus"), this.transform).
			GetComponent<ParticleSystem> ();
		CreateUI ();
	}

	void CreateUI ()
	{
		Instantiate (Resources.Load<GameObject> ("Tutorial/UITutorialAttack"), HUD);
	}

	public void ActiveFocus (Transform target)
	{
		parFocus.gameObject.SetActive (true);
		parFocus.transform.position = target.position;
		parFocus.Play ();
	}
}
