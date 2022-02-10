using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(Character))]
public class CharacterEditor : Editor
{
	Character c = null;

	void OnEnable ()
	{
		c = (Character)target;
	}

	public override void OnInspectorGUI ()
	{
		if (GUILayout.Button ("SaveData")) {
			for (int i = 0; i < c.config.Length; i++) {
				c.config [i].SPD = 1F + (float)i * 0.01F;
			}
			EditorUtility.SetDirty (c);
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh ();
		}
		base.OnInspectorGUI ();
	}

}
