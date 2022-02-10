using UnityEngine;
using System.Collections;

public class CreateObjectPooling : MonoBehaviour
{
	[SerializeField]GameObject pref = null;
	GameObject[] objectsPool;
	[SerializeField] int maxnumOb = 0;
	void Awake ()
	{
		_CREATE ();
	}

	public void _CREATE ()
	{
		objectsPool = new GameObject[maxnumOb];
		for (int i = 0; i < maxnumOb; i++) {
			objectsPool [i] = Instantiate (pref) as GameObject;
			objectsPool [i].name = pref.name + i;
			objectsPool [i].SetActive (false);
			objectsPool [i].transform.parent = transform;
		}
	}

	public void _ChooseEffect (Vector3 pos)
	{
		for (int i = 0; i < objectsPool.Length; i++) {
			if (objectsPool [i] != null) {
				if (!objectsPool [i].activeInHierarchy) {
					objectsPool [i].transform.position = pos;
					objectsPool [i].transform.parent = null;
					objectsPool [i].SetActive (true);
					break;
				}
			}
		}
	}

	public void _ChooseEffect (Vector3 pos, float angle)
	{
		for (int i = 0; i < objectsPool.Length; i++) {
			if (objectsPool [i] != null) {
				if (!objectsPool [i].activeInHierarchy) {
					objectsPool [i].transform.position = pos;
					objectsPool [i].transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
					objectsPool [i].transform.parent = null;
					objectsPool [i].SetActive (true);
					break;
				}
			}
		}
	}
}
