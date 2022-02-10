using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
[CustomEditor (typeof(ChestCantro))]
public class ChestCantroEditor : Editor
{
	ChestCantro cc = null;

	void OnEnable ()
	{
		cc = (ChestCantro)target;
	}
	public override void OnInspectorGUI ()
	{
		if (GUILayout.Button ("SyncData")) {
			cc._OnValidate ();
		}
		base.OnInspectorGUI ();
	}
}
