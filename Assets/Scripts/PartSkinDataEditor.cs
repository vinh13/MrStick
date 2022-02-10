#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor (typeof(PartSkinData))]
public class PartSkinDataEditor : Editor
{
	PartSkinData t = null;

	void OnEnable ()
	{
		t = (PartSkinData)target;
	}

	public override void OnInspectorGUI ()
	{
		if (GUILayout.Button ("LoadData")) {
			List<SkinID> list = new List<SkinID> ();
			string path = AssetDatabase.GetAssetPath (t);
			string[] paths = path.Split ('/');
			GameObject[] gos = Resources.LoadAll<GameObject> (
				                   paths [2] + "/" +
				                   paths [3]);
			for (int i = 0; i < gos.Length; i++) {
				list.Add (GetSkinID (gos [i].name));
			}

			t.skinID = list.ToArray ();
			t.MaxPart = list.Count;

			EditorUtility.SetDirty (t);
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh ();


		}
		base.OnInspectorGUI ();
	}

	SkinID GetSkinID (string text)
	{
		SkinID temp = SkinID.None;
		foreach (SkinID ID in Enum.GetValues(typeof(SkinID))) {
			if (ID.ToString ().Equals (text)) {
				temp = (SkinID)ID;
				break;
			}
		}
		return temp;
	}
}
#endif

