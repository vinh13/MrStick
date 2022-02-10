using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PartCollision : MonoBehaviour
{
	Action<CollisionLog> cb = null;
	TypeCharacter typeChar = TypeCharacter.None;
	RagdollStatus status = null;
	bool bBlocked = false;
	Action<Vector2> BreakJoint = null;
	bool bBreaked = false;
	ObFollowContainer followContainer = null;
	CollisionLog collLog = new CollisionLog ();
	Color _colorHit;

	public Action<Color> Setup (Action<Action> registerRemove, PartType _type, Action<CollisionLog> a, TypeCharacter t, RagdollStatus s, Action<Vector2> br)
	{
		cb = a;
		typeChar = t;
		status = s;
		BreakJoint = br;
		bBreaked = (br == null);
		followContainer = gameObject.AddComponent<ObFollowContainer> ();

		collLog.partType = _type;

		registerRemove.Invoke (followContainer.BreakObject);


		return SetColorHit;
	}

	void SetColorHit (Color color)
	{
		_colorHit = color;
		followContainer.colorHit = _colorHit;
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (!Logic.bReady)
			return;
		if (bBlocked)
			return;
		if (!status.bDeath) {
			int collLayer = coll.collider.gameObject.layer;
			if (LayerCollisionRagoll.checkHit (collLayer, typeChar)) {
				ObjectHit hit = coll.collider.transform.parent.GetComponent<ObjectHit> ();
				if (hit.hited)
					return;
				followContainer.bEnd = false;
				Vector3 point = coll.transform.position;
				if (coll.contacts.Length > 0)
					point = coll.contacts [0].point;
				bBlocked = true;
				EffectManager.Instance.Play_BloodA (transform.position, Vector3.up, _colorHit);
				hit.OnHit (followContainer);
				collLog.collLayer = collLayer;
				collLog.typeHit = hit.typeHit;
				collLog.posHit = point;
				collLog.damage = hit.damage;

				collLog.hitObject = hit.hiObject;

				cb.Invoke (collLog);
				TaskUtil.Schedule (this, Take, GameConfig.durationBlockDame);
			}
		} else {
			int collLayer = coll.collider.gameObject.layer;
			if (LayerCollisionRagoll.checkHit (collLayer, typeChar)) {

				ObjectHit hit = coll.collider.transform.parent.GetComponent<ObjectHit> ();
				if (hit.hited)
					return;
				followContainer.bEnd = true;
				if (!bBreaked) {
					bBreaked = true;
					Vector3 point = coll.transform.position;
					Vector2 dir = transform.position - point;
					BreakJoint.Invoke (dir.normalized);
				}
				bBlocked = true;


				hit.OnHit (followContainer);

				cb.Invoke (collLog);

				TaskUtil.Schedule (this, Take, GameConfig.durationBlockDame);
			}
		}
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (!Logic.bReady)
			return;
		if (bBlocked)
			return;
		if (!status.bDeath) {
			int collLayer = coll.gameObject.layer;
			if (LayerCollisionRagoll.checkHit (collLayer, typeChar)) {
				ObjectHit hit = coll.transform.parent.GetComponent<ObjectHit> ();
				if (hit == null)
					return;
				if (hit.hited)
					return;
				Vector3 point = coll.transform.position;
				bBlocked = true;
				EffectManager.Instance.Play_BloodA (transform.position, Vector3.up, _colorHit);
				hit.OnHit (followContainer);
				collLog.collLayer = collLayer;
				collLog.typeHit = hit.typeHit;
				collLog.posHit = point;
				collLog.damage = hit.damage;
				collLog.hitObject = hit.hiObject;

				cb.Invoke (collLog);
				TaskUtil.Schedule (this, Take, GameConfig.durationBlockDame);
			}
		}
	}

	void Take ()
	{
		bBlocked = false;
	}
}
