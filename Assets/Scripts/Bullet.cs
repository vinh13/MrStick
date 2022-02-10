using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
	public abstract void Attack (Vector2 vel, bool isP = false, float damage = 0);

	public abstract void Active (bool b = false);

	public abstract void Init ();

	public abstract void Setup (float angle = 0);

}
