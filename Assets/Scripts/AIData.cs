using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum AIDataLevel
{
	None = 0,
	Easy = 1,
	Normal = 2,
	Hard = 3,
	Hardcore = 4,
}

public class AIData : MonoBehaviour
{
	public static AIData Instance = null;
	public AIDataLevel all_AILevel = AIDataLevel.None;

	public float maxSpeedEnemy {
		get {
			switch (all_AILevel) {
			case AIDataLevel.None:
				return 9;
			case AIDataLevel.Easy:
				return 11;
			case AIDataLevel.Normal:
				return 12;
			case AIDataLevel.Hard:
				return 13;
			case AIDataLevel.Hardcore:
				return 14;
			default:
				return 10;
			}
		}
	}

	public int GetMaxEnemyLap_point {
		get { 
			switch (all_AILevel) {
			case AIDataLevel.None:
				return 2;
			case AIDataLevel.Easy:
				return 3;
			case AIDataLevel.Normal:
				return 3;
			case AIDataLevel.Hard:
				return 4;
			case AIDataLevel.Hardcore:
				return 4;
			default:
				return 2;
			}
		}
	}

	public float GetTimeLap_point {
		get { 
			switch (all_AILevel) {
			case AIDataLevel.None:
				return 12;
			case AIDataLevel.Easy:
				return 10;
			case AIDataLevel.Normal:
				return 10;
			case AIDataLevel.Hard:
				return 8;
			case AIDataLevel.Hardcore:
				return 8;
			default:
				return 12;
			}
		}
	}

	void Awake ()
	{
		if (Instance == null) {
			Instance = this;
		}
		all_AILevel = LevelData.aiDataLevel;
	}
}
