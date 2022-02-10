using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateChecking : MonoBehaviour
{
	void Start ()
	{
		StartCoroutine (checkUpdate ());
	}

	IEnumerator checkUpdate ()
	{
		while (!TdzRemote.RemoveSyncDone) {
			yield return null;
		}
		if (TdzRemote.UpdateVersion) {
			UpdateVS ();
		}
	}

	void UpdateVS ()
	{
		if (!GameData.CheckUpdate) {
			UpdateCantro uct = Instantiate (Resources.Load<GameObject> ("UI/UIUpdate"), transform.GetChild (0))
				.GetComponent<UpdateCantro> ();
			uct.Register (Exit);
		}
	}

	void Exit ()
	{
					
	}
}
