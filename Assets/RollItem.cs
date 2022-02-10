using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollItem : MonoBehaviour
{
	[SerializeField]Image[] imgs = new Image[2];
	[SerializeField]Text textCount = null;
	public void SetItem (int count, BootType bootType)
	{
		switch (bootType) {
		case BootType.Health:
			imgs [0].enabled = false;
			imgs [1].enabled = true;
			break;
		case BootType.Shield:
			imgs [0].enabled = true;
			imgs [1].enabled = false;
			break;
		}
		textCount.text = "x" + count;
	}
}
