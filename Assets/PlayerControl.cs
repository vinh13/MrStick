using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TungDz;
using UnityEngine.UI;

[System.Serializable]
public enum TypeControl
{
	None = 0,
	Slide = 1,
	Jump = 2,
	UpSpeed = 3,

}

public class PlayerControl : MonoBehaviour
{
	public static PlayerControl Instance = null;
	[SerializeField]VehicleControl vehicleControl = null;
	[SerializeField]float durationSlide = 2F;
	Action<object> _StartRace = null;
	[SerializeField]HoldStatus holdStaus = null;
	[SerializeField]HPBarUI uiHpbar = null, uiPowerBar = null;
	[SerializeField]Image imgHit = null;
	Action<float> _UpPower = null;
	RankConnect rank = null;
	BootConnect bootConnect = null;
	bool bTutorial = false;
	bool bUseHealth = false, bUseShield = false, bUseWeapon = false;
	float rHealth = 1F;
	bool isShield = false;
	public float ratioPower = 0;
	[Header ("Nitro")]
	[SerializeField]NitroConfig nitroConfig = null;

	public Transform GetPosSlow
	{ get { return vehicleControl.GetSlow (); } }

	public void ActiveTutorialWeapon (bool b)
	{
		bUseWeapon = b;
	}

	public float GetRHealth {
		get { 
			return rHealth;
		}
	}

	Character character = null;
	int speedLevel = 0, healthLevel = 0, atkLevel = 0;
	int speedLevelBonus = 0, healthLevelBonus = 0, atkLevelBonus = 0;

	public void ChangeHealth (float ratio)
	{
		rHealth = ratio;
		if (bUseHealth) {
			if (rHealth <= 0.7F) {
				PlayTutorial.Instance.ShowTutorial (TutorialID.UseHealth);
			}
		}
		if (bUseShield) {
			if (rHealth <= 0.4F) {
				PlayTutorial.Instance.ShowTutorial (TutorialID.UseShield);
			}
		}
	}

	public void ShowShield (bool b)
	{
		isShield = b;
		Logic.bShield = b;
		bootConnect.ShowShield (b);
		if (b) {
			SFXManager.Instance.Play ("shield");
			SkipShield ();
		}
	}

	public void SkipShield ()
	{
		if (TutorialData.bUseShield) {
			TutorialData.bUseShield = false;
		}
		if (bUseShield) {
			bUseShield = false;
			PlayTutorial.Instance.HideTutorial (TutorialID.UseShield);
		}
	}

	public bool bShieldRestore = false;

	public void RestoreHealth (float r)
	{
		SkipHealth ();
		SFXManager.Instance.Play ("health");
		bootConnect.RestoreHealth (r);
		if (!isShield) {
			bShieldRestore = true;
			Logic.bShield = true;
			StartCoroutine (_RestoreHealth ());
		}
	}

	IEnumerator _RestoreHealth ()
	{
		yield return new WaitForSeconds (0.8F);
		bShieldRestore = false;
		if (!isShield) {
			if (!UIManager.Instance.bEnd)
				Logic.bShield = false;
		}
	}

	public void SkipHealth ()
	{
		if (TutorialData.bUseHealth) {
			TutorialData.bUseHealth = false;
		}
		if (bUseHealth) {
			bUseHealth = false;
			PlayTutorial.Instance.HideTutorial (TutorialID.UseHealth);
		}
	}

	public void RegisterBootConnect (BootConnect cn)
	{
		bootConnect = cn;
	}

	public int GetRank {
		get {
			return rank.iRank;
		}
	}

	public HoldStatus GetHold {
		get { 
			return holdStaus;
		}
	}

	public float GetHealth {
		get { 
			float HP = character.config [healthLevel - 1].HP;
			float HPMax = character.config [19].HP;
			float HPMin = character.config [0].HP;
			if (healthLevelBonus != 0) {
				float valueBonus = (float)(healthLevelBonus / 20F) * (HPMax - HPMin);
				return (HP + valueBonus);
			} else {
				return HP;
			}
		}
	}

