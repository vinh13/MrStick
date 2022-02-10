using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObFollow : MonoBehaviour
{
	public abstract void Active (bool b = false);

	public abstract void Break ();

	public abstract void SetUp (Transform parent, Transform sync);

	public abstract void Disable (float timer = 0);

	public abstract void SetColor (Color color);
}
