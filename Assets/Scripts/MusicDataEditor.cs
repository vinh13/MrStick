#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(MusicData))]
public class MusicDataEditor : Editor
{
	MusicData ad = null;

	void OnEnable ()
	{
		ad = (MusicData)target;
	}

	public override void OnInspectorGUI ()
	{	
		if (GUILayout.Button ("SaveData")) {
			EditorUtility.SetDirty (ad);
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh ();
		}
		base.OnInspectorGUI ();
	}
}
#endif
