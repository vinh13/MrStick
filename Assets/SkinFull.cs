using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinFull : MonoBehaviour
{
	EquipConfig[] equipConfigs;
	[SerializeField]UIButton btnEquip = null;
	bool bClicked = false;

	void Start ()
	{
		bClicked = false;
		btnEquip.Register (Click);
	}

	void Click ()
	{
		//EquipSkin
		if (bClicked)
			return;
		bClicked = true;
		GifManager.Instance.EquipSkin (equipConfigs, true);
		btnEquip.Block (true);
	}

	void OnDisable ()
	{
		if (!bClicked) {
			GifManager.Instance.EquipSkin (equipConfigs, false);
		}
	}

	public void Register (object[] obs)
	{
		equipConfigs = new EquipConfig[obs.Length];
		for (int i = 0; i < obs.Length; i++) {
			equipConfigs [i] = (EquipConfig)obs [i];
		}
		UnlockSkin ();
	}

	public void Register (object ob)
	{
		equipConfigs = new EquipConfig[1];
		equipConfigs [0] = (EquipConfig)ob;
		UnlockSkin ();
	}

	void UnlockSkin ()
	{
		for (int i = 0; i < equipConfigs.Length; i++) {
			string key = EquipData.GetKey (equipConfigs [i].equipType);
			bool b = EquipData.GetUnlocked (key + "_" + equipConfigs [i].ID);
			if (!b) {
				EquipData.SetUnlocked (key + "_" + equipConfigs [i].ID, true);
			}
			int count = EquipData.GetCount (key + "_" + equipConfigs [i].ID);
			count += 1;
			EquipData.SetCount (key + "_" + equipConfigs [i].ID, count);
		}
	}
}
