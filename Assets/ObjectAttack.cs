using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ObjectAttack : MonoBehaviour
{
	public abstract void Attack ();

	public abstract void RegisterAttack (Action<float> a = null);

	public abstract void Setup (TypeCharacter typeChar, float dame = 0);
}
