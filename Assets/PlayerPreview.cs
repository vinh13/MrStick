using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPreview : MonoBehaviour
{
	[SerializeField]SkinManager skinManager = null;
	[SerializeField]SkinBikeManager skinBike = null;

	void Awake ()
	{
		if (SkinData.bPreview) {
			SkinData.bPreview = false;
			string data = SkinData.GetSkinPreview;
			SyncData (data);
		} else {
			string data = SkinData.GetSkin;
			SyncData (data);
		}
		SyncBike ();
	}

	void SyncData (string data)
	{
		string[] texts = data.Split ('.');
		for (int i = 0; i < texts.Length; i++) {
			if (texts [i] != "")
				skinManager.CreatePart (texts [i]);
		}
	}

	void SyncBike ()
	{
		string text = SkinData.GetBike;
		string[] texts = text.Split ('.');
		for (int i = 0; i < texts.Length; i++) {
			if (texts [i] != "")
				skinBike.CreateSkin (texts [i]);
		}
	}
}
