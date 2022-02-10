using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public enum WeaponDir
{
	None = 0,
	st1 = 1,
	nd2 = 2,
}

public class PickObjectManager : MonoBehaviour
{
	[SerializeField]Rigidbody2D rg2dConnect = null;
	[SerializeField]Transform[] tPos = new Transform[2];
	Weapon[] wps = new Weapon[2];
	Action _OnAttack = null;
	TypeCharacter typeChar = TypeCharacter.None;
	ObjectType[] currentTypes = new ObjectType[2];
	PickObject pickObject = null;
	[SerializeField]HandControl handControl = null;
	float dame = 0;
	bool bCoverObject = false;

	void Awake ()
	{
		gameObject.layer = 0;
	}

	public void ActiveHand (bool b)
	{
		if (b) {
			handControl.AddWeapon ();
		} else {
			handControl.RemoveWeapon ();
		}
	}

	void Start ()
	{
		VehicleControl temp = transform.root.GetComponentInChildren<VehicleControl> ();
		_OnAttack = temp.Attack;
		typeChar = temp.typeChar;
		//handControl.RegisterReset (temp.ResetWeapon);
		temp.RegisterBreakWeapon (BreakObject);
		if (typeChar == TypeCharacter.Enemy) {
			pickObject = this.transform.root.GetComponent<PickObject> ();
			TaskUtil.Schedule (this, this._StartEnemy, 0.5F);
		} else {
			TaskUtil.Schedule (this, this._StartPlayer, 0.5F);
			PlayerControl.Instance.RegisterEquipObject (P_EquipObject);
			dame = PlayerControl.Instance.GetDame;
		}
	}

	void _StartEnemy ()
	{
		dame = transform.root.GetComponentInChildren<AIConnect> ().damage;
		ObjectType type = EnemyLogic.lateTObjectType;
		if (!AIEnemyManager.Instance.bSpawnBoss) {
			if (type == ObjectType.None) {
				ActiveHand (true);
				return;
			}
			Equip (type);
		} else {
			if (type == ObjectType.None)
				type = ObjectType.Saw;
			Equip (type);
			AIEnemyManager.Instance.bSpawnBoss = false;
		}
	}

	void _StartPlayer ()
	{
		ActiveHand (true);
	}

	void P_EquipObject (ObjectType _t, bool bCover, float timeLate)
	{
		Equip (_t, timeLate);
		bCoverObject = bCover;
	}

	void BreakObject ()
	{
		for (int i = 0; i < wps.Length; i++) {
			if (wps [i] != null) {
				wps [i].UnEquip (true);
				wps [i] = null;
			}
		}
		this.enabled = false;
		if (typeChar == TypeCharacter.Enemy)
			pickObject.EndObject ();
	}

	void SetUpWp (int i, int index, bool bReplace)
	{
		if (!bReplace) {
			wps [i] = ObjectContainer.Instance.GetOject (index).GetComponent<Weapon> ();
		} else {
			if (wps [i] == null)
				wps [i] = ObjectContainer.Instance.GetOject (index).GetComponent<Weapon> ();
		}
		wps [i].Init (tPos [i]);
		wps [i].Equip (rg2dConnect, typeChar == TypeCharacter.Player, dame);
		wps [i].RegisterOnHit (OnAttack);
		wps [i].Active (true);
	}

	void CbSetup (WeaponDir dir)
	{
		int i = dir == WeaponDir.st1 ? 0 : 1;
		PlayerControl.Instance.RegisterObjectAttack (wps [i].GetComponent<ObjectAttack> (), dir);
	}

	void CbSetupEnemy (WeaponDir dir)
	{
		int i = dir == WeaponDir.st1 ? 0 : 1;
		pickObject.RegisterAttack (wps [i].GetComponent<ObjectAttack> (), i, dame);
	}

	public void Equip (ObjectType type, float time = 0)
	{

		WeaponDir wpdir = WeaponDir.None;
		int index = 100;
		switch (type) {
		case ObjectType.Saw:
			index = 0;
			wpdir = WeaponDir.st1;
			break;
		case ObjectType.SawI:
			index = 1;
			wpdir = WeaponDir.st1;
			break;
		case ObjectType.GunSimple:
			index = 2;
			wpdir = WeaponDir.st1;
			break;
		case ObjectType.GunRocket:
			index = 3;
			wpdir = WeaponDir.st1;
			break;
		case ObjectType.GunBomb:
			index = 4;
			wpdir = WeaponDir.nd2;
			break;
		case ObjectType.Bow:
			index = 5;
			wpdir = WeaponDir.st1;
			break;
		case ObjectType.BowII:
			index = 6;
			wpdir = WeaponDir.st1;
			break;
		case ObjectType.BowIII:
			index = 7;
			wpdir = WeaponDir.st1;
			break;
		case ObjectType.GunBlue:
			index = 8;
			wpdir = WeaponDir.st1;
			break;
		case ObjectType.GunGreen:
			index = 9;
			wpdir = WeaponDir.st1;
			break;
		case ObjectType.GunRed:
			index = 10;
			wpdir = WeaponDir.st1;
			break;
		case ObjectType.GunYellow:
			index = 11;
			wpdir = WeaponDir.st1;
			break;
		}
		int i = wpdir == WeaponDir.st1 ? 0 : 1;
		bool bReplace = type != currentTypes [i] ? false : true;
		if (!bReplace) {
			if (typeChar == TypeCharacter.Player) {
				UIObjectManager.Instance.Eject (i);
			} else {
				UnEquip (type);
			}
		}
		if (i == 0) {
			ActiveHand (false);
		}

		SetUpWp (i, index, bReplace);
		currentTypes [i] = type;
		//SetUI
		if (typeChar == TypeCharacter.Player) {
			UIObjectManager.Instance.PickItem (CbSetup, type, UnEquip, wpdir, time);
		} else {
			pickObject.PickItem (CbSetupEnemy, type, UnEquip, wpdir);
		}
	}

	void UnEquip (ObjectType type)
	{
		int index = 100;
		switch (type) {
		case ObjectType.GunBomb:
			index = 1;
			break;
		default:
			index = 0;
			break;
		}
		if (wps [index] != null) {
			wps [index].UnEquip (typeChar == TypeCharacter.Player);
			wps [index] = null;
		}
		if (index == 0) {
			ActiveHand (true);
			if (typeChar == TypeCharacter.Player) {
				if (bCoverObject) {
					bCoverObject = false;
					UIObjectManager.Instance.ActiveSave ();
				}
			}
		} 
	}

	void OnAttack ()
	{
		_OnAttack.Invoke ();
	}

	#region Checking

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (!this.enabled)
			return;
		if (coll.CompareTag ("Item")) {
			CacheItem item = coll.gameObject.GetComponent<CacheItem> ();
			if (item.bPicked)
				return;
			Choose (item);
		}
	}

	void Choose (CacheItem item)
	{
		if (item.GetType == ObjectType.Power) {
			UpPower ();
		} else {
			if (!bCoverObject) {
				Equip (item.GetType);
			} else {
				if (typeChar == TypeCharacter.Player) {
					switch (item.GetType) {
					case ObjectType.GunBomb:
						Equip (item.GetType);
						break;
					default:
						UIObjectManager.Instance.RegisterSaveObject (item.GetType);
						break;
					}
				}
			}
		}
		item.OnPick ();
	}

	#endregion

	#region Power

	void UpPower ()
	{
		if (typeChar == TypeCharacter.Player) {
			PlayerControl.Instance.UpPower (0.51F);
		} else {

		}
	}

	#endregion
}
