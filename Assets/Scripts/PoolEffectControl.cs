using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolEffectControl : MonoBehaviour
{
	[SerializeField]string path = "";
	List<EffectControl> listGo = new List<EffectControl> ();
	[SerializeField]int numberBulet = 0;

	void Start ()
	{
		for (int i = 0; i < numberBulet; i++) {
			listGo.Add (CreateGo.GetComponent<EffectControl> ());
			listGo [i].Active (false);
		}
	}

	GameObject CreateGo {
		get {
			return Instantiate (Resources.Load<GameObject> (path));
		}
	}

	public EffectControl Current {
		get {
			for (int i = 0; i < listGo.Count; i++) {
				if (!listGo [i].gameObject.activeSelf) {
					return listGo [i];
				}
			}
			return NewGo;
		}
	}

	EffectControl NewGo {
		get {
			int id = listGo.Count;
			listGo.Add (CreateGo.GetComponent<EffectControl> ());
			listGo [id].Active (false);
			return listGo [id];
		}
	}
}