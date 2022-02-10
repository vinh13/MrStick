using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[System.Serializable]
public class LeaderboardEntry
{
    public string leaderboardID;
    public string keyPrefs;
}

public class AllInOne : MonoBehaviour
{
    Action<bool> callbackVideo;
    private static AllInOne instance;
    public int countAdmob = 0;
    public bool videoShowing = false;

    public static AllInOne Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = Instantiate(Resources.Load<GameObject>("AllInOne/AllInOne"));
                instance = go.GetComponent<AllInOne>();
            }

            return instance;
        }
    }

    [HideInInspector] public bool bStarted = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        isLeaderboardLogin = false;
        countBanner = TaskUtil.GetInt("countBanner");
        removeADS = TaskUtil.GetInt("removeAds") == 0 ? false : true;
        countAdmob = TaskUtil.GetInt("CountADS");
        blockAds = false;
    }

    public void Init()
    {
    }
#if UNITY_ANDROID
    [SerializeField] [Header("Android ==> Add Component now!")]
    LeaderboardCustomAndroid lbAndroid;
#endif
#if UNITY_IPHONE
	[SerializeField][Header ("IOS ==> Add Component now!")]
	LeaderboardCustomIOS lbIOS;
#endif
    LeaderboardEntry[] lbentry;
    public bool isLeaderboardLogin = false;
    public bool removeADS = false;
    int tempADS = 1;

    void Start()
    {
        //UnityAds.SetGDPRConsentMetaData (true);
        bShowed = false;
        bCallbacked = false;
        ShowBottom();
    }

    public void ShowBottom()
    {
        //if (!removeADS)
        //{
        //	if (LevelData.LevelPlay >= 2)
        //	{
        //		ShowAdmobBanner();
        //	}
        //}
    }


    public void setLeadboardId(LeaderboardEntry[] temp)
    {
        this.lbentry = temp;
    }

    public void submitScore()
    {
        if (!isLeaderboardLogin)
            return;
#if UNITY_ANDROID
        if (lbAndroid != null)
        {
            for (int i = 0; i < lbentry.Length; i++)
            {
                lbAndroid.SummitScores(lbentry[i].leaderboardID, lbentry[i].keyPrefs);
            }
        }
#endif
#if UNITY_IOS
		if (lbIOS != null) {
		for (int i = 0; i < lbentry.Length; i++) {
		lbIOS.SummitScores (lbentry [i].leaderboardID, lbentry [i].keyPrefs);
		}
		}
#endif
    }

    int countBanner = 0;

    public void RemoveAllAds()
    {
        if (removeADS)
            return;
        HideAdmobBanner();
        removeADS = true;
        TaskUtil.SetInt("removeAds", 1);
    }

    //	public void loadAdmob (string _banner, string fullbanner_, string _video)
    //	{
    //	}

    public void ShowAdmobBanner()
    {
        if (!removeADS)
        {
            //admobController.ShowBanner ();
            //IronSource.Agent.displayBanner();
            //IronSourceManager.Instance.ShowBanner();
        }
        else
        {
            HideAdmobBanner();
        }
    }

    public void HideAdmobBanner()
    {
        //admobController.HideBanner ();
        //IronSourceManager.Instance.HideBanner();
    }

    bool blockAds = false;
    Action<bool> aShowAdmobFULL = null;

    public void ShowAdmobFULL(Action<bool> a, string wh, int level)
    {
        if (removeADS)
        {
            a.Invoke(false);
            return;
        }

        countAdmob += 1;
        TaskUtil.SetInt("CountADS", countAdmob);
        if (countAdmob < tempADS)
        {
            a.Invoke(false);
            return;
        }

        if (blockAds)
        {
            a.Invoke(false);
            return;
        }
#if UNITY_ANDROID || UNITY_IOS
        // if (IronSourceManager.Instance.checkInter)
        // {
        // 	//IronSourceManager.Instance.ShowInterstitialAd(wh, level);
        // 	blockAds = true;
        // 	aShowAdmobFULL = a;
        // 	StartCoroutine(delayShowAdmobFULL());
        // }
        // else
        // {
        a.Invoke(false);
        //}
#else
		a.Invoke (false);
#endif
    }

    public void cbShowAdmobFULL(bool b)
    {
        if (aShowAdmobFULL != null)
        {
            aShowAdmobFULL.Invoke(b);
            aShowAdmobFULL = null;
        }
    }

    IEnumerator delayShowAdmobFULL()
    {
        yield return new WaitForSecondsRealtime(100);
        blockAds = false;
    }

    //	public void InitunityAds (string idunityADs)
    //	{
    //		unityadsCustom.InitUnityADS (idunityADs);
    //	}
    public void _OnLoginLB()
    {
#if UNITY_EDITOR
        print("Login Leaderboard");
#elif UNITY_ANDROID
		lbAndroid.LoginLB();
#elif UNITY_IPHONE
		lbIOS.LoginLB();
#endif
    }

    public void _OnShowLB()
    {
#if UNITY_EDITOR
        print("Show LB");
#elif UNITY_ANDROID
		lbAndroid.OnShowLeaderBoard();
#elif UNITY_IPHONE
		lbIOS.showLeaderBoardsUI();
#endif
    }

    bool bShowed = false;

    #region VideoReward

    float timerPenanty = 5F;
    bool bVideoStarted = false;

    public bool ShowVideoReward(Action<bool> callback, string wh, int level)
    {
        if (bShowed)
            return false;
        timerPenanty = 5F;
        bShowed = true;
#if UNITY_ANDROID || UNITY_IOS
        bool temp = Show_AdmobVideoRewarded(wh, level);
        callbackVideo = callback;
        if (temp)
        {
            Manager.Instance.ShowWaitting(true);
            bVideoStarted = false;
            StartCoroutine(_ShowVideoReward());
            Debug.Log("Show video!!!");
        }
        else
        {
            Manager.Instance.ShowWaitting(false);
            callbackVideo.Invoke(false);
            callbackVideo = null;
            Debug.Log("Not video!!!");
        }

        return temp;
#else
		callbackVideo = callback;
		OnVideoComplete (true);
		return true;
#endif
    }

    IEnumerator _ShowVideoReward()
    {
        yield return new WaitForSecondsRealtime(timerPenanty);
        if (!bVideoStarted)
        {
            //Debug.Log ("Video khong dc chay");
            //OnVideoComplete (false);
            bVideoStarted = true;
        }
    }

    public void VideoShowed()
    {
        Debug.Log("Video dc chay");
        bVideoStarted = true;
    }

    bool Show_AdmobVideoRewarded(string wh, int lv)
    {
        return true;
        //return true;
    }

    bool bCallbacked = false;

    public void OnVideoComplete(bool bcomplete)
    {
        Debug.Log("On complete video " + bcomplete);
        bShowed = false;
        if (callbackVideo == null)
            return;
        if (bCallbacked)
            return;
        bCallbacked = true;
        StartCoroutine(CompleteNow(bcomplete));
    }

    IEnumerator CompleteNow(bool bcomplete)
    {
        yield return new WaitForSecondsRealtime(0.5F);
        if (callbackVideo != null)
        {
            Manager.Instance.ShowWaitting(false);
            callbackVideo.Invoke(bcomplete);
            callbackVideo = null;
        }

        bCallbacked = false;
        //		int lateCount = GameData.CountVideoAllTime;
        //		lateCount += 1;
        //		GameData.CountVideoAllTime = lateCount;
    }

    public bool CheckVideoReward()
    {
#if UNITY_EDITOR
        return true;
#else
		return IronSourceManager.Instance.IsVideoAdsAvailable();
#endif
    }

    void OnApplicationFocus(bool hasFocus)
    {
        cbShowAdmobFULL(true);
    }

    #endregion
}