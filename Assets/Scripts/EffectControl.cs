using UnityEngine;

public abstract class EffectControl : MonoBehaviour
{
	public abstract void Active (bool b = false);

	public abstract void SetColor (Color color);
}
