using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
	public static ItemManager Instance = null;
	[SerializeField]RectTransform rectUI = null;
	[SerializeField]string path = "";

	void Awake ()
	{
		if (Instance == null)
			Instance = this;
	}

	public void CreateItem (float posX, Color color)
	{
		float ratio = (posX - MapManager.Intance.GetXStart) / MapManager.Intance.GetRangeX;
		item.Setup (ratio, color);
	}

	UIItemSync item {
		get {
			return Instantiate (Resources.Load<GameObject> (path), rectUI).GetComponent<UIItemSync> ();
		}
	}

}
