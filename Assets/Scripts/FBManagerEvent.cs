using UnityEngine;
using System.Collections;
using Facebook.Unity;
using System.Collections.Generic;

public class FBManagerEvent : Singleton<FBManagerEvent>
{
	protected FBManagerEvent ()
	{
		//
	}

	public void Init ()
	{
		
	}

	public void PostBuyInAppEvent (string packageName, float priceAmount, string priceCurrency)
	{
		if (!FB.IsInitialized)
			return;
		var iapParameters = new Dictionary<string, object> ();
		iapParameters ["tung_dz_vl"] = packageName;
		FB.LogPurchase (
			priceAmount,
			priceCurrency,
			iapParameters
		);
	}

	public void PostBuyUnlockLevel (int Story, int level)
	{
		if (!FB.IsInitialized) {
			return;
		}
		FB.LogAppEvent ("Complete_Map_" + Story + "level_" + level, 1);
	}

	public void PostEventCustom (string s)
	{
		#if UNITY_ANDROID
		string app = "n2_";
		#elif UNITY_IPHONE
		string app = "b3_";
		#endif
		//if (FirebaseCantro.Instance != null)
		//	FirebaseCantro.Instance.PostEventCustom (app + s);
		if (!FB.IsInitialized) {
			return;
		}
		FB.LogAppEvent (app + s, 1);
	}

	public void LogTimePlay (double timePlay, double valToSum, string nameLevel)
	{
		#if UNITY_ANDROID
		string app = "";
		#elif UNITY_IPHONE
		string app = "I2";
		#endif
		var parameters = new Dictionary<string, object> ();
		parameters ["TimePlay"] = timePlay;
		FB.LogAppEvent (
			app + nameLevel,
			(float)valToSum,
			parameters
		);
	}
}
