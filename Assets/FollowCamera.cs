using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
	float originalY = 0;

	void Awake ()
	{
		originalY = this.transform.position.y;
	}

	public void Follow (Vector3 pos)
	{
		pos.y = originalY;
		pos.z = 0;
		this.transform.position = pos;
	}
}
