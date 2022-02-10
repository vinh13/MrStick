using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public enum MenuType
{
	Home = 0,
	Level = 1,
}

public class MenuTutorial : MonoBehaviour
{
	Dictionary<TutorialID,Action<object>> Listeners = new Dictionary<TutorialID, Action<object>> ();
	public static MenuTutorial Instance = null;
	[SerializeField]Transform HUD = null;
	[SerializeField]MenuType menuType = MenuType.Home;
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
			parFocus.gameObject.SetActive (false);
		}
	}

	#endregion

	void Awake ()
	{
		if (menuType == MenuType.Home) {
			if (!TutorialData.bToATK) {
				if (TutorialData.bMenuTutorial) {
					GameObject go = Instantiate (Resources.Load<GameObject> ("UI/UIMenuTutorial"), HUD);
					if (Instance == null)
						Instance = this;
				} else {
					Destroy (this.gameObject);
				}
			} else {
				if (TutorialData.bTutorialUpgradeATK) {
					GameObject go = Instantiate (Resources.Load<GameObject> ("UI/UIUpgradeATK"), HUD);
					if (Instance == null)
						Instance = this;
				} else {
					Destroy (this.gameObject);
				}
			}
		} else if (menuType == MenuType.Level) {
			if (TutorialData.bLevelTutorial) {
				GameObject go = Instantiate (Resources.Load<GameObject> ("UI/UILevelTutorial"), HUD);
				if (Instance == null)
					Instance = this;
			} else {
				Destroy (this.gameObject);
			}
		}
	}

	public UIGame uiGame {
		get { 
			return GetComponent<UIGame> ();
		}
	}
}
