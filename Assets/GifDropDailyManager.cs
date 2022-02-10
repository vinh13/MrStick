using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GifSync
{
	public int ID = 0;
	public bool bRewarded = false;
}

public class GifDropDailyManager : MonoBehaviour
{
	[SerializeField]TotalStar toltalStar = null;
	[SerializeField]float rangeX = 396.8F;
	[SerializeField]float numberStar = 0;
	[SerializeField]float currentStar = 0;
	[SerializeField]GifTarget[] gifsTarget;
	[SerializeField]Transform panels = null;
	float[] targetStars = {
		3, 7, 13, 19, 25, 31
	};
	bool bAvailableGif = false;
	bool[] bGifRewared;
	bool[] bUnlocked;
	int idUnlocked = 1;
	#if UNITY_EDITOR
	void OnValidate ()
	{
		gifsTarget = panels.GetComponentsInChildren<GifTarget> ();
	}
	#endif
	public void SetStar (AddStarS star)
	{
		numberStar = 31;
		bAvailableGif = GameData.GetDailyGifRewarded;
		if (bAvailableGif) {
			SyncOff (star);
			return;
		}
		currentStar = GameData.GetStarGif;
		currentStar += star.star;
		if (currentStar >= numberStar) {
			bAvailableGif = true;
			GameData.GetDailyGifRewarded = bAvailableGif;
		}
		GameData.GetStarGif = (int)currentStar;
		float ratio = currentStar / numberStar;
		bGifRewared = new bool[gifsTarget.Length];
		bUnlocked = new bool[gifsTarget.Length];
		toltalStar.UpdateStar (ratio, "" + currentStar, "" + numberStar, true);
		bool bShowRoll = false;
		for (int i = 0; i < gifsTarget.Length; i++) {
			bGifRewared [i] = GameData.GetGifRewarded ("tdz" + i);
			if (currentStar >= targetStars [i]) {
				bUnlocked [i] = true;
			}
		}
		if (star.bLate) {
			//CheckAutoShow ();
			ShowCardGift ();
		}

		SetTarget ();
	}

	void SyncOff (AddStarS ad)
	{
		currentStar = numberStar;
		float ratio = currentStar / numberStar;
		toltalStar.UpdateStar (ratio, "" + currentStar, "" + numberStar, false);
		bGifRewared = new bool[gifsTarget.Length];
		bUnlocked = new bool[gifsTarget.Length];
		for (int i = 0; i < gifsTarget.Length; i++) {
			bGifRewared [i] = GameData.GetGifRewarded ("tdz" + i);
			if (currentStar >= targetStars [i]) {
				bUnlocked [i] = true;
			}
		}
		if (ad.bLate) {
			//CheckAutoShow ();
			ShowCardGift ();
		}
		SetTarget ();
	}

	void ShowCardGift ()
	{
		bool bShowRoll = false;
		for (int i = gifsTarget.Length - 1; i >= 0; i--) {
			if (bUnlocked [i]) {
				if (!bGifRewared [i]) {
					if (!bShowRoll) {
						bGifRewared [i] = true;
						GameData.SetGifRewarded ("tdz" + i, bGifRewared [i]);
						Manager.Instance.ShowWaitting (true);
						StartCoroutine (ShowCardGiftAuto ());
						bShowRoll = true;
						break;
					}

				}
			}
		}
	}

	IEnumerator ShowCardGiftAuto ()
	{
		yield return new WaitForSecondsRealtime (0.5F);
		SFXManager.Instance.Play ("popup_open");
		//GifManager.Instance.ShowRoll (id, true, bGifRewared [id], SyncData);
		GifManager.Instance.ShowCardGift ();
		Manager.Instance.ShowWaitting (false);
	}

	//	void CheckAutoShow ()
	//	{
	//		bool bShowRoll = false;
	//		for (int i = gifsTarget.Length - 1; i >= 0; i--) {
	//			if (bUnlocked [i]) {
	//				if (!bGifRewared [i]) {
	//					if (!bShowRoll) {
	//						if (!GameData.GetRollAction (i + 1, "gRoll")) {
	//							Manager.Instance.ShowWaitting (true);
	//							StartCoroutine (ShowRollAuto (i));
	//						}
	//						bShowRoll = true;
	//					}
	//				}
	//			}
	//		}
	//	}

	void SetTarget ()
	{
		for (int i = 0; i < gifsTarget.Length; i++) {
			float ratio = targetStars [i] / numberStar;
			Vector3 localPos = Vector2.zero;
			localPos.x = ratio * rangeX;
			//gifsTarget [i].Setup (ShowRoll, bGifRewared [i], i, bUnlocked [i]);
			gifsTarget [i].Setup (ClickGift, bGifRewared [i], i, bUnlocked [i], (int)targetStars [i]);
			gifsTarget [i].transform.localPosition = localPos;
		}
	}

	void ClickGift (object ob)
	{
		
	}

	//	void ShowRoll (object ob)
	//	{
	//		int id = (int)ob;
	//		if (bUnlocked [id]) {
	//			GifManager.Instance.ShowRoll (id, true, bGifRewared [id], SyncData);
	//		} else {
	//			GifManager.Instance.ShowRoll (id, true, true, SyncData);
	//		}
	//	}

	//	IEnumerator ShowRollAuto (int id)
	//	{
	//		yield return new WaitForSecondsRealtime (0.5F);
	//		SFXManager.Instance.Play ("popup_open");
	//		//GifManager.Instance.ShowRoll (id, true, bGifRewared [id], SyncData);
	//		Manager.Instance.ShowWaitting (false);
	//	}
	//
	//	void SyncData (object ob)
	//	{
	//		GifSync gif = (GifSync)ob;
	//		int i = gif.ID;
	//		bGifRewared [i] = gif.bRewarded;
	//		GameData.SetGifRewarded ("tdz" + i, bGifRewared [i]);
	//		if (bGifRewared [i]) {
	//			bUnlocked [i] = false;
	//		}
	//		gifsTarget [i].Setup (bGifRewared [i], i, bUnlocked [i]);
	//	}

	//	void Update ()
	//	{
	//		if (Input.GetKey (KeyCode.U)) {
	//			SetStar (21);
	//		}
	//	}
}
