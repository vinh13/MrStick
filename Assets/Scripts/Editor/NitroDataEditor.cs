using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(NitroData))]
public class NitroDataEditor : Editor
{
	NitroData data = null;

	void OnEnable ()
	{
		data = (NitroData)target;
	}

	public override void OnInspectorGUI ()
	{
		if (GUILayout.Button ("SaveData")) {
			TextAsset textAsset = Resources.Load<TextAsset> ("PlayerLevel/Nitro");
			string text = textAsset.text;
			data.nitroConfigs = nitroConfigs (text);
			EditorUtility.SetDirty (data);
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh ();
		}
		base.OnInspectorGUI ();
	}

	NitroConfig[] nitroConfigs (string text)
	{
		string[] texts = text.Split ('\n');
		List<NitroConfig> list = new List<NitroConfig> ();
		for (int i = 0; i < texts.Length; i++) {
			list.Add (entry (texts [i]));
		}
		return list.ToArray ();
	}

	NitroConfig entry (string text)
	{
		string[] texts = text.Split ('\t');
		NitroConfig temp = new NitroConfig ();
		temp.display = float.Parse (texts [0]);
		string[] time = texts [1].Split (',');
		string newTime = time [0] + "." + time [1];
		temp.time = float.Parse (newTime, System.Globalization.NumberStyles.AllowDecimalPoint);
		temp.damage = float.Parse (texts [2]);
		return temp;
	}
}
