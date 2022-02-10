using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinBike : MonoBehaviour
{

	[SerializeField]Vector3 localPos = Vector3.zero;
	[SerializeField]Vector3 locaScale = Vector3.zero;
	[SerializeField]Quaternion localQua = Quaternion.identity;
	[SerializeField]SpriteRenderer[] sprs;

	#region Editor

	void OnValidate ()
	{
		localPos = this.transform.localPosition;
		locaScale = this.transform.localScale;
		localQua = this.transform.localRotation;
		sprs = GetComponentsInChildren<SpriteRenderer> ();
	}

	public void Setup ()
	{
		this.transform.localPosition = localPos;
		this.transform.localScale = locaScale;
		this.transform.localRotation = localQua;
	}

	public void SetSkin (string sName)
	{
		for (int i = 0; i < sprs.Length; i++) {
			sprs [i].sortingLayerName = sName;
		}
	}

	public void DestroySkin ()
	{
		Destroy (gameObject);	
	}

	#endregion
}
