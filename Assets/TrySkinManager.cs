using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PartSkin
{
	public int ID = 0;
	public int Oder = 0;
	public string skinID = "";
}

public class TrySkinManager : MonoBehaviour
{
	[SerializeField]UIButton btnBuy = null, btnVideo = null;
	[SerializeField]Text textPrice = null, textCountVideo = null;
	[SerializeField]Transform[] rects = new Transform[2];
	public List<PartSkin> partSkins = new List<PartSkin> ();
	int oderMax = 0;
	int indexMax = 0;
	public int IDSkinNext = 0;
	int price = 200;
	int countVideo = 0;
	const string bootType = "trySkin";
	bool bTry = false;

	void Awake ()
	{
		btnBuy.Register (Buy);
		btnVideo.Register (ShowVideo);
	}

	#region Buy

	void UpdatePrice ()
	{
		textPrice.text = "" + price;
	}

	void UpdateCountVideo ()
	{
		textCountVideo.text = "" + countVideo;
		CharacterData.SetCountVideoBoot (bootType.ToString (), countVideo);
	}

	void Buy ()
	{
		if (CoinManager.Instance.CheckGem (price)) {
			CoinManager.Instance.PurchaseGem (-price, false);
			ActiveTry ();
		}
	}

	void ShowVideo ()
	{
		FBManagerEvent.Instance.PostEventCustom ("videoBuy_skin");
		FBManagerEvent.Instance.PostEventCustom ("videoBuy_skin_" + SkinData.NameSkinNext (IDSkinNext));
		btnVideo.Block (true);
		btnBuy.Block (true);
		AllInOne.Instance.ShowVideoReward (CallbackVideo,"TrySkin",LevelData.IDLevel);
	}

	void CallbackVideo (bool b)
	{
		if (b) {
			countVideo--;
			countVideo = Mathf.Clamp (countVideo, 0, 20);
			UpdateCountVideo ();
			if (countVideo == 0) {
				ActiveTry ();
			} else {
				btnVideo.Block (false);
				btnBuy.Block (false);
			}
			price = CoinManager.Instance.coinPerVideo * countVideo;
			UpdatePrice ();
		} else {
			btnVideo.Block (true);
			StartCoroutine (CheckVideo ());
		}
	}

	IEnumerator CheckVideo ()
	{
		while (!AllInOne.Instance.CheckVideoReward ()) {
			yield return null;
		}
		btnVideo.Block (false);
	}

	void ShowBuy (bool b)
	{
		rects [0].gameObject.SetActive (b);
		rects [1].gameObject.SetActive (!b);
	}

	void ActiveTry ()
	{
		SkinData.bTry = true;
		SkinData.IdTry = IDSkinNext;
		ShowBuy (false);
	}

	#endregion

	void OnEnable ()
	{
		bTry = SkinData.bTry;
		if (!bTry) {
			int idMax = 0;
			int index = 0;
			string data = SkinData.GetSkin;
			string[] texts = data.Split ('.');
			for (int i = 0; i < texts.Length; i++) {
				if (texts [i] != "") {
					PartSkin pTemp = _PartSkin (texts [i]);
					int temp = GetOder (texts [i]);
					pTemp.Oder = temp;
					if (temp >= idMax) {
						idMax = temp;
						index = pTemp.ID;
					}
					partSkins.Add (pTemp);
				}
			}
			oderMax = idMax;
			indexMax = index;
			GetNextSkin ();
			ShowBuy (true);
			btnBuy.Block (false);
			btnVideo.Block (true);
			StartCoroutine (CheckVideo ());
		} else {
			ShowBuy (false);
			SyncTry ();
		}
	}

	void SyncTry ()
	{
		int id = 0;
		id = SkinData.IdTry;
		CreatePreview (Resources.Load<PartSkinData> ("PlayerSkin/" + id + "/" + id));
	}

	void GetNextSkin ()
	{
		PartSkinData d = Resources.Load<PartSkinData> ("PlayerSkin/" + indexMax + "/" + indexMax);
		int countIndex = 0;
		float ratio = 0;
		bool b = false;
		if (d != null) {
			for (int i = 0; i < partSkins.Count; i++) {
				if (partSkins [i].ID == indexMax) {
					countIndex++;
				}
			}
			ratio = (float)countIndex / (float)d.MaxPart;
			b = Current (ratio);
			if (oderMax == SkinData.OderSkin (SkinData.MaxIDSkin)) {
				if (ratio < 1F) {
					IDSkinNext = SkinData.MaxIDSkin;
				} else {
					//Debug.Log ("Max Level Skin");
					//IDSkinNext = SkinData.MaxIDSkin;
					//ShowBuy (false);
					IDSkinNext = SkinData.IDSkin (Random.Range (1, 7));
				}
			} else {
				if (b) {
					IDSkinNext = SkinData.IDSkin (oderMax);
				} else {
					IDSkinNext = SkinData.IDSkin (SkinData.GetNext (oderMax));
				}
			}
		} else {
			IDSkinNext = SkinData.IDSkin (Random.Range (8, 13));
		}
		int idSugget = SkinData.SuggestId;
		if (idSugget != 0) {
			if (SkinData.OderSkin (IDSkinNext) < SkinData.OderSkin (idSugget)) {
				IDSkinNext = idSugget;
			} else {
				IDSkinNext = idSugget;
				SkinData.SuggestId = 0;
			}
		}
		CreatePreview (Resources.Load<PartSkinData> ("PlayerSkin/" + IDSkinNext + "/" + IDSkinNext));
		if (b) {
			if (ratio >= 0.5F) {
				price = 200 + SkinData.GetPriceNext (IDSkinNext);
			} else {
				price = 200 + SkinData.GetPriceNext (IDSkinNext);
			}
		} else {
			price = 200 + SkinData.GetPriceNext (IDSkinNext);
		}
		countVideo = CharacterData.GetCountVideoBoot (bootType.ToString ());
		//if (countVideo == 0) {
		countVideo = 1;
		//}
		UpdatePrice ();
		UpdateCountVideo ();

	}

	void CreatePreview (PartSkinData d)
	{
		for (int i = 0; i < d.MaxPart; i++) {
			SkinEditor.Instance.CreatePartSkin (IDSkinNext, d.skinID [i]);
		}
	}

	bool Current (float ratio)
	{
		bool b = true;
		if (ratio >= 0.5F) {
			b = false;
		}
		return b;
	}

	int GetOder (string text)
	{
		string[] texts = text.Split ('_');
		int id = int.Parse (texts [0]);
		return SkinData.OderSkin (id);
	}

	PartSkin _PartSkin (string text)
	{
		PartSkin temp = new PartSkin ();
		string[] texts = text.Split ('_');
		temp.ID = int.Parse (texts [0]);
		temp.skinID = texts [1];
		return temp;
	}
}
