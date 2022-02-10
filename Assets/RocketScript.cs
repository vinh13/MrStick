using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : Bullet
{
	[SerializeField]ObjectHit objectHit = null;
	[SerializeField]Rigidbody2D rg2d = null;
	[SerializeField]Collider2DManager colls = null;
	[SerializeField]float ratioDame = 1F;
	bool bParabola = false;
	bool ofP = false;
	float damageBase = 0;

	#region override

	public override void Init ()
	{
		objectHit.Register (OnHit, TypeHit.Weapon);
		colls.InitCollider2D ();
	}

	public override void Setup (float angle = 0)
	{
		transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
	}

	public override void Attack (Vector2 vel, bool isP = false, float damage = 0)
	{
		damageBase = damage;
		objectHit.damage = damage * ratioDame;
		ofP = isP;
		if (isP) {
			colls.ChangeLayer (LayerConfig._layerPWP);
		} else {
			colls.ChangeLayer (LayerConfig._layerEWP);

		}
		rg2d.velocity = vel;
		bParabola = true;

		SFXManager.Instance.Play (TypeSFX.rocket1);
	}

	public override void Active (bool b = false)
	{
		gameObject.SetActive (b);
	}

	#endregion

	void OnDisable ()
	{
		StopAllCoroutines ();
	}

	void OnHit (object ob = null)
	{
		Effect ef = BulletContainer.Instance.HitRocket;
		ef.Active (true);
		ef.Attack ((Vector2)transform.position, ofP, damageBase);
		SFXManager.Instance.Play ("explosion1");
		bParabola = false;
		gameObject.SetActive (false);
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.collider.CompareTag ("ground")) {
			OnHit ();
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
			OnHit ();
		}
	}

	void Update ()
	{
		if (!bParabola)
			return;
		Vector2 vel = rg2d.velocity;
		float angle = Mathf.Atan2 (vel.y, vel.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Lerp (transform.rotation,
			Quaternion.AngleAxis (angle, Vector3.forward),
			10F * Time.deltaTime
		);
	}
}
