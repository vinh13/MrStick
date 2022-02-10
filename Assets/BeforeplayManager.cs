using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeforeplayManager : MonoBehaviour
{

	private static BeforeplayManager instance;

	public static BeforeplayManager Instance {
		get {
			if (instance == null) {
				GameObject singletonObject = Instantiate (Resources.Load<GameObject> ("Manager/BeforeplayManager"));
				instance = singletonObject.GetComponent<BeforeplayManager> ();
				singletonObject.name = "Singleton - BeforeplayManager";
			}
			return instance;
		}
	}

	public static bool HasInstance ()
	{
		return instance != null;
	}

	[SerializeField]UIBeforePlay ui = null;


	public void Init ()
	{
	}

	void Awake ()
	{
		if (instance != null && instance.GetInstanceID () != this.GetInstanceID ()) {
			Destroy (gameObject);
		} else {
			instance = this as BeforeplayManager;
			DontDestroyOnLoad (gameObject);
			bTutorial = TutorialData.bBeforePlay;
		}
	}

	bool bTutorial = false;

	public void Show ()
	{
		ui.Show ();
	}

	public void ClickPlay ()
	{
		if (bTutorial) {
			bTutorial = false;
			TutorialData.bBeforePlay = false;
			BeforeTutorial.Instance.HideTutorial (TutorialID.BeforePlay);
		}
		Manager.Instance.LoadScene (SceneName.Main, true);
		Logic.bReady = false;
	}

	public void CreateTutorial ()
	{
		GameObject go = Instantiate (Resources.Load<GameObject> ("Tutorial/BeforeTutorial"), transform);
		go.GetComponent<BeforeTutorial> ().Register (ui.transform.parent);
	}
}
