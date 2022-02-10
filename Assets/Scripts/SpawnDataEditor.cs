#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor (typeof(SpawnData))]
public class SpawnDataEditor : Editor
{
	SpawnData sd = null;

	void OnEnable ()
	{
		sd = (SpawnData)target;
	}

	public override void OnInspectorGUI ()
	{

		if (GUILayout.Button ("LoadData")) {
			sd.enemiesData = datas (sd.path + sd.name + ".csv");
		}
		if (GUILayout.Button ("SaveData")) {
			EnemyData[] enemiesData = sd.enemiesData;
			for (int i = 0; i < enemiesData.Length - 1; i++) {
				enemiesData [i].enemyType = EnemyType.None;
			}
			enemiesData [enemiesData.Length - 1].enemyType = EnemyType.Boss;
			EditorUtility.SetDirty (sd);
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh ();
		}
		base.OnInspectorGUI ();
	}

	EnemyData[] datas (string path)
	{
		List<EnemyData> list = new List<EnemyData> ();
		string[] texts = ReadCSV.DataCSV (path);
		for (int i = 1; i < texts.Length; i++) {
			list.Add (enemyData (texts [i]));
		}
		return list.ToArray ();
	}

	EnemyData enemyData (string s)
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

	AILevel aiLevel (string text)
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