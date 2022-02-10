using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollGold : MonoBehaviour
{
	[SerializeField]Text textCoin = null;
	public void SetGold (string text)
	{
		textCoin.text = text;
	}
}
