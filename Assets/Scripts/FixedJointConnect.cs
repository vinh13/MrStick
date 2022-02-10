using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedJointConnect : MonoBehaviour
{
	[SerializeField]FixedJoint2D fjx2d = null;

	public void _Start (HingeJoint2D hj2d, float fre)
	{
		fjx2d = gameObject.AddComponent<FixedJoint2D> ();
		fjx2d.connectedBody = hj2d.connectedBody;
		fjx2d.frequency = fre;
	}

	public void Active (bool b)
	{
		fjx2d.enabled = b;
	}
}
