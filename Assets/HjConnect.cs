using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HjConnect : TypeJoint
{
	[SerializeField]HingeJoint2D hj2d = null;
	public override void Active (bool b = true)
	{
		hj2d.enabled = b;
	}
	public override void Equip (Rigidbody2D rg2d)
	{
		
	}
}
