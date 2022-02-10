using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIFreeExchange : MonoBehaviour
{
	[SerializeField]UIButton btnBack = null, btnEquip = null;
	[SerializeField]Text textPrice = null;
	[SerializeField]RollSkin rollSkin = null;
	[SerializeField]RollItem rollBoot = null;
	[SerializeField]RollGold rollGold = null;
	[SerializeField]AnimatorPopUpScript animPop = null;
	Action<object> cb = null;
	string mess = "";
	int Gold = 0;
	public void ShowExchangeSkin (Action<object> a, int coin, object ob)
	{
		rollGold.gameObject.SetActive (false);
		rollBoot.gameObject.SetActive (false);

		rollSkin.gameObject.SetActive (true);

		EquipConfig equipConfig = (EquipConfig)ob;

		rollSkin.SetSkin (equipConfig);

		Gold = coin;
		cb = a;


		mess = "" + equipConfig.equipType.ToString () + "_" + equipConfig.ID.ToString () + "_";

		textPrice.text = TaskUtil.Convert (Gold);

		Show (true);
	}

	public void ShowExchangeGold (Action<object> a, int coin, object ob)
	{
		rollBoot.gameObject.SetActive (false);
		rollSkin.gameObject.SetActive (false);

		rollGold.gameObject.SetActive (true);

		rollGold.SetGold ("" + TaskUtil.Convert (coin * 2));

		Gold = coin;
		cb = a;

		mess = "Gold_" + Gold + "_";

		textPrice.text = TaskUtil.Convert (Gold);

		Show (true);

	}

	public void ShowExchangeBoot (Action<object> a, int coin, object ob)
	{
		rollBoot.gameObject.SetActive (true);
		rollSkin.gameObject.SetActive (false);
		rollGold.gameObject.SetActive (false);

		ItemBoot itemBoot = (ItemBoot)ob;

		rollBoot.SetItem (itemBoot._valueBoot, itemBoot.bootType);

		Gold = coin;
		cb = a;

		mess = "" + itemBoot.bootType + "_" + itemBoot._valueBoot + "_";

		textPrice.text = TaskUtil.Convert (Gold);

		Show (true);

	}

	void Show (bool b)
	{
		if (b) {
			animPop.show (null);
			btnBack.Block (false);
			btnEquip.Block (false);
			StartCoroutine (checkVideo ());
		} else {
			animPop.hide (OnHide);
		}
	}

	void OnHide ()
	{
		Destroy (this.gameObject);
	}

	IEnumerator checkVideo ()
	{
		while (!AllInOne.Instance.CheckVideoReward ()) {
			yield return null;
		}
		btnEquip.Block (false);
	}

	void Start ()
	{
		btnBack.Register (ClickBack);
		btnEquip.Register (ClickEquip);
	}

	void ClickBack ()
	{
		mess += "0";
		btnBack.Block (true);
		btnEquip.Block (true);
		cb.Invoke (mess);
		Show (false);
	}

	void ClickEquip ()
	{
		btnBack.Block (true);
		btnEquip.Block (true);
		AllInOne.Instance.ShowVideoReward (cbVideo,"FreeExchange",LevelData.IDLevel);
	}

	void cbVideo (bool b)
	{
		if (b) {
			mess += "1";
			btnBack.Block (true);
			btnEquip.Block (true);
			cb.Invoke (mess);
			Show (false);
		} else {
			ClickBack ();
		}
	}
}
