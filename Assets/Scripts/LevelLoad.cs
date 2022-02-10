#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Object = System.Object;

public class LevelLoad
{
	[MenuItem ("All/WriteAllData_Spawner")]
	public static void CreateAsset ()
	{
		Object[] sp = Resources.LoadAll ("SpawnData", typeof(SpawnData));
		string path = System.IO.Directory.GetCurrentDirectory ();
		foreach (Object t in sp) {
			SpawnData temp = t as SpawnData;
			try {
				EnemyData[] enemiesData = datas (path + "/Assets/DataExcel/levels/" + temp.name + ".csv");
				if (enemiesData.Length != 0) {
					for (int i = 0; i < enemiesData.Length - 1; i++) {
						enemiesData [i].enemyType = EnemyType.None;
					}
					enemiesData [enemiesData.Length - 1].enemyType = EnemyType.Boss;
					EditorUtility.SetDirty (t as SpawnData);
					temp.enemiesData = enemiesData;
				}
			} catch {
				Debug.Log (temp.name);
			}

		}
		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh ();
	}

	static EnemyData[] datas (string path)
	{
		List<EnemyData> list = new List<EnemyData> ();
		string[] texts = ReadCSV.DataCSV (path);
		for (int i = 1; i < texts.Length; i++) {
			list.Add (enemyData (texts [i]));
		}
		return list.ToArray ();
	}

	static EnemyData enemyData (string s)
	{
		string[] texts = s.Split (',');
		EnemyData temp = new EnemyData ();
		temp.health = float.Parse (texts [1]);
		temp.damageBase = float.Parse (texts [3]);
		temp.speed = float.Parse (texts [4]);
		temp.IDSkin = int.Parse (texts [5]);
		temp.aiLevel = aiLevel (texts [2]);
		return temp;
	}

	static AILevel aiLevel (string text)
	{
		AILevel temp = AILevel.Easy;
		foreach (AILevel AI in Enum.GetValues(typeof(AILevel))) {
			if (text == AI.ToString ()) {
				temp = (AILevel)AI;
				break;
			}
		}
		if (temp == AILevel.None)
			temp = AILevel.Easy;
		return temp;
	}
}
#endif
