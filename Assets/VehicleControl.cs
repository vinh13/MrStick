using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public enum TypeCharacter
{
	None = 0,
	Player = 1,
	Enemy = 2,
}

[System.Serializable]
public enum HitDir
{
	None = 0,
	Top = 1,
	Bottom = 2,
	Left = 3,
	Right = 4,
}

public class VehicleControl : MonoBehaviour
{
	public TypeCharacter typeChar = TypeCharacter.None;
	[SerializeField]Rigidbody2D rg2d = null;
	[SerializeField]float moveSpeed = 0;
	[SerializeField]JointConnect jointConnect = null;
	[SerializeField]float rangeAngleLeft = -80, rangeAngleRight = 30, angle360 = -360;
	[SerializeField]float veAngleWheel = 10;
	[SerializeField]WheelSync wheelSync = null;
	[SerializeField]LineCastCheck lineCastCheck = null;
	[SerializeField]Vector2 vel_Jump = Vector2.zero;
	[SerializeField]RagdollManager ragdollManager = null;

	public Transform GetSlow ()
	{
		return ragdollManager.getPosSlow ();
	}

	[SerializeField]Transform dirMove = null;
	[SerializeField]VehicleStatus status = null;
	List<Action> listActionEnd = new List<Action> ();
	bool bMoveLeft = false;
	bool bMoveRight = false;
	float speedTemp = 0;
	bool bGround = false;
	bool bAngleLeft = false;
	bool bAngleRight = false;
	bool bAngleIdle = false;
	bool bUpSpeed = false;
	bool bSlowSpeed = false;
	float originalSpeed = 0;
	bool bEnd = false;
	bool bStart = false;
	Action _JumpDone = null;
	float ratioSpeed = 0;
	bool bCheckRender = false;
	float fSpeedY = 0;

	public void Init (EnemyType enemyType, float ratioSPD)
	{
		lineCastCheck.RegisterCheck (new int[]{ 10 });

		ragdollManager.RegisterEnd (End, typeChar, OnHit, enemyType, bike.GetComponent<VehicleCollision> ().tPosHP);
		bEnd = false;
		bStart = false;
		transform.Find ("TriggerNextMap").GetComponent<TriggerNextMap> ().RegisterNextRace (NextLap);
		bCheckRender = true;
		bInCam = false;
		if (typeChar == TypeCharacter.Enemy) {
			EnemyUpdateRenderer ();
			ratioSpeed = ratioSPD;
			moveSpeed *= ratioSPD;
			originalSpeed = moveSpeed;
		} else {
			ratioSpeed = ratioSPD;
			moveSpeed *= ratioSPD;
			originalSpeed = moveSpeed;
		}
	}

	public void StartRace ()
	{
		if (bStart)
			return;
		bStart = true;
		if (typeChar == TypeCharacter.Enemy) {
//			float ratioSPD = PlayerControl.Instance.GetSpeed;
//
//			ratioSPD = Mathf.Clamp (ratioSPD, 1F, Mathf.Infinity);
//
//			ratioSpeed = ratioSPD;
//			moveSpeed *= ratioSPD;
//
//			originalSpeed = moveSpeed;
			LeaderboardManager.Instance.UpdateRank (status.ID, false);
		} 
	}

	public void CreateSkin (int idSkin)
	{
		ragdollManager.CreateSkin (idSkin);
	}

	void EnemyUpdateRenderer ()
	{
		string s = ragdollManager.Init ();
		bike.Change (s);
	}

	public void StartVehicle ()
	{
		if (bStart)
			return;
		bStart = true;
		bMoveRight = true;
		speedTemp = moveSpeed;
		if (typeChar == TypeCharacter.Enemy) {
//			float ratioSPD = PlayerControl.Instance.GetSpeed;
//			ratioSPD = Mathf.Clamp (ratioSPD, 1F, Mathf.Infinity);
//
//
//			ratioSpeed = ratioSPD;
//			moveSpeed *= ratioSPD;
//			originalSpeed = moveSpeed;
			LeaderboardManager.Instance.UpdateRank (status.ID, true);
		}
	}

	void Update ()
	{
		if (!bStart) {
			StopAll ();
			return;
		}
		if (status.bRotateIdle || bEnd) {
			return;
		}
		bGround = lineCastCheck.Check ();
		if (bGround && !bJump)
			MoveRight ();
		if (!bGround && !bJump) {
			MoveGround ();
		}
		if (bCheckRender)
			CheckEnemy ();
	
	}

	bool bInCam = false;

	void CheckEnemy ()
	{
		bool b = CameraControl.Instance.CheckObjectInViewport (this.transform.position);
		if (b) {
			if (!bInCam) {
				ragdollManager.OnOffPhy (true);
				bike.OnOff (true);
				bInCam = true;
			}
		} else {
			if (bInCam) {
				ragdollManager.OnOffPhy (false);
				bike.OnOff (false);
				bInCam = false;
			}
		}
	}

