using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public enum AILevel
{
	None = 0,
	Easy = 1,
	Normal = 2,
	Hard = 3,
	HardCode = 4,
	Boss = 5,
}

public class AIBase : MonoBehaviour,AICall
{
	VehicleControl vehicleControl = null;
	[SerializeField]LineCastCheck checkSlide = null;
	[SerializeField]LineCastCheck checkJump = null;
	[SerializeField]LineCastCheck checkShoot = null;
	[SerializeField]LineCastCheck checkShootB = null;
	[SerializeField]LineCastCheck checkShield = null;
	[SerializeField]AILevel aiLevel = AILevel.None;
	[SerializeField]EnemyType enemyType = EnemyType.None;
	bool canSlide = false;
	bool canJump = false;
	bool bSlide = false;
	bool bRegister = false;
	bool bStop = false;
	bool[] bCanShoot = new bool[2];
	bool[] bShoot = new bool[2];
	AIStatus AIstatus = null;
	PickObject pickObject = null;
	Transform posPlayer = null;
	RankConnect rankConnect = null;
	float rangeX = 0;
	bool bStarted = false;
	int numberAvoid = 0;
	float ratioAvoid = 0;
	int countAvoid = 0;
	int countShoot_Shield = 0;
	int[] countShoot = new int[2];
	int[] maxShoot = new int[2];
	bool[] bAvoid;
	float[] delayAttack = new float[2];
	bool bBoss = false;
	bool bHit = false;
	int IDSKin = 0;
	bool b1stBehindAttack = false;

	void Start ()
	{
		pickObject = this.transform.root.GetComponent<PickObject> ();
		posPlayer = AIEnemyManager.Instance.getPlayer;
		rangeX = AIEnemyManager.Instance.rangeEnemy;
		rankConnect = transform.root.GetComponent<RankConnect> ();
		checkSlide.RegisterCheck (new int[]{ 11 });
		checkJump.RegisterCheck (new int[]{ 10, 11 });
		checkShoot.RegisterCheck (new int[]{ 8 });
		checkShootB.RegisterCheck (new int[]{ 8 });
		checkShield.RegisterCheck (new int[]{ 21 });
		bStarted = true;
	}

	void PlayerGoTop ()
	{
		
	}

	#region OnHit

	bool bMoveLeft = false;
	bool bMoveRight = false;

	void OnHit (float ratio)
	{
		if (!bHit) {
			bHit = true;
			TaskUtil.ScheduleWithTimeScale (this, _OnHit, 0.25F);
		}
		if (countAvoid >= numberAvoid)
			return;
		if (!bAvoid [countAvoid]) {
			if (ratio <= ratioAvoid * (numberAvoid - countAvoid)) {
				bAvoid [countAvoid] = true;
				countAvoid += 1;
				if (!AIstatus.bJump) {
					canJump = true;
					float xA = transform.position.x;
					float xP = posPlayer.position.x;
					if (aiLevel.GetHashCode () > 2) {
						if (xA > xP) {
							bMoveLeft = true;
							bMoveRight = false;
						} else {
							bMoveLeft = false;
							bMoveRight = true;
						}
					}
				}
			}
		}

	}

	void _OnHit ()
	{
		bHit = false;
	}

	#endregion

	void CallStart ()
	{
		vehicleControl.StartRace ();
		if (enemyType == EnemyType.Boss)
			AIEnemyManager.Instance.ShowBoss (true);
		if (AIEnemyManager.Instance.BLapPoint) {
			AddSpeed ((float)aiLevel.GetHashCode ());
		}
	}

	void Update ()
	{
		if (!bRegister || bStop || AIstatus.bEnd) {
			if (AIstatus.bEnd) {
				StopAllCoroutines ();
				this.enabled = false;
			}
			return;
		}
		if (!bHit) {
			CheckJump ();
			CheckSlide ();
		}
		if (CameraControl.Instance.CheckObjectInViewport (this.transform.position)) {
			CheckShoot ();
			CheckShootB ();
		}
		CheckRange ();
		if (!bStop) {
			if (EnemyLogic.bStop) {
				StopAI ();
			}
		}
		if (!bPause) {
			if (EnemyLogic.pause) {
				Brake (true);
				bPause = true;
			}
		}
	}

