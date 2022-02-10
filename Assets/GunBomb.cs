using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBomb : MonoBehaviour
{
	GunScript gunScript = null;
	[SerializeField]Transform tStart = null;
	[SerializeField]Rigidbody2D rg2d = null;

	void Start ()
	{
		gunScript = GetComponent<GunScript> ();
		gunScript.RegisterAttack (Attack);
	}

	void Attack (float damage)
	{
		bool isP = gunScript.typeCharacter == TypeCharacter.Player;
		Bullet bullet = BulletContainer.Instance.Bomb;
		Vector2 posS = tStart.position;
		Vector2 vel = rg2d.velocity;
		bullet.transform.position = posS;
		bullet.Active (true);
		bullet.Attack (vel, isP, damage);
	}
}