	#region Jump

	bool bJump = false;

	public void JumpMove (int dir)
	{
		if (!bJump)
			return;
		Vector2 vel = rg2d.velocity;
		if (dir < 0) {
			vel.x = -originalSpeed * 0.8F;
		} else {
			vel.x = originalSpeed;
		}
		rg2d.velocity = vel;
	}

	public void Jump (Action jumpDone)
	{
		if (bJump || bEnd) {
			jumpDone.Invoke ();
			return;
		}
		if (bGround) {
			fSpeedY = 0;
			_JumpDone = jumpDone;
			bJump = true;
			Vector2 vel = vel_Jump;
			StopAll ();
			rg2d.velocity = vel;
			if (typeChar == TypeCharacter.Player) {
				CameraControl.Instance.Jump (1);
				PlayerControl.Instance.OnJump (true);
			}
			jointConnect.Jump ();
			StartCoroutine (_Jump ());
		}
	}

	IEnumerator _Jump ()
	{
		yield return new WaitForSeconds (0.2F);
		StartCoroutine (_Jumping ());
		StartCoroutine (CheckJumpEndGround ());
	}

	IEnumerator _Jumping ()
	{
		bool done = false;
		while (!done) {
			Vector2 vel = rg2d.velocity;
			if (vel.y <= -1)
				done = true;
			yield return null;
		}
		jointConnect.JumpDone ();
		if (typeChar == TypeCharacter.Player)
			CameraControl.Instance.EndJump ();
	}

	IEnumerator CheckJumpEndGround ()
	{
		while (!bGround) {
			yield return null;
		}
		rg2d.gravityScale = 2F;
		bJump = false;
		if (typeChar == TypeCharacter.Player)
			CameraControl.Instance.EndJump ();
		jointConnect.EndJump ();
		if (_JumpDone != null) {
			_JumpDone.Invoke ();
			_JumpDone = null;
		}
		if (typeChar == TypeCharacter.Player)
			PlayerControl.Instance.OnJump (false);
		if (!bEnd)
			StopAllCoroutines ();
		if (bHit) {
			bHit = false;
			if (typeChar == TypeCharacter.Player)
				PlayerControl.Instance.ShowHit (false);
		}
	}

	#endregion

	#region Move

	void ChangeSpeed (int dir)
	{
		if (dir > 0) {
			if (typeChar == TypeCharacter.Player) {
				if (PlayerControl.Instance.ratioPower < 0.5F) {
					moveSpeed = (originalSpeed + 6F);
					speedTemp = originalSpeed + (6F * PlayerControl.Instance.ratioPower);
					speedTemp = Mathf.Clamp (speedTemp, originalSpeed, moveSpeed);
				} else {
					moveSpeed = (originalSpeed + 6F);
					float ratio = PlayerControl.Instance.ratioPower;
					ratio -= 0.3F;
					speedTemp = moveSpeed + (moveSpeed * ratio);
				}
			} else {
				moveSpeed = (originalSpeed + 8F);
				speedTemp = moveSpeed * 0.8F;
			}
		} else {
			moveSpeed = (originalSpeed);
		}
	}

	public void AddSpeed (float value)
	{
		originalSpeed += value;
		originalSpeed = Mathf.Clamp (originalSpeed, 0, AIData.Instance.maxSpeedEnemy);
		if (!bUpSpeed)
			moveSpeed = originalSpeed;
	}

	void MoveLeft ()
	{
		if (!bMoveLeft) {
			StopAll ();
			bMoveLeft = true;
			bMoveRight = false;
			speedTemp = 0;
		}
		Move (-1);
	}

	void MoveRight ()
	{
		if (!bMoveRight) {
			StopAll ();
			bMoveLeft = false;
			bMoveRight = true;
			speedTemp = 0;
		}
		Move (1);
	}

	#region Brake

	public void Brake (bool b)
	{
		if (b) {
			moveSpeed = 0;
		} else {
			moveSpeed = originalSpeed;
		}
	}

	#endregion

	void MoveGround ()
	{
		Vector2 vel = rg2d.velocity;
		if (fSpeedY < moveSpeed) {
			fSpeedY += 0.5F;
		}
		vel.y = -fSpeedY;
		rg2d.velocity = vel;
	}

	void Move (int d)
	{
		if (speedTemp < moveSpeed)
			speedTemp += 0.1F * ratioSpeed;
		else
			speedTemp -= 0.25F * ratioSpeed;
		Mathf.Clamp (speedTemp, 0, Mathf.Infinity);
		Vector2 dir = dirMove.position - transform.position;
		dir.Normalize ();
		Vector2 vel = Vector2.zero;
		vel = (speedTemp * d) * dir;
		rg2d.velocity = vel;
	}

