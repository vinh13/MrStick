using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGifManager : MonoBehaviour
{
	[SerializeField]GifChil[] gifChils;
	[SerializeField]float rangeX = 0;
	string keyPrefix = "LevelGif_";
	bool[] bActives;
	int[] starGif = {
		10, 20, 30, 50,
	};
	int totalStar = 0;

	public void Register (int _totalStar)
	{
		totalStar = _totalStar;
		bActives = new bool[starGif.Length];
	}

	void Start ()
	{
		Setup ();
	}

	public void Setup ()
	{
		for (int i = 0; i < gifChils.Length; i++) {
			float ratio = (float)starGif [i] / (float)totalStar;
			gifChils [i].Register (GetGif, i, ratio, keyPrefix + "" + i, rangeX);
		}
	}

	void GetGif (object ob)
	{
		GifLevel gifLevel = (GifLevel)ob;
		//Do something
	}

	public void SyncGif (int star)
	{
		for (int i = 0; i < bActives.Length; i++) {
			bActives [i] = starGif [i] <= star;
			gifChils [i].Active (bActives [i]);
		}
	}

	void Update ()
	{
		if (Input.GetKey (KeyCode.A)) {
			SyncGif (100);
		}
	}
}
