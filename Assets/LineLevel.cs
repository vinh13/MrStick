using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineLevel : MonoBehaviour
{
	[SerializeField]LineRenderer lineRenderer = null;

	public void Setup (Vector3 s, Vector3 e)
	{
		lineRenderer.SetPosition (0, s);
		lineRenderer.SetPosition (1, e);
	}

	public void Active (bool b)
	{
		if (b) {
			lineRenderer.startColor = Color.white;
			lineRenderer.endColor = Color.white;
		} else {
			lineRenderer.startColor = Color.gray;
			lineRenderer.endColor = Color.black;
		}
	}
}
