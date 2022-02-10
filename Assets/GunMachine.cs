using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TypeBullet
{
	None = 0,
	Machine = 1,
	Rocket = 2,
	Red = 3,
	Blue = 4,
	Yellow = 5,
	Green = 6,
}

public class GunMachine : MonoBehaviour
{
	[SerializeField]Transform start = null, end = null;
	[SerializeField]float timeHit = 1F;
	[SerializeField]int numberBullet = 3;
	GunScript gunScript = null;
	float[] posY = { 0, 0.1F, -0.1F };
	float[] timeDelay = { 0, 0.02F, 0.04F };
	[SerializeField]TypeBullet typeBullet = TypeBullet.None;

	void Start ()
	{
		gunScript = GetComponent<GunScript> ();
		gunScript.RegisterAttack (Attack);
	}

	void Attack (float dame)
	{
		if (!enabled)
			return;
		bool isP = gunScript.typeCharacter == TypeCharacter.Player;
		for (int i = 0; i < numberBullet; i++) {
			StartCoroutine (_Shoot (i, isP, dame));
		}
		SFXManager.Instance.Play (TypeSFX.gunmachine);
	}

	IEnumerator _Shoot (int i, bool isP, float dame)
	{
		yield return new WaitForSeconds (timeDelay [i]);
		Vector2 target = end.position;
		Vector2 posS = start.position;
		target.y += Random.Range (-1F, 1F);
		Bullet bullet = BulletContainer.Instance.BulletMachine (typeBullet);
		bullet.Active (true);
		Vector2 s = posS + new Vector2 (0, posY [i]);
		bullet.transform.position = s;
		Vector2 vel = ShootConfig.Throw (s, target, timeHit);
		bullet.Attack (vel, isP, dame);
	}
}
