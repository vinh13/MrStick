using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DailyRewardChecking : MonoBehaviour
{
	int countDay = 0;
	bool bRewarded = false;
	float timer = 0;
	[SerializeField]UIButton btnDaily = null;

	void Start ()
	{
		btnDaily.Register (CLick);
		countDay = GameData.DayCount;
		bRewarded = GameData.GetDayRewarded ("day" + countDay);
		if (!bRewarded) {
			if (!TutorialData.bMenuTutorial && !TutorialData.bToATK) {
				timer = 0;
				StartCoroutine (ShowDaily ());
			}
		}
	}

	void CLick ()
	{
		LoadDailyReward ();
	}

	public void LoadDailyReward ()
	{
		Manager.Instance.ShowWaitting (true);
		if (!SceneManager.GetSceneByName (SceneName.DailyReward.ToString ()).isLoaded)
			SceneManager.LoadSceneAsync (SceneName.DailyReward.ToString (), LoadSceneMode.Additive);
	}

	IEnumerator ShowDaily ()
	{
		bool done = false;
		while (!done) {
			timer += Time.deltaTime;
			done = AllInOne.Instance.CheckVideoReward ();
			if (timer >= 3F) {
				done = true;
			}
			yield return null;
		}
		LoadDailyReward ();
	}

	void OnDisable ()
	{
		StopAllCoroutines ();
	}
}
