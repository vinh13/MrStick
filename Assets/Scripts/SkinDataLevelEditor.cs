#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor (typeof(SkinDataLevel))]
public class SkinDataLevelEditor : Editor
{
	SkinDataLevel sk = null;
	void OnEnable ()
	{
		sk = (SkinDataLevel)target;
	}

	public override void OnInspectorGUI ()
	{
		if (GUILayout.Button ("ClearData")) {
			sk.skinDatas = new List<string> ();
		}
		base.OnInspectorGUI ();
	}
}
#endif