	bool bPause = false;
	bool bRange = false;

	void CheckRange ()
	{
		if (!bRange) {
			bRange = true;
			TaskUtil.ScheduleWithTimeScale (this, _CheckRange, GameConfig.durationCheckRange);
		}
	}

	void _CheckRange ()
	{
		if (bSlide || AIstatus.bJump) {
			_RangeDone ();
			return;
		}
		float distance = Vector2.Distance (transform.position, posPlayer.position);
		if (rankConnect.iRank > PlayerControl.Instance.GetRank) {
			if (distance - 2 > rangeX) {
				float time = distance - 2 - rangeX;
				time = Mathf.Clamp (time, 1F, 5F);
				if (AIEnemyManager.Instance.BLapPoint) {
					if (time >= 3) {
						if (aiLevel.GetHashCode () >= 0)
							AddSpeed (1F);
					}
				}
				UpSpeed (time);
				TaskUtil.ScheduleWithTimeScale (this, _RangeDone, time);
			} else {
				if (distance < 3F) {
					Brake (true);
					TaskUtil.ScheduleWithTimeScale (this, _BrakeDone, 1F);
				} else {
					_RangeDone ();
				}
			}
		} else {
			if (distance < 2F) {
				UpSpeed (0.5F);
				TaskUtil.ScheduleWithTimeScale (this, _RangeDone, 0.5F);
			} else if (distance > CameraControl.Instance.GetX / 2F) {
				Brake (true);
				TaskUtil.ScheduleWithTimeScale (this, _BrakeDone, 1F);
			} else {
				_RangeDone ();
			}
		}
	}

	void _RangeDone ()
	{
		bRange = false;
	}

	void _BrakeDone ()
	{
		bRange = false;
		Brake (false);
	}

	void StopAI ()
	{
		bStop = true;
		vehicleControl.AIStop ();
	}

	bool bCheck = false;

	#region ShootF

	void CheckShoot ()
	{
		if (!AIstatus.bAvailable [0] || bShoot [0])
			return;
		if (!bCanShoot [0]) {
			if (this.transform.position.x < posPlayer.position.x) {
				bCanShoot [0] = checkShoot.Check ();
			}
		}
		if (bCanShoot [0]) {
			if (!bShoot [0]) {
				Shoot ();
			}
		}
	}

	void Shoot ()
	{
		bShoot [0] = true;
		float time = 1F;
		if (EnemyLogic.isShield) {
			if (countShoot_Shield > 0) {
				time = pickObject.Attack (0);
				countShoot [0]++;
				countShoot_Shield--;
			} else {
				time = 1F;
			}
		} else {
			time = pickObject.Attack (0);
			countShoot [0]++;
		}
		time += delayAttack [0];
		if (countShoot [0] < maxShoot [0]) {
			TaskUtil.Schedule (this, this._Shoot, time);
		} else {
			TaskUtil.Schedule (this, this._ResetShoot, 3F);
		}
	}

	void _ResetShoot ()
	{
		_Shoot ();
		countShoot [0] = 0;
	}

	void _Shoot ()
	{
		bShoot [0] = false;
		bCanShoot [0] = false;
	}

	#endregion

	#region ShootB

	void CheckShootB ()
	{
		
		if (!AIstatus.bAvailable [1] || bShoot [1])
			return;
		if (!bCanShoot [1]) {
			if (this.transform.position.x - posPlayer.position.x >= 5F) {
				bCanShoot [1] = checkShootB.Check ();
			} else {
				return;
			}
		}
		if (bCanShoot [1]) {
			if (!bShoot [1]) {
				if (b1stBehindAttack) {
					ShootB ();
				} else {
					bShoot [1] = true;
					TaskUtil.Schedule (this, this.ShootB, delayAttack [1] / 2F);
				}
			}
		}
	}

