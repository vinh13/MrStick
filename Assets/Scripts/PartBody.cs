using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class JointLimit
{
	public float min = 0;
	public float max = 0;
}

public class PartBody : MonoBehaviour
{
	[SerializeField]HingeJoint2D hj2d = null;
	[SerializeField]SpriteRenderer[] sprs;
	[SerializeField]Rigidbody2D rg2d = null;
	[SerializeField]Collider2D[] colls;
	bool bCanBreak = false;
	bool bDeath = false;
	Vector3 localPos = Vector3.zero;
	Quaternion localRo;
	List<Action> listRemoveGo = new List<Action> ();
	Color _colorHit;
	bool bSkined = false;
	bool bNeck = false;
	[SerializeField]int idLimit = 0;

	public void Skined (bool b, bool _bNeck)
	{
		bSkined = b;
		sprs [0].enabled = !b;
		if (name == "HEAD") {
			bNeck = _bNeck;
			sprs [sprs.Length - 1].enabled = !_bNeck;
		}
	}

	void RigesterOnDestroy (Action a)
	{
		listRemoveGo.Add (a);
	}

	#region Main

	#if UNITY_EDITOR
	void OnValidate ()
	{
		hj2d = GetComponent<HingeJoint2D> ();
		rg2d = GetComponent<Rigidbody2D> ();
		sprs = GetComponentsInChildren<SpriteRenderer> ();
		List<CapsuleCollider2D> collsTemp = new List<CapsuleCollider2D> ();
		for (int i = 0; i < sprs.Length; i++) {
			CapsuleCollider2D coll = sprs [i].GetComponent<CapsuleCollider2D> ();
			if (coll != null) {
				collsTemp.Add (coll);
			}
		}
		colls = collsTemp.ToArray ();
		for (int i = 0; i < colls.Length; i++) {
			colls [i].enabled = true;
		}

	}
	#endif
	#region Setup

	public void Setup (int partID, Color color, string slayerOder, int ilayerOder)
	{
		idLimit = partID;
		for (int i = 0; i < sprs.Length; i++) {
			sprs [i].sortingLayerName = slayerOder;
			if (!sprs [i].gameObject.CompareTag ("skin")) {
				sprs [i].sortingOrder = ilayerOder;
			} else {
				sprs [i].sortingOrder = ilayerOder + 2;
			}
			sprs [i].color = color;
		}
	}

	public void OnOffPhy (bool b)
	{
		if (bSkined) {
			if (!bNeck) {
				for (int i = 1; i < sprs.Length; i++) {
					sprs [i].enabled = b;
				}
			} else {
				for (int i = 1; i < sprs.Length - 1; i++) {
					sprs [i].enabled = b;
				}
			}
		} else {
			for (int i = 0; i < sprs.Length; i++) {
				if (sprs [i] != null)
					sprs [i].enabled = b;
			}
		}
		
	}

	public void Setup (Color color)
	{
		for (int i = 0; i < sprs.Length; i++) {
			sprs [i].color = color;
		}
	}

	public void Setup (string slayerOder)
	{
		for (int i = 0; i < sprs.Length; i++) {
			sprs [i].sortingLayerName = slayerOder;
		}
	}

	public void RefreshSkin ()
	{
		sprs = GetComponentsInChildren<SpriteRenderer> ();
	}

	public void Setup (JointLimit l)
	{
		JointAngleLimits2D limit = hj2d.limits;
		limit.min = l.min;
		limit.max = l.max;
		hj2d.limits = limit;
	}

	#endregion

	void Awake ()
	{
		bDeath = false;
		rg2d.isKinematic = false;
		hj2d.useLimits = true;
		SetMass (4);
		localPos = transform.localPosition;
		localRo = transform.localRotation;
	}

	public void SetMass (float mass)
	{
		rg2d.mass = mass;
	}

	public void Stop ()
	{
		rg2d.velocity = Vector2.zero;
		rg2d.angularVelocity = 0;
	}

	public void ToAnim ()
	{
		rg2d.isKinematic = true;
		Reset ();
		for (int i = 0; i < colls.Length; i++) {
			colls [i].enabled = false;
		}
	}

	public void Reset ()
	{
		transform.localPosition = localPos;
		transform.localRotation = localRo;
	}


	bool bWhithFj = false;

	public void AddFixedJoint2D (float fre)
	{
		bWhithFj = true;
		gameObject.AddComponent<FixedJointConnect> ()._Start (hj2d, fre);
	}

	#endregion

	#region Collision

	public void AddCheckCollision (PartType _type, Action<CollisionLog> a, TypeCharacter typeC, RagdollStatus s, bool canBreak, Color clh)
	{
		_colorHit = clh;
		bCanBreak = canBreak;
		if (bCanBreak) {
			this.gameObject.AddComponent<PartCollision> ().Setup (RigesterOnDestroy, _type, a, typeC, s, Breakjoint).Invoke (_colorHit);
		} else {
			this.gameObject.AddComponent<PartCollision> ().Setup (RigesterOnDestroy, _type, a, typeC, s, null).Invoke (_colorHit);
		}
	}

	#endregion

	#region ChangeLayer

	public void ChangeLayer (int l)
	{
		for (int i = 0; i < colls.Length; i++) {
			colls [i].gameObject.layer = l;
		}
	}

	public void ToDeath (int l)
	{
		if (bDeath)
			return;
		for (int i = 0; i < colls.Length; i++) {
			colls [i].gameObject.layer = l;
		}
		bDeath = true;
		transform.SetParent (null);
		if (ColorManager.Instance.bChangeJoint) {
			JointLimit limit = ColorManager.Instance.GetJointDealth (idLimit);
			this.Setup (limit);
		}
	}

	#endregion

	#region BreakJoint

	bool bEffectBreak = false;
	int idEffectBreak = 0;

	void Breakjoint (Vector2 dir)
	{
		return;
		if (!bCanBreak)
			return;
		hj2d.enabled = false;
		if (bWhithFj)
			this.GetComponent<FixedJointConnect> ().Active (false);
		bEffectBreak = true;
		SFXManager.Instance.PlayBreak ();
		rg2d.AddForce (GameConfig.forceOnBreak * dir);
		EffectFollow ef = EffectFollowManager.Instance.GetEffect ();
		idEffectBreak = ef.ID;
		ef.Play (transform, _colorHit);
	}

	#endregion

	#region Clear

	void OnTriggerExit2D (Collider2D coll)
	{
		if (!bDeath)
			return;
		if (coll.CompareTag ("OutRange")) {
			if (bEffectBreak)
				EffectFollowManager.Instance.BreakEffect (idEffectBreak);
			for (int i = 0; i < listRemoveGo.Count; i++) {
				listRemoveGo [i].Invoke ();
			}
			Destroy (gameObject);
		}
	}

	#endregion
}
