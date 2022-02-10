using UnityEngine;
using System;

public abstract class Weapon : MonoBehaviour
{
	public abstract void RegisterOnHit (Action a = null);

	public abstract void Equip (Rigidbody2D rg2d = null, bool bPlayer = false, float dame = 0);

	public abstract void UnEquip (bool b);

	public abstract void Active (bool b = false);

	public abstract void Init (Transform t = null);

	public abstract void SyncRotation (float angle);
}
