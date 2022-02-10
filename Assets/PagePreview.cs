using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PagePreview : MonoBehaviour
{
	[SerializeField]ButtonChoose[] btns;
	[SerializeField]Mover mover = null;
	int currentID = 0;
	[Header ("Animation")]
	[SerializeField]float fSpeed = 0;
	[SerializeField]Vector3 localTarget = Vector3.zero;
	Vector3 originalPos = Vector3.zero;
	[SerializeField]Transform posTarget = null;

	void Show (bool b)
	{
		if (b) {
			this.transform.localPosition = localTarget;
			this.gameObject.SetActive (true);
			mover._Move (originalPos, fSpeed, OnShow);
		} else {
			this.transform.localPosition = originalPos;
			mover._Move (localTarget, fSpeed, OnHide);
		}
		this.gameObject.SetActive (b);
	}

	void OnShow ()
	{
	}

	void OnHide ()
	{
		this.gameObject.SetActive (false);	
	}

	void Start ()
	{
		originalPos = this.posTarget.localPosition;
		for (int i = 0; i < btns.Length; i++) {
			btns [i].Setup (i, Choose);
		}
		OffAll (null);
		currentID = 0;
		btns [currentID].Select (true);
		//if (upgradeType == UpgradeType.Health) {
		EquipCantro.Instance.RegisterPageShow (Show, OffAll);
		EquipCantro.Instance.Show (currentID);
//		} else if (upgradeType == UpgradeType.Speed) {
//			//BikeCantro.Instance.Show (currentID);
//			//BikeCantro.Instance.RegisterPageShow (Show);
//		} else if (upgradeType == UpgradeType.ATK) {
//			//
//		}
//		gameObject.SetActive (false);
	}

	void Choose (int id)
	{
		btns [currentID].Select (false);
		EquipCantro.Instance.Hide (currentID);
		currentID = id;
		btns [currentID].Select (true);
//		if (upgradeType == UpgradeType.Health) {
		EquipCantro.Instance.Show (currentID);
		SkinEditor.Instance.ClearPreview ();
//		} else if (upgradeType == UpgradeType.Speed) {
//			//BikeCantro.Instance.Show (currentID);
//		} else if (upgradeType == UpgradeType.ATK) {
//			//
//		}
		UpgradeType upgradeType = id == 3 ? UpgradeType.Speed : UpgradeType.Health;
		UpgradeManager.Instance.Preview (upgradeType, 0, false);
	}

	void OffAll (object ob)
	{
		for (int i = 0; i < btns.Length; i++) {
			btns [i].Select (false);
		}
	}
}
