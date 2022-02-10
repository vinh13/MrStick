using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletContainer : MonoBehaviour
{
	public static BulletContainer Instance = null;

	void Awake ()
	{
		if (Instance == null)
			Instance = this;
		
	}

	public void RegisterBullet (ObjectType _type)
	{
		switch (_type) {
		case ObjectType.GunSimple:
			poolBullets [0]._Start ();
			break;
		case ObjectType.GunRocket:
			poolRocket._Start ();
			poolEffectHitRocket._Start ();
			break;
		case ObjectType.GunBomb:
			poolBomb._Start ();
			poolEffectHitBomb._Start ();
			break;
		case ObjectType.Bow:
			poolArrow._Start ();
			poolArrowHit._Start ();
			break;
		case ObjectType.GunBlue:
			poolBullets [1]._Start ();
			break;
		case ObjectType.GunGreen:
			poolBullets [2]._Start ();
			break;
		case ObjectType.GunRed:
			poolBullets [3]._Start ();
			break;
		case ObjectType.GunYellow:
			poolBullets [4]._Start ();
			break;
		}
	}

	[SerializeField]PoolBullet[] poolBullets;

	public Bullet BulletMachine (TypeBullet _t)
	{
		int indexBullet = 0;
		switch (_t) {
		case TypeBullet.Machine:
			indexBullet = 0;
			break;
		case TypeBullet.Blue:
			indexBullet = 1;
			break;
		case TypeBullet.Green:
			indexBullet = 2;
			break;
		case TypeBullet.Red:
			indexBullet = 3;
			break;
		case TypeBullet.Yellow:
			indexBullet = 4;
			break;
		}
		return poolBullets [indexBullet].Current;
	}

	[SerializeField]PoolBullet poolRocket = null;

	public Bullet Rocket {
		get { 
			return poolRocket.Current;
		}
	}

	[SerializeField]PoolEffect poolEffectHitRocket = null;

	public Effect HitRocket {
		get {
			return poolEffectHitRocket.Current;
		}
	}



	[SerializeField]PoolBullet poolBomb = null;

	public Bullet Bomb {
		get { 
			return poolBomb.Current;
		}
	}

	[SerializeField]PoolEffect poolEffectHitBomb = null;

	public Effect HitBomb {
		get {
			return poolEffectHitBomb.Current;
		}
	}


	[SerializeField]PoolBullet poolArrow = null;

	public Bullet Arrow {
		get { 
			return poolArrow.Current;
		}
	}

	[SerializeField]PoolObFollow poolArrowHit = null;

	public ObFollow ArrowHit {
		get { 
			return poolArrowHit.Current;
		}
	}


}
