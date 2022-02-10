using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ScrollViewTdz : MonoBehaviour
{
	[SerializeField]Transform posSync = null, view = null;
	[SerializeField]Transform[] dirs = new Transform[2];
	Vector2 dirMove = Vector2.zero;
	[SerializeField]float fSpeed = 5000;
	float lateY = 0;
	[SerializeField]float limitMin = 0, limitMax = 0;
	void Start ()
	{
		dirMove = dirs [1].localPosition - dirs [0].localPosition;
		dirMove.Normalize ();
	}

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
