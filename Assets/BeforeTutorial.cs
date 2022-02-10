using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BeforeTutorial : MonoBehaviour
{

	Dictionary<TutorialID,Action<object>> Listeners = new Dictionary<TutorialID, Action<object>> ();
	public static BeforeTutorial Instance = null;
	[SerializeField]Transform HUD = null;
	[SerializeField]ParticleSystem parFocus = null;
	bool bJumpMove = false;

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
			Listeners [ID].Invoke (true);
		}
	}

	public void HideTutorial (TutorialID ID)
	{
		if (Listeners.ContainsKey (ID)) {
			Listeners [ID].Invoke (false);
			Logic.UNPAUSE ();
			parFocus.gameObject.SetActive (false);
			if (ID == TutorialID.BeforePlay)
				Destroy (this.gameObject);
		}
	}

	#endregion

	public void Register (Transform rectHUD)
	{
		HUD = rectHUD;
		if (TutorialData.bBeforePlay) {
			GameObject go = Instantiate (Resources.Load<GameObject> ("UI/UIBeforeTutorial"), HUD);
			if (Instance == null)
				Instance = this;
		} else {
			Destroy (this.gameObject);
		}

	}

	public UIGame uiGame {
		get { 
			return HUD.GetChild (0).GetComponent<UIGame> ();
		}
	}
}