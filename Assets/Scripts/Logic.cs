using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Logic
{
	public static bool bWarmup = false;
	public static bool isPause = false;
	public static bool bNext = false;
	public static bool bStarted = false;
	public static bool bBoss = false;
	public static bool bShield = false;
	public static bool GameStared = false;
	public static bool bPlayerLoadDone = false;
	public static bool bReady = false;
	public static bool bShowAds = false;
	public static string sWhere = "";
	public static void PAUSE ()
	{
		if (isPause)
			return;
		isPause = true;
		Time.timeScale = 0;
	}

	public static void UNPAUSE ()
	{
		if (!isPause)
			return;
		isPause = false;
		Time.timeScale = 1;
	}
}
