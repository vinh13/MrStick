using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCastCheck : MonoBehaviour
{
	int[] maskChecks;
	[SerializeField]Transform _s = null, _e = null;
	bool bRegistered = false;

	public void RegisterCheck (int[] masks)
	{
		if (!bRegistered)
			bRegistered = true;
		maskChecks = masks;
	}

	public bool Check ()
	{
		if (!bRegistered)
			return false;
		bool b = false;
		Debug.DrawLine (_s.position, _e.position, Color.red);
		for (int i = 0; i < maskChecks.Length; i++) {
			b = Physics2D.Linecast (_s.position, _e.position, 1 << maskChecks [i]);
			if (b)
				break;
		}
		return b;
	}

	public void Resize (float range)
	{
		Vector3 localPos = _s.localPosition;
		localPos.x = range;
		localPos.y = 0;
		_e.localPosition = localPos;
	}
}
