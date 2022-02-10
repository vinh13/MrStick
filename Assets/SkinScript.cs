using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SkinScript : MonoBehaviour
{
	[SerializeField]Vector3 localPos = Vector3.zero;
	[SerializeField]Vector3 locaScale = Vector3.zero;
	[SerializeField]Quaternion localQua = Quaternion.identity;
	[SerializeField]bool bSkined = false;
	[SerializeField]bool bNeck = false;
	#region Editor
	void OnValidate ()
	{
		localPos = this.transform.localPosition;
		locaScale = this.transform.localScale;
		localQua = this.transform.localRotation;
	}

	void Start ()
	{
		transform.parent.parent.GetComponent<PartBody> ().Skined (bSkined, bNeck);
	}

	public void Setup ()
	{
		this.transform.localPosition = localPos;
		this.transform.localScale = locaScale;
		this.transform.localRotation = localQua;
	}

	public void SetSkin (string sName)
	{
		GetComponent<SpriteRenderer> ().sortingLayerName = sName;
	}

	public void DestroySkin ()
	{
		transform.parent.parent.GetComponent<PartBody> ().Skined (false, bNeck);
		Destroy (gameObject);	
	}

	#endregion
}
