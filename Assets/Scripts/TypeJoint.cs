using UnityEngine;

public abstract class TypeJoint : MonoBehaviour
{
	public abstract void Active (bool b = true);

	public abstract void Equip (Rigidbody2D rg2d = null);
}
