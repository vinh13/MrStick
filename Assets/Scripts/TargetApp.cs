using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetApp : MonoBehaviour
{
	void OnEnable ()
	{
		Application.targetFrameRate = 60;
	}
}
