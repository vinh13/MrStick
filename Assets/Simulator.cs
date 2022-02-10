using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using System;
using TungDz;
public class Simulator : MonoBehaviour
{
	[SerializeField]Rigidbody2D rg2d;
	[SerializeField]Transform dirMove = null;
	[SerializeField]float moveSpeed = 0;
	float speedTemp = 0;
	bool bReady = false;
	Action<object> _StartRace = null;
	void Start ()
	{
		bReady = false;
		_StartRace = (param) => StartRace ();
		EventDispatcher.Instance.RegisterListener (EventID.StartRace, _StartRace);
	}

	void StartRace ()
	{
		bReady = true;
		EventDispatcher.Instance.RemoveListener (EventID.StartRace, _StartRace);
	}

	void OnDestroy ()
	{
		EventDispatcher.Instance.RemoveListener (EventID.StartRace, _StartRace);
	}

	void Update ()
	{
		if (!bReady)
			return;
		Move (1);
	}
	void Move (int d)
	{
		
		if (speedTemp < moveSpeed)
			speedTemp += 0.1F;
		else
			speedTemp -= 0.1f;
		Mathf.Clamp (speedTemp, 0, Mathf.Infinity);
		Vector2 dir = dirMove.position - transform.position;
		dir.Normalize ();
		Vector2 vel = Vector2.zero;
		vel = (speedTemp * d) * dir;
		rg2d.velocity = vel;
	}
}
