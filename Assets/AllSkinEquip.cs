using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllSkinEquip : MonoBehaviour
{
	[SerializeField]Transform[] rects;
	[SerializeField]Transform view = null;
	bool bDragging = false;
	[SerializeField]Vector2 dirMove = Vector2.zero;
	[SerializeField]float fSpeed = 0;
	[SerializeField]Transform posSync = null;
	[SerializeField]float limitMin = 0, limitMax = 450F;

	public void RegisterRect (int number)
	{
		dirMove = rects [2].localPosition - rects [1].localPosition;
		dirMove.Normalize ();
		this.gameObject.SetActive (false);
	}

	public Transform GetParent (int index)
	{
		return rects [index - 1];
	}

	public void Show (bool b)
	{
		this.gameObject.SetActive (b);
	}

	float lateY = 0;

	public void UpdateNow ()
	{
		float y = posSync.localPosition.y;
		if (Mathf.Abs (lateY - y) < 0.1F) {
			return;
		}
		int dir = (y - lateY) > 0 ? 1 : -1;
		Vector2 pos = view.localPosition;
		pos += dir * fSpeed * Time.deltaTime * dirMove;
		lateY = y;
		if (pos.y < limitMax && pos.y > limitMin) {
			view.localPosition = pos;
		}
	}
}
