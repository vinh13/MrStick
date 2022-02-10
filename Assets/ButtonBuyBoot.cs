using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ButtonBuyBoot : MonoBehaviour,IPointerDownHandler
{
	Action<TypePurchase> request = null;
	[SerializeField]Transform tBlock = null;
	bool bBlock = false;
	TypePurchase typePurchase = TypePurchase.None;

	public void Register (Action<TypePurchase> a, TypePurchase t)
	{
		typePurchase = t;
		request = a;
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		if (bBlock)
			return;
		request.Invoke (typePurchase);
	}

	public void Block (bool b)
	{
		tBlock.gameObject.SetActive (b);
		bBlock = b;
	}
}
