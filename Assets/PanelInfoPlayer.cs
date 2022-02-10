using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelInfoPlayer : MonoBehaviour
{
	[SerializeField]float range = 200;
	[SerializeField]Transform[] rects;
	[SerializeField]HPBarUI[] hpbars;
	[SerializeField]HPBarUI barPreview = null;
	[SerializeField]Transform rectPreview = null;
	public void SetValue (float[] r)
	{
		Vector3 localPos = rects [0].localPosition;
		localPos.x = 0;
		rects [0].localPosition = localPos;
		hpbars [0].Change (r [0]);

		localPos.x = r [0] * range;
		rects [1].localPosition = localPos;
		hpbars [1].Change (r [1]);

		localPos.x = (r [0] + r [1]) * range;
		rects [2].localPosition = localPos;
		hpbars [2].Change (r [2]);
	}

	public void Preview (float r0, float ratio, bool b)
	{
		barPreview.Active (b);
		Vector3 localPos = rects [0].localPosition;
		localPos.x = r0 * range;
		rectPreview.localPosition = localPos;
		barPreview.Change (ratio);

	}

	public void SyncReal (float r)
	{
		Vector3 localPos = rects [0].localPosition;
		localPos.x = 0;
		rects [0].localPosition = localPos;
		hpbars [0].Change (r);
	}

	public void SyncBounusI (float r, float r0)
	{
		Vector3 localPos = rects [0].localPosition;
		localPos.x = r0 * range;
		rects [1].localPosition = localPos;
		hpbars [1].Change (r);
	}

	public void SyncBounusII (float r)
	{
//		Vector3 localPos = rects [0].localPosition;
//		localPos.x = 0;
//		rects [0].localPosition = localPos;
//		hpbars [0].Change (r [0]);
	}

}
