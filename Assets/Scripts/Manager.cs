using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
	Preload = 0,
	Main = 1,
	Home = 2,
	Level = 3,
	InApp = 4,
	DailyReward = 5,
	Player = 6,
	PlayerPreview = 7,
	VideoReward = 8,
	Achievement = 9,
	FlashSale = 10,
	StartOffer = 11,
	Chest = 12,
}

public class Manager : MonoBehaviour
{
	#region Manager

	private static Manager instance;

	public static Manager Instance {
		get { 

			if (instance == null) {
				GameObject singletonObject = Instantiate (Resources.Load<GameObject> ("Manager/Manager"));
				instance = singletonObject.GetComponent<Manager> ();
				singletonObject.name = "Singleton - Manager";
			}
			return instance;
		}
	}
	[SerializeField]UIToast uiToast = null;
	public void ShowToas (int v, TypeReward typeReward, bool bPlus)
	{
		switch (typeReward) {
		case TypeReward.Gold:
			uiToast.AddCoin ("" + v, bPlus);
			break;
		case TypeReward.Shield:
			uiToast.AddShield ("" + v, bPlus);
			break;
		case TypeReward.Health:
			uiToast.AddHealth ("" + v, bPlus);
			break;
		case TypeReward.Gem:
			uiToast.AddGem ("" + v, bPlus);
			break;
		case TypeReward.Key:
			uiToast.AddKey ("" + v, bPlus);
			break;
		}
	}

	public static bool HasInstance ()
	{
		return instance != null;
	}

	void Awake ()
	{
		if (instance != null && instance.GetInstanceID () != this.GetInstanceID ()) {
			Destroy (gameObject);
		} else {
			instance = this as Manager;
			uiNotEnough._Start ();
			DontDestroyOnLoad (gameObject);

		}
	}

	[SerializeField]Transform tWaitting = null;

	public void ShowWaitting (bool b)
	{
		tWaitting.gameObject.SetActive (b);
	}

	public void ShowPlayer (bool b)
	{
		if (b) {
			Logic.bPlayerLoadDone = false;
			if (!SceneManager.GetSceneByName (SceneName.Player.ToString ()).isLoaded)
				SceneManager.LoadSceneAsync (SceneName.Player.ToString (), LoadSceneMode.Additive);
		} else {
			if (SceneManager.GetSceneByName (SceneName.Player.ToString ()).isLoaded)
				SceneManager.UnloadSceneAsync (SceneName.Player.GetHashCode ());
		}
	}

	public void ShowPlayerPreview (bool b)
	{
		if (b) {
			if (!SceneManager.GetSceneByName (SceneName.PlayerPreview.ToString ()).isLoaded)
				SceneManager.LoadSceneAsync (SceneName.PlayerPreview.ToString (), LoadSceneMode.Additive);
		} else {
			if (SceneManager.GetSceneByName (SceneName.PlayerPreview.ToString ()).isLoaded)
				SceneManager.UnloadSceneAsync (SceneName.PlayerPreview.GetHashCode ());
		}
	}


	public void ShowShop (bool bGem)
	{
		CacheScene.bInAppShowGem = bGem;
		if (!CacheScene.bInShop) {
			CacheScene.bInShop = true;
			Logic.PAUSE ();
			Manager.instance.ShowWaitting (true);
			SceneManager.LoadSceneAsync (SceneName.InApp.GetHashCode (), LoadSceneMode.Additive);
		} else {
			TungDz.EventDispatcher.Instance.PostEvent (EventID.RequestShop, "");
		}
	}

	[SerializeField]UINotEnough uiNotEnough = null;

	public void ShowNotEnough (TypePurchase typeReward, string t, string e,int en)
	{
		uiNotEnough.Show (typeReward, t,e ,en);
	}

	#endregion

	#region LoadScene

	public void LoadScene (SceneName sceneName, bool Preload)
	{
		if (!Preload) {
			StartCoroutine (LoadRoutine (sceneName.GetHashCode ()));
		} else {
			CacheScene.indexScene = sceneName.GetHashCode ();
			SceneManager.LoadScene (0, LoadSceneMode.Single);
		}
	}

	private string loadProgress = "Loading...";
	private string lastLoadProgress = null;

	private IEnumerator LoadRoutine (int indexLevel)
	{
		yield return new WaitForSecondsRealtime (0.25F);
		AsyncOperation op = SceneManager.LoadSceneAsync (indexLevel, LoadSceneMode.Single);
		op.allowSceneActivation = true;
		while (!op.isDone) {
			if (op.progress < 0.9f) {
				loadProgress = "Loading: " + (op.progress * 100f).ToString ("F0") + "%";
			} else { // if progress >= 0.9f the scene is loaded and is ready to activate.
				//				if (Input.anyKeyDown) {
				//					op.allowSceneActivation = true;
				//				}
				//loadProgress = "Loading ready for activation, Press any key to continue";
			}
			if (lastLoadProgress != loadProgress) {
				lastLoadProgress = loadProgress;
				Debug.Log (loadProgress);
			} // Don't spam console.
			yield return null;
		}
		loadProgress = "Load complete.";
		Debug.Log (loadProgress);
	}

	public bool AcceptLoadScene = false;

	private IEnumerator LoadRoutine_Before (int indexLevel)
	{
		yield return new WaitForSecondsRealtime (0.25F);
		AsyncOperation op = SceneManager.LoadSceneAsync (indexLevel);
		op.allowSceneActivation = false;
		while (!op.isDone) {
			if (op.progress < 0.9f) {
				loadProgress = "Loading: " + (op.progress * 100f).ToString ("F0") + "%";
			} else {
				// if progress >= 0.9f the scene is loaded and is ready to activate.
				if (AcceptLoadScene) {
					op.allowSceneActivation = true;
				}
				loadProgress = "Loading ready for activation, Press any key to continue";
			}
			if (lastLoadProgress != loadProgress) {
				lastLoadProgress = loadProgress;
				Debug.Log (loadProgress);
			} // Don't spam console.
			yield return null;
		}
		loadProgress = "Load complete.";
		Debug.Log (loadProgress);
	}

	#endregion

}
