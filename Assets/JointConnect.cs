using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointConnect : MonoBehaviour
{
	[SerializeField]Rigidbody2D rgMain = null;
	[SerializeField]FixedJoint2D fj2d = null;
	[SerializeField]HingeJoint2D hj2d = null;
	[SerializeField]Transform centerOfMass = null;
	[SerializeField]float fSpeedRo = 10;
	[SerializeField]JointLimit limitNormal = new JointLimit ();
	[SerializeField]JointLimit limitAngle = new JointLimit ();
	[SerializeField]JointLimit limitJump = new JointLimit ();
	[SerializeField]JointLimit limitJumpH = new JointLimit ();
	[SerializeField]JointLimit limitDeath = new JointLimit ();
	[SerializeField]WheelConnect wheelConnect = null;
	public float originalAngle = 0;
	bool Idle = false;
	JointAngleLimits2D jointAngleLimits2d = new JointAngleLimits2D ();
	Vector3 localPos;
	Quaternion localRot;

	public void LimitHit ()
	{
		fj2d.enabled = false;
		ChangeLimit (limitAngle);
	}

	public void OffHit ()
	{
		ChangeLimit (limitNormal);
		fj2d.enabled = true;
	}

	public void StopAll ()
	{
		rgMain.velocity = Vector2.zero;
		rgMain.angularVelocity = 0;
		Idle = false;
		SyncIdle ();
	}

	public void Reset ()
	{
		rgMain.transform.localPosition = localPos;
		rgMain.transform.localRotation = localRot;
		wheelConnect.Reset ();
	}

	void Start ()
	{
		rgMain.centerOfMass = centerOfMass.localPosition;
		originalAngle = rgMain.rotation;
		UnIdle (true);
		localPos = rgMain.transform.localPosition;
		localRot = rgMain.transform.localRotation;
	}

	public void SyncRo (float angle)
	{
		UnIdle (true);
		float _ro = rgMain.rotation;
		_ro = Mathf.Lerp (_ro, angle, fSpeedRo * Time.deltaTime);
		rgMain.MoveRotation (_ro);
	}

	public void SyncRo360 (float angle)
	{
		ChangeLimit (limitDeath);
		float _ro = rgMain.rotation;
		_ro = Mathf.Lerp (_ro, angle, 2F * fSpeedRo * Time.deltaTime);
		rgMain.MoveRotation (_ro);
	}

	public void SyncRoRight (float angle)
	{
		UnIdle (false);
		float _ro = rgMain.rotation;
		_ro = Mathf.Lerp (_ro, angle, fSpeedRo * Time.deltaTime);
		rgMain.MoveRotation (_ro);
	}

	public void SyncIdle ()
	{
		if (Idle)
			return;
		Idle = true;
		fj2d.enabled = true;
		ChangeLimit (limitNormal);
		wheelConnect.On ();

	}

	void UnIdle (bool bOff)
	{
		if (!Idle)
			return;
		Idle = false;
		fj2d.enabled = false;
		if (bOff)
			wheelConnect.Off ();
		ChangeLimit (limitAngle);
	}

	void ChangeLimit (JointLimit limit)
	{
		jointAngleLimits2d.min = limit.min;
		jointAngleLimits2d.max = limit.max;
		hj2d.limits = jointAngleLimits2d;
	}

	void OnWheel (bool b)
	{
		if (!Idle)
			return;
		if (b)
			wheelConnect.On ();
		else {
			wheelConnect.Off ();
		}
	}

	public void Jump ()
	{
		fj2d.enabled = false;
		ChangeLimit (limitJump);
		OnWheel (false);
	}

	public void JumpDone ()
	{
		ChangeLimit (limitJumpH);
	}

	public void EndJump ()
	{
		fj2d.enabled = true;
		ChangeLimit (limitNormal);
		OnWheel (true);
	}

	public void ToEnd ()
	{
		fj2d.enabled = false;
		ChangeLimit (limitDeath);
		OnWheel (false);
	}
}