	public float GetDame {
		get { 
			return character.config [atkLevel - 1].ATK;
		}
	}

	public float GetSpeed {
		get { 
			float ratioSpeed = character.config [speedLevel - 1].SPD;
			float maxSpeed = character.config [19].SPD;
			float minSpeed = character.config [0].SPD;
			if (speedLevelBonus != 0) {
				float valueBonus = (float)(speedLevelBonus / 20F) * (maxSpeed - minSpeed);
				return ratioSpeed + valueBonus;
			} else {
				return ratioSpeed;
			}
		}
	}

	public void UpdateTutorial (bool bH, bool bS)
	{
		if (bH) {
			bUseHealth = TutorialData.bUseHealth;
		}
		if (bS) {
			bUseShield = TutorialData.bUseShield;
		}
	}

	void Awake ()
	{
		if (Instance == null)
			Instance = this;
		Logic.bShield = false;
		isShield = false;
		bTutorial = TutorialData.bTutorialStart;

		//Speed
		speedLevel = UpgradeData.GetUpgradeLevel (UpgradeType.Speed);
		if (speedLevel == 0) {
			speedLevel = 1;
		}

		NitroData nitroData = Resources.Load<NitroData> ("Character/NitroData1");
		nitroConfig = nitroData.nitroConfigs [speedLevel - 1];
		nitroScript.Register (nitroConfig.damage);
		holdStaus.durationNitro = nitroConfig.time;
		speedLevelBonus = UpgradeData.GetUpgradeLevelBonus (UpgradeType.Speed);

		//Health
		healthLevel = UpgradeData.GetUpgradeLevel (UpgradeType.Health);
		if (healthLevel == 0) {
			healthLevel = 1;
		}
		healthLevelBonus = UpgradeData.GetUpgradeLevelBonus (UpgradeType.Health);
		//ATK
		atkLevel = UpgradeData.GetUpgradeLevel (UpgradeType.ATK);
		if (atkLevel == 0) {
			atkLevel = 1;
		}
		atkLevelBonus = UpgradeData.GetUpgradeLevelBonus (UpgradeType.ATK);

		character = Resources.Load<Character> ("Character/Character1");


		_StartRace = (param) => StartRace ();

		vehicleControl.Init (EnemyType.None, GetSpeed);

		EventDispatcher.Instance.RegisterListener (EventID.StartRace, _StartRace);

		if (bTutorial) {
			GameObject trigger =
				Instantiate (Resources.Load<GameObject> ("Tutorial/TriggerCheckEnemy"),
					vehicleControl.transform);
			trigger.transform.localPosition = new Vector3 (-4.96F, 0, 0);
		}
	}

	public void ShowHit (bool b)
	{
		imgHit.enabled = b;
	}

	public RankConnect rankConnect (int ID)
	{
		rank = vehicleControl.gameObject.AddComponent<RankConnect> ();
		vehicleControl.GetComponent<VehicleStatus> ().ID = ID;
		rank.ID = ID;
		return rank;
	}

	public void StartRace ()
	{
		OnJump (false);
		vehicleControl.StartRace ();
		EventDispatcher.Instance.RemoveListener (EventID.StartRace, _StartRace);
	}

	void Update ()
	{
		if (Input.GetKey (KeyCode.D)) {
			Slide (null);
		}
		if (Input.GetKey (KeyCode.Space)) {
			Jump (null);
		}
		if (bHoldUpSpeed) {
			vehicleControl.HoldUpSpeed ();
		}
		if (Input.GetKeyDown (KeyCode.W)) {
			EndRace (true);
		}
	}

	#region Action

	public float Slide (Action a)
	{
		if (bTutorial) {
			TutorialManager.Instance.SendTutorial (TutorialID.Slide);
		}
		if (!vehicleControl.Slide (durationSlide, a))
			return 0;
		else
			return durationSlide;
	}

	public void Jump (Action a)
	{
		if (bTutorial) {
			TutorialManager.Instance.SendTutorial (TutorialID.Jump);
		}
		vehicleControl.Jump (a);
	}

	#endregion

