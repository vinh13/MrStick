using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : Bullet
{
	[SerializeField]ObjectHit objectHit = null;
	[SerializeField]Rigidbody2D rg2d = null;
	[SerializeField]Collider2DManager colls = null;
	[SerializeField]float ratioDame = 1F;
	[SerializeField]TypeBullet typeBullet = TypeBullet.None;
	bool ofP = false;

	#region override

	public override void Init ()
	{
		objectHit.Register (OnHit, TypeHit.Weapon);
		objectHit.hiObject = HitObject.Bullet;
		colls.InitCollider2D ();
	}

	public override void Setup (float angle = 0)
	{
	}

	public override void Attack (Vector2 vel, bool isP = false, float damage = 0)
	{
		objectHit.damage = damage * ratioDame;
		ofP = isP;
		if (isP) {
			colls.ChangeLayer (LayerConfig._layerPWP);
			rg2d.velocity = vel;
		} else {
			colls.ChangeLayer (LayerConfig._layerEWP);
			rg2d.velocity = vel;
		}
	}

	public override void Active (bool b = false)
	{
		gameObject.SetActive (b);
	}

	#endregion

	void OnHit (object ob = null)
	{
		EffectFollowManager.Instance.PlayeEffectBulletHit (typeBullet, transform.position);
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
}
