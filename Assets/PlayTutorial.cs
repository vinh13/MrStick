using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayTutorial : MonoBehaviour
{
	Dictionary<TutorialID,Action<object>> Listeners = new Dictionary<TutorialID, Action<object>> ();
	public static PlayTutorial Instance = null;
	Transform HUD = null;
	ParticleSystem parFocus = null;

	public void ActiveFocus (Transform target)
	{
		parFocus.gameObject.SetActive (true);
		parFocus.transform.position = target.position;
		parFocus.Play ();
	}

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

	[HideInInspector]
	public UIGame uiGame = null;
	public void Register (Transform t, UIGame ui, ParticleSystem par)
	{
		if (Instance == null) {
			Instance = this;
		}
		HUD = t;
		uiGame = ui;
		parFocus = par;
		int index = 0;
		index += CreateHealth ();
		index += CreateShield ();
		index += CreateWeapon ();
		if (index == 3) {
			Destroy (gameObject);
			TutorialData.bDonePlay = false;
		}
	}

	int CreateHealth ()
	{
		if (TutorialData.bUseHealth) {
			GameObject go = Instantiate (Resources.Load<GameObject> ("Tutorial/UITutorialHealth"), HUD);
			return 0;
		} else {
			return 1;
		}
	}

	int CreateShield ()
	{
		if (TutorialData.bUseShield) {
			GameObject go = Instantiate (Resources.Load<GameObject> ("Tutorial/UITutorialShield"), HUD);
			return 0;
		} else {
			return 1;
		}
	}

	int CreateWeapon ()
	{
		if (TutorialData.bUseWeapon) {
			GameObject go = Instantiate (Resources.Load<GameObject> ("Tutorial/UITutorialWeapon"), HUD);
			return 0;
		} else {
			return 1;
		}
	}
}
