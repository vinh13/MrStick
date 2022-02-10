using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public enum EquipType
{
	None = 0,
	Head = 1,
	Body = 2,
	Arm = 3,
	Wheels = 4,
}

[System.Serializable]
public enum EquipLevel
{
	None = 0,
	Level1 = 1,
	Level2 = 2,
	Level3 = 3,
	Level4 = 4,
	Level5 = 5,
	Level6 = 6,
}

[System.Serializable]
public class GiftEquip
{
	public int ID = 0;
	public bool bPreview = false;
}

[System.Serializable]
public class EquipBoss : MonoBehaviour
{

	public EquipConfig[] SaveData ()
	{
		List<EquipConfig> list = new List<EquipConfig> ();
		for (int i = 1; i < idItem.Length; i++) {
			string path = "SkinData/" + idItem [i] + "/" + equipType.ToString ();
			EquipConfig equipConfig = Resources.Load<EquipConfig> (path);
			equipConfig.equipType = equipType;
			equipConfig.level = levels [i];
			equipConfig.price = prices [i];
			equipConfig.skinId = idItem [i];
			equipConfig.spr = sprs [i];
			equipConfig.ID = i;
			list.Add (equipConfig);
		}
		return list.ToArray ();
	}

	public SkinDataLevel[] SaveSkinDataLevel ()
	{
		List<SkinDataLevel> list = new List<SkinDataLevel> ();
		for (int i = 1; i < idItem.Length; i++) {
			string path = "PlayerSkin/SkinLevel" + levels [i].GetHashCode ();
			SkinDataLevel skinDataLevel = Resources.Load<SkinDataLevel> (path);
			List<string> datas = skinDataLevel.skinDatas;
			string sData = "";
			switch (equipType) {
			case EquipType.Head:
				sData = "Head_" + i + "_" + idItem [i];
				//datas.Add ("Head_" + idItem [i]);
				//skinDataLevel.headDatas = datas.ToArray ();
				break;
			case EquipType.Body:
				sData = "Body_" + i + "_" + idItem [i];
				//datas.Add ("Body_" + idItem [i]);
				//skinDataLevel.bodyDatas = datas.ToArray ();
				break;
			case EquipType.Arm:
				sData = "Arm_" + i + "_" + idItem [i];
				//datas.Add ("Arm_" + idItem [i]);
				//skinDataLevel.armDatas = datas.ToArray ();
				break;
			case EquipType.Wheels:
				sData = "Wheels_" + i + "_" + idItem [i];
				//datas.Add ("Wheels_" + idItem [i]);
				//skinDataLevel.wheelDatas = datas.ToArray ();
				break;
			}
			if (datas.Count == 0) {
				datas.Add (sData);
			} else {
				bool bCheck = false;
				for (int j = 0; j < datas.Count; j++) {
					if (datas [j] == sData) {
						bCheck = true;
						break;
					}
				}
				if (!bCheck) {
					datas.Add (sData);
				}
			}
			skinDataLevel.skinDatas = datas;
			list.Add (skinDataLevel);
		}
		return list.ToArray ();
	}

	void OnValidate ()
	{
		btns = panels.GetComponentsInChildren<ButtonEquip> ();
	}

	[SerializeField]UpgradeType upgradeType = UpgradeType.Health;
	[SerializeField]Transform panels = null;
	[SerializeField]EquipType equipType = EquipType.None;
	[SerializeField]ButtonEquip[] btns;
	[SerializeField]EquipLevel[] levels;
	[SerializeField]int[] prices;
	[SerializeField]Sprite[] sprs;
	[SerializeField]int[] idItem;
	bool[] bUnlockeds;
	int currentID = 0;
	int idPriview = 0;
	int levelBonus = 0;
	int levelBonusPreview = 0;

	public int LevelBonus {
		get {
			return levelBonus;
		}
	}

	public int LevelBonusPreview {
		get {
			return levelBonusPreview;
		}
	}


