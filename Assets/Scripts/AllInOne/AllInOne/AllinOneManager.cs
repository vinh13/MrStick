using UnityEngine;
using System.Collections;

public class AllinOneManager : MonoBehaviour
{
    //	[Header ("Admob Banner Defaulf-------------------                   ---------------------")]
    //	[SerializeField]string admobIdBanerDefult = "";
    //	[SerializeField]string admobIdBanerDefult_IOS = "";
    //	[Header ("Admob Interstitial Defaulf--------------                   ---------------------")]
    //	[SerializeField]string admobIdInterstitialDefult = "";
    //	[SerializeField]string admobIdInterstitialDefult_IOS = "";
    //	[Header ("Admob Video Reward--------------                   ---------------------")]
    //	[SerializeField]string admobIdVideoReward = "";
    //	[SerializeField]string admobIdVideoReward_IOS = "";
    //	[Header ("LEADERBOARD-----------------------                   ---------------------")]
    public LeaderboardEntry[] leaderboardIDs;
    public LeaderboardEntry[] leaderboardIDs_IOS;

    void Start()
    {
        if (!AllInOne.Instance.bStarted)
        {
#if UNITY_ANDROID
            AllInOne.Instance.setLeadboardId(leaderboardIDs);
            //		AllInOne.Instance.loadAdmob (admobIdBanerDefult, admobIdInterstitialDefult, admobIdVideoReward);
            AllInOne.Instance._OnLoginLB();
#elif UNITY_IOS
            AllInOne.Instance.setLeadboardId(leaderboardIDs_IOS);
            //			AllInOne.Instance.loadAdmob (admobIdBanerDefult_IOS, admobIdInterstitialDefult_IOS, admobIdVideoReward_IOS);
            AllInOne.Instance._OnLoginLB ();
#endif
            AllInOne.Instance.bStarted = true;
        }
    }
}
