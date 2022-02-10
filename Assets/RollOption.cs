using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollOption : MonoBehaviour
{
	GameObject currentGif = null;
	TypeReward lateReward = TypeReward.None;

	public void LoadReward (object ob, int ID, TypeReward t)
	{
		CreateReward (ob, t);
	}

	void CreateReward (object ob, TypeReward typeReward)
	{
		bool b = false;
		switch (typeReward) {
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
		case TypeReward.Key:
			b = lateReward == TypeReward.Key;
			CreateGem (ob, b);
			lateReward = TypeReward.Key;
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

	void CreateGem (object ob, bool b)
	{
		if (!b) {
			if (currentGif != null)
				Destroy (currentGif);
			GameObject go = Instantiate (Resources.Load<GameObject> ("Chest/Gem"), transform);
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
			GameObject go = Instantiate (Resources.Load<GameObject> ("Chest/Key"), transform);
			go.GetComponent<RollGold> ().SetGold ((string)ob);
			go.gameObject.SetActive (true);
			currentGif = go;
		} else {
			currentGif.GetComponent<RollGold> ().SetGold ((string)ob);
		}
	}


	void CreateGold (object ob, bool b)
	{
		if (!b) {
			if (currentGif != null)
				Destroy (currentGif);
			GameObject go = Instantiate (Resources.Load<GameObject> ("Chest/Gold"), transform);
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
			GameObject go = Instantiate (Resources.Load<GameObject> ("Chest/Skin"), transform);
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
			GameObject go = Instantiate (Resources.Load<GameObject> ("Roll/Item"), transform);
			go.GetComponent<RollItem> ().SetItem (temp._valueBoot, temp.bootType);
			go.gameObject.SetActive (true);
			currentGif = go;
		} else {
			currentGif.GetComponent<RollItem> ().SetItem (temp._valueBoot, temp.bootType);
		}
	}
}