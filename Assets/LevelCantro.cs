using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCantro : MonoBehaviour
{
	[SerializeField]UIButton btnBack = null;
	[SerializeField]UIButton btnPlus = null;
	[SerializeField]UIButton btnPlusGem = null;

	void Awake ()
	{
		btnBack.Register (ClickBack);
		btnPlus.Register (ClickPlus);
		btnPlusGem.Register (ClickPlusGem);
		BeforeplayManager.Instance.Init ();
	}

	void ClickBack ()
	{
		Manager.Instance.LoadScene (SceneName.Home, true);
	}

	void ClickPlus ()
	{	
		Manager.Instance.ShowShop (false);
	}

	void ClickPlusGem ()
	{
		Manager.Instance.ShowShop (true);
	}

}
