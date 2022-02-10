using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public enum BikeID
{
	None = 0,
	Wheel = 1,
	Body = 2,
}

public class SkinBikeManager : MonoBehaviour
{
	[SerializeField]Transform[] tPos;
	Dictionary<BikeID,SkinBike> listSkin = new Dictionary<BikeID, SkinBike> ();

	public void CreateSkin (string key)
	{
		string[] texts = key.Split ('_');
		int id = int.Parse (texts [0]);
		BikeID bikeID = Convert (texts [1]);
		listSkin.Add (bikeID, _CreateSkin (id, bikeID, bikeID.GetHashCode () - 1));
	}

	public void CreatePart (string key)
	{
		string[] texts = key.Split ('_');
		int id = int.Parse (texts [0]);
		BikeID temp = Convert (texts [1]);
		if (listSkin.ContainsKey (temp)) {
			listSkin [temp].DestroySkin ();
			listSkin.Remove (temp);
		}
		listSkin.Add (temp, _CreateSkin (id, temp, temp.GetHashCode () - 1));
	}

	public void ClearPart (BikeID temp)
	{
		if (listSkin.ContainsKey (temp)) {
			listSkin [temp].DestroySkin ();
			listSkin.Remove (temp);
		}
	}

	public void CreatePart (int id, BikeID temp)
	{
		if (listSkin.ContainsKey (temp)) {
			listSkin [temp].DestroySkin ();
			listSkin.Remove (temp);
		}
		listSkin.Add (temp, _CreateSkin (id, temp, temp.GetHashCode () - 1));
	}

	BikeID Convert (string text)
	{
		BikeID temp = BikeID.None;
		foreach (BikeID id in Enum.GetValues(typeof(BikeID))) {
			if (id.ToString () == text) {
				temp = (BikeID)id;
				break;
			}
		}
		return temp;
	}

	SkinBike _CreateSkin (int IDSkin, BikeID ID, int indexPos)
	{
		SkinBike sk = new SkinBike ();
		GameObject temp = Resources.Load<GameObject> ("Bike/" + IDSkin + "/" + ID.ToString ());
		if (temp == null) {
			temp = Resources.Load<GameObject> ("Bike/Body");
		}
		GameObject go = Instantiate (temp, tPos [indexPos]);
		sk = go.GetComponent<SkinBike> ();
		return sk;
	}
}
