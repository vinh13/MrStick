using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragControl : MonoBehaviour
{
	[SerializeField]Transform tTarget = null, tSync = null;
	[SerializeField]float minX = 0, maxX = 0;
	[SerializeField]float fSpeedNextPage = 0;
	bool bDrag = false;
	bool bNext = false;

	public void StartDrag ()
	{
		if (bDrag)
			return;
		bDrag = true;
	}

	public void EndDrag ()
	{
		if (!bDrag)
			return;
		bDrag = false;
	}

	void LateUpdate ()
	{
		if (bNext)
			return;
		Drag ();
	}

	public void Drag ()
	{
		Vector3 localP = tTarget.localPosition;
		localP.x = Mathf.Clamp (localP.x, minX, maxX);
		tTarget.localPosition = localP;
		Vector3 p = tTarget.position;
		p.z = 0;
		tSync.position = p;
	}

	public void NextPage (float range)
	{
		bNext = true;
		Vector3 localP = tTarget.localPosition;
		localP.x = -range;
		StartCoroutine (_NextPage (localP));
	}

	IEnumerator _NextPage (Vector3 target)
	{
		bool done = false;
		while (!done) {
			Vector3 localP = tTarget.localPosition;
			tTarget.localPosition = Vector3.MoveTowards (localP, target, fSpeedNextPage * Time.deltaTime);
			if (localP == target) {
				done = true;
			}
			//Sync
			Vector3 p = tTarget.position;
			p.z = 0;
			tSync.position = p;
			yield return null;
		}
		Debug.Log ("Next Page Done!");
		bNext = false;
	}

}
