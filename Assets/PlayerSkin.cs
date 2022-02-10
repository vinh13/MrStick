using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkin : MonoBehaviour
{
	[SerializeField]SkinManager skinManager = null;
	[SerializeField]SkinBikeManager skinBike = null;

	void Awake ()
	{
		SyncBike ();
		if (SkinData.bTry) {
			SyncTry ();
			SkinData.bTry = false;
		} else {
			SyncData ();
		}
	}

	void SyncData ()
	{
		string data = SkinData.GetSkin;
		string[] texts = data.Split ('.');
		for (int i = 0; i < texts.Length; i++) {
			if (texts [i] != "")
				skinManager.CreatePart (texts [i]);
		}
	}

	void SyncTry ()
	{
		int idTry = SkinData.IdTry;
		skinManager.CreateSkin (idTry);
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
