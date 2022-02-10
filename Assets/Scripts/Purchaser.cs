using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using TungDz;

// Placing the Purchaser class in the CompleteProject namespace allows it to interact with ScoreManager,
// one of the existing Survival Shooter scripts.
//namespace CompleteProject
//{
// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
public class Purchaser : MonoBehaviour, IStoreListener
{
	[SerializeField]string[] idProduct;
	public static Purchaser Instance;
	private static IStoreController m_StoreController;
	// The Unity Purchasing system.
	private static IExtensionProvider m_StoreExtensionProvider;
	// The store-specific Purchasing subsystems.
	Action<object> ActionBuyComplete;

	public void ReqestPurchase (Action<object> ac, int idPack)
	{
		ActionBuyComplete = ac;
		BuyProductID (idProduct [idPack]);
		//Test
//		ActionBuyComplete.Invoke ("done_" + idPack);
//		AllInOne.Instance.RemoveAllAds ();
	}
	void Awake ()
	{
		if (Instance == null)
			Instance = this;
	}

	void Start ()
	{		
		if (m_StoreController == null) {			
			InitializePurchasing ();
		}
//		if (IsInitialized ())
//			CheckRemoveAds (PRODUCT_1_COIN);
			
	}
	//	public void CheckRemoveAds (string productId)
	//	{
	//		Product product = m_StoreController.products.WithID (productId);
	//		if (product != null && product.hasReceipt) {
	//			// Owned Non Consumables and Subscriptions should always have receipts.
	//			// So here the Non Consumable product has already been bought.
	//			RemoveAds ();
	//		}
	//
	//	}
	//
	//	public void RemoveAds ()
	//	{
	//
	//	}

	public void InitializePurchasing ()
	{		
		if (IsInitialized ()) {			
			return;
		}
		var builder = ConfigurationBuilder.Instance (StandardPurchasingModule.Instance ());
		for (int i = 0; i < idProduct.Length; i++) {
			builder.AddProduct (idProduct [i], ProductType.Consumable);
		}

		UnityPurchasing.Initialize (this, builder);
	}


	public bool IsInitialized ()
	{	
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}


	void BuyProductID (string productId)
	{		
		if (IsInitialized ()) {						
			Product product = m_StoreController.products.WithID (productId);
			if (product != null && product.availableToPurchase) {
				Debug.Log (string.Format ("Purchasing product asychronously: '{0}'", product.definition.id));
				m_StoreController.InitiatePurchase (product);
				FBManagerEvent.Instance.PostEventCustom ("Request_IAP " + product.definition.id);

			} else {				
				Debug.Log ("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
				ActionBuyComplete.Invoke ("fail_0");
			}
		} else {
			Debug.Log ("BuyProductID FAIL. Not initialized.");
			ActionBuyComplete.Invoke ("fail_0");
		}
	}

	public void RestorePurchases ()
	{		
		if (!IsInitialized ()) {			
			Debug.Log ("RestorePurchases FAIL. Not initialized.");
			return;
		}
			
		if (Application.platform == RuntimePlatform.IPhonePlayer
		    || Application.platform == RuntimePlatform.OSXPlayer) {			
			Debug.Log ("RestorePurchases started ...");

			var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions> ();

			apple.RestoreTransactions ((result) => {				
				Debug.Log ("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
			});
		} else {			
			Debug.Log ("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
		}
	}
	//
	// --- IStoreListener
	//
	public void OnInitialized (IStoreController controller, IExtensionProvider extensions)
	{		
		Debug.Log ("OnInitialized: PASS");

		m_StoreController = controller;
		m_StoreExtensionProvider = extensions;
	}

	public void OnInitializeFailed (InitializationFailureReason error)
	{		
		Debug.Log ("OnInitializeFailed InitializationFailureReason:" + error);
	}

	public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs args)
	{
		// A consumable product has been purchased by this user.
		for (int i = 0; i < idProduct.Length; i++) {
			if (String.Equals (args.purchasedProduct.definition.id, idProduct [i], StringComparison.Ordinal)) {
				//		ShopCoinButton.Instance.UpdateCoin (100);
				ActionBuyComplete.Invoke ("done_" + i);
				FBManagerEvent.Instance.PostBuyInAppEvent (args.purchasedProduct.definition.id, (float)i, "USD");
				FBManagerEvent.Instance.PostEventCustom ("IAP " + args.purchasedProduct.definition.id);
				AllInOne.Instance.RemoveAllAds ();

			}
		}
		return PurchaseProcessingResult.Complete;
	}

	public void OnPurchaseFailed (Product product, PurchaseFailureReason failureReason)
	{
		// A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
		// this reason with the user to guide their troubleshooting actions.
		Debug.Log (string.Format ("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
		ActionBuyComplete.Invoke ("fail_0");
		FBManagerEvent.Instance.PostEventCustom ("Fail_IAP " + product.definition.storeSpecificId);
	}

}
//}
