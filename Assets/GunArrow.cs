using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunArrow : MonoBehaviour
{
	[SerializeField]Transform start = null, end = null, dir = null;
	[SerializeField]float timeHit = 1F;
	GunScript gunScript = null;
	[SerializeField]int numberArrow = 0;
	float[] posY = { 0.1F, 0, -0.1F };
	float[] timeDelay = { 0, 0.02F, 0.04F };

	void Start ()
	{
		gunScript = GetComponent<GunScript> ();
		if (numberArrow == 1) {
			gunScript.RegisterAttack (IAttack);
		} else {
			gunScript.RegisterAttack (IIAttack);
		}
	}

	void Attack (float damage)
	{
		bool isP = gunScript.typeCharacter == TypeCharacter.Player;
		Vector2 target = end.position;
		Vector2 posS = start.position;
		target.y += Random.Range (-1F, 1F);
		Bullet bullet = BulletContainer.Instance.Arrow;
		bullet.transform.position = posS;
		Vector2 vel = ShootConfig.Throw (posS, target, timeHit);
		bullet.Active (true);
		Vector2 d = dir.position - start.position;
		d.Normalize ();
		float angle = Mathf.Atan2 (d.y, d.x) * Mathf.Rad2Deg;
		bullet.Setup (angle);
		bullet.Attack (vel, isP, damage);
	}

	void IAttack (float damage)
	{
		Attack (damage);
	}

	void IIAttack (float dame)
	{
		if (!enabled)
			return;
		bool isP = gunScript.typeCharacter == TypeCharacter.Player;
		Vector2 d = dir.position - start.position;
		d.Normalize ();
		float angle = Mathf.Atan2 (d.y, d.x) * Mathf.Rad2Deg;
		for (int i = 0; i < numberArrow; i++) {
			if (!enabled)
				return;
			StartCoroutine (_Shoot (i, isP, dame, angle));
		}
	}

	IEnumerator _Shoot (int i, bool isP, float dame, float angle)
	{
		yield return new WaitForSeconds (timeDelay [i]);
		Vector2 target = end.position;
		Vector2 posS = start.position;
		target.y += Random.Range (-1F, 1F);
		Bullet bullet = BulletContainer.Instance.Arrow;
		Vector2 s = posS + new Vector2 (0, posY [i]);

		bullet.transform.position = s;

		Vector2 vel = ShootConfig.Throw (s, target, timeHit);
		bullet.Active (true);

		bullet.Setup (angle);
		bullet.Attack (vel, isP, dame);
	}
}
