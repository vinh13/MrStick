using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InfoTutorial : MonoBehaviour
{
	Dictionary<TutorialID,Action<object>> Listeners = new Dictionary<TutorialID, Action<object>> ();
	public static InfoTutorial Instance = null;
	[SerializeField]ParticleSystem parFocus = null;

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
			Debug.Log ("Show" + ID);
			Listeners [ID].Invoke (true);
		}
	}

	public void HideTutorial (TutorialID ID)
	{
		if (Listeners.ContainsKey (ID)) {
			Logic.UNPAUSE ();
			Debug.Log ("Hide" + ID);
			parFocus.gameObject.SetActive (false);
			Listeners [ID].Invoke (false);
		}
	}

	#endregion


	void Awake ()
	{
		if (Instance == null)
			Instance = this;
		GameObject go = Instantiate (Resources.Load<GameObject> ("UI/UITutorialToATK"), this.transform);
		parFocus = Instantiate (Resources.Load<GameObject> ("Tutorial/focus")).GetComponent<ParticleSystem> ();
	}

	public UIGame uiGame {
		get { 
			return this.GetComponent<UIGame> ();
		}
	}
}
