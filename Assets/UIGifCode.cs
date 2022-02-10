using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGifCode : MonoBehaviour
{
	[SerializeField]UIButton btnQuit = null, btnGif = null, btnMore = null;
	[SerializeField]InputField textInput = null;
	[SerializeField]Text textStatus = null;
	string[] gifText = {"mrstick@1010gg", "mrstick@1100sk", "mrstick@1090sk"
	};

	void Start ()
	{
		btnQuit.Register (ClickQuit);
		btnGif.Register (ClickGif);
		btnMore.Register (ClickMore);
	}

	void ClickQuit ()
	{
		Destroy (this.gameObject);
	}

	void ClickMore ()
	{
		HomeManager.Instance.ConnectFacebook ();
	}

	void ClickGif ()
	{
		string text = textInput.text;
		int index = -100;
		for (int i = 0; i < gifText.Length; i++) {
			if (text.Equals (gifText [i])) {
				index = i;
				break;
			}
			
		}
		if (index != -100) {
			bool bGift = GameData.GifCode (text);
			if (bGift) {
				textStatus.text = "Giftcode used";
			} else {
				textStatus.text = "Giftcode";
				Claim (index + 1);
			}
		} else {
			textStatus.text = "Giftcode not found";
			Debug.Log ("Giftcode not found");
		}
	}

	void Claim (int index)
	{
		GiftCode giftCode = Resources.Load<GiftCode> ("Gift/GiftCode" + (index));
		List<GifData> listData = new List<GifData> ();
		switch (giftCode.giftType) {
		case GifType.Gold:
			GifData gold = new GifData ();
			gold.gifType = giftCode.giftType;
			gold.Ob = giftCode.data;
			listData.Add (gold);
			break;
		case GifType.Gem:
			GifData gem = new GifData ();
			gem.gifType = giftCode.giftType;
			gem.Ob = giftCode.data;
			listData.Add (gem);
			break;
		case GifType.Skin:
			for (int i = 0; i < giftCode.obs.Length; i++) {
				GifData _Skin = new GifData ();
				_Skin.gifType = giftCode.giftType;
				_Skin.Ob = (EquipConfig)giftCode.obs [i];
				listData.Add (_Skin);
			}
			break;
		case GifType.Wheels:
			for (int i = 0; i < giftCode.obs.Length; i++) {
				GifData wheel = new GifData ();
				wheel.gifType = giftCode.giftType;
				wheel.Ob = (EquipConfig)giftCode.obs [i];
				listData.Add (wheel);
			}
			break;
		case GifType.Health:
			GifData health = new GifData ();
			health.gifType = giftCode.giftType;
			health.Ob = giftCode.data;
			listData.Add (health);
			break;
		case GifType.Shield:
			GifData shield = new GifData ();
			shield.gifType = giftCode.giftType;
			shield.Ob = giftCode.data;
			listData.Add (shield);
			break;
		}
		GifManager.Instance.GifNow (listData.ToArray ());
	}
}
