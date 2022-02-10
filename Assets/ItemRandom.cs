using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRandom : MonoBehaviour
{
	[SerializeField]ObjectType[] typeObjects;
	[SerializeField]Transform[] tRenders;
	CacheItem cacheItem = null;
	int lateItem = 0;
	int max = 0;

	void Start ()
	{
		max = typeObjects.Length;
		ItemScript item = GetComponent<ItemScript> ();
		cacheItem = GetComponent<CacheItem> ();
		for (int i = 0; i < typeObjects.Length; i++) {
			ObjectContainer.Instance.ReisterObjectPool (typeObjects [i]);
		}
		RandomItem ();
		item.RegisterPick (RandomItem);
	}

	void RandomItem ()
	{
		tRenders [lateItem].gameObject.SetActive (false);
		int index = IndexItem ();
		tRenders [index].gameObject.SetActive (true);
		cacheItem.SetType (typeObjects [index]);
	}

	int IndexItem ()
	{
		int index = Random.Range (1, max + 1) - 1;
		if (lateItem == index) {
			index = IndexItem ();
		} else {
			lateItem = index;
		}
		return index;
	}
}