	void MoveAngleLeft ()
	{
		if (!bAngleLeft) {
			bAngleLeft = true;
			bAngleIdle = false;
			bAngleRight = false;
			ToSlowSpeed ();
		}
		jointConnect.SyncRo (rangeAngleLeft);
	}

	public void MoveAngle360 ()
	{
		jointConnect.SyncRo360 (angle360);
	}

	#endregion

	#region Angle

	void MoveAngleRight ()
	{
		if (!bAngleLeft) {
			bAngleRight = true;
			bAngleLeft = false;
			bAngleIdle = false;
			UpSpeed ();
		}
		jointConnect.SyncRoRight (rangeAngleRight);
	}



	void Idle ()
	{
		if (!bAngleIdle) {
			bAngleIdle = true;
			bAngleLeft = false;
			bAngleRight = false;
			jointConnect.SyncIdle ();
			if (!bUpSpeed)
				OutSlowSpeed ();
			DownSpeed ();
		}
	}

	void StopAll ()
	{
		ragdollManager.StopAll ();
		jointConnect.StopAll ();
		wheelSync.StopAll ();
		rg2d.velocity = Vector2.zero;
		rg2d.angularVelocity = 0;
	}

	void UpSpeed ()
	{
		if (bUpSpeed || bEnd)
			return;
		bUpSpeed = true;
		ChangeSpeed (1);
	}

	void DownSpeed ()
	{
		if (!bUpSpeed)
			return;
		bUpSpeed = false;
		ChangeSpeed (-1);
	}


	void ToSlowSpeed ()
	{
		if (bSlowSpeed)
			return;
		bSlowSpeed = true;
		moveSpeed = originalSpeed * 1.2F;
	}

	void OutSlowSpeed ()
	{
		if (!bSlowSpeed)
			return;
		bSlowSpeed = false;
		moveSpeed = originalSpeed;
	}

	#endregion

	#region acb

	public void AIStop ()
	{
		StopAll ();
		bEnd = true;
	}

	public void HoldUpSpeed ()
	{
		MoveAngleRight ();
	}

	public void EndHoldUpSpeed ()
	{
		Idle ();
	}

	bool bSlide = false;

	public bool Slide (float duration, Action cb)
	{
		if (bEnd || bSlide || bJump) {
			if (!bJump)
				return false;
			else {
				StartCoroutine (_SlideJump (duration, cb));
				return true;
			}
		} else {
			if (bHit) {
				bHit = false;
				if (_JumpDone != null) {
					_JumpDone.Invoke ();
					_JumpDone = null;
				}
				if (!bEnd)
					StopAllCoroutines ();
			}
			bSlide = true;
			StartCoroutine (_Slide (duration, cb));
			return true;
		}
	}

	IEnumerator _SlideJump (float duration, Action cb)
	{
		bool done = false;
		float timer = 0;
		while (!done) {
			timer += Time.deltaTime;
			rg2d.gravityScale = 0.1F;
			//MoveAngleLeft ();
			MoveAngle360 ();
			if (timer > duration)
				done = true;
			yield return null;
		}
		if (cb != null)
			cb.Invoke ();
		rg2d.gravityScale = 2F;
	}

	IEnumerator _Slide (float duration, Action cb)
	{
		bool done = false;
		float timer = 0;
		while (!done) {
			timer += Time.deltaTime;
			MoveAngleLeft ();
			//MoveAngle360 ();
			if (timer > duration)
				done = true;
			yield return null;
		}
		if (cb != null)
			cb.Invoke ();
		bSlide = false;
		Idle ();
	}

	#endregion


	#region EndPlay

	public void RegisterOnEnd (Action a)
	{
		listActionEnd.Add (a);
	}

	public void End ()
	{
		if (bEnd)
			return;

		//ClearData
		for (int i = 0; i < listActionEnd.Count; i++) {
			listActionEnd [i].Invoke ();
		}
		//ClearData
		StopAllCoroutines ();
		if (typeChar == TypeCharacter.Player) {
			if (bHit) {
				bHit = false;
				PlayerControl.Instance.ShowHit (false);
			}
		} else {
			AIEnemyManager.Instance.RemoveEnemy ();
			LeaderboardManager.Instance.RemoveRank (status.ID);

		}
		bJump = false;
		bCheckOut = true;
		bEnd = true;
		moveSpeed = 1;
		bMoveRight = true;
		jointConnect.ToEnd ();
		wheelSync.StopAll ();
		BreakWeapon ();
		StartCoroutine (_End ());
	}

	bool bCheckOut = false;

