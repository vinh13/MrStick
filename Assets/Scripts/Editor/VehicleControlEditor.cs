using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(VehicleControl))]
public class VehicleControlEditor :Editor
{
	VehicleControl vc = null;

	void OnEnable ()
	{
		vc = (VehicleControl)target;
	}

	public override void OnInspectorGUI ()
	{
		if (vc.typeChar == TypeCharacter.Enemy) {
			if (GUILayout.Button ("ChangeAlpha")) {
				SpriteRenderer[] sprs = vc.transform.GetComponentsInChildren<SpriteRenderer> ();
				for (int i = 0; i < sprs.Length; i++) {
					ChangeAlpha (sprs [i]);
				}
			}
			if (GUILayout.Button ("ChangeLayerEnemyVehicle")) {
				Transform[] chils = vc.gameObject.GetComponentsInChildren<Transform> ();
				for (int i = 0; i < chils.Length; i++) {
					chils [i].gameObject.layer = 14;
				}
			}
			if (GUILayout.Button ("ChangeLayerRagdollEnemy")) {
				Transform[] chils = vc.transform.GetChild (1).GetComponentsInChildren<Transform> ();
				for (int i = 0; i < chils.Length; i++) {
					chils [i].gameObject.layer = 13;
				}
			}
			if (GUILayout.Button ("CreateAIEnemy")) {
				AIConnect ai = vc.GetComponent<AIConnect> ();
				PickObject pb = vc.GetComponent<PickObject> ();
				if (ai == null) {
					ai = vc.gameObject.AddComponent<AIConnect> ();
				}
				if (pb == null) {
					pb = vc.gameObject.AddComponent<PickObject> ();
				}
			}
		}
		base.OnInspectorGUI ();
	}

	void ChangeAlpha (SpriteRenderer spr)
	{
		Color32 color = spr.color;
		color.a = 100;
		spr.color = color;
	}

}
