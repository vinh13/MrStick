using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class RollScript : MonoBehaviour
{
	[SerializeField]int id = 0;
	[SerializeField]Mover mover = null;
	[SerializeField]Text text = null;
	[SerializeField]Scaler sacler = null;

	public int ID {
		get { 
			return id;
		}
		set {
			id = value;
			text.enabled 
			 = false;
			text.text = "" + id;
		}
	}

	public void Move (Vector3 localPos, float fSpeed, Action a)
	{
		mover._Move (localPos, fSpeed, a);
	}

	public void MoveNow (Vector3 localPos)
	{
		transform.localPosition = localPos;
	}

	public void _Scale (Vector3 localScale, float time)
	{
		sacler.Scale (localScale.x, time);
	}
}
