using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolEffect : MonoBehaviour
{
	[SerializeField]string path = "";
	List<Effect> listGo = new List<Effect> ();
	[SerializeField]int numberEffect = 0;
	bool bStarted = false;
	public void _Start ()
	{
		if (bStarted)
			return;
		bStarted = true;
		for (int i = 0; i < numberEffect; i++) {
			listGo.Add (CreateGo.GetComponent<Effect> ());
			listGo [i].Init ();
			listGo [i].Active (false);
		}
	}

	GameObject CreateGo {
		get {
			return Instantiate (Resources.Load<GameObject> (path));
		}
	}

	public Effect Current {
		get {
			for (int i = 0; i < listGo.Count; i++) {
				if (!listGo [i].gameObject.activeSelf) {
					return listGo [i];
				}
			}
			return NewGo;
		}
	}

	Effect NewGo {
		get {
			int id = listGo.Count;
			listGo.Add (CreateGo.GetComponent<Effect> ());
			listGo [id].Init ();
			listGo [id].Active (false);
			return listGo [id];
		}
	}
}
