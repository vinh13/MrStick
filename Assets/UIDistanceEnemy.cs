using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDistanceEnemy : MonoBehaviour
{
	string path = "UI/imgEnemy";
	[SerializeField]float fRange = 400;
	[SerializeField]RectTransform[] rects;
	[SerializeField]bool[] bActives;

	public void CreateUI (int number)
	{
		rects = new RectTransform[number];
		bActives = new bool[number];
		for (int i = 0; i < number; i++) {
			rects [i] = Enemy.GetComponent<RectTransform> ();
			rects [i].gameObject.SetActive (false);
		}
	}

	GameObject Enemy {
		get { 
			return Instantiate (Resources.Load<GameObject> (path), this.transform);
		}
	}

	public void Change (float ratio, int index)
	{
		float xNew = fRange * ratio;
		rects [index].transform.localPosition = new Vector3 (xNew, 0, 0);
	}

	public int GetIndex {
		get { 
			int temp = 0;
			for (int i = 0; i < bActives.Length; i++) {
				if (bActives [i] == false) {
					temp = i;
					rects [i].gameObject.SetActive (true);
					bActives [i] = true;
					break;
				}
			}
			return temp;
		}
	}

	public void RemoveIndex (int index)
	{
		bActives [index] = false;
		rects [index].gameObject.SetActive (false);
	}

	public void DisableRank ()
	{
		for (int i = 0; i < rects.Length; i++) {
			rects [i].gameObject.SetActive (false);
		}
	}

}