	void ShootB ()
	{
		bShoot [1] = true;
		float time = 1F;
		if (EnemyLogic.isShield) {
			if (countShoot_Shield > 0) {
				time = pickObject.Attack (1);
				countShoot [1]++;
				countShoot_Shield--;
			} else {
				time = 1F;
			}
		} else {
			time = pickObject.Attack (1);
			countShoot [1]++;
		}
		time += delayAttack [1];
		if (countShoot [1] < maxShoot [1]) {
			TaskUtil.Schedule (this, this._ShootB, time);
		} else {
			TaskUtil.Schedule (this, this._ResetShootB, time);
		}
	}

	void _ResetShootB ()
	{
		_ShootB ();
		countShoot [1] = 0;
	}

	void _ShootB ()
	{
		bShoot [1] = false;
		bCanShoot [1] = false;
	}

	#endregion

	#region Slide



	void CheckSlide ()
	{
		if (!canSlide) {
			canSlide = checkSlide.Check ();
		}
		if (canSlide) {
			if (!bSlide) {
				vehicleControl.Slide (GameConfig.durationSlide, SlideDone);
				bSlide = true;
				if (bUpSpeed) {
					StopAllCoroutines ();
					bUpSpeed = false;
					vehicleControl.EndHoldUpSpeed ();
				}
			}
		}
	}

	void SlideDone ()
	{
		canSlide = false;
		bSlide = false;
	}

	#endregion

	#region Jump

	void CheckJump ()
	{
		if (!canJump) {
			canJump = checkJump.Check ();
			if (canJump)
				bMoveRight = true;
		}
		if (canJump) {
			if (!AIstatus.bJump) {
				vehicleControl.Jump (JumDone);
				AIstatus.bJump = true;
				StartCoroutine (_JumpMove ());
			}
		}
	}

	IEnumerator _JumpMove ()
	{
		yield return new WaitForSeconds (0.5F);
		float timer = 0;
		bool done = false;
		while (!done) {
			if (bMoveLeft) {
				timer += Time.deltaTime;
				JumpMove (-1);
			} else {
				timer += Time.deltaTime;
				JumpMove (1);
			}
			done = timer >= 0.05F ? true : !AIstatus.bJump;
			yield return null;
		}
		bMoveLeft = false;
		bMoveRight = false;
	}


	void JumDone ()
	{
		canJump = false;
		AIstatus.bJump = false;
	}

	#endregion

	#region override

	public void StartRace ()
	{
		EnemyManager.Instance.RegisterStart (CallStart);
	}

	public void Register (VehicleControl cv, EnemyData data)
	{
		ratioSPD = PlayerControl.Instance.GetSpeed;
		AIstatus = this.transform.root.GetComponent<AIStatus> ();
		IDSKin = data.IDSkin;
		cv.CreateSkin (IDSKin);
		enemyType = data.enemyType;
		vehicleControl = cv;
		bRegister = true;
		HealthScript h = transform.root.GetComponent<HealthScript> ();
		h.RegisterChange (OnHit);
		h.SetHealth (data.health, null);
		h.Init (AIEnemyManager.Instance.GetHpBarBoss);
		aiLevel = data.aiLevel;
		switch (aiLevel) {
		case AILevel.Easy:
			checkShoot.Resize (5);
			checkShootB.Resize (-8);
			countShoot_Shield = 4;
			delayAttack [0] = 0.75F;
			maxShoot [0] = 2;
			delayAttack [1] = 20F;
			maxShoot [1] = 1;
			break;
		case AILevel.Normal:
			checkShoot.Resize (6);
			checkShootB.Resize (-8);
			countShoot_Shield = 3;
			delayAttack [0] = 0.5F;
			maxShoot [0] = 3;
			delayAttack [1] = 18F;
			maxShoot [1] = 1;
			break;
		case AILevel.Hard:
			checkShoot.Resize (7);
			checkShootB.Resize (-7);
			countShoot_Shield = 2;
			delayAttack [0] = 0.25F;
			maxShoot [0] = 4;
			delayAttack [1] = 16F;
			maxShoot [1] = 2;
			break;
		case AILevel.HardCode:
			checkShoot.Resize (8);
			checkShootB.Resize (-6);
			countShoot_Shield = 1;
			delayAttack [0] = 0;
			maxShoot [0] = 5;
			delayAttack [1] = 15F;
			maxShoot [1] = 3;
			break;
		}
		numberAvoid = aiLevel.GetHashCode () * 2;
		countAvoid = 0;
		ratioAvoid = 1F / (numberAvoid + 1);
		bAvoid = new bool[numberAvoid];
		for (int i = 0; i < numberAvoid; i++) {
			bAvoid [i] = false;
		}
	}

