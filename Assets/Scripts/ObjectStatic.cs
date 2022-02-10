using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectStatic : Weapon
{
	[SerializeField]Rigidbody2D rg2d = null;
	[SerializeField]TypeJoint joint = null;
	[SerializeField]ObjectHit objectHit = null;
	[SerializeField]Collider2DManager coll2DMan = null;
	[SerializeField]float ratioDamage = 0.1F;
	[SerializeField]ObjectType objectType = ObjectType.None;
	Action OnAttack = null;
	Vector3 originalLocalPos = Vector3.zero;
	Quaternion originalLocalRot = Quaternion.Euler (Vector3.zero);
	bool bEquip = false;
	bool bHit = false;
	SpriteRenderer[] sprs;
	bool bShow = false;


	public override void SyncRotation (float angle)
	{
		//rg2d.MoveRotation (angle);
	}

	void Awake ()
	{
		sprs = this.GetComponentsInChildren<SpriteRenderer> ();
		bShow = false;
	}

	void OnHit (object ob = null)
	{
		if (bHit)
			return;
		ObFollowContainer of = (ObFollowContainer)ob;
		bHit = true;
		OnAttack.Invoke ();
		if (!of.bEnd)
			SFXManager.Instance.ObjectHit (objectType);
		TaskUtil.Schedule (this, _OnHit, 0.05F);
	}

	void _OnHit ()
	{
		bHit = false;
		objectHit.hited = false;
	}

	#region override

	public override void Init (Transform t = null)
	{
		transform.SetParent (t);
		objectHit.Register (OnHit, TypeHit.Weapon);
		coll2DMan.InitCollider2D ();
	}

	public override void Equip (Rigidbody2D rgConnect, bool bPlayer = false, float dame = 0)
	{
		objectHit.damage = dame * ratioDamage;
		bEquip = true;
		transform.localPosition = originalLocalPos;
		transform.localRotation = originalLocalRot;
		rg2d.bodyType = RigidbodyType2D.Dynamic;
		joint.Equip (rgConnect);
		joint.Active (true);
		if (bPlayer) {
			coll2DMan.ChangeLayer (17);
		} else {
			coll2DMan.ChangeLayer (16);
		}
		bShow = true;
		Show (true);
	}

	public override void UnEquip (bool b)
	{
		if (b) {
			joint.Active (false);
			coll2DMan.ChangeLayer (19);
			bEquip = false;
			transform.SetParent (null);
			Vector2 vel = rg2d.velocity;
			vel.x = -5;
			rg2d.velocity = vel;
		} else {
			joint.Active (false);
			coll2DMan.ChangeLayer (19);
			bEquip = false;
			transform.SetParent (null);
//			Vector2 vel = rg2d.velocity;
//			vel.x = -5;
//			rg2d.velocity = vel;
			gameObject.SetActive (false);
		}
	}

	public override void Active (bool b = false)
	{
		gameObject.SetActive (b);
	}

	public override void RegisterOnHit (Action a)
	{
		OnAttack = a;
	}

	#endregion

	#region Exit2D

	void OnTriggerExit2D (Collider2D coll)
	{
		if (bShow) {
			if (coll.CompareTag ("OutRange")) {
				bShow = false;
				Show (false);
			}
		}
		if (bEquip)
			return;
		if (coll.CompareTag ("OutRange")) {
			gameObject.SetActive (false);
		}
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (!bShow) {
			if (coll.CompareTag ("OutRange")) {
				bShow = true;
				Show (true);
			}
		}
	}

	void Show (bool b)
	{
		for (int i = 0; i < sprs.Length; i++) {
			sprs [i].enabled = b;
		}
	}


	#endregion
}
