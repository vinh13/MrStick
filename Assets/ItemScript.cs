using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[SelectionBase]
public class ItemScript : MonoBehaviour
{
	[SerializeField]Transform _tRender = null;
	[SerializeField]float delay = 1F;
	CacheItem cacheItem = null;
	List<Action> listCall = new List<Action> ();
	bool bActive = false;
	string[] _tags = new string[] { "Untagged",
		"Item"
	};
	public void RegisterPick (Action a)
	{
		listCall.Add (a);
	}

	void Awake ()
	{
		bActive = false;
		tActive (false);
		delay = 0.25F;
	}

	void Start ()
	{
		cacheItem = GetComponent<CacheItem> ();
		cacheItem.RegisterPick (OnPick);
		cacheItem.bPicked = false;
		ItemManager.Instance.CreateItem (this.transform.position.x,
			this.transform.Find ("Effect").GetComponent<SpriteRenderer> ().color);
		if (GetComponent<ItemRandom> () == null)
			ObjectContainer.Instance.ReisterObjectPool (cacheItem.GetType);
	}

	void OnPick ()
	{
		Active (false);
		cacheItem.bPicked = true;
		TaskUtil.ScheduleWithTimeScale (this, ResetPick, delay);
	}

	void ResetPick ()
	{
		for (int i = 0; i < listCall.Count; i++) {
			listCall [i].Invoke ();
		}
		Active (true);
		cacheItem.bPicked = false;
	}

	void Active (bool b)
	{
		tag = _tags [b ? 1 : 0];
		_tRender.gameObject.SetActive (b);
	}

	void OnTriggerStay2D (Collider2D coll)
	{
		if (bActive)
			return;
		if (coll.CompareTag ("OutRange")) {
			bActive = true;
			tActive (true);
		}
	}

	void OnTriggerExit2D (Collider2D coll)
	{
		if (!bActive)
			return;
		if (coll.CompareTag ("OutRange")) {
			bActive = false;
			tActive (false);
		}
	}

	void tActive (bool b)
	{
		this.transform.GetChild (0).gameObject.SetActive (b);
		this.transform.GetChild (1).gameObject.SetActive (b);
	}
}

