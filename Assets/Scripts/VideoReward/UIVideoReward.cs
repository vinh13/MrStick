using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVideoReward : MonoBehaviour
{
	[SerializeField]int indexReward = 0;
	[SerializeField]Transform rectDone = null, rectLock = null, pos = null;
	[SerializeField]UIButton btnVideo = null;
	TypeReward lateReward = TypeReward.None;
	GameObject currentGif = null;
	bool bLoadVideoNow = false;

	public void ActiveReward ()
	{
		StartCoroutine (checkVideo ());
	}

	IEnumerator checkVideo ()
	{
		while (!AllInOne.Instance.CheckVideoReward ()) {
			Debug.Log ("Nhu lon!!!");
			yield return null;
		}
		btnVideo.Block (false);
	}

	public void BlockReward ()
	{
		btnVideo.Block (true);
		rectDone.gameObject.SetActive (true);
		rectLock.gameObject.SetActive (false);
	}


	public void LoadReward (object ob, int ID, TypeReward t, bool Availaible, bool bd)
	{
		btnVideo.Register (ClickButton);
		btnVideo.Block (true);
		indexReward = ID;
		rectDone.gameObject.SetActive (bd);
		rectLock.gameObject.SetActive (Availaible);
		CreateReward (ob, t);
	}

	void CreateGold (object ob, bool b)
	{
		if (!b) {
			if (currentGif != null)
				Destroy (currentGif);
			GameObject go = Instantiate (Resources.Load<GameObject> ("Video/Gold"), pos);
			go.GetComponent<RollGold> ().SetGold ((string)ob);
			go.gameObject.SetActive (true);
			currentGif = go;
		} else {
			currentGif.GetComponent<RollGold> ().SetGold ((string)ob);
		}
	}

	void CreateGem (object ob, bool b)
	{
		if (!b) {
			if (currentGif != null)
				Destroy (currentGif);
			GameObject go = Instantiate (Resources.Load<GameObject> ("Video/Gem"), pos);
			go.GetComponent<RollGold> ().SetGold ((string)ob);
			go.gameObject.SetActive (true);
			currentGif = go;
		} else {
			currentGif.GetComponent<RollGold> ().SetGold ((string)ob);
		}
	}

	void CreateKey (object ob, bool b)
	{
		if (!b) {
			if (currentGif != null)
				Destroy (currentGif);
			GameObject go = Instantiate (Resources.Load<GameObject> ("Video/Key"), pos);
			go.GetComponent<RollGold> ().SetGold ((string)ob);
			go.gameObject.SetActive (true);
			currentGif = go;
		} else {
			currentGif.GetComponent<RollGold> ().SetGold ((string)ob);
		}
	}

	void CreateSkin (object ob, bool b)
	{
		if (!b) {
			if (currentGif != null)
				Destroy (currentGif);
			GameObject go = Instantiate (Resources.Load<GameObject> ("Video/Skin"), pos);
			go.GetComponent<RollSkin> ().SetSkin ((EquipConfig)ob);
			go.gameObject.SetActive (true);
			currentGif = go;
		} else {
			currentGif.GetComponent<RollSkin> ().SetSkin ((EquipConfig)ob);
		}
	}

	void CreateBoot (object ob, bool b)
	{
		ItemBoot temp = (ItemBoot)ob;
		if (!b) {
			if (currentGif != null)
				Destroy (currentGif);
			GameObject go = Instantiate (Resources.Load<GameObject> ("Video/Item"), pos);
			go.GetComponent<RollItem> ().SetItem (temp._valueBoot, temp.bootType);
			go.gameObject.SetActive (true);
			currentGif = go;
		} else {
			currentGif.GetComponent<RollItem> ().SetItem (temp._valueBoot, temp.bootType);
		}
	}

	void CreateReward (object ob, TypeReward typeReward)
	{
		bool b = false;
		switch (typeReward) {
		case TypeReward.Key:
			b = lateReward == TypeReward.Key;
			CreateKey (ob, b);
			lateReward = TypeReward.Key;
			break;
		case TypeReward.Gold:
			b = lateReward == TypeReward.Gold;
			CreateGold (ob, b);
			lateReward = TypeReward.Gold;
			break;
		case TypeReward.Gem:
			b = lateReward == TypeReward.Gem;
			CreateGem (ob, b);
			lateReward = TypeReward.Gem;
			break;
		case TypeReward.Skin:
			b = lateReward == TypeReward.Skin ? true : lateReward == TypeReward.Wheels;
			CreateSkin (ob, b);
			lateReward = TypeReward.Skin;
			break;
		case TypeReward.Wheels:
			b = lateReward == TypeReward.Skin ? true : lateReward == TypeReward.Wheels;
			CreateSkin (ob, b);
			lateReward = TypeReward.Wheels;
			break;
		case TypeReward.Health:
			b = lateReward == TypeReward.Health ? true : lateReward == TypeReward.Shield;
			ItemBoot hp = new ItemBoot ();
			hp.bootType = BootType.Health;
			hp._valueBoot = (int)ob;
			CreateBoot (hp, b);
			lateReward = TypeReward.Health;
			break;
		case TypeReward.Shield:
			b = lateReward == TypeReward.Health ? true : lateReward == TypeReward.Shield;
			ItemBoot se = new ItemBoot ();
			se.bootType = BootType.Shield;
			se._valueBoot = (int)ob;
			CreateBoot (se, b);
			lateReward = TypeReward.Shield;
			break;
		}
	}

	void ClickButton ()
	{
		AllInOne.Instance.ShowVideoReward (CallbackVideo,"RewardVideo",LevelData.IDLevel);
		btnVideo.Block (true);
	}

	void CallbackVideo (bool b)
	{
		if (b) {
			VideoRewardManager.Instance.OnCompleteVideo (indexReward);
			rectDone.gameObject.SetActive (VideoRewardManager.Instance.Done (indexReward));
			rectLock.gameObject.SetActive (VideoRewardManager.Instance.Availaible (indexReward));
		} else {
			ActiveReward ();
		}
	}
}
