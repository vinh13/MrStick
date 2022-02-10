using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCheckCam : MonoBehaviour
{
	[SerializeField]Transform render = null;
	bool bCheck = false;
	Vector3 posCheck = Vector3.zero;

	void Start ()
	{
		bCheck = render != null;
		posCheck = this.transform.position;
	}

	void Update ()
	{
		if (!bCheck)
			return;
		bool b = CameraControl.Instance.CheckObjectInViewport (posCheck);
		render.gameObject.SetActive (b);
	}
}
