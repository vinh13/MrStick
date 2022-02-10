using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class UIChest : MonoBehaviour
{
	[SerializeField]AnimatorPopUpScript animPop = null;
	[SerializeField]Text textKey = null, textPrice = null;
	[SerializeField]UIButton[] btnKeys = new UIButton[2];
	[SerializeField]UIButton btnInfo = null, btnExit = null, btnBuy = null;
	[SerializeField]ChestAnimation chestAnim = null;
	[SerializeField]AnimatorPopUpScript chestOpen = null;
	[SerializeField]Transform chestIdle = null;
	[SerializeField]UIInventoryInfo uiInfor = null;

	public void ShowInfor ()
	{
		uiInfor.Show ();
	}

	const int price = 100;
	int countKey = 0;
	bool bPlay = false;
	int sibIdle = 0;
	Action<object> _OpenChest = null;
	Action _ResetRoll = null;

	public void OnGifDone ()
	{
		chestOpen.hide (_OnGifDone);
		chestAnim.gameObject.SetActive (false);
		chestIdle.gameObject.SetActive (true);
	}

	void _OnGifDone ()
	{
		_ResetRoll.Invoke ();
		Check ();
		bPlay = false;
		btnExit.Block (false);
		btnBuy.Block (false);
	}

	public void Register (Action<object> a, Action ResetRoll)
	{
		_ResetRoll = ResetRoll;
		_OpenChest = a;
	}

	void Start ()
	{
		sibIdle = chestOpen.transform.GetSiblingIndex ();
		btnKeys [0].Register (ClickKey);
		btnKeys [1].Register (ClickKey);
		btnInfo.Register (ClickInfo);
		btnExit.Register (ClickExit);
		btnBuy.Register (ClickBuy);
		countKey = CoinManager.Instance.Key;
		UpdateKey ();
		UpdatePrice ();
		bPlay = false;
		Check ();
	}

	#region Click

	void UpdateKey ()
	{
		textKey.text = "1/" + countKey;
	}

	void UpdatePrice ()
	{
		textPrice.text = "-" + CoinManager.Instance.Convert (price);
	}

	void Check ()
	{
		if (countKey > 0) {
			btnKeys [0].Block (false);
			btnKeys [1].Block (false);
		} else {
			btnKeys [0].Block (true);
			btnKeys [1].Block (true);
		}
	}

	void ClickKey ()
	{
		if (bPlay)
			return;
		bPlay = true;
		countKey -= 1;
		CoinManager.Instance.PurchaserKey (-1);
		UpdateKey ();
		Check ();
		OpenChest ();
		FBManagerEvent.Instance.PostEventCustom ("Chest_open_key");
		btnBuy.Block (true);
	}

	void OpenChest ()
	{
		btnExit.Block (true);
		chestAnim.PlayAnim (Done, OffIdle);
	}

	void OffIdle (object ob)
	{
		chestIdle.gameObject.SetActive (false);
	}

	void Done (object  ob)
	{
		//bPlay = false;
		ShowRoll ();
	}

	void ShowRoll ()
	{
		chestOpen.show (OnShowRoll);
	}

	void OnShowRoll ()
	{
		chestOpen.transform.SetAsLastSibling ();
		StartCoroutine (_OnShowRoll ());
	}

	IEnumerator _OnShowRoll ()
	{
		yield return new WaitForSecondsRealtime (0.2F);
		_OpenChest.Invoke (true);
	}

	void ClickBuy ()
	{
		
		if (CoinManager.Instance.CheckGem (price)) {
			CoinManager.Instance.PurchaseGem (-price, false);
			UpdatePrice ();
			OpenChest ();
			FBManagerEvent.Instance.PostEventCustom ("Chest_open_gem");
			btnBuy.Block (true);
			btnKeys [0].Block (true);
			btnKeys [1].Block (true);
		}
	}

	void ClickInfo ()
	{
		ShowInfor ();
	}

	void ClickExit ()
	{
		Show (false);
	}

	#endregion

	public void Show (bool b)
	{
		if (b) {
			animPop.show (OnShow);
			Manager.Instance.ShowWaitting (false);
		} else {
			animPop.hide (OnHide);
		}
	}

	void OnShow ()
	{
		
	}

	void OnHide ()
	{
		SceneManager.UnloadSceneAsync (SceneName.Chest.GetHashCode ());
	}
}
