using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipAll : MonoBehaviour
{
	[SerializeField]ButtonEquipAll[] btns;
	[SerializeField]Transform parent = null;
	[SerializeField]EquipLevel[] levels;
	[SerializeField]int[] ids;
	[SerializeField]Sprite[] sprs;

	void OnValidate ()
	{
		btns = parent.GetComponentsInChildren<ButtonEquipAll> ();
	}

	void Awake ()
	{
		for (int i = 0; i < btns.Length; i++) {
			btns [i].Register (i, Choose, sprs [i], levels [i]);
		}
	}

	void Choose (int id)
	{
		EquipCantro.Instance.ShowSkin (ids [id]);	
	}
}
