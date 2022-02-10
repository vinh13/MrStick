using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum EnemyType
{
	None = 0,
	Boss = 1,
}

[System.Serializable]
public class EnemyData
{
	public float health = 0;
	public AILevel aiLevel = AILevel.None;
	public float damageBase = 10F;
	public float speed = 1F;
	public EnemyType enemyType = EnemyType.None;
	public int IDSkin = 0;
}

public class SpawnData : ScriptableObject
{
	public string path = "D:\\DataExcel\\OnWheel\\Levels\\";
	public float timeNextEnemy = 0;
	public float rangeX = 0;
	public int numberPosP = 0;
	public int numberEnemyInMap = 0;
	public int maxEnemyInMap = 0;
	public EnemyData[] enemiesData;
}
