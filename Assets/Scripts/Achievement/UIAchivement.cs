using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class UIAchivement : MonoBehaviour
{
	[SerializeField]AnimatorPopUpScript animPop = null;
	public void Show (Action a)
	{
		animPop.show (a);
	}
	public void Hide (Action a)
	{
		animPop.hide (a);
	}
}
