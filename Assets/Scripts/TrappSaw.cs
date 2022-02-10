using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class TrappSaw : Trapp
{
	[SerializeField]Collider2DManager colls = null;
	[SerializeField]ObjectHit objectHit = null;

	public override void Active (bool b)
	{
		this.gameObject.SetActive (b);
	}

	void OnHit (object ob = null)
	{
		colls.Active (false);
		TaskUtil.Schedule (this, this._OnHit, 0.25F);
	}

	void Start ()
	{
		colls.InitCollider2D ();
		colls.Active (true);
		colls.ChangeLayer (11);
		objectHit.Register (OnHit, TypeHit.Object);
		objectHit.damage = TrappConfig.dameSaw;
		objectHit.typeHit = TypeHit.Trapp;
	}

	void _OnHit ()
	{
		colls.Active (true);
		objectHit.hited = false;
	}
}
