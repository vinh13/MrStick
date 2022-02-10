using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CacheItem : MonoBehaviour
{
	List<Action> Pick = new List<Action> ();

	public void RegisterPick (Action a)
	{
		Pick.Add (a);
	}

	public bool bPicked = false;
	[SerializeField]ObjectType objectType = ObjectType.None;

	public ObjectType GetType {
		get {
			return objectType;
		}
	}

	public void OnPick ()
	{
		for (int i = 0; i < Pick.Count; i++) {
			Pick [i].Invoke ();
		}
	}

	public void SetType (ObjectType ob)
	{
		objectType = ob;
	}
}
