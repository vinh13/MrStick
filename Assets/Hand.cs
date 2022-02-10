using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Hand : MonoBehaviour
{
	[SerializeField]Rigidbody2D rg2d = null;
	[SerializeField]HingeJoint2D hj2D = null;
	Transform _target = null;
	Transform _simulator = null;

	public void Setup (Transform t, Rigidbody2D rgConnect)
	{
		_target = t;
		hj2D.connectedBody = rgConnect;
		hj2D.enabled = false;
		rg2d.isKinematic = true;
		GameObject go = new GameObject ();
		go.transform.SetParent (_target.parent);
		go.name = "_simulator";
		_simulator = go.transform;
	}

	public void ToIdle (Transform tWp, Action a)
	{
		gameObject.layer = transform.root.gameObject.layer;
		_simulator.position = tWp.position;
		hj2D.enabled = true;
		rg2d.isKinematic = false;
		this.gameObject.SetActive (true);
		timer = 0;
		bCheck = false;
		StartCoroutine (Move (a));
	}

	float timer = 0;
	bool bCheck = false;
	bool bSync = false;

	IEnumerator Move (Action cb)
	{
		bool done = false;
		while (!done) {
			bSync = true;
			Vector3 localPos = _simulator.localPosition;
			localPos = Vector3.MoveTowards (localPos, _target.localPosition, 2F * Time.deltaTime);
			_simulator.localPosition = localPos;
			if (!bCheck) {
				if (localPos == _target.localPosition) {
					bCheck = true;
				}
			} else {
				timer += Time.deltaTime;
				if (timer >= 1F)
					done = true;
			}
			yield return null;
		}
		hj2D.enabled = false;
		rg2d.isKinematic = true;
		bSync = false;
		cb.Invoke ();
		this.gameObject.SetActive (false);
	}

	void Update ()
	{
		if (bSync) {
			rg2d.MovePosition (_simulator.position);
		}
	}
}
