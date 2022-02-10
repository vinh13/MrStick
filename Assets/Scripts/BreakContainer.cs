using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakContainer : MonoBehaviour
{
	[SerializeField]string pathEffect = "Effect/BloodBreak";
	List<EffectFollow> effectFollow = new List<EffectFollow> ();
	[SerializeField]int numberEfect = 3;

	void Start ()
	{
		for (int i = 0; i < numberEfect; i++) {
			GameObject go = Instantiate (Resources.Load<GameObject> (pathEffect)) as GameObject;
			effectFollow.Add (go.GetComponent<EffectFollow> ());
			effectFollow [i].ID = i;
			go.SetActive (false);
		}
	}

	public EffectFollow GetEffect ()
	{
		int temp = -1;
		for (int i = 0; i < numberEfect; i++) {
			if (effectFollow [i].bReady) {
				temp = i;
				break;
			}
		}
		if (temp == -1) {
			EffectFollow ef = _GetEffect ();
			return ef;
		} else
			return effectFollow [temp];
	}

	EffectFollow _GetEffect ()
	{
		numberEfect++;
		GameObject go = Instantiate (Resources.Load<GameObject> (pathEffect)) as GameObject;
		effectFollow.Add (go.GetComponent<EffectFollow> ());
		effectFollow [numberEfect - 1].ID = numberEfect - 1;
		go.SetActive (false);
		return effectFollow [numberEfect - 1];
	}

	public void BreakEffect (int id)
	{
		effectFollow [id].UnPlay ();
	}
}
