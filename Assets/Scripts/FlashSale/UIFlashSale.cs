using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIFlashSale : MonoBehaviour
{
	[SerializeField]AnimatorPopUpScript animPop = null;

	public void Show (Action a)
	{
		animPop.show (a);
		//AudioUIManager.Instance.Play ("UI_Popup_Open");
	}

	public void Hide (Action a)
	{
		animPop.hide (a);
		//AudioUIManager.Instance.Play ("UI_Back");
	}
}
