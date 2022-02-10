using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider2DManager : MonoBehaviour
{
	[SerializeField]Collider2D[] colls;

	public void InitCollider2D ()
	{
		colls = GetComponentsInChildren<Collider2D> ();
	}

	public void ChangeLayer (int l)
	{
		for (int i = 0; i < colls.Length; i++) {
			colls [i].gameObject.layer = l;
		}
	}

	public void Active (bool b)
	{
		for (int i = 0; i < colls.Length; i++) {
			colls [i].enabled = b;
		}
	}

}
