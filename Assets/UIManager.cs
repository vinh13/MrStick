using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
	public bool bIgnoreReady = false;
	public static UIManager Instance = null;
	[SerializeField]Transform rectHUD = null;
	[SerializeField]Text textTime = null;
	[SerializeField]Image _UIGame = null;
	bool bTutorial = false;
	public bool bWin = false;
	public bool bWinWithKillAll = false;

	#region TutorialPlay

	public void CreateTutorialAttack2 (UIGame uiGame)
	{
		this.gameObject.AddComponent<TutorialAttack> ().Register (rectHUD, uiGame);
	}


	#endregion

	public int TimeGame {
		get {
			return timeGame;
		}
	}

	int timeGame = 0;
	public bool bEnd = false;

	void Awake ()
	{
		bTutorial = TutorialData.bTutorialStart;
		if (Instance == null)
			Instance = this;
		Logic.UNPAUSE ();
		EnemyLogic.bStop = false;
		textTime.text = "00:00";
		bEnd = false;
		GifManager.Instance.Init ();
	}

	void UpdateTime ()
	{
		TimeSpan temp = TimeSpan.FromSeconds (timeGame);
		string s = string.Format ("{0:D2} : {1:D2}", temp.Minutes, temp.Seconds);
		textTime.text = s;
	}

	public void StartTime ()
	{
		Timing ();
	}

	void Timing ()
	{
		TaskUtil.ScheduleWithTimeScale (this, this._Timing, 1F);
	}

	void _Timing ()
	{
		timeGame++;
		UpdateTime ();
		Timing ();
	}

	void Start ()
	{
		CreateReady ();
	}

	public void ClickPause ()
	{
		if (bTutorial)
			return;
		//ShowUIGame (false);
		Logic.PAUSE ();
		GameObject go = CreateUI ("UI/UIPause");
		UILogEndGame ui = go.GetComponent<UILogEndGame> ();
		ui.Hide (HidePause);
	}

	void HidePause ()
	{
		//ShowUIGame (true);
		Logic.UNPAUSE ();
	}

	void CreateReady ()
	{
		GameObject go = CreateUI ("UI/UIReady");
	}

	GameObject CreateUI (string p)
	{
		return Instantiate (Resources.Load<GameObject> (p), rectHUD);
	}

	#region WinLose

	UILogEndGame cacheLogEndGame = null;

	public void Win (bool winBykill)
	{
		if (bEnd)
			return;
		bEnd = true;
		Logic.bShield = true;
		EffectManager.Instance.PlayEffectWin ();
		//CameraControl.Instance.ShowPlayer ();
		if (!bTutorial) {
			bWinWithKillAll = winBykill;
			bWin = true;
			ShowUIGame (false);
			GameObject go = CreateUI ("UI/UIWin");
			cacheLogEndGame = go.GetComponent<UILogEndGame> ();
			cacheLogEndGame.Show ();
			TaskUtil.Schedule (this, HideWinLose, 2F);
			MapManager.Intance.OnEnd (true);
		} else {
			ShowEndTutorial ();
		}
	}

	public void Lose ()
	{
		if (bEnd)
			return;
		bEnd = true;
		if (!bTutorial) {
			bWin = false;
			ShowUIGame (false);
			GameObject go = CreateUI ("UI/UILose");
			cacheLogEndGame = go.GetComponent<UILogEndGame> ();
			cacheLogEndGame.Show ();
			TaskUtil.Schedule (this, HideWinLose, 2F);
			MapManager.Intance.OnEnd (false);
		} else {
			ShowEndTutorial ();
		}
	}

	void ShowEndTutorial ()
	{
		bTutorial = false;
		Logic.PAUSE ();
		ShowUIGame (false);
		GameObject go = CreateUI ("UI/UITutorialEnd");
	}

	void HideWinLose ()
	{
		cacheLogEndGame.Hide (OnHideWinLose);
	}

	void OnHideWinLose ()
	{
		Logic.PAUSE ();
		GameObject go = CreateUI ("UI/UIInfo");
		cacheLogEndGame = go.GetComponent<UILogEndGame> ();
		cacheLogEndGame.Show ();
	}

	#endregion

	#region UIGame

	public void ShowUIGame (bool b)
	{
		_UIGame.enabled = !b;
	}

	#endregion

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.T)) {
			Win (true);
		}
		if (Input.GetKeyDown (KeyCode.L)) {
			Lose ();
		}
	}
}
