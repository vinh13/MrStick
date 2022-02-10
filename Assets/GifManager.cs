using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public enum ExchangeType
{
	None = 0,
	GoldToSkin = 1,
	GoldX2 = 3,
	GoldToHealth = 4,
	GoldToShield = 5,
	GoldToWheels = 6,
}

[System.Serializable]
public enum GifType
{
	None = 0,
	Gold = 1,
	Gem = 2,
	Skin = 3,
	Wheels = 4,
	Health = 5,
	Shield = 6,
	Key = 7,
}

[System.Serializable]
public class GifData
{
	public GifType gifType = GifType.None;
	public object Ob = null;
}

public class GifManager : MonoBehaviour
{
	Dictionary<string,Action<object>> Listeners = new Dictionary<string,Action<object>> ();

	#region Listeners

	public void Register (string ID, Action<object> cb)
	{
		if (!Listeners.ContainsKey (ID)) {
			Listeners.Add (ID, cb);
		}
	}

	public void Remove (string ID)
	{
		if (Listeners.ContainsKey (ID)) {
			Listeners.Remove (ID);
		} 
	}

	#endregion

	private static GifManager instance;

	public static GifManager Instance {
		get { 
			if (instance == null) {
				GameObject singletonObject = Instantiate (Resources.Load<GameObject> ("Manager/GifManager"));
				instance = singletonObject.GetComponent<GifManager> ();
				singletonObject.name = "Singleton - GifManager";
			}
			return instance;
		}
	}

	public static bool HasInstance ()
	{
		return instance != null;
	}

	void Awake ()
	{
		if (instance != null && instance.GetInstanceID () != this.GetInstanceID ()) {
			Destroy (this.gameObject);
		} else {
			instance = this as GifManager;
			DontDestroyOnLoad (this.gameObject);
		}
	}

	#region NewSkin

	string pathPrefix = "UI/Gifts/";

	UISkinEquipConvert uiSkinEquipConvert{ get { return Instantiate (Resources.Load<GameObject> (pathPrefix + "UISkinEquipConvert"), rectHUD).GetComponent<UISkinEquipConvert> (); } }

	UISkinEquip uiSkinEquip{ get { return Instantiate (Resources.Load<GameObject> (pathPrefix + "UISkinEquip"), rectHUD).GetComponent<UISkinEquip> (); } }

	Action cardGift = null;

	public void RegisterCardGift (Action a)
	{
		cardGift = a;
	}

	public void ShowCardGift ()
	{
		if (cardGift != null) {
			cardGift.Invoke ();
			FBManagerEvent.Instance.PostEventCustom ("OpenCard_Show");
		}
	}

	public void Init ()
	{
		Debug.Log ("GifManager Init");
	}

	public void ShowSkin (EquipConfig equiConfig, Action a)
	{
		popUp = a;
		string key = EquipData.GetKey (equiConfig.equipType);
		bool bUnlocked = EquipData.GetUnlocked (key + "_" + equiConfig.ID);
		if (!bUnlocked) {
			EquipData.SetUnlocked (key + "_" + equiConfig.ID, true);
			uiSkinEquip.ShowNewSkin (equiConfig.equipType.ToString (),
				equiConfig.spr, EquipSkin, equiConfig.ID, equiConfig.level.GetHashCode ());
		} else {
			if (AllInOne.Instance.CheckVideoReward ()) {
				uiSkinEquipConvert.ShowNewSkin ("Skin available",
					equiConfig.spr, ConvertGold, equiConfig.ID, equiConfig.level.GetHashCode (),
					equiConfig.price
				);
			} else {
				CallPopup ();
			}
		}
	}

	void ConvertGold (object ob)
	{
		FBManagerEvent.Instance.PostEventCustom ("DailyConvertToGold");
		CallPopup ();
	}

	void EquipSkin (object ob)
	{
		string text = (string)ob;
		string[] texts = text.Split ('_');
		// EquipType + ID;
		if (Listeners.ContainsKey (texts [0])) {
			GiftEquip g = new GiftEquip ();
			g.ID = int.Parse (texts [1]);
			g.bPreview = texts [2].Equals ("1");
			Listeners [texts [0]].Invoke (g);
		} else {
			if (texts [2].Equals ("1")) {
				if (texts [0].Equals ("Wheels")) {
					BikeEditor.Instance.Preview (EquipType.Wheels, int.Parse (texts [1]), true);
				} else {
					SkinEditor.Instance.Preview (texts [0], int.Parse (texts [1]), true);
				}
			}
		} 
		CallPopup ();
	}

	public void EquipSkin (EquipConfig[] equip, bool bPreview)
	{
		for (int i = 0; i < equip.Length; i++) {
			if (Listeners.ContainsKey (equip [i].equipType.ToString ())) {
				GiftEquip g = new GiftEquip ();
				g.ID = equip [i].ID;
				g.bPreview = bPreview;
				Listeners [equip [i].equipType.ToString ()].Invoke (g);
			}
		}
	}

	#endregion

	#region Gold_FreeExchange

	UIFreeExchange goldExchange{ get { return Instantiate (Resources.Load<GameObject> (pathPrefix + "UIFreeExchangeGold"), rectHUD).GetComponent<UIFreeExchange> (); } }

