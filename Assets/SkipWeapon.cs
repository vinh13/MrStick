using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SkipWeapon : MonoBehaviour
{
	[SerializeField]UIButton btn = null;
	void Start ()
	{
		btn.Register (Click);
	}
	public void Click ()
	{
		TutorialData.bUseWeapon = false;
		PlayTutorial.Instance.HideTutorial (TutorialID.UseWeapon);
	}
}
