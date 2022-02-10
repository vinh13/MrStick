using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinEditor : MonoBehaviour
{
	public static SkinEditor Instance = null;
	string data = "";
	[SerializeField]SkinManager skinManager = null;
	[SerializeField]Skin skinSetup = null;
	string sdataHead = "";
	string sdataBody = "";
	string sdataArm = "";

	void Awake ()
	{
		if (Instance == null)
			Instance = this;
		data = SkinData.GetSkin;
		sdataBody = "";
		sdataHead = "";
		sdataArm = "";
		SyncData ();
	}

	void SyncData ()
	{
		string[] texts = data.Split ('.');
		for (int i = 0; i < texts.Length; i++) {
			if (texts [i] != "") {
				skinManager.CreatePart (texts [i]);
				AddPart (texts [i]);
			}
		}
	}

	EquipType GetEquipType (string text)
	{
		EquipType temp = EquipType.None;
		for (int i = 0; i < skinSetup.partHead.Length; i++) {
			if (text == skinSetup.partHead [i].ToString ()) {
				temp = EquipType.Head;
				break;
			}
		}
		if (temp == EquipType.None) {
			for (int i = 0; i < skinSetup.partBody.Length; i++) {
				if (text == skinSetup.partBody [i].ToString ()) {
					temp = EquipType.Body;
					break;
				}
			}
		}
		if (temp == EquipType.None) {
			for (int i = 0; i < skinSetup.partArm.Length; i++) {
				if (text == skinSetup.partArm [i].ToString ()) {
					temp = EquipType.Arm;
					break;
				}
			}
		}
		return temp;
	}

	void AddPart (string text)
	{
		string[] texts = text.Split ('_');
		int id = int.Parse (texts [0]);
		EquipType type = EquipType.None;
		type = GetEquipType (texts [1]);
		switch (type) {
		case EquipType.Head:
			sdataHead += Convert (id, texts [1]);
			break;
		case EquipType.Body:
			sdataBody += Convert (id, texts [1]);
			break;
		case EquipType.Arm:
			sdataArm += Convert (id, texts [1]);
			break;
		}
	}

	public void ClearPreview ()
	{
		Clear (sdataHead, EquipType.Head);
		Clear (sdataBody, EquipType.Body);
		Clear (sdataArm, EquipType.Arm);
		data = sdataHead + sdataBody + sdataArm;
		SkinData.SetSkin (data);
	}

	public void TryClear ()
	{
		
	}

	void Clear (string text, EquipType equipType)
	{
		if (text != "") {
			string[] texts = text.Split ('.');
			for (int i = 0; i < texts.Length; i++) {
				if (texts [i] != "") {
					skinManager.CreatePart (texts [i]);
				}
			}
		} else {
			SkinID[] skinid;
			switch (equipType) {
			case EquipType.Head:
				skinid = skinSetup.partHead;
				break;
			case EquipType.Body:
				skinid = skinSetup.partBody;
				break;
			case EquipType.Arm:
				skinid = skinSetup.partArm;
				break;
			default :
				skinid = skinSetup.partHead;
				break;
			}
			for (int i = 0; i < skinid.Length; i++) {
				skinManager.ClearPart (skinid [i]);
			}
		}
	}

	public void CreatePartSkin (int id, SkinID skinID)
	{
		skinManager.CreatePart (id, skinID);
	}


	public void Preview (EquipType equipType, EquipLevel level, int idSkin, bool bUnlocked)
	{
		SkinID[] skinid;
		string text = "";
		switch (equipType) {
		case EquipType.Head:
			skinid = skinSetup.partHead;
			break;
		case EquipType.Body:
			skinid = skinSetup.partBody;
			break;
		case EquipType.Arm:
			skinid = skinSetup.partArm;
			break;
		default :
			skinid = skinSetup.partHead;
			break;
		}
		for (int i = 0; i < skinid.Length; i++) {
			skinManager.CreatePart (idSkin, skinid [i]);
			if (bUnlocked) {
				text += Convert (idSkin, skinid [i]);
			}
		}
		if (bUnlocked) {
			SyncSdata (equipType, text);
			data = sdataHead + sdataBody + sdataArm;
			SkinData.SetSkin (data);
		}
	}

	public void Preview (string equipType, int idSkin, bool bUnlocked)
	{
		SkinID[] skinid;
		EquipType equip = EquipType.None;
		string text = "";
		switch (equipType) {
		case "Head":
			skinid = skinSetup.partHead;
			equip = EquipType.Head;
			break;
		case "Body":
			skinid = skinSetup.partBody;
			equip = EquipType.Body;
			break;
		case "Arm":
			skinid = skinSetup.partArm;
			equip = EquipType.Arm;
			break;
		default :
			skinid = skinSetup.partHead;
			equip = EquipType.Head;
			break;
		}
		int idReal = SkinData.IDSkin (idSkin);
		for (int i = 0; i < skinid.Length; i++) {
			skinManager.CreatePart (idReal, skinid [i]);
			if (bUnlocked) {
				text += Convert (idReal, skinid [i]);
			}
		}
		if (bUnlocked) {
			SyncSdata (equip, text);
			data = sdataHead + sdataBody + sdataArm;
			SkinData.SetSkin (data);
		}
	}

	void SyncSdata (EquipType equipType, string text)
	{
		switch (equipType) {
		case EquipType.Head:
			sdataHead = text;
			break;
		case EquipType.Body:
			sdataBody = text;
			break;
		case EquipType.Arm:
			sdataArm = text;
			break;
		default :
			break;
		}
	}

	string Convert (int id, SkinID t)
	{
		return "" + id + "_" + t.ToString () + ".";
	}

	string Convert (int id, string t)
	{
		return "" + id + "_" + t + ".";
	}
}