	IEnumerator _End ()
	{
		bool done = false;
		while (!done) {
			bGround = lineCastCheck.Check ();
			if (bGround && !bJump)
				MoveRight ();
			if (speedTemp <= 1)
				done = true;
			yield return null;
		}
	}

	void OnTriggerExit2D (Collider2D coll)
	{
		if (!bCheckOut)
			return;
		if (typeChar == TypeCharacter.Player)
			return;
		if (coll.CompareTag ("OutRange")) {
			BreakWeapon ();
			Destroy (gameObject);
		}
	}

	void OnDisable ()
	{
		StopAllCoroutines ();
	}

	#endregion

	#region OnAttack

	bool bAttack = false;

	public void Attack ()
	{
		if (bAttack)
			return;
		bAttack = true;
		speedTemp /= 2;
		TaskUtil.Schedule (this, this._Attack, 0.1F);
	}

	void _Attack ()
	{
		bAttack = false;
	}

	#endregion

	#region OnHit

	bool bHit = false;

	public void OnHit (object ob)
	{
		if (bHit)
			return;
		if (typeChar == TypeCharacter.Player)
			PlayerControl.Instance.ShowHit (true);
		HitDir hd = (HitDir)ob;
		bHit = true;
		if (hd == HitDir.Top) {
			jointConnect.LimitHit ();
		} else if (hd == HitDir.Bottom) {
			
		} else if (hd == HitDir.Left) {
			speedTemp += 2;
		} else {
			speedTemp += 2;
		}
		if (_JumpDone != null) {
			_JumpDone.Invoke ();
			_JumpDone = null;
		}
		TaskUtil.Schedule (this, this._OnHit, 0.25F);
	}

	void _OnHit ()
	{
		bHit = false;
		if (typeChar == TypeCharacter.Player)
			PlayerControl.Instance.ShowHit (false);
		if (!bJump)
			jointConnect.OffHit ();
	}

	#endregion

	#region BreakWeapon

	Action _BreakWeapon = null;

	void BreakWeapon ()
	{
		if (_BreakWeapon != null)
			_BreakWeapon.Invoke ();
	}

	public void RegisterBreakWeapon (Action a)
	{
		_BreakWeapon = a;
	}

	#endregion

	#region EndRace

	bool bEndRace = false;

	public void EndRace ()
	{
		if (bEndRace)
			return;
		bEndRace = true;
		moveSpeed = 0;
		TaskUtil.Schedule (this, _EndRace, 2F);
		StopAll ();
	}

	public void OffPhysic ()
	{
		jointConnect.ToEnd ();
		ragdollManager.StopAll ();
		jointConnect.StopAll ();
		wheelSync.StopAll ();
		rg2d.velocity = Vector2.zero;
		rg2d.angularVelocity = 0;
		BreakWeapon ();
	}

	void _EndRace ()
	{
		StopAll ();
	}

	#endregion

	#region NextLap

	void NextLap ()
	{
		if (bEnd)
			return;
		bEnd = true;
		ragdollManager.ActiveEffect (false);
		StopAll ();
		StartCoroutine (__NextLap ());
		TaskUtil.Schedule (this, _NextLap, 1F);
	}

	void _NextLap ()
	{
		ragdollManager.ActiveEffect (true);
	}

	IEnumerator __NextLap ()
	{
		yield return new WaitForEndOfFrame ();
		float x = MapManager.Intance.GetXStart;
		Vector2 pos = transform.position;
		pos.x = x;
		transform.position = pos;
		jointConnect.Reset ();
		ragdollManager.Reset ();
		wheelSync.Reset ();
		jointConnect.SyncIdle ();
		yield return new WaitForEndOfFrame ();
		bEnd = false;
		if (typeChar == TypeCharacter.Player) {
			LeaderboardManager.Instance.NextLap (status.ID);
			MapManager.Intance.NextLap ();
		} else {
			LeaderboardManager.Instance.NextLap (status.ID);
		}
		ragdollManager.OnOffPhy (true);
		bike.OnOff (true);
		bInCam = true;
	}

	public void ResetWeapon ()
	{
		jointConnect.Reset ();
		ragdollManager.Reset ();
		wheelSync.Reset ();
		jointConnect.SyncIdle ();
	}

	#endregion


	#region ChangeSpeed

	public void ChangeSpeed (float _moveSpeed)
	{
		moveSpeed = _moveSpeed;
	}

	public void ResetSpeed ()
	{
		moveSpeed = originalSpeed;
	}

	public void AIBrake ()
	{
		speedTemp = 1F;
	}

	#endregion

	#region SetupSkin

	public void SetSkin (Color[] colors, int idSkin)
	{
		ragdollManager.SetLayerNameAndColor (colors, idSkin);
	}

	[SerializeField]Bike bike = null;

	#endregion
}
