using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPreview : MonoBehaviour
{
	bool bShow = false;

	void Start ()
	{
		if (bShow)
			return;
		bShow = true;
		Manager.Instance.ShowPlayerPreview (true);
	}

	void OnDisable ()
	{
		if (!bShow)
			return;
		bShow = false;
		Manager.Instance.ShowPlayerPreview (false);
	}
}
