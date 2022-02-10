using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ILevel : MonoBehaviour
{
	public abstract bool Setup (int id, string idLevel, Action<int> a);

	public abstract bool Unlocked ();

	public abstract void Select ();

	public abstract int GetStar ();

	public abstract void ShowTarget ();

}