	void Awake ()
	{
		bUnlockeds = new bool[btns.Length];
		string key = EquipData.GetKey (equipType);
		bool b = upgradeType == UpgradeType.Speed;
		for (int i = 0; i < btns.Length; i++) {
			bUnlockeds [i] = btns [i].Register (i, Choose, key + "_" + i, prices [i], sprs [i], levels [i], b, equipType);
		}
		OffAll ();
		currentID = EquipData.GetEquipTypeCurrentID (equipType);	
		btns [currentID].Select (true);
		levelBonus = UpgradeData.GetLevelEquipTypeBonus (equipType);
	}

	void Start ()
	{
		GifManager.Instance.Register (equipType.ToString (), GifEquip);
		Invoke ("_Start", 0.2F);
	}

	void _Start ()
	{
		ShowPanel (false);
	}

	void GifEquip (object ob)
	{
		GiftEquip g = (GiftEquip)ob;
		int id = g.ID;
		btns [id].Unlock (g.bPreview);
	}

	public void UnlockSkin (int index)
	{
		btns [index].Unlock (true);
	}

	void Choose (int id, bool bUnlocked)
	{
		if (bUnlocked) {
			SkinData.bTry = false;
			btns [currentID].Select (false);
			currentID = id;
			EquipData.SetEquipTypeCurrentID (equipType, currentID);	
			btns [currentID].Select (true);
			levelBonus = levels [id].GetHashCode ();
			UpgradeData.SetLevelEquipTypeBonus (equipType, levelBonus);
			if (upgradeType == UpgradeType.Health) {
				SkinData.SuggestId = idItem [id];
				UpgradeManager.Instance.UpdateBonus (upgradeType, EquipCantro.Instance.GetLevelBonusHealth ());
				SkinEditor.Instance.Preview (equipType, levels [id], idItem [id], true);
			} else if (upgradeType == UpgradeType.Speed) {
				UpgradeManager.Instance.UpdateBonus (upgradeType, EquipCantro.Instance.GetLevelBonusSpeed ());
				BikeEditor.Instance.Preview (equipType, levels [id], idItem [id], true);
			} else {
				//ATK
			}
		} else {
			btns [idPriview].Preiview (false);
			idPriview = id;
			levelBonusPreview = levels [id].GetHashCode ();
			btns [idPriview].Preiview (true);
			if (upgradeType == UpgradeType.Health) {
				SkinData.SuggestId = idItem [id];
				SkinEditor.Instance.Preview (equipType, levels [id], idItem [id], false);
				int levelTotal = EquipCantro.Instance.GetLevelBonusHealthPreview ();
				levelTotal -= levelBonusPreview;
				UpgradeManager.Instance.Preview (upgradeType, levelTotal + levels [id].GetHashCode (), true);
			} else if (upgradeType == UpgradeType.Speed) {
				int levelTotal = EquipCantro.Instance.GetLevelBonusSpeedPreview ();
				levelTotal -= levelBonusPreview;
				BikeEditor.Instance.Preview (equipType, levels [id], idItem [id], false);
				UpgradeManager.Instance.Preview (upgradeType, levelTotal + levels [id].GetHashCode (), true);
			}
		}
	}

	void OffAll ()
	{
		for (int i = 0; i < btns.Length; i++) {
			btns [i].Select (false);
		}
	}

	public void ShowPanel (bool b)
	{
		panels.gameObject.SetActive (b);
	}

	void OnDisable ()
	{
		GifManager.Instance.Remove (equipType.ToString ());
	}

	public void SetParent (Transform parent, int index)
	{
		btns [index].transform.SetParent (parent);
		Vector3 localS = btns [index].transform.localScale;
		localS.x = 0.9F;
		localS.y = 0.9F;
		localS.z = 0.9F;
		btns [index].transform.localScale = localS;
	}

	public void Reset (int index)
	{
		btns [index].Reset ();
		Vector3 localS = new Vector3 (1, 1, 1);
		btns [index].transform.localScale = localS;
	}

	public bool GetUnlocked (int index)
	{
		return btns [index].GetUnlocked;
	}

	public bool ShowSkin (int index)
	{
		btns [index].ClickAll ();
		return GetUnlocked (index);
	}
}
