using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectHit : MonoBehaviour
{
	Action<object> _hit = null;
	public TypeHit typeHit = TypeHit.None;
	public HitObject hiObject = HitObject.None;
	[HideInInspector]public float damage = 0;
	private bool _bHited = false;

	public bool hited {
		get { 
			return _bHited;
		}
		set { 
			_bHited = value;
		}
	}

	public void Register (Action<object> a, TypeHit tHit)
	{
		_hit = a;
		typeHit = tHit;
	}

	public void OnHit (object ob = null)
	{
		_bHited = true;
		_hit.Invoke (ob);	
	}

	void OnEnable ()
	{
		_bHited = false;
	}
}
