using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollSkin : MonoBehaviour
{
	[SerializeField]Image imgPreview = null;
	[SerializeField]panelStars panelStar = null;

	public void SetSkin (EquipConfig equipConfig)
	{
		imgPreview.sprite = equipConfig.spr;
		panelStar.SetStar (equipConfig.level.GetHashCode ());
	}
}
