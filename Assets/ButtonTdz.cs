using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public abstract class ButtonTdz : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
	public void OnPointerDown (PointerEventData eventData)
	{
		Click ();
	}

	public void OnPointerUp (PointerEventData eventData)
	{
		UnClick ();
	}

	public abstract void Click ();

	public abstract void UnClick ();

	public abstract void RegisterClick (Action a, float timer = 0);

	public abstract void Block (bool b = false);

	public abstract void Active (bool b = false);

}

