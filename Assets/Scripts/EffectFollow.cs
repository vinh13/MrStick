using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectFollow : MonoBehaviour
{
	public bool bReady = false;
	public int ID = 0;

	ParticleSystem pars {
		get { 
			if (p == null)
				p = GetComponent<ParticleSystem> ();
			return p;
		}
	}
	ParticleSystem p = null;
	void OnEnable ()
	{
		bReady = false;
	}

	void OnDisable ()
	{
		bReady = true;
	}

	public void Play (Transform _p, Color color)
	{
		var main = pars.main;
		main.startColor = color;
		transform.SetParent (_p);
		transform.localPosition = Vector2.zero;
		transform.localRotation = Quaternion.AngleAxis (0, Vector3.forward);
		this.gameObject.SetActive (true);
	}

	public void UnPlay ()
	{
		transform.SetParent (null);
		this.gameObject.SetActive (false);
	}
}
