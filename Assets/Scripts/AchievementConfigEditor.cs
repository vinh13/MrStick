#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor (typeof(AchievementConfig))]
public class AchievementConfigEditor : Editor
{
	AchievementConfig config = null;

	void OnEnable ()
	{
		config = (AchievementConfig)target;
	}

	public override void OnInspectorGUI ()
	{
		if (config.AchievementName != "AchievementConfig") {
			config.ID = (AchievementID)Enum.Parse (typeof(AchievementID), config.name);
		}
		if (GUILayout.Button ("SaveData")) {
			EditorUtility.SetDirty (config);
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh ();
		}
		base.OnInspectorGUI ();
	}
}
#endif
