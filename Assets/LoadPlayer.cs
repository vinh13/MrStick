using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadPlayer : MonoBehaviour
{
	bool bShow = false;
	void Awake ()
	{
		if (bShow)
			return;
		bShow = true;
		Manager.Instance.ShowPlayer (true);
	}

	void OnDisable ()
	{
		if (!bShow)
			return;
		bShow = false;
		Manager.Instance.ShowPlayer (false);
	}
}
