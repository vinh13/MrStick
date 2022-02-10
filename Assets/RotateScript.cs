using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour
{
	[SerializeField]float angle = 0;
	void Update ()
	{
		transform.Rotate (Vector3.forward * angle * Time.deltaTime);
	}
}
