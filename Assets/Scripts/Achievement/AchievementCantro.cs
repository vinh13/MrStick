using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AchievementCantro : MonoBehaviour
{
	[SerializeField]UIAchivement uiAchievement = null;
	[SerializeField]UIButton btnQuit = null;

	void Start ()
	{
		btnQuit.Register (_UnloadScene);
		uiAchievement.Show (null);
		TaskUtil.Schedule (this, Show, 0.1F);

	}

	void Show ()
	{
		Manager.Instance.ShowWaitting (false);
		//AudioUIManager.Instance.Play ("UI_Popup_Open");
	}

	[SerializeField]AchievementType type = AchievementType.None;
	[SerializeField]string keyMore = "";

	void Update ()
	{
		if (Input.GetKey (KeyCode.U)) {
			int i = AchievementData.GetCurrentValue (type, keyMore);
			i += 10;
			AchievementData.SetCurrentValue (type, keyMore, i);
		}
	}

	public void UnloadScene ()
	{
		uiAchievement.Hide (_UnloadScene);
		//AudioUIManager.Instance.Play ("UI_Back");
	}

	void _UnloadScene ()
	{
		SceneManager.UnloadSceneAsync (SceneName.Achievement.GetHashCode ());
	}
}
