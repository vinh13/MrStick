using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;
public class HandControl : MonoBehaviour
{
	[SerializeField]TypeJoint jointHandMain = null;
	[SerializeField]TypeJoint fjArm = null;
	[SerializeField]Transform tWeapon = null;
	[SerializeField]Transform tArm = null, tHand = null;
	//Action resetWp = null;

	void Awake ()
	{
	}

//	public void RegisterReset (Action a)
//	{
//		resetWp = a;
//	}
//
	public void AddWeapon ()
	{
		
	}

	public void RemoveWeapon ()
	{
		
	}
//	void Update ()
//	{
//		if (Input.GetKey (KeyCode.H)) {
//			JointVehicle ();
//		}
//		if (Input.GetKey (KeyCode.J)) {
//			UnJointVehicle ();
//		}
//	}
//
//	void JointVehicle ()
//	{
//		resetWp.Invoke ();
//		jointHandMain.Active (true);
//		fjArm.Active (false);
//	}
//
//	void UnJointVehicle ()
//	{
//		jointHandMain.Active (false);
//		fjArm.Active (true);
//	}
}
