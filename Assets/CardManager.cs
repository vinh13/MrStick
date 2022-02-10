using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
	[SerializeField]CardGiftCantro cardGiftCantro = null;

	void Awake ()
	{
		GifManager.Instance.RegisterCardGift (Show);
	}

	public void Show ()
	{
		cardGiftCantro.Show (true);
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.D)) {
			Show ();
		}
	}
}
