using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAvatar : MonoBehaviour
{
	[SerializeField]Text textName = null;
	[SerializeField]Text textLevel = null;
	[SerializeField]Text textExp = null;
	[SerializeField]Image imgAvatar = null;
	[SerializeField]Image imgFill = null;
	[SerializeField]UIButton btnRegister = null;

	void Awake ()
	{
		PlayerManagerTdz.Instance.Register (PlayerSettingID.ChangeName, ChangeName);
		PlayerManagerTdz.Instance.Register (PlayerSettingID.ChangeExp, ChangeExp);
		PlayerManagerTdz.Instance.Register (PlayerSettingID.ChangeAvatar, ChangeAvatar);
		btnRegister.Register (ClickRegister);
	}

	void Start ()
	{
		SyncLocal ();
	}

	void SyncLocal ()
	{
		string pName = GameData.PlayerName;
		if (pName.Equals ("")) {
			pName = "Userguest";
			GameData.PlayerName = pName;
		}
		textName.text = pName;
		PlayerManagerTdz.Instance.UpdateExp (0);
	}


	void OnDisable ()
	{
		PlayerManagerTdz.Instance.Remove (PlayerSettingID.ChangeName);
		PlayerManagerTdz.Instance.Remove (PlayerSettingID.ChangeExp);
		PlayerManagerTdz.Instance.Remove (PlayerSettingID.ChangeAvatar);
	}

	void ChangeName (object ob)
	{
		textName.text = GameData.PlayerName;
	}

	void ChangeExp (object ob)
	{
		ExpLevelChange el = (ExpLevelChange)ob;
		textLevel.text = "" + el.level;
		textExp.text = "" + el.exp + "/" + el.maxExp;
		imgFill.fillAmount = el.ratioFill;
	}

	void ChangeAvatar (object ob)
	{
		
	}

	void ClickRegister ()
	{
		PlayerManagerTdz.Instance.ShowRegisterName (transform.root);
	}
}
