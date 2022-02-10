using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public enum ObjectType
{
	None = 0,
	Saw = 1,
	Power = 2,
	SawI = 3,
	GunSimple = 4,
	GunRocket = 5,
	GunBomb = 6,
	Bow = 7,
	BowII = 8,
	BowIII = 9,
	GunBlue = 11,
	GunGreen = 12,
	GunRed = 13,
	GunYellow = 14,
}

[System.Serializable]
public enum ObTypeAttack
{
	None = 0,
	HoldShoot = 1,
	ClickShoot = 2,
}

public class UIObjectManager : MonoBehaviour
{
	public static UIObjectManager Instance = null;
	[SerializeField]UIItemScript[] UIItem = new UIItemScript[2];
	[SerializeField]ButtonTdz btnAttack = null;
	[SerializeField]ButtonTdz zbtnAttack = null;
	[SerializeField]ButtonTdz zbtnAttack2nd = null;
	[SerializeField]ItemContainer sprContainer = null;
	[SerializeField]ButtonEquipObject btnEquipObject = null;
	bool bTutorialAttack2 = false;

	public Sprite sprObject (ObjectType _t)
	{
		return sprContainer.GetSprite (_t);
	}

	void OnValidate ()
	{
		UIItem = GetComponentsInChildren<UIItemScript> ();
	}

	public void RegisterSaveObject (ObjectType type)
	{
		btnEquipObject.ShowObject (type, ObjectData.time_Saw);
	}

	public void ActiveSave ()
	{
		btnEquipObject.ActiveSave ();
	}

	void Awake ()
	{
		if (Instance == null)
			Instance = this;
		for (int i = 0; i < UIItem.Length; i++) {
			UIItem [i].Setup (EjectAll, i);
		}

		btnAttack.RegisterClick (Attack, 0.25F);
		btnAttack.Block (true);


		zbtnAttack.RegisterClick (Attack, 0.5F);
		zbtnAttack.Block (true);


		zbtnAttack2nd.RegisterClick (Attack2nd, 1F);
		zbtnAttack2nd.Block (true);

		bTutorialAttack2 = TutorialData.bAttack_2Hand;

	}

	public void PickItem (Action<WeaponDir> cb, ObjectType type, Action<ObjectType> a, WeaponDir dir, float time = 0)
	{
		if (UIManager.Instance.bEnd)
			return;

		float timer = time;
		if (timer == 0) {
			timer = ObjectData.time_Saw;
		}

		SFXManager.Instance.Play (TypeSFX.pickwp);
		ObTypeAttack typeAttack = ObjectData.GetTypeAttack (type);
		if (dir == WeaponDir.st1) {
			UIItem [0].Fill (type, timer, a, dir, sprContainer.GetSprite (type));
			if (typeAttack == ObTypeAttack.HoldShoot) {
				EnableAttack ();
				cb.Invoke (dir);
			} else if (typeAttack == ObTypeAttack.ClickShoot) {
				zEnableAttack ();
				cb.Invoke (dir);
			}
		} else if (dir == WeaponDir.nd2) {
			UIItem [1].Fill (type, timer, a, dir, sprContainer.GetSprite (type));
			if (typeAttack == ObTypeAttack.ClickShoot) {
				zEnableAttack2nd ();
				cb.Invoke (dir);
			}
		}
	}

	#region Eject

	public void EjectAll (int id)
	{
		UIItem [id].Eject ();
	}

	public void Eject (int dir)
	{
		UIItem [dir].Eject ();
	}

	#endregion

	#region Attack

	void EnableAttack ()
	{
		btnAttack.Active (true);
		zbtnAttack.Active (false);
		btnAttack.Block (false);
	}

	void zEnableAttack ()
	{
		btnAttack.Active (false);
		zbtnAttack.Active (true);
		zbtnAttack.Block (false);
	}

	void zEnableAttack2nd ()
	{
		if (bTutorialAttack2) {
			UIManager.Instance.CreateTutorialAttack2 (GetComponent<UIGame> ());
		}
		zbtnAttack2nd.Active (true);
		zbtnAttack2nd.Block (false);
	}

	public void CompleteAttack2nd ()
	{
		if (!bTutorialAttack2)
			return;
		bTutorialAttack2 = false;
		TutorialData.bAttack_2Hand = false;
	}

	void zDisableAttack2nd ()
	{
		zbtnAttack2nd.Block (true);
	}

	public void DisableAttack (WeaponDir dir)
	{
		if (dir == WeaponDir.st1) {
			btnAttack.Block (true);
			zbtnAttack.Block (true);
		} else if (dir == WeaponDir.nd2) {
			zDisableAttack2nd ();
		}
	}

	public void Attack ()
	{
		PlayerControl.Instance.ObjectAttackNow (WeaponDir.st1);
	}

	public void Attack2nd ()
	{
		if (bTutorialAttack2) {
			TutorialAttack.Instance.HideTutorial (TutorialID.Attack);
			bTutorialAttack2 = false;
		}
		PlayerControl.Instance.ObjectAttackNow (WeaponDir.nd2);
	}

	#endregion
}
