using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
	[SerializeField]string path;
	List<GameObject> listGo = new List<GameObject> ();
	bool bCreated = false;
	public void CreatePool (int number)
	{
		if (bCreated)
			return;
		bCreated = true;
		for (int i = 0; i < number; i++) {
			listGo.Add (CreateGo);
			listGo [i].SetActive (false);
		}
	}

	GameObject CreateGo {
		get {
			return Instantiate (Resources.Load<GameObject> (path));
		}
	}

	public GameObject Current {
		get {
			for (int i = 0; i < listGo.Count; i++) {
				if (!listGo [i].activeSelf) {
					return listGo [i];
				}
			}
			return NewGo;
		}
	}

	GameObject NewGo {
		get {
			int id = listGo.Count;
			listGo.Add (CreateGo);
			return listGo [id];
		}
	}
}
