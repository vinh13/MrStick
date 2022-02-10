using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EquipCantro : MonoBehaviour
{
	public static EquipCantro Instance = null;
	[SerializeField]UIButton btnInfo = null, btnHideInventory = null;
	[SerializeField]EquipBoss[] equips;
	[SerializeField]AllSkinEquipManager allManager = null;
	[SerializeField]UIInventoryInfo uiInfo = null;
	Action<bool> pageShow = null;
	Action<object> OffAll = null;
	bool bShowAll = false;
	[SerializeField]BuySkin buySkin = null;
	[SerializeField]int[] pricesGem;

	void Awake ()
	{
		if (Instance == null)
			Instance = this;
		btnInfo.Register (ShowInfo);
		btnHideInventory.Register (HideInventory);
		buySkin.Register (BuySkin, TryKin);
	}

	void BuySkin (bool b)
	{
		if (b) {
			for (int j = 0; j < 3; j++) {
				equips [j].UnlockSkin (idSkinBuy);
			}
		}
	}

	void TryKin (bool b)
	{
		
	}

	void ShowInfo ()
	{
		uiInfo.Show ();
	}

	void HideInventory ()
	{
		buySkin.Show (false);
		HomeManager.Instance.HideInventory ();
	}

	public void ShowAllEquip ()
	{
		if (bShowAll)
			return;
		bShowAll = true;
		OffAll.Invoke (true);
		for (int i = 0; i < equips.Length; i++) {
			Hide (i);
		}
		GroupAll ();
	}

	void GroupAll ()
	{
		for (int i = 1; i <= allManager.numberSkill; i++) {
			for (int j = 0; j < 3; j++) {
				equips [j].SetParent (allManager.GetParent (i), i);
			}
		}
	}

	public void ShowSkin (int idSkin)
	{
		bool bShow = false;
		int count = 0;
		for (int j = 0; j < 3; j++) {
			bool b = equips [j].ShowSkin (idSkin);
			if (!b) {
				count++;
				if (!bShow)
					bShow = true;
			}
		}
		ShowBuy (bShow, count, idSkin);
	}

	int idSkinBuy = 0;

	void ShowBuy (bool bShow, int count, int idSkin)
	{
		
		if (count == 0) {
			buySkin.Show (false);
		} else {
			buySkin.Show (bShow);
			idSkinBuy = idSkin;
			buySkin.Refesh (count * pricesGem [idSkinBuy], !bShow);
		}
	}

	public void RegisterPageShow (Action<bool> a, Action<object> b)
	{
		pageShow = a;
		OffAll = b;
	}

	public void Show (int index)
	{
		equips [index].ShowPanel (true);
		if (!bShowAll) {
			return;
		}
		bShowAll = false;
		allManager.Reset ();
		Reset ();
	}

	void Reset ()
	{
		for (int i = 1; i <= allManager.numberSkill; i++) {
			for (int j = 0; j < 3; j++) {
				equips [j].Reset (i);
			}
		}
	}

	public void Hide (int index)
	{
		equips [index].ShowPanel (false);
	}

	public int GetLevelBonusHealth ()
	{
		int level = 0;
		for (int i = 0; i < 3; i++) {
			level += equips [i].LevelBonus;
		}
		return level;
	}

	public int GetLevelBonusHealthPreview ()
	{
		int level = 0;
		for (int i = 0; i < 3; i++) {
			level += equips [i].LevelBonusPreview;
		}
		return level;
	}


	public int GetLevelBonusSpeed ()
	{
		return equips [3].LevelBonus;
	}

	public int GetLevelBonusSpeedPreview ()
	{
		return equips [3].LevelBonusPreview;
	}

	bool bFirst = false;

	public void ShowPage (bool b)
	{
		pageShow.Invoke (b);
		if (!bFirst) {
			equips [0].ShowPanel (true);
			bFirst = true;
		}
	}
}
