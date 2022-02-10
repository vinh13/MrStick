#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(AudioData))]
public class AudioDataEditor : Editor
{
	AudioData ad = null;

	void OnEnable ()
	{
		ad = (AudioData)target;
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
