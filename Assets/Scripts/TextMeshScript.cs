using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMeshScript : MonoBehaviour
{
	[SerializeField]TextMesh textMesh;
	Fader fader;
	Mover mover;
	[SerializeField]float range = 6F;
	Vector3 v3Target;
	Color32 colorTemp;
	bool bFade = false;

	void Awake ()
	{
		fader = gameObject.AddComponent<Fader> ();
		mover = gameObject.AddComponent<Mover> ();
	}

	public void SetText (Vector3 pos, string text, bool ofP, bool Critical)
	{
		gameObject.SetActive (true);
		v3Target = pos;
		v3Target.y += range / 2F;
		transform.position = pos;
		textMesh.text = text;
		bFade = true;
		if (!Critical) {
			if (ofP)
				textMesh.color = Color.red;
			else
				textMesh.color = Color.white;
		} else {
			if (ofP) {
				textMesh.color = Color.red;
			} else {
				textMesh.color = Color.green;
			}
		}

		colorTemp = textMesh.color;

		mover._Move (v3Target, 10F, MoveI);

	}

	public void SexTextNormal (Vector3 pos, string text)
	{
		bFade = false;
		gameObject.SetActive (true);
		v3Target = pos;
		v3Target.y += range / 2F;
		transform.position = pos;
		textMesh.text = text;
		textMesh.color = Color.white;
		colorTemp = textMesh.color;
		mover._Move (v3Target, 10F, MoveI);
	}

	public void SexTextNormal (Vector3 pos, string text, Color32 c)
	{
		bFade = false;
		gameObject.SetActive (true);
		v3Target = pos;
		v3Target.y += range / 2F;
		transform.position = pos;
		textMesh.text = text;
		textMesh.color = c;
		colorTemp = c;
		mover._Move (v3Target, 10F, MoveI);
	}



	void MoveI ()
	{
		if (bFade)
			fader.Fade (Fade, 255, 0, 0.25F);
		v3Target.y += range / 2F;
		mover._Move (v3Target, 5F, Done);
	}

	void Done ()
	{
		gameObject.SetActive (false);
	}

	void Fade (byte value)
	{
		if (!bFade)
			return;
		colorTemp.a = value;
		textMesh.color = colorTemp;
	}
	//	void Update ()
	//	{
	//		if (!bMoving)
	//			return;
	//		Vector3 temp = transform.position;
	//		temp = Vector3.MoveTowards (temp, v3Target, 10 * Time.deltaTime);
	//		transform.position = temp;
	//		if (Vector3.Distance (temp, v3Target) <= 0.05F) {
	//			bMoving = false;
	//			fader.StopAllCoroutines ();
	//			gameObject.SetActive (false);
	//		}
	//	}
}
