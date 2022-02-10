using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRocket : MonoBehaviour
{
	[SerializeField]Transform start = null, end = null, dir = null;
	[SerializeField]float timeHit = 1F;
	GunScript gunScript = null;
	void Start ()
	{
		gunScript = GetComponent<GunScript> ();
		gunScript.RegisterAttack (Attack);
	}

	void Attack (float dame)
	{
		bool isP = gunScript.typeCharacter == TypeCharacter.Player;
		Vector2 target = end.position;
		Vector2 posS = start.position;
		Bullet bullet = BulletContainer.Instance.Rocket;
		bullet.transform.position = posS;
		Vector2 vel = ShootConfig.Throw (posS, target, timeHit);
		bullet.Active (true);
		Vector2 d = dir.position - start.position;
		d.Normalize ();
		float angle = Mathf.Atan2 (d.y, d.x) * Mathf.Rad2Deg;
		bullet.Setup (angle);
		bullet.Attack (vel, isP, dame);
	}

}
