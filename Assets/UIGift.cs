using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGift : MonoBehaviour
{
	string pathPrefix = "Gift/";
	List<object> skinData = new List<object> ();
	[SerializeField]Transform rectPanel = null;
	[SerializeField]UIButton btnQuit = null;

	void Start ()
	{
		btnQuit.Register (ClickQuit);
		btnQuit.Block (false);
	}

	void ClickQuit ()
	{
		Destroy (this.gameObject);
	}

	public void Register (object[] obs)
	{
		GifData[] gifs = new GifData[obs.Length];
		for (int i = 0; i < gifs.Length; i++) {
			gifs [i] = (GifData)obs [i];
			if (gifs [i].gifType != GifType.Skin) {
				CreateGift (gifs [i]);
			} else {
				Debug.Log ("add new data");
				skinData.Add (gifs [i].Ob);
			}
		}

		if (skinData.Count != 0) {
			CreateSkin ();
		}
	}

	void CreateGift (GifData gif)
	{
		switch (gif.gifType) {
		case GifType.Key:
			RollGold key = CreateGift (pathPrefix + "Key").GetComponent<RollGold> ();
			key.SetGold ((string)gif.Ob);
			int k = int.Parse ((string)gif.Ob);
			CoinManager.Instance.PurchaserKey (k);
			break;
		case GifType.Gold:
			RollGold gold = CreateGift (pathPrefix + "Gold").GetComponent<RollGold> ();
			gold.SetGold ((string)gif.Ob);
			int ig = int.Parse ((string)gif.Ob);
			CoinManager.Instance.PurchaseCoin (ig);
			break;
		case GifType.Gem:
			RollGold gem = CreateGift (pathPrefix + "Gem").GetComponent<RollGold> ();
			gem.SetGold ((string)gif.Ob);
			int ge = int.Parse ((string)gif.Ob);
			CoinManager.Instance.PurchaseGem (ge, false);
			break;
		case GifType.Wheels:
			SkinFull wheel = CreateGift (pathPrefix + "Skin").GetComponent<SkinFull> ();
			wheel.Register (gif.Ob);
			break;
		case GifType.Health:
			RollItem health = CreateGift (pathPrefix + "Item").GetComponent<RollItem> ();
			health.SetItem ((int)gif.Ob, BootType.Health);
			int h = int.Parse ((string)gif.Ob);
			CharacterData.AddBoot (BootType.Health, h);
			break;
		case GifType.Shield:
			RollItem shield = CreateGift (pathPrefix + "Item").GetComponent<RollItem> ();
			shield.SetItem ((int)gif.Ob, BootType.Shield);
			int s = int.Parse ((string)gif.Ob);
			CharacterData.AddBoot (BootType.Shield, s);
			break;
		}
	}

	void CreateSkin ()
	{
		SkinFull skin = CreateGift (pathPrefix + "Skin").GetComponent<SkinFull> ();
		skin.Register (skinData.ToArray ());
		Debug.Log ("CreateSkin");
	}

	GameObject CreateGift (string path)
	{
		return Instantiate (Resources.Load<GameObject> (path), rectPanel);
	}
}
