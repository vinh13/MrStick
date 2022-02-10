using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBullet : MonoBehaviour
{
	[SerializeField]string path = "";
	List<Bullet> listGo = new List<Bullet> ();
	[SerializeField]int numberBulet = 0;
	bool bStarted = false;

	public void _Start ()
	{
		if (bStarted)
			return;
		bStarted = true;
		for (int i = 0; i < numberBulet; i++) {
			listGo.Add (CreateGo.GetComponent<Bullet> ());
			listGo [i].Init ();
			listGo [i].Active (false);
		}
	}

	GameObject CreateGo {
		get {
			return Instantiate (Resources.Load<GameObject> (path));
		}
	}

	public Bullet Current {
		get {
			for (int i = 0; i < listGo.Count; i++) {
				if (!listGo [i].gameObject.activeSelf) {
					return listGo [i];
				}
			}
			return NewGo;
		}
	}

	Bullet NewGo {
		get {
			int id = listGo.Count;
			listGo.Add (CreateGo.GetComponent<Bullet> ());
			listGo [id].Init ();
			listGo [id].Active (false);
			return listGo [id];
		}
	}
}
