using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BootConnect : MonoBehaviour
{
	[SerializeField]Transform shieldConnect = null;
	[SerializeField]Transform healthConnect = null;
	ParticleSystem parSHealth = null;
	Action<float> aReHealth = null;

	void Start ()
	{
		PlayerControl.Instance.RegisterBootConnect (this);
		GameObject s = Instantiate (Resources.Load<GameObject> ("Boot/Shield"), shieldConnect);
		s.transform.localPosition = Vector2.zero;
		parSHealth = Instantiate (Resources.Load<GameObject> ("Boot/RestoreHealth"), healthConnect)
			.GetComponent<ParticleSystem> ();
		parSHealth.transform.localPosition = Vector3.zero;
		ShowShield (false);
		aReHealth = GetComponent<HealthScript> ().RestoreHealth;
	}

	public void ShowShield (bool b)
	{
		shieldConnect.gameObject.SetActive (b);
		EnemyLogic.isShield = b;
	}

	public void RestoreHealth (float r)
	{
		parSHealth.gameObject.SetActive (true);
		parSHealth.Play ();
		aReHealth.Invoke (r);
	}
}