	public void ExchangeGold (int gold, ExchangeType type, object ob)
	{
		switch (type) {
		case ExchangeType.GoldToSkin:
			EquipConfig equipConfig = (EquipConfig)ob;
			goldExchange.ShowExchangeSkin (_EquipSkin, gold, equipConfig);
			break;
		case ExchangeType.GoldX2:
			goldExchange.ShowExchangeGold (ExGold, gold, ob);
			break;
		case ExchangeType.GoldToHealth:
			ItemBoot item = new ItemBoot ();
			item.bootType = BootType.Health;
			item._valueBoot = (int)ob;
			goldExchange.ShowExchangeBoot (ExBoot, gold, item);
			break;
		case ExchangeType.GoldToShield:
			ItemBoot S = new ItemBoot ();
			S.bootType = BootType.Shield;
			S._valueBoot = (int)ob;
			goldExchange.ShowExchangeBoot (ExBoot, gold, S);
			break;
		case ExchangeType.GoldToWheels:
			EquipConfig wheels = (EquipConfig)ob;
			goldExchange.ShowExchangeSkin (_EquipSkin, gold, wheels);
			break;
		}
	}

	UIFreeExchangeGem uiFreeExchangeGem{ get { return Instantiate (Resources.Load<GameObject> (pathPrefix + "UIFreeExchangeGem"), rectHUD).GetComponent<UIFreeExchangeGem> (); } }

	public void x2Gem (int gem)
	{
		uiFreeExchangeGem.ShowExchangeGEM (ExGold, gem);
	}

	UIFreeExchangeKey uiFreeExchangeKey{ get { return Instantiate (Resources.Load<GameObject> (pathPrefix + "UIFreeExchangeKey"), rectHUD).GetComponent<UIFreeExchangeKey> (); } }

	public void x2Key (int key)
	{
		uiFreeExchangeKey.ShowExchangeKey (ExGold, key);
	}

	void ExGold (object ob)
	{
		string text = (string)ob;

		Debug.Log (text);

		string[] texts = text.Split ('_');
		bool x2 = texts [2].Equals ("1");
		int value = int.Parse (texts [1]);
		switch (texts [0]) {
		case "Gold":
			if (x2) {
				CoinManager.Instance.PurchaseCoin (value * 2);
			} else {
				CoinManager.Instance.PurchaseCoin (value);
			}
			break;
		case "Gem":
			if (x2) {
				CoinManager.Instance.PurchaseGem (value * 2, false);
			} else {
				CoinManager.Instance.PurchaseGem (value, false);
			}
			break;
		case "Key":
			if (x2) {
				CoinManager.Instance.PurchaserKey (value * 2);
			} else {
				CoinManager.Instance.PurchaserKey (value);
			}
			break;
		}
	}

	void ExBoot (object ob)
	{
		
	}

	public void _EquipSkin (object ob)
	{
		string text = (string)ob;
		string[] texts = text.Split ('_');
		if (texts [2].Equals ("1")) {
			//Done
			EquipConfig equipConfig = Resources.Load<EquipConfig> ("SkinData/" + texts [1] + "/" + texts [0]);
			ShowSkin (equipConfig, null);
		} else {
			//False
			int Gold = int.Parse (texts [3]);
		}
	}

	#endregion

	#region ShowRoll

	public void ShowRoll (int id, bool b, bool bRewared, Action<object> a)
	{
		//rollManager.ShowRoll (id, b, bRewared, a);
	}

	public void OffRoll ()
	{
		//rollManager.OffRoll ();
	}

	#endregion

	#region UIFreeExchangeBoot

	UIFreeExchangeBoot uiExchangeBoot {
		get {
			return Instantiate (Resources.Load<GameObject> (pathPrefix + "UIFreeExchangeBoot"), rectHUD).GetComponent<UIFreeExchangeBoot> ();
		}
	}

	Action popUp = null;

	public void X2Boot (ItemBoot item, Action a)
	{
		popUp = a;
		ItemBoot n = new ItemBoot ();
		n.bootType = item.bootType;
		n._valueBoot = item._valueBoot * 2;
		uiExchangeBoot.ExChangeX2 (item, n, _X2Boot);
	}

	void _X2Boot (object ob)
	{
		string text = (string)ob;
		string[ ] texts = text.Split ('_');
		bool x2 = false;
		if (texts [2].Equals ("1")) {
			x2 = true;
		} else {
			x2 = false;
		}
		int v = int.Parse (texts [1]);
		switch (texts [0]) {
		case "Health":
			int lateH = CharacterData.GetBoot (BootType.Health);
			if (x2) {
				lateH += (v * 2);
				Manager.Instance.ShowToas (v * 2, TypeReward.Health, true);
			} else {
				lateH += v;
				Manager.Instance.ShowToas (v, TypeReward.Health, true);
			}
			CharacterData.SetBoot (BootType.Health, lateH);
			break;
		case "Shield":
			int lateS = CharacterData.GetBoot (BootType.Shield);
			if (x2) {
				lateS += (v * 2);
				Manager.Instance.ShowToas (v * 2, TypeReward.Shield, true);
			} else {
				lateS += v;
				Manager.Instance.ShowToas (v, TypeReward.Shield, true);
			}
			CharacterData.SetBoot (BootType.Shield, lateS);
			break;
		}
		CallPopup ();
	}

	void CallPopup ()
	{
		if (popUp != null) {
			popUp.Invoke ();
			popUp = null;
		}
	}

	#endregion

	#region Gif

	[SerializeField]Transform rectHUD = null;

	public void GifNow (object[] obs)
	{
		UIGift ui = Instantiate (Resources.Load<GameObject> ("UI/UIGift"), rectHUD).GetComponent<UIGift> ();
		ui.Register (obs);
	}

	#endregion

}
