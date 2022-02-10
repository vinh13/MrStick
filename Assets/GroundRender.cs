using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRender : MonoBehaviour
{
	//	[SerializeField]SpriteRenderer[] sprs = null;
	//	bool bOn = false;
	//	#if UNITY_EDITOR
	//	void Awake ()
	//	{
	//		bOn = false;
	//		On (bOn);
	//	}
	//	public void _OnValidate ()
	//	{
	//		sprs = GetComponentsInChildren<SpriteRenderer> ();
	//	}
	//	#endif
	//	void On (bool b)
	//	{
	//		for (int i = 0; i < sprs.Length; i++) {
	//			sprs [i].enabled = b;
	//		}
	//	}
	//	void OnTriggerStay2D (Collider2D coll)
	//	{
	//		if (bOn)
	//			return;
	//		if (coll.CompareTag ("OutRange")) {
	//			bOn = true;
	//			On (bOn);
	//		}
	//	}
	//
	//	void OnTriggerExit2D (Collider2D coll)
	//	{
	//		if (!bOn)
	//			return;
	//		if (coll.CompareTag ("OutRange")) {
	//			bOn = false;
	//			On (bOn);
	//		}
	//	}
}
