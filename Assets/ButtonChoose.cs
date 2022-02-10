using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
public class ButtonChoose : MonoBehaviour,IPointerDownHandler
{
	public int index = 0;
	Action<int> _Choose = null;
	bool bBlock = false;
	[SerializeField]Transform[] rects = new Transform[2];
	public void Setup (int id, Action<int> a)
	{
		_Choose = a;
		index = id;
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		if (bBlock)
			return;
		Choose ();
	}

	public void Choose ()
	{
		_Choose.Invoke (index);
		
	}

	public void Select (bool b)
	{
		rects [0].gameObject.SetActive (!b);
		rects [1].gameObject.SetActive (b);
	}
}
