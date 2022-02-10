using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectContainer : MonoBehaviour
{
	public static ObjectContainer Instance = null;
	[SerializeField]ObjectPool poolSaw = null;
	[SerializeField]ObjectPool poolSawI = null;
	[SerializeField]ObjectPool poolGunSimple = null;
	[SerializeField]ObjectPool poolGunRocket = null;
	[SerializeField]ObjectPool poolGunBomb = null;
	[SerializeField]ObjectPool poolGunArrow = null, poolGunArrowII = null, poolGunArrowIII = null;
	[SerializeField]ObjectPool poolGunBlue = null;
	[SerializeField]ObjectPool poolGunGreen = null;
	[SerializeField]ObjectPool poolGunRed = null;
	[SerializeField]ObjectPool poolGunYellow = null;

	void Awake ()
	{
		if (Instance == null)
			Instance = this;
	}

	public void ReisterObjectPool (ObjectType _type)
	{
		CreatePool (_type, 3);
	}

	public GameObject GetOject (int index)
	{
		switch (index) {
		case 0:
			return poolSaw.Current;
		case 1:
			return poolSawI.Current;
		case 2:
			return poolGunSimple.Current;
		case 3:
			return poolGunRocket.Current;
		case 4:
			return poolGunBomb.Current;
		case 5:
			return poolGunArrow.Current;
		case 6:
			return poolGunArrowII.Current;
		case 7:
			return poolGunArrowIII.Current;
		case 8:
			return poolGunBlue.Current;
		case 9:
			return poolGunGreen.Current;
		case 10:
			return poolGunRed.Current;
		case 11:
			return poolGunYellow.Current;
		default:
			return null;
		}
	}

	void CreatePool (ObjectType _type, int number)
	{
		switch (_type) {
		case ObjectType.Saw:
			poolSaw.CreatePool (number);
			break;
		case ObjectType.SawI:
			poolSawI.CreatePool (number);
			break;
		case ObjectType.GunSimple:
			BulletContainer.Instance.RegisterBullet (_type);
			poolGunSimple.CreatePool (number);
			break;
		case ObjectType.GunRocket:
			BulletContainer.Instance.RegisterBullet (_type);
			poolGunRocket.CreatePool (number);
			break;
		case ObjectType.GunBomb:
			BulletContainer.Instance.RegisterBullet (_type);
			poolGunBomb.CreatePool (number);
			break;
		case ObjectType.Bow:
			BulletContainer.Instance.RegisterBullet (_type);
			poolGunArrow.CreatePool (number);
			break;
		case ObjectType.BowII:
			BulletContainer.Instance.RegisterBullet (ObjectType.Bow);
			poolGunArrowII.CreatePool (number);
			break;
		case ObjectType.BowIII:
			BulletContainer.Instance.RegisterBullet (ObjectType.Bow);
			poolGunArrowIII.CreatePool (number);
			break;
		case ObjectType.GunBlue:
			BulletContainer.Instance.RegisterBullet (ObjectType.GunBlue);
			poolGunBlue.CreatePool (number);
			break;
		case ObjectType.GunGreen:
			BulletContainer.Instance.RegisterBullet (ObjectType.GunGreen);
			poolGunGreen.CreatePool (number);
			break;
		case ObjectType.GunRed:
			BulletContainer.Instance.RegisterBullet (ObjectType.GunRed);
			poolGunRed.CreatePool (number);
			break;
		case ObjectType.GunYellow:
			BulletContainer.Instance.RegisterBullet (ObjectType.GunYellow);
			poolGunYellow.CreatePool (number);
			break;
		}
	}
}
