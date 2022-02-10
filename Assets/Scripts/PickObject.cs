using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PickObject : MonoBehaviour
{
	ObjectAttack[] objectAttack = new ObjectAttack[2];
	[SerializeField]ObjectTimer[] objectsTimer = new ObjectTimer[2];
	AIStatus AIstatus = null;
	ObjectType curObjectType = ObjectType.None;
	float delayAttack = 0;

	void Awake ()
	{
		AIstatus = this.gameObject.AddComponent<AIStatus> ();
	}

	public float Attack (int id)
	{
		if (!AIstatus.bAvailable [id])
			return 1F;
		if (objectAttack [id] != null)
			objectAttack [id].Attack ();
		return delayAttack;
	}

	void Start ()
	{
		objectsTimer = new ObjectTimer[2];
		for (int i = 0; i < objectsTimer.Length; i++) {
			objectsTimer [i] = this.gameObject.AddComponent<ObjectTimer> ();
			objectsTimer [i].RegisterDisableAttack (DisableAttack);
		}
	}

	public void RegisterAttack (ObjectAttack at, int i, float damageBase)
	{
		objectAttack [i] = at;
		objectAttack [i].Setup (TypeCharacter.Enemy, damageBase);
	}

	public void PickItem (Action<WeaponDir> cb, ObjectType type, Action<ObjectType> a, WeaponDir dir)
	{
		ObTypeAttack typeAttack = ObjectData.GetTypeAttack (type);
		curObjectType = type;
		EnemyLogic.lateTObjectType = type;
		if (dir == WeaponDir.st1) {

			objectsTimer [0].Fill (type, ObjectData.time_Saw, a, dir);
		
			if (typeAttack == ObTypeAttack.HoldShoot) {
				//EnableAttack ();
				cb.Invoke (dir);
				AIstatus.bAvailable [0] = true;
			} else if (typeAttack == ObTypeAttack.ClickShoot) {
				//zEnableAttack ();
				cb.Invoke (dir);
				AIstatus.bAvailable [0] = true;
			} else {
				AIstatus.bAvailable [0] = false;
			}

		} else if (dir == WeaponDir.nd2) {
			objectsTimer [1].Fill (type, ObjectData.time_Saw, a, dir);
			AIstatus.bAvailable [1] = true;
			if (typeAttack == ObTypeAttack.ClickShoot) {
				//zEnableAttack2nd ();
				cb.Invoke (dir);
			}
		}
		switch (curObjectType) {
		case ObjectType.Bow:
			delayAttack = EnemyLogic.durationArrow;
			break;
		case ObjectType.GunBomb:
			delayAttack = EnemyLogic.durationBomb;
			break;
		case ObjectType.GunRocket:
			delayAttack = EnemyLogic.durationRocket;
			break;
		case ObjectType.GunSimple:
			delayAttack = EnemyLogic.durationMachine;
			break;
		case ObjectType.BowII:
			delayAttack = 2F;
			break;
		case ObjectType.BowIII:
			delayAttack = 2F;
			break;
		default:
			delayAttack = 1F;
			break;
		}
	}

	public void DisableAttack (WeaponDir dir)
	{
		if (dir == WeaponDir.st1) {
			AIstatus.bAvailable [0] = false;
		} else if (dir == WeaponDir.nd2) {
			AIstatus.bAvailable [1] = false;
		}
	}

	public void EndObject ()
	{
		AIstatus.bEnd = true;
	}
}
