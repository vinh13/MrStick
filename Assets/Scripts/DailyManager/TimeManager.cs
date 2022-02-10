using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
	public static TimeManager Instance;
	private string _url = "http://leatonm.net/wp-content/uploads/2017/candlepin/getdate.php";
	//change this to your own
	private string _timeData;
	private string _currentTime;
	public string _currentDate;


	public bool isTEST = false;
	public string _currentDateTEST;
	public string _currentTimeTEST;
	//make sure there is only one instance of this always.
	public bool TimeIsReady = false;
	public int CountDay = 0;
	public bool bSameDay = false;

	void CheckSameDay ()
	{
		string lateDay = TaskUtil.GetString ("Tnh_TimeManager_StartGame");
		if (lateDay == "") {
			lateDay = TimeSystem ();
		}
		string curDay = TimeSystem ();
		bSameDay = DayToInt (TextToDateTime (lateDay)) >= DayToInt (TextToDateTime (curDay));
		if (!bSameDay) {
			CountDay += 1;
			GameData.DayCount = CountDay;
			GameData.ResetAllRewardVideo ();
			GameData.ClearRoll ();
			//GameData.BlockShowFlashSale = false;
			//GameData.CountClickExit = 0;
			//VideoRewardData.ResetVideo ();
			TaskUtil.SetString ("Tnh_TimeManager_StartGame", TimeSystem ());
			VideoRewardData.ResetVideo ();
			GameData.VideoNotEnoughCoin = 0;
			//VipSurvival ();
		}
	}
	//	void VipSurvival ()
	//	{
	//		int rankVip = GameData.UserVip;
	//
	//		int count = VipMemberData.MaxCountPlaySurvaival [rankVip];
	//
	//		SurvivalData.CountPlayVip = count - 3;
	//		SurvivalData.CountPlay = 3;
	//	}
	//
	void Awake ()
	{
		if (Instance == null)
			Instance = this;
		CountDay = GameData.DayCount;
		if (CountDay == 0) {
			TaskUtil.SetString ("Tnh_TimeManager_StartGame", TimeSystem ());
			CountDay = 1;
			GameData.DayCount = CountDay;
			bSameDay = false;
			//VipSurvival ();
		} else {
			CheckSameDay ();
		}
	}
	//time fether coroutine
	public IEnumerator getTime ()
	{
		Debug.Log ("connecting to php");
		WWW www = new WWW (_url);
		yield return www;
		if (www.error != null) {
			TimeIsReady = false;
		} else {
			Debug.Log ("got the php information");
			_timeData = www.text;
			string[] words = _timeData.Split ('/');	
			//timerTestLabel.text = www.text;
			Debug.Log ("The date is : " + words [0]);
			Debug.Log ("The time is : " + words [1]);
			//setting current time
			_currentDate = words [0];
			_currentTime = words [1];
			TimeIsReady = true;
		}
	}


	//get the current time at startup
	void Start ()
	{
		Debug.Log ("TimeManager script is Ready.");
		StartCoroutine ("getTime");
	}

	string TimeSystem ()
	{
		int day = System.DateTime.Now.Day;
		int month = System.DateTime.Now.Month;
		int year = System.DateTime.Now.Year;
		string text = "" + month + "-" + day + "-" + year;
		return text;
	}
	//get the current date - also converting from string to int.
	//where 12-4-2017 is 1242017
	public int getCurrentDateNowInt ()
	{
		if (isTEST) {
			_currentDate = _currentDateTEST;
		}
		if (!TimeIsReady) {
			_currentDate = TimeSystem ();
		}
		string[] words = _currentDate.Split ('-');
		int x = int.Parse (words [0] + words [1] + words [2]);
		return x;			
	}

	public long getCurrentDateNow ()
	{
		if (isTEST) {
			_currentDate = _currentDateTEST;
		}
		if (!TimeIsReady) {
			_currentDate = TimeSystem ();
		}
		string[] words = _currentDate.Split ('-');
		DateTime date = new DateTime (int.Parse (words [2]), int.Parse (words [0]), int.Parse (words [1]));
		return DayToInt (date);
	}

	public long DayToInt (DateTime date)
	{
		int secondOfOneDay = 24 * 60 * 60;
		int tickUnit = 10000000;
		return date.Ticks / tickUnit / secondOfOneDay;
	}

	//get the current Time
	public string System_getCurrentTimeNow ()
	{
		//		if (isTEST) {
		//			_currentTime = _currentTimeTEST;
		//		}
		//		if (!TimeIsReady) {
		int h = System.DateTime.Now.Hour;
		int m = System.DateTime.Now.Minute;
		int s = System.DateTime.Now.Second;
		string text = "" + h + ":" + m + ":" + s;
		//	}
		return text;
	}

	public string Internet_GetCurrentTimeNow ()
	{
		return _currentTime;
	}

	DateTime TextToDateTime (string text)
	{
		string[] words = text.Split ('-');
		DateTime date = new DateTime (int.Parse (words [2]), int.Parse (words [0]), int.Parse (words [1]));
		return date;
	}

}