	bool bHoldUpSpeed = false;
	//Do something
	[SerializeField]NitroScript nitroScript = null;

	public void UpSpeed ()
	{
		if (bTutorial) {
			TutorialManager.Instance.SendTutorial (TutorialID.UpSpeed);
		}
		if (bHoldUpSpeed)
			return;
		bHoldUpSpeed = true;
	}

	public void ActiveNitro (bool b)
	{
		nitroScript.PlayAnim (b);
	}

	public void DownSpeed ()
	{
		if (!bHoldUpSpeed)
			return;
		bHoldUpSpeed = false;
		vehicleControl.EndHoldUpSpeed ();
	}

	void OnDestroy ()
	{
		EventDispatcher.Instance.RemoveListener (EventID.StartRace, _StartRace);
	}

	#region Power

	public void RegisterUpPower (Action<float> a)
	{
		_UpPower = a;
	}

	public void UpPower (float ratio)
	{
		_UpPower.Invoke (ratio);
	}

	public void PowerUpdate (float ratio, Color32 color)
	{
		uiPowerBar.SetColor (color);
		uiPowerBar.Change (ratio);
	}


	#endregion

	#region Health

	public HPBar ChangeHP {
		get { 
			return uiHpbar;
		}
	}

	#endregion

	#region EndRace

	[SerializeField]PlayerAnimation playerAnim = null;

	public void EndRace (bool win)
	{
		vehicleControl.EndRace ();
		if (win) {
			if (playerAnim.OnWin ()) {
				vehicleControl.OffPhysic ();
			} else {
				UIManager.Instance.Win (true);
			}
		} else {
			UIManager.Instance.Win (false);
		}
	}

	#endregion

	#region ObjectAttack

	ObjectAttack[] objectAttack = new ObjectAttack[2];

	public void RegisterObjectAttack (ObjectAttack at, WeaponDir dir)
	{
		int i = dir == WeaponDir.st1 ? 0 : 1;
		objectAttack [i] = at;
		objectAttack [i].Setup (TypeCharacter.Player, GetDame);
	}

	public void ObjectAttackNow (WeaponDir dir)
	{
		if (bTutorial) {
			TutorialManager.Instance.SendTutorial (TutorialID.Shot);
			bTutorial = false;
		}
		int i = dir == WeaponDir.st1 ? 0 : 1;
		if (objectAttack [i] == null)
			Debug.Log ("objectAttack " + i);
		else
			objectAttack [i].Attack ();
	}

	#endregion

	#region GetPosPlayer

	public Vector3 GetPosPlayer {
		get {
			return vehicleControl.transform.position;
		}
	}

	#endregion

	#region Brake

	public void Breake (bool b)
	{
		vehicleControl.Brake (b);
	}

	#endregion

	#region JumpMove

	public void Jumpmove (bool bLeft)
	{
		if (bTutorial) {
			TutorialManager.Instance.SendTutorial (TutorialID.JumpMove);
		}
		vehicleControl.JumpMove (bLeft ? 1 : -1);
	}

	[SerializeField]Image tGround = null, tJump = null;

	public void OnJump (bool b)
	{
		tGround.enabled = b;
		tGround.raycastTarget = !b;
		tJump.enabled = !b;
		tJump.raycastTarget = b;
		if (b)
			tJump.transform.SetSiblingIndex (4);
		else
			tGround.transform.SetSiblingIndex (4);
	}

	#endregion

	#region _StartPlayer

	Action<ObjectType,bool,float> _EquipObject = null;

	public void RegisterEquipObject (Action<ObjectType,bool,float> a)
	{
		_EquipObject = a;
	}

	public void EquipObject (ObjectType _type, bool bCover, float timeLate)
	{
		_EquipObject.Invoke (_type, bCover, timeLate);
		if (bUseWeapon) {
			if (TutorialData.bUseWeapon) {
				TutorialData.bUseWeapon = false;
				PlayTutorial.Instance.HideTutorial (TutorialID.UseWeapon);
			}
			bUseWeapon = false;
		}
	}

	#endregion
}
