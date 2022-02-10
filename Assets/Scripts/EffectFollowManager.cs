using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectFollowManager : MonoBehaviour
{
	public static EffectFollowManager Instance = null;
	[SerializeField]BreakContainer breakContainer = null;
	[SerializeField]ParticleSystem[] parsS;

	void Awake ()
	{
		if (Instance == null)
			Instance = this;
	}

	public EffectFollow GetEffect ()
	{
		return breakContainer.GetEffect ();
	}

	public void BreakEffect (int id)
	{
		breakContainer.BreakEffect (id);
	}

	public void PlayeEffectBulletHit (TypeBullet typeBullet, Vector3 pos)
	{
		switch (typeBullet) {
		case TypeBullet.Machine:
			break;
		case TypeBullet.Blue:
			parsS [1].transform.position = pos;
			parsS [1].Play ();
			break;
		case TypeBullet.Green:
			parsS [2].transform.position = pos;
			parsS [2].Play ();
			break;
		case TypeBullet.Red:
			parsS [3].transform.position = pos;
			parsS [3].Play ();
			break;
		case TypeBullet.Yellow:
			parsS [4].transform.position = pos;
			parsS [4].Play ();
			break;
		}
	}
}
