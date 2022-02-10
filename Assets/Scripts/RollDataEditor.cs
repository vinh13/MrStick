#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(RollData))]
public class RollDataEditor : Editor
{
	RollData sd = null;

	void OnEnable ()
	{
		sd = (RollData)target;
	}

	public override void OnInspectorGUI ()
	{
		if (GUILayout.Button ("Save")) {
			EditorUtility.SetDirty (sd);
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh ();
		}
		base.OnInspectorGUI ();
	}
}
#endif
