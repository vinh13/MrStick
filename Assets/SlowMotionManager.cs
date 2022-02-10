using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionManager : MonoBehaviour
{
	public static SlowMotionManager Instance = null;
	[SerializeField]float duration = 0;
	[SerializeField]float timeSacle = 0;
	[SerializeField]bool isSlow = false;

	void Awake ()
	{
		if (Instance == null)
			Instance = this;
	}

	public void Slow (bool b)
	{
		isSlow = b;
	}

	void Update ()
	{
		if (Logic.isPause)
			return;
		if (isSlow) {
			Time.timeScale = timeSacle;
		} else {
			Time.timeScale = 1F;
		}
		Time.fixedDeltaTime = 0.02F * Time.timeScale;
	}
}
