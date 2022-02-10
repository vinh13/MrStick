using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxClear : MonoBehaviour
{
	[SerializeField]BoxCollider2D boxColl = null;

	void Start ()
	{
		Invoke ("_Start", 1F);
	}

	void _Start ()
	{
		float size = CameraControl.Instance.GetSizeOut;
		float aspect = CameraControl.Instance.GetAspect;
		size *= 2F;
		boxColl.size = new Vector2 (size * aspect, size);
	}
}
