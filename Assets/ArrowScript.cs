using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript :  Bullet
{
	[SerializeField]ObjectHit objectHit = null;
	[SerializeField]Rigidbody2D rg2d = null;
	[SerializeField]Collider2DManager colls = null;
	bool bParabola = false;
	bool ofP = false;
	bool bCreateHit = false;
	[SerializeField]float ratioDame = 1F;

	#region override

	public override void Init ()
	{
		objectHit.Register (OnHit, TypeHit.Weapon);
		objectHit.hiObject = HitObject.Bullet;
		colls.InitCollider2D ();
	}

	public override void Setup (float angle = 0)
	{
		transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
	}

	public override void Attack (Vector2 vel, bool isP = false, float damage = 0)
	{
		objectHit.damage = damage * ratioDame;
		bCreateHit = false;
		ofP = isP;
		if (isP) {
			colls.ChangeLayer (LayerConfig._layerPWP);
		} else {
			colls.ChangeLayer (LayerConfig._layerEWP);

		}
		SFXManager.Instance.Play (TypeSFX.bow);
		bParabola = true;
		rg2d.velocity = vel;
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
		if (bCreateHit)
			return;
		if (ob != null) {
			ObFollow go = BulletContainer.Instance.ArrowHit;
			ObFollowContainer obf = (ObFollowContainer)ob;
			obf.AddObject (go.Break);
			go.SetUp (obf.transform, this.transform);
			go.SetColor (obf.colorHit);
			go.Active (true);
			go.Disable (10F);
		}
		bParabola = false;
		this.gameObject.SetActive (false);
		bCreateHit = true;
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.collider.CompareTag ("ground")) {
			ObFollow go = BulletContainer.Instance.ArrowHit;
			transform.rotation = Quaternion.AngleAxis (Random.Range (-15, -25), Vector3.forward);
			go.SetUp (coll.transform, this.transform);
			go.Active (true);
			go.SetColor (Color.white);
			go.Disable (5F);
			gameObject.SetActive (false);
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
			gameObject.SetActive (false);
		}
	}

	void Update ()
	{
		if (!bParabola)
			return;
		Vector2 vel = rg2d.velocity;
		float angle = Mathf.Atan2 (vel.y, vel.x) * Mathf.Rad2Deg;
		vel.y -= 0.1F;
		rg2d.velocity = vel;
		transform.rotation = Quaternion.Lerp (transform.rotation,
			Quaternion.AngleAxis (angle, Vector3.forward),
			10F * Time.deltaTime
		);
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.CompareTag ("shield")) {
			OnHit (null);
		}
	}
}
