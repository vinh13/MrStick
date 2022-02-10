using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class VideoCantro : MonoBehaviour
{
	[SerializeField]UIButton btnExit = null;
	[SerializeField]AnimatorPopUpScript animPop = null;
	void Start ()
	{
		Manager.Instance.ShowWaitting (true);
		btnExit.Register (ClickExit);
		Invoke ("Show", 0.5F);
	}

	void Show ()
	{
		animPop.show (OnShow);
	}

	void OnShow ()
	{
		Manager.Instance.ShowWaitting (false);
		VideoRewardManager.Instance.ActiveReward ();
	}

	void Hide ()
	{
		animPop.hide (OnHide);
	}

	void OnHide ()
	{
		if (SceneManager.GetSceneByName (SceneName.VideoReward.ToString ()).isLoaded)
			SceneManager.UnloadSceneAsync (SceneName.VideoReward.GetHashCode ());
	}

	void ClickExit ()
	{
		Hide ();
	}
}
