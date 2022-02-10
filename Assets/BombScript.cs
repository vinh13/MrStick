using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : Bullet
{

	[SerializeField]ObjectHit objectHit = null;
	[SerializeField]Rigidbody2D rg2d = null;
	[SerializeField]Collider2DManager colls = null;
	[SerializeField]float ratioDame = 10;
	bool ofP = false;
	bool bHit = false;

	#region override

	float damageBase = 0;

	public override void Init ()
	{
		objectHit.Register (OnHit, TypeHit.Weapon);
		colls.InitCollider2D ();
	}

	public override void Attack (Vector2 vel, bool isP = false, float damage = 0)
	{
		bGround = false;
		bHit = false;
		damageBase = damage;
		objectHit.damage = damage * ratioDame;
		ofP = isP;
		if (isP) {
			colls.ChangeLayer (LayerConfig._layerPWP);
		} else {
			colls.ChangeLayer (LayerConfig._layerEWP);
		}
		rg2d.velocity = new Vector2 (vel.x, -15F);
		SFXManager.Instance.Play (TypeSFX.rocket2);
	}

	public override void Active (bool b = false)
	{
		gameObject.SetActive (b);
	}


	public override void Setup (float angle = 0)
	{
		throw new System.NotImplementedException ();
	}

	#endregion

	void OnHit (object ob = null)
	{
		if (bHit)
			return;
		bHit = true;
		Effect ef = BulletContainer.Instance.HitBomb;
		ef.Active (true);
		ef.Attack ((Vector2)transform.position, ofP, damageBase);
		Disable ();
		if (CameraControl.Instance.CheckObjectInViewport (this.transform.position)) {
			SFXManager.Instance.Play ("explosion2");
		}
	}

	bool bGround = false;

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (bGround || bHit)
			return;
		if (coll.collider.CompareTag ("ground")) {
			bGround = true;
			if (this.enabled)
				StartCoroutine (waitHit ());
		}
		if (!ofP) {
			if (coll.collider.CompareTag ("shield")) {
				colls.ChangeLayer (LayerConfig._layerPWP);
			}
			ofP = true;
		}
	}

	void OnTriggerExit2D (Collider2D coll)
	{
		if (coll.CompareTag ("OutRange")) {
			Disable ();
		}
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.CompareTag ("shield")) {
			OnHit ();
		}
	}

	void Disable ()
	{
		this.Active (false);
	}

	IEnumerator waitHit ()
	{
		yield return new WaitForSeconds (GameConfig.durationBombExp);
		OnHit ();
	}

	void OnDisable ()
	{

		StopAllCoroutines ();
	}


}
