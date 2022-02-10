using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TungDz;
using System;
public class AchievementChecking : MonoBehaviour
{
	void Start ()
	{
	}
	void LoadScene ()
	{
		CacheScene.bAchievementChecking = true;
		SceneManager.LoadSceneAsync (SceneName.Achievement.GetHashCode (), LoadSceneMode.Additive);
	}
}
