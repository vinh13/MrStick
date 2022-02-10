using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(GiftCode))]
public class GiftCodeEditor : Editor
{
	GiftCode giftCode = null;

	void OnEnable ()
	{
		giftCode = (GiftCode)target;
	}

	public override void OnInspectorGUI ()
	{
		if (GUILayout.Button ("SaveObject")) {
			EditorUtility.SetDirty (giftCode);
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh ();
		}
		base.OnInspectorGUI ();
	}
}
