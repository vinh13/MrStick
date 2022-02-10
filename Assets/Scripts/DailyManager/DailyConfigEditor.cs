#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(DailyConfig))]
public class DailyConfigEditor : Editor
{
	DailyConfig dailyConfig = null;

	void OnEnable ()
	{
		dailyConfig = (DailyConfig)target;
	}

	public override void OnInspectorGUI ()
	{

		if (GUILayout.Button ("SaveData")) {

			EditorUtility.SetDirty (dailyConfig);
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh ();
		}
		base.OnInspectorGUI ();
	}
}
#endif
