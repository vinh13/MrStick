using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
	[SerializeField]float fSpeed = -5000;
	[SerializeField]Rigidbody2D rg2d = null;

	void Start ()
	{
		Logic.bPlayerLoadDone = true;
	}

	void Update ()
	{
		rg2d.AddTorque (fSpeed);
	}

	void OnDisable ()
	{
		Logic.bPlayerLoadDone = false;
	}
}
