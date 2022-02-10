using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour
{
	[SerializeField]Transform tStart = null, tEnd = null;
	public int iLap = 0;
	float rangeX = 0;

	public float GetRangeX {
		get { 
			return  tEnd.position.x - tStart.position.x;
		}
	}

	public void TakeLimimit ()
	{
		CameraControl.Instance.TakeLimit (tStart.position, tEnd.position);
		rangeX = tEnd.position.x - tStart.position.x;
	}

	public void NextMap ()
	{
		Vector3 pos = transform.position;
		pos.x += rangeX;
		transform.position = pos;
		CameraControl.Instance.TakeLimit (tStart.position, tEnd.position);
	}

	public float ratioDistance (float xNew)
	{
		float x = xNew - tStart.position.x;
		return x / rangeX;
	}

	public float GetXStart {
		get {
			return tStart.position.x;
		}
	}

	public void DestroyMap ()
	{
		gameObject.SetActive (false);
	}
}
