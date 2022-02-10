using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class panelStars : MonoBehaviour
{
	[SerializeField]RectTransform[] rectStar;
	[SerializeField]Image[] imgs;
	public void SetStar (int star)
	{
		for (int i = 0; i < rectStar.Length; i++) {
			if (i < star) {
				rectStar [i].gameObject.SetActive (true);
			} else {
				rectStar [i].gameObject.SetActive (false);
			}
		}
	}
	public void SetStar (bool active)
	{
		imgs = transform.GetComponentsInChildren<Image> ();
		for (int i = 0; i < imgs.Length; i++) {
			string path = active ? "Image/Star/Star1" : "Image/Star/Star2";
			imgs [i].sprite = Resources.Load<Sprite> (path);
		}
	}
}
