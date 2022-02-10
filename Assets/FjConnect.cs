using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FjConnect : TypeJoint
{
	[SerializeField]FixedJoint2D fj2d = null;
	public override void Active (bool b = true)
	{
		fj2d.enabled = b;
	}
	public override void Equip (Rigidbody2D rg2d)
	{
		fj2d.connectedBody = rg2d;
	}
}
