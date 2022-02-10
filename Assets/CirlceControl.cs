using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirlceControl : MonoBehaviour
{
	[SerializeField]TriggerCamera tA = null;
	[SerializeField]TriggerEnd tE = null;
	[SerializeField]Rigidbody2D[] rg2ds = null;
	[SerializeField]SpriteRenderer[] sprs;
	#if UNITY_EDITOR
	void OnValidate ()
	{
		sprs = GetComponentsInChildren<SpriteRenderer> ();
		rg2ds = GetComponentsInChildren<Rigidbody2D> ();

	}
	#endif
	void Start ()
	{
		tA.Register (CheckActive);
		tE.Register (CheckEnd);






		CheckEnd ();

	}

	void CheckActive ()
	{
		tA.Active (false);
		tE.Active (true);
		Sync (false);
	}

	void CheckEnd ()
	{
		tA.Active (true);
		tE.Active (false);
		Sync (true);
	}

	void SyncSprite (bool b)
	{
		for (int i = 0; i < sprs.Length; i++) {
			sprs [i].enabled = b;
		}
	}

	void Sync (bool b)
	{
		RigidbodyType2D type = b ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;
		for (int i = 1; i < rg2ds.Length - 1; i++) {
			rg2ds [i].bodyType = type;
		}
		SyncSprite (!b);
	}
}
