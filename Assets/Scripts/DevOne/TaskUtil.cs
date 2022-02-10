using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using CodeStage.AntiCheat.ObscuredTypes;

public static class TaskUtil
{
	public static void Schedule (MonoBehaviour mon, Action action, float delay)
	{
		mon.StartCoroutine (DelayRoutine (action, delay));
	}

	private static IEnumerator DelayRoutine (Action action, float delay)
	{
		yield return new WaitForSecondsRealtime (delay);
		action.Invoke ();
	}


	public static void Schedule (MonoBehaviour mon, Action<object> action, float delay, object ob)
	{
		mon.StartCoroutine (DelayRoutine (action, delay, ob));
	}

	private static IEnumerator DelayRoutine (Action<object> action, float delay, object ob)
	{
		yield return new WaitForSecondsRealtime (delay);
		action.Invoke (ob);
	}



	public static void ScheduleWithTimeScale (MonoBehaviour mon, Action action, float delay)
	{
		mon.StartCoroutine (DelayRoutineWithTimeScale (action, delay));
	}

	private static IEnumerator DelayRoutineWithTimeScale (Action action, float delay)
	{
		yield return new WaitForSeconds (delay);
		action.Invoke ();
	}

	public static bool Tdz {
		get {
			int index = Random.Range (1, 11);
			return index <= 5;
		}
	}

	public static bool CheckKeyOne (string key)
	{
		bool temp = PlayerPrefs.GetInt (key, 0) == 0 ? true : false;
		if (temp) {
			PlayerPrefs.SetInt (key, 1);
			PlayerPrefs.Save ();
		}
		return temp;
	}

	public static string Convert (int value)
	{
		string text = value.ToString ("N1");
		string[] texts = text.Split ('.');
		return texts [0];
	}


	#region PlayerPrefs

	public static int GetInt (string key)
	{
		return ObscuredPrefs.GetInt (key, 0);
	}

	public static void SetInt (string key, int value)
	{
		ObscuredPrefs.SetInt (key, value);
		ObscuredPrefs.Save ();
	}

	public static float GetFloat (string key)
	{
		return ObscuredPrefs.GetFloat (key, 0);
	}

	public static void SetFloat (string key, float value)
	{
		ObscuredPrefs.SetFloat (key, value);
		ObscuredPrefs.Save ();
	}

	public static string GetString (string key)
	{
		return ObscuredPrefs.GetString (key, "");
	}

	public static void SetString (string key, string value)
	{
		ObscuredPrefs.SetString (key, value);
		ObscuredPrefs.Save ();
	}


	#endregion
}
