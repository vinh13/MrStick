using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllSkinEquipManager : MonoBehaviour
{
	[SerializeField]AllSkinEquip allSkinEquip = null;
	[SerializeField]public int numberSkill = 6;
	[SerializeField]ButtonChoose btnChoose = null;
	bool bShow = false;

	void ClickAll (int ID)
	{
		if (bShow)
			return;
		bShow = true;
		EquipCantro.Instance.ShowAllEquip ();
		btnChoose.Select (true);
		allSkinEquip.Show (true);
	}

	public void Reset ()
	{
		if (!bShow)
			return;
		bShow = false;
		btnChoose.Select (false);
		allSkinEquip.Show (false);
	}

	void Awake ()
	{
		allSkinEquip.RegisterRect (numberSkill);
		btnChoose.Setup (0, ClickAll);
	}

	public Transform GetParent (int index)
	{
		return allSkinEquip.GetParent (index);
	}
}
