using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class X2Anim : MonoBehaviour
{
	Scaler scaler = null;
	bool b = false;
	[SerializeField]float valueS = 0;
	float originalS = 0;

	void OnEnable ()
	{
		if (scaler == null) {
			originalS = transform.localScale.x;
			scaler = gameObject.AddComponent<Scaler> ();
		}
		b = false;
		Play ();
	}

	void Play ()
	{
		b = !b;
		if (b) {
			scaler.Scale (originalS + valueS, 0.1F, Done);
		} else {
			scaler.Scale (originalS, 0.1F, Done);
		}
	}

	void Done ()
	{
		TaskUtil.Schedule (this, this.Play, 0.25F);
	}

	void OnDisable ()
	{
		StopAllCoroutines ();
	}
}
