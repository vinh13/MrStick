using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRateUS : MonoBehaviour
{
	[SerializeField]AnimatorPopUpScript animpop = null;
	[SerializeField]btnStar[] listBtn = new btnStar[5];
	[SerializeField]UIButton btnComfirm = null;
	[SerializeField]UIButton btnExit = null;
	[SerializeField]UIButton btnFacebook = null;
	int starRate = 0;

	void Start ()
	{
		for (int i = 0; i < listBtn.Length; i++) {
			listBtn [i].Setup (i, RateUS);
		}
		btnComfirm.Register (Confirm);
		btnExit.Register (Exit);
		btnFacebook.Register (JointUS);
		RateUS (5);
	}

	public void Show ()
	{
		//AudioUIManager.Instance.Play ("UI_Popup_Open");
		animpop.show (null);
	}

	void RateUS (int index)
	{
		for (int i = 0; i < listBtn.Length; i++) {
			if (i <= index) {
				listBtn [i].Active ();
			} else {
				listBtn [i].Disable ();
			}
		}
		starRate = index + 1;
	}

	public void Exit ()
	{
		//AudioUIManager.Instance.Play ("UI_Back");
		animpop.hide (null);
	}

	public void Confirm ()
	{
		if (starRate > 0) {
			FBManagerEvent.Instance.PostEventCustom ("user_rate_" + starRate);
			if (starRate >= 4) {
				#if UNITY_ANDROID
				Application.OpenURL ("market://details?id=stickman.ragdoll.happy.wheel");
				#elif UNITY_IPHONE
				Application.OpenURL ("https://itunes.apple.com/us/app/ragdoll-warriors-epic-war/id1446820332?mt=8");
				#endif
			}
		}
		GameData.RateUSNow = true;
		animpop.hide (null);
	}

	public void JointUS ()
	{
		Application.OpenURL (
			"https://www.facebook.com/mrstickepic/");
	}

	void OnDisable ()
	{
		Destroy (gameObject);
	}
}
