using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeEditor : MonoBehaviour
{
	[SerializeField]SkinBikeManager skinBike = null;
	string data = "";
	string sdataWheel = "";
	public static BikeEditor Instance = null;

	void Awake ()
	{
		if (Instance == null)
			Instance = this;
		data = SkinData.GetBike;
		sdataWheel = "";
		SyncBike ();
	}

	void SyncBike ()
	{
		string[] texts = data.Split ('.');
		for (int i = 0; i < texts.Length; i++) {
			if (texts [i] != "") {
				skinBike.CreateSkin (texts [i]);
				AddPart (texts [i]);
			}
		}
	}

	void AddPart (string text)
	{
		string[] texts = text.Split ('_');
		EquipType type = EquipType.None;
		type = GetEquipType (texts [1]);
		switch (type) {
		case EquipType.Wheels:
			sdataWheel += "" + texts [0] + "_" + texts [1] + ".";
			break;
		}
	}

	public void ClearPreview ()
	{
		Clear (sdataWheel, EquipType.Wheels);
		data = sdataWheel;
		SkinData.SetBike (data);
	}

	void Clear (string text, EquipType equipType)
	{
		if (text != "") {
			string[] texts = text.Split ('.');
			for (int i = 0; i < texts.Length; i++) {
				if (texts [i] != "") {
					skinBike.CreatePart (texts [i]);
				}
			}
		} else {
			switch (equipType) {
			case EquipType.Wheels:
				skinBike.ClearPart (BikeID.Wheel);
				break;
			}
		}
	}

	EquipType GetEquipType (string text)
	{
		EquipType temp = EquipType.None;
		if (text == "Wheel") {
			temp = EquipType.Wheels;
		}
		return temp;
	}

	public void Preview (EquipType equipType, EquipLevel level, int idSkin, bool bUnlocked)
	{
		string text = "";
		switch (equipType) {
		case EquipType.Wheels:
			skinBike.CreatePart (idSkin, BikeID.Wheel);
			if (bUnlocked) {
				text += Convert (idSkin, BikeID.Wheel);
			}
			break;
		}
		if (bUnlocked) {
			SyncSdata (equipType, text);
			data = sdataWheel;
			SkinData.SetBike (data);
		}
	}

	public void Preview (EquipType equipType, int idSkin, bool bUnlocked)
	{
		string text = "";
		switch (equipType) {
		case EquipType.Wheels:
			skinBike.CreatePart (idSkin, BikeID.Wheel);
			if (bUnlocked) {
				text += Convert (idSkin, BikeID.Wheel);
			}
			break;
		}
		if (bUnlocked) {
			SyncSdata (equipType, text);
			data = sdataWheel;
			SkinData.SetBike (data);
		}
	}

	void SyncSdata (EquipType equipType, string text)
	{
		switch (equipType) {
		case EquipType.Wheels:
			sdataWheel = text;
			break;
		}
	}


	string Convert (int id, BikeID t)
	{
		return "" + id + "_" + t.ToString () + ".";
	}
}
