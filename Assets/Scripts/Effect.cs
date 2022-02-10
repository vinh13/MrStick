using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : MonoBehaviour
{
	public abstract void Init ();

	public abstract void Active (bool b = false);

	public abstract void Attack (Vector2 pos, bool isP = false, float damage = 0);

}