	bool bUpSpeed = false;

	public void UpSpeed (float time = 0)
	{
		if (bUpSpeed || AIstatus.bEnd)
			return;
		bUpSpeed = true;
		StartCoroutine (_UpSpeed (time));
	}

	float ratioSPD = 1F;

	public void Init ()
	{
		vehicleControl.Init (enemyType, ratioSPD);
	}

	public void StartVehicle ()
	{
		vehicleControl.StartVehicle ();
		if (enemyType == EnemyType.Boss) {
			AIEnemyManager.Instance.ShowBoss (true);
			aiLevel = AILevel.Boss;
			checkShoot.Resize (10);
			checkShootB.Resize (-5);
			countShoot_Shield = 1;
			delayAttack [0] = 0;
			maxShoot [0] = 6;
			delayAttack [1] = 4;
			maxShoot [1] = 5;
			numberAvoid = aiLevel.GetHashCode () * 2;
			countAvoid = 0;
			ratioAvoid = 1F / (numberAvoid + 1);
			bAvoid = new bool[numberAvoid];
			for (int i = 0; i < numberAvoid; i++) {
				bAvoid [i] = false;
			}
			bBoss = true;
			GameObject go = Instantiate (Resources.Load<GameObject> ("avaBoss"), transform.root);
			vehicleControl.RegisterOnEnd (go.GetComponent<AvaBoss> ().OnEnd);
			vehicleControl.RegisterOnEnd (BossEnd);
		}
	}

	void BossEnd ()
	{
		CameraControl.Instance.ShowTarget (vehicleControl.GetSlow (), 3F,PlayerControl.Instance.GetPosSlow);
	}

	#endregion

	#region UpSpeed

	IEnumerator _UpSpeed (float duration)
	{
		bool done = false;
		float timer = 0;
		while (!done) {
			timer += Time.deltaTime;
			if (timer >= duration)
				done = true;
			vehicleControl.HoldUpSpeed ();
			if (EnemyLogic.isShield) {
				if (checkShield.Check ()) {
					done = true;
				}
			}
			yield return null;
		}
		bUpSpeed = false;
		vehicleControl.EndHoldUpSpeed ();
	}

	void Brake (bool b)
	{
		vehicleControl.Brake (b);
	}

	void JumpMove (int dir)
	{
		vehicleControl.JumpMove (dir);
	}


	#endregion

	#region Trigger

	bool conflictAI = false;

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (!bStarted)
			return;
		if (AIstatus.bJump)
			return;
		if (!conflictAI) {
			if (coll.CompareTag ("AI")) {
				if (rankConnect.iRank > PlayerControl.Instance.GetRank) {
					UpSpeed (2);
					TaskUtil.Schedule (this, ResetConflictAI, 2F);
					conflictAI = true;
				}
			}
		}
	}

	void ResetConflictAI ()
	{
		conflictAI = false;
	}

	bool bOverlap = false;

	void OnTriggerStay2D (Collider2D coll)
	{
		if (!bStarted)
			return;
		if (bOverlap || conflictAI)
			return;
		if (coll.CompareTag ("AI")) {
			if (transform.position.x < coll.transform.position.x) {
				vehicleControl.AIBrake ();
				bOverlap = true;
				TaskUtil.Schedule (this, ResetOverlap, 2F);
			}
		}
	}

	void ResetOverlap ()
	{
		bOverlap = false;
	}

	public void AddSpeed (float value)
	{
		vehicleControl.AddSpeed (value);
	}

	#endregion
}
