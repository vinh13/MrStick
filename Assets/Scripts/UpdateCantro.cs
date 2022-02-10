using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class UpdateCantro : MonoBehaviour
{
	[SerializeField]Text textUpdate = null;
	[SerializeField]UIButton btnLater = null, btnUpdate = null;
	Action cbLoadScene = null;
	void Start ()
	{
		textUpdate.text = TdzRemote.InfoUpdate;
		btnLater.Register (ClickLater);
		btnUpdate.Register (ClickUpdate);
	}

	public void Register (Action a)
	{
		cbLoadScene = a;
	}

	public void ClickFB ()
	{
		Application.OpenURL ("https://www.facebook.com/mrstickepic/");
	}

	public void ClickLater ()
	{
		cbLoadScene.Invoke ();
		GameData.CheckUpdate = true;
		Destroy (this.gameObject);
	}

	public void ClickUpdate ()
	{
		cbLoadScene.Invoke ();
		GameData.CheckUpdate = true;
		#if UNITY_ANDROID
		Application.OpenURL ("market://details?id=stickman.ragdoll.happy.wheel");
		#elif UNITY_IPHONE
		//Application.OpenURL ("https://www.facebook.com/mrstickepic/");
		#endif
		Destroy (this.gameObject);
	}
}
