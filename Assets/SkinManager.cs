using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[System.Serializable]
public enum SkinID
{
	None = 0,
	Spine = 1,
	SpineB = 2,
	LegL = 3,
	LegLB = 4,
	FootL = 5,
	LegR = 6,
	LegRB = 7,
	FootR = 8,
	ArmL = 9,
	HandL = 10,
	ArmR = 11,
	HandR = 12,
	Head = 13,
	Neck = 14,
}

public class SkinManager : MonoBehaviour
{
	[SerializeField]Transform[] tPos;
	Dictionary<SkinID,SkinScript> listSkin = new Dictionary<SkinID, SkinScript> ();
	[SerializeField]int IDSkin = 1;

	public void CreateSkin (int idSkin)
	{
		if (idSkin == 0)
			return;
		IDSkin = idSkin;
		listSkin.Add (SkinID.Spine, _CreateSkin (SkinID.Spine, 0));
		listSkin.Add (SkinID.SpineB, _CreateSkin (SkinID.SpineB, 1));

		listSkin.Add (SkinID.LegL, _CreateSkin (SkinID.LegL, 2));
		listSkin.Add (SkinID.LegLB, _CreateSkin (SkinID.LegLB, 3));
		listSkin.Add (SkinID.FootL, _CreateSkin (SkinID.FootL, 4));

		listSkin.Add (SkinID.LegR, _CreateSkin (SkinID.LegR, 5));
		listSkin.Add (SkinID.LegRB, _CreateSkin (SkinID.LegRB, 6));
		listSkin.Add (SkinID.FootR, _CreateSkin (SkinID.FootR, 7));

		listSkin.Add (SkinID.ArmL, _CreateSkin (SkinID.ArmL, 8));
		listSkin.Add (SkinID.HandL, _CreateSkin (SkinID.HandL, 9));


		listSkin.Add (SkinID.ArmR, _CreateSkin (SkinID.ArmR, 10));
		listSkin.Add (SkinID.HandR, _CreateSkin (SkinID.HandR, 11));

		listSkin.Add (SkinID.Head, _CreateSkin (SkinID.Head, 12));
		listSkin.Add (SkinID.Neck, _CreateSkin (SkinID.Neck, 13));

	}
	public void CreatePart (string key)
	{
		//MainPlay
		string[] texts = key.Split ('_');
		int id = int.Parse (texts [0]);
		SkinID temp = Convert (texts [1]);
		if (listSkin.ContainsKey (temp)) {
			listSkin [temp].DestroySkin ();
			listSkin.Remove (temp);
		}
		if (id == 0)
			return;
		listSkin.Add (temp, _CreateSkin (id, temp, temp.GetHashCode () - 1));
	}

	public void ClearPart (SkinID temp)
	{
		if (listSkin.ContainsKey (temp)) {
			listSkin [temp].DestroySkin ();
			listSkin.Remove (temp);
		}
	}

	public void CreatePart (int id, SkinID temp)
	{
		if (listSkin.ContainsKey (temp)) {
			listSkin [temp].DestroySkin ();
			listSkin.Remove (temp);
		}
		listSkin.Add (temp, _CreateSkin (id, temp, temp.GetHashCode () - 1));
	}

	SkinID Convert (string text)
	{
		SkinID temp = SkinID.None;
		foreach (SkinID i in Enum.GetValues(typeof(SkinID))) {
			if (text == i.ToString ()) {
				temp = (SkinID)i;
				break;
			}
		}
		return temp;
	}

	SkinScript _CreateSkin (SkinID ID, int indexPos)
	{
		SkinScript sk = new SkinScript ();
		GameObject temp = Resources.Load<GameObject> ("PlayerSkin/" + IDSkin + "/" + ID.ToString ());
		if (temp == null) {
			temp = Resources.Load<GameObject> ("PlayerSkin/Body");
		}
		GameObject go = Instantiate (temp, tPos [indexPos]);
		sk = go.GetComponent<SkinScript> ();
		return sk;
	}

	SkinScript _CreateSkin (int idSkin, SkinID ID, int indexPos)
	{
		SkinScript sk = new SkinScript ();
		GameObject temp = Resources.Load<GameObject> ("PlayerSkin/" + idSkin + "/" + ID.ToString ());
		if (temp == null) {
			temp = Resources.Load<GameObject> ("PlayerSkin/Body");
		}
		GameObject go = Instantiate (temp, tPos [indexPos]);
		sk = go.GetComponent<SkinScript> ();
		return sk;
	}

	public void ChangeLayer (string sName)
	{
		SkinScript[] sks = listSkin.Values.ToArray ();
		for (int i = 0; i < sks.Length; i++) {
			sks [i].SetSkin (sName);
		}
	}
}
