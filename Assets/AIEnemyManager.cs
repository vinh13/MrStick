using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIEnemyManager : MonoBehaviour
{
	int maskGround = 0;
	public static AIEnemyManager Instance = null;
	[SerializeField]Transform _tP = null, _posPlayer = null;
	[SerializeField]UIDistanceEnemy uiDistanceEnemy = null;
	[SerializeField]int numberEnemy = 0;
	[SerializeField]EnemyContainer enemyContainer = null;
	[SerializeField]float range = 0;
	[SerializeField]int numberPosP = 2;
	[SerializeField]int numberEnemyInMap = 2;
	[SerializeField]UIBoss uiBoss = null;
	float rangeX = 0;
	Vector2 posLeft = Vector2.zero;
	Vector2 posRight = Vector2.zero;
	[SerializeField]bool bDontCreate = false;
	[SerializeField]bool bTest = false;
	SpawnData spawnData = null;
	[SerializeField]int iLevel = 1;
	int aliveInMap = 0;
	bool bTutorial = false;

	public void ShowBoss (bool b)
	{
		if (bTutorial)
			return;
		uiBoss.ShowBoss (b);
		if (b) {
			Logic.bBoss = true;
			EnemyManager.Instance.ShowBoss ();
			SFXManager.Instance.Play ("warning");
		} else {
			Logic.bBoss = false;
		}
	}

	public HPBar GetHpBarBoss {
		get { 
			return uiBoss.GetHpBar;
		}
	}

	public void DisableDistance ()
	{
		uiDistanceEnemy.DisableRank ();
	}

	public float rangeEnemy {
		get {
			return range;
		}
	}

	private int RegsiterIndexChange {
		get { 
			return uiDistanceEnemy.GetIndex;
		}
	}

	public void RemoveIndexChange (int index)
	{
		uiDistanceEnemy.RemoveIndex (index);
	}

	public void ChangeDistance (float x, int index)
	{
		float ratio = MapManager.Intance.GetRatioDistance (x);
		uiDistanceEnemy.Change (ratio, index);
	}

	public Transform getPlayer {
		get { 
			return _posPlayer;
		}
	}

	void Awake ()
	{
		Logic.bReady = false;
		Logic.bBoss = false;
		EnemyLogic.lateTObjectType = ObjectType.None;
		if (!bTest) {
			iLevel = LevelData.IDLevel + 1;
		}
		if (Instance == null)
			Instance = this;
		string p = "level" + iLevel;
		if (!TutorialData.bTutorialStart) {
			bTutorial = false;
			spawnData = Resources.Load<SpawnData> ("SpawnData/" + p);
		} else {
			bTutorial = true;
			spawnData = Resources.Load<SpawnData> ("SpawnData/Tutorial");
		}
		numberEnemy = spawnData.enemiesData.Length;
		range = spawnData.rangeX;
		numberPosP = spawnData.numberPosP;
		numberEnemyInMap = spawnData.numberEnemyInMap;
	}

	void Start ()
	{
		if (bDontCreate)
			return;
		maskGround = 10;
		Vector2 pos = _posPlayer.position;
		enemyContainer.CreateEnemies (numberEnemy, spawnData.enemiesData);
		EnemyManager.Instance.Setup (numberEnemy);
		bool bLeft = false;
		int indexLeft = 0;
		int indexRight = 0;
		uiDistanceEnemy.CreateUI (5);
		for (int i = 0; i < numberEnemyInMap; i++) {
			bLeft = !bLeft;
			Vector2 p = pos;
			if (bLeft) {
				indexLeft++;
				p.x += indexLeft * range;
			} else {
				indexRight++;
				p.x -= indexRight * range;
			}
			AIInterface ai = enemyContainer.GetAIInterface;
			ai.Active (true, p);
			ai.StartRace ();
			ai.RegisterIndexDistance (RegsiterIndexChange);
			numberEnemy--;
			aliveInMap++;
		}
		SpawnNext ();
	}

	public void GetAllEnemy ()
	{
		StopAllCoroutines ();
		float time = 0;
		if (numberEnemy > 2)
			numberEnemy = 1;
		for (int i = 0; i < numberEnemy; i++) {
			time++;
			StartCoroutine (_getllAlllEnemy (i));
		}
		TaskUtil.Schedule (this, Lose, (time + 1));
	}

	void Lose ()
	{
		UIManager.Instance.Lose ();	
	}

	IEnumerator _getllAlllEnemy (float timer)
	{
		yield return new WaitForSeconds (timer);
		SpawnEnemy ();
	}

	bool bWin = false;

	void _RemoveEnemy ()
	{
		aliveInMap--;
		EnemyManager.Instance.RemoveEnemy ();
		if (numberEnemy > 1) {
			SpawnEnemy ();
		} else if (numberEnemy == 1) {
			if (!bTutorial) {
				bSpawnBoss = true;
				TaskUtil.ScheduleWithTimeScale (this, this.WarringBoss, 1F);
			} else {
				SpawnEnemy ();
			}
		} else {
			if (!bWin) {
				if (EnemyManager.Instance.CheckEnemy) {
					bWin = true;
					uiBoss.OnHide ();
				}
			}
		}
	}

	public void RemoveEnemy ()
	{
		TaskUtil.ScheduleWithTimeScale (this, this._RemoveEnemy, 1F);
	}

	void Win ()
	{
		UIManager.Instance.Win (true);
	}

	[HideInInspector]public bool bSpawnBoss = false;
	bool bBossWarring = false;

	void WarringBoss ()
	{
		if (!bBossWarring) {
			bBossWarring = true;
			SpawnBoss ();
		}
	}

	void SpawnBoss ()
	{
		StartCoroutine (_SpawnBoss ());
	}

	IEnumerator _SpawnBoss ()
	{
		while (aliveInMap > 0 && !EnemyManager.Instance.CheckBoss) {
			yield return null;
		}
		uiBoss.IntroBoss ();
		TaskUtil.ScheduleWithTimeScale (this, this.SpawnEnemy, 1F);
	}

	void SpawnEnemy ()
	{
		numberEnemy--;
		if (numberEnemy < 0)
			return;
		Sync ();
		Vector2 p = posSpawn (posLeft);
		AIInterface ai = enemyContainer.GetAIInterface;
		if (ai == null)
			return;
		ai.Active (true, p);
		ai.StartVehicle ();
		ai.RegisterIndexDistance (RegsiterIndexChange);
		aliveInMap++;
	}

	void Sync ()
	{
		Vector3 pos = _tP.position;
		pos.z = 0;
		float xW = CameraControl.Instance.GetX;
		rangeX = xW;
		posLeft = pos;
		posLeft.x -= rangeX;
		posRight = pos;
		posRight.x += rangeX;
	}

	Vector2 posSpawn (Vector2 posStart)
	{
		Vector2 origi = posStart;
		origi.y += 20;
		RaycastHit2D hit2d = Physics2D.Raycast (origi, Vector2.down, 200F, 1 << maskGround);
		if (hit2d.collider == null) {
			return posStart;
		} else {
			Vector2 pos = hit2d.point;
			pos.y += 2.2F;
			return pos;
		}
	}

	bool bCanSpawn = false;
	bool bLapPoint = false;

	public bool BLapPoint {
		get {
			return bLapPoint;
		}
	}

	IEnumerator _SpawnNextCoroutine = null;

	public void LapPoint ()
	{
		bLapPoint = true;
		bCanSpawn = false;
		if (!bSpawnBoss)
			SpawnNext ();
	}

	void SpawnNext ()
	{
		if (numberEnemy > 0) {
			float time = bLapPoint ? AIData.Instance.GetTimeLap_point : spawnData.timeNextEnemy;
			TaskUtil.Schedule (this, this._SpawnNext, time);
		}
	}

	void _SpawnNext ()
	{
		if (bCanSpawn) {
			//Do something
		} else {
			bCanSpawn = true;
			_SpawnNextCoroutine = _NextEnemy ();
			StartCoroutine (_SpawnNextCoroutine);
		}
		SpawnNext ();
	}

	IEnumerator _NextEnemy ()
	{
		while (bCanSpawn) {
			int count = bLapPoint ? AIData.Instance.GetMaxEnemyLap_point : spawnData.maxEnemyInMap;
			if (aliveInMap < count) {
				if (numberEnemy > 1) {
					SpawnEnemy ();
				}
				bCanSpawn = false;
			}
			yield return null;
		}
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.B)) {
			LapPoint ();
		}
	}
}
