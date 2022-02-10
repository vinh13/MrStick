using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIComingSoon : MonoBehaviour
{
	[SerializeField]Button btnQuit = null, btnFacebook = null;

	void Start ()
	{
		btnQuit.onClick.AddListener (delegate() {
			ClickQuit ();	
		});
		btnFacebook.onClick.AddListener (delegate() {
			ClickFacebook ();	
		});
	}

	public void Show ()
	{
		gameObject.SetActive (true);
	}

	void ClickQuit ()
	{
		gameObject.SetActive (false);
	}

	void ClickFacebook ()
	{
		Application.OpenURL ("https://www.facebook.com/Stickman-Happy-Wheel-109739750394555");
	}
}

