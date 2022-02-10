using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TungDz;
using System;

public class InAppCantro : MonoBehaviour
{
	[SerializeField]UIButton btnExit = null, btnExitGold = null;
	[SerializeField]AnimatorPopUpScript rectGem = null, rectGold = null;
	Action<object> _RequestShop = null;
	bool bShowGem = false;
	bool bDoNotHideGold = false;
	void Start ()
	{
		//		rectGem.gameObject.SetActive (CacheScene.bInAppShowGem);
		//		rectGold.gameObject.SetActive (!CacheScene.bInAppShowGem);
		//		btnShowVideo.onClick.AddListener (delegate() {
		//			ClickShowVideo ();	
		//		});
		//		btnShowVideoGem.onClick.AddListener (delegate() {
		//			ClickShowVideo ();	
		//		});

		btnExit.Register (ClickExit);
		btnExitGold.Register (ClickExit);
		TaskUtil.Schedule (this, Show, 0.1F);
		CacheScene.bInShop = true;
		_RequestShop = (param) => RequestShop ();
		EventDispatcher.Instance.RegisterListener (EventID.RequestShop, _RequestShop);
	}

	void OnDestroy ()
	{
		EventDispatcher.Instance.RemoveListener (EventID.RequestShop, _RequestShop);
	}

	void RequestShop ()
	{
		if (!bShowGem) {
			//rectGold.hide (null);
			rectGem.transform.SetAsLastSibling ();
			rectGem.show (OnShow);
			bShowGem = true;
			CacheScene.bInAppShowGem = true;
			bDoNotHideGold = true;
		} else {
			rectGem.hide (null);
			rectGold.transform.SetAsLastSibling ();
			rectGold.show (OnShow);
			bShowGem = false;
			CacheScene.bInAppShowGem = false;

		}	
		//AudioUIManager.Instance.Play ("UI_Popup_Open");
	}

	void Show ()
	{
		Logic.UNPAUSE ();
		if (CacheScene.bInAppShowGem) {
			bShowGem = true;
			rectGem.show (OnShow);
		} else {
			rectGold.show (OnShow);
			bShowGem = false;
		}
		//Manager.Instance.HideWaiting ();
		//AudioUIManager.Instance.Play ("UI_Popup_Open");
	}

	void OnShow ()
	{
		if (!FlashSaleChecking.bShowFlashSale) {
			Manager.Instance.ShowWaitting (false);
		} else {
			if (bShowGem) {
				Manager.Instance.ShowWaitting (true);
				SceneManager.LoadSceneAsync (SceneName.FlashSale.GetHashCode (), LoadSceneMode.Additive);
			} else {
				Manager.Instance.ShowWaitting (false);
			}
		}
	}

	void ClickExit ()
	{
		if (CacheScene.bInAppShowGem) {
			rectGem.hide (Hide);
		} else {
			rectGold.hide (Hide);
		}
		//AudioUIManager.Instance.Play ("UI_Back");
	}

	void Hide ()
	{
		if (!bDoNotHideGold) {
			SceneManager.UnloadSceneAsync (SceneName.InApp.ToString ());
			CacheScene.bInShop = false;
		} else {
			bDoNotHideGold = false;
			CacheScene.bInAppShowGem = false;
		}
	}
}
