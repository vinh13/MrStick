using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bike : MonoBehaviour
{
	[SerializeField]SpriteRenderer[] sprs;
	#if UNITY_EDITOR
	void OnValidate ()
	{
		sprs = GetComponentsInChildren<SpriteRenderer> ();
	}
	#endif
	public void Change (string sName)
	{
		for (int i = 0; i < sprs.Length; i++) {
			sprs [i].sortingLayerName = sName;
		}
	}

	public void OnOff (bool b)
	{
		for (int i = 0; i < sprs.Length; i++) {
			sprs [i].enabled = b;
		}
	}
}
