using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarScript : MonoBehaviour,HPBar
{
	[SerializeField]Transform tFill = null;

	public void Change (float ratio = 0)
	{
		Vector3 S = tFill.localScale;
		S.x = ratio * 1F;
		tFill.localScale = S;
	}

	public void Disable ()
	{
		gameObject.SetActive (false);
	}

	void LateUpdate ()
	{
		transform.rotation = Quaternion.Euler (Vector3.zero);
	}
	public float Ratio ()
	{
		throw new System.NotImplementedException ();
	}

}
