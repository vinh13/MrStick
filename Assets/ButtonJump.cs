using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonJump :  MonoBehaviour,IPointerDownHandler
{
	[SerializeField]Image imgClick = null;

	public void OnPointerDown (PointerEventData eventData)
	{
		//Block (true);
		PlayerControl.Instance.Jump (UnLock);
	}

	void UnLock ()
	{
		//Block (false);
	}

	void Block (bool b)
	{
		imgClick.raycastTarget = !b;
	}
}
