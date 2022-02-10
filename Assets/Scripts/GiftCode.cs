using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftCode : ScriptableObject
{
	public GifType giftType = GifType.None;
	public string data = "";
	public Object[] obs;
}
