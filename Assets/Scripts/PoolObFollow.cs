using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObFollow : MonoBehaviour
{
	[SerializeField]string path;
	List<ObFollow> listGo = new List<ObFollow> ();
	bool bStarted = false;

	public void _Start ()
	{
		if (bStarted)
			return;
		bStarted = true;
		for (int i = 0; i < 3; i++) {
			listGo.Add (CreateGo.GetComponent<ObFollow> ());
			listGo [i].Active (false);
		}
	}

	GameObject CreateGo {
		get {
			return Instantiate (Resources.Load<GameObject> (path));
		}
	}

	public ObFollow Current {
		get {
			for (int i = 0; i < listGo.Count; i++) {
				if (!listGo [i].gameObject.activeSelf) {
					return listGo [i];
				}
			}
			return NewGo;
		}
	}

	ObFollow NewGo {
		get {
			int id = listGo.Count;
			listGo.Add (CreateGo.GetComponent<ObFollow> ());
			listGo [id].Active (false);
			return listGo [id];
		}
	}
}
