using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgTdz : MonoBehaviour
{
	void Start ()
	{
		float angle = Random.Range (40, 60);
		this.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
	}
}
