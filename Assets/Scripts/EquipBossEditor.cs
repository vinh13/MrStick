#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor (typeof(EquipBoss))]
public class EquipBossEditor : Editor
{
	EquipBoss eq = null;
	void OnEnable ()
	{
		eq = (EquipBoss)target;
	}
	public override void OnInspectorGUI ()
	{
		if (GUILayout.Button ("CreateData")) {
			EquipConfig[] list = eq.SaveData ();
			SkinDataLevel[] skinDataLevel = eq.SaveSkinDataLevel ();
			for (int i = 0; i < list.Length; i++) {
				EditorUtility.SetDirty (list [i]);
				EditorUtility.SetDirty (skinDataLevel [i]);
			}
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh ();
		}
		base.OnInspectorGUI ();
	}
}
#endif
