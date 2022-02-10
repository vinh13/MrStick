using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class FlashSaleChecking : MonoBehaviour
{
	[SerializeField]Button btnNoel = null;
	[SerializeField]Transform rectActive = null;
	public static bool bShowFlashSale = false;
	void Start ()
	{

		int day = DateTime.UtcNow.Day;
		int month = DateTime.UtcNow.Month;
		int year = DateTime.UtcNow.Year;
		bool bShow = false;
		if (year == 2018) {
			if (month >= 12) {
				if (day >= 17) {
					bShow = true;
				}
			}
		} else {
			if (month <= 1) {
				if (day <= 5) {
					bShow = true;
				}
			}
		}
		if (bShow) {
			btnNoel.onClick.AddListener (delegate {
				Click ();	
			});
			btnNoel.interactable = true;
			rectActive.gameObject.SetActive (true);

		} else {
			btnNoel.interactable = false;
			rectActive.gameObject.SetActive (false);
		}
		if (bShow) {
			if (!GameData.BlockShowFlashSale) {
				TaskUtil.Schedule (this, AutoCheck, 0.6F);
			}
		}
		bShowFlashSale = bShow;
	}

	public void Click ()
	{
		Manager.Instance.ShowWaitting (true);
		SceneManager.LoadSceneAsync (SceneName.FlashSale.GetHashCode (), LoadSceneMode.Additive);
	}

	public void AutoCheck ()
	{
//		if (!BadLogic.bUpgrade && !BadLogic.bTutorialFight && TutorialData.bCompleteBuyHero) {
//			if (!BadLogic.bShowFlashSale) {
//				BadLogic.bShowFlashSale = true;
//				return;
//			} else {
		Click ();
//			}
//		}
	}
}
