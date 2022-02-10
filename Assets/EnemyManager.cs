using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TungDz;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
	public static EnemyManager Instance = null;
	Action<object> _StartRace = null;
	List<Action> listStart = new List<Action> ();
	[SerializeField]Text textKill = null, textAlive = null;
	[SerializeField]UILog_Kill uiLog_Kill = null;
	[SerializeField]UILog_Kill uiLog_Combo = null;
	int numberEnemy = 0;
	int numberKill = 0;
	int maxEnemy = 0;
	[SerializeField]float timePenalty = 1F;
	bool bWaitKill = false;
	int countKill = 0;

	public float DamageDone {
		get {
			return damageDone;
		}
	}

	float damageDone = 0;

	public void AddDamage (float d)
	{
		damageDone += d;
	}

	string[] sfxs = {"firstblood", "killingspree", "dominating", "unstoppable", "godlike", "legendary", "legendarykill"
	};
	string[] sfxs_kill = { "double", "tripple", "quadra", "penta", "hexa"
	};

	public void Setup (int e)
	{
		numberEnemy = e;
		maxEnemy = e;
		numberKill = 0;
		UpdateText ();
	}

	public void RegisterStart (Action a)
	{
		listStart.Add (a);
	}

	void Awake ()
	{
		if (Instance == null)
			Instance = this;
		_StartRace = (param) => StartRace ();
		EventDispatcher.Instance.RegisterListener (EventID.StartRace, _StartRace);
	}

	void StartRace ()
	{
		for (int i = 0; i < listStart.Count; i++) {
			listStart [i].Invoke ();
		}
		EventDispatcher.Instance.RemoveListener (EventID.StartRace, _StartRace);
	}

	void OnDestroy ()
	{
		int late = AchievementData.TopKiller;
		late += numberKill;
		AchievementData.TopKiller = late;
		EventDispatcher.Instance.RemoveListener (EventID.StartRace, _StartRace);
	}

	public bool CheckEnemy {
		get { 
			return 0 == numberEnemy;
		}
	}

	public bool CheckBoss {
		get {
			return 1 == numberEnemy;
		}
	}

	void ShowKill (string text, int id)
	{
		if (!Logic.bBoss) {
			uiLog_Kill.Show (text);
		}
		SFXManager.Instance.Play (sfxs [id]);
	}

	bool blockCombo = false;

	void ShowCombo (string text, int id)
	{
		if (!Logic.bBoss) {
			uiLog_Combo.Show (text);
		}
		if (!blockCombo) {
			blockCombo = true;
			StartCoroutine (_ShowCombo ());
		} else {
			if (id > 0) {
				SFXManager.Instance.Stop (sfxs_kill [id - 1]);
			}
		}
		SFXManager.Instance.Play (sfxs_kill [id]);
	}

	IEnumerator _ShowCombo ()
	{
		yield return new WaitForSeconds (1F);
		blockCombo = false;
	}

	int countSfx = 0;
	bool bFirst = false;

	public void RemoveEnemy ()
	{
		numberEnemy -= 1;
		numberKill += 1;
		if (numberEnemy <= 0) {
			AIEnemyManager.Instance.DisableDistance ();
		}
		UpdateText ();
		bool bPlayCombo = false;
		countKill++;
		if (!bWaitKill) {
			lateCountKill = countKill;
			bWaitKill = true;
			StartCoroutine (WaitKill ());
		} else {
			switch (countKill) {
			case 2:
				ShowCombo ("Doublekill", 0);
				bPlayCombo = true;
				break;
			case 3:
				ShowCombo ("Tripplekill", 1);
				bPlayCombo = true;
				break;
			case 4:
				ShowCombo ("Quadrakill", 2);
				bPlayCombo = true;
				break;
			case 5:
				ShowCombo ("Pentakill", 3);
				bPlayCombo = true;
				break;
			case 6:
				ShowCombo ("Hexakill", 4);
				bPlayCombo = true;
				break;
			}
		}
		if (!bPlayCombo) {
			countSfx++;
			LogKill (countSfx);
		}

	}

	void LogKill (int ef)
	{
		ef = Mathf.Clamp (ef, 1, 7);
		switch (ef) {
		case 1:
			ShowKill ("First blood", 0);
			break;
		case 3:
			ShowKill ("Killing spree", 1);
			break;
		case 4:
			ShowKill ("Unstoppable", 3);
			break;
		case 5:
			ShowKill ("Dominating", 2);
			break;
		case 6:
			ShowKill ("Godlike", 4);
			break;
		case 7:
			ShowKill ("Legendary", 5);
			break;
		}
	}

	int lateCountKill = 0;

	IEnumerator WaitKill ()
	{
		yield return new WaitForSeconds (timePenalty);
		if (lateCountKill != countKill) {
			lateCountKill = countKill;
			StartCoroutine (WaitKill ());
		} else {
			bWaitKill = false;
			lateCountKill = 0;
			countKill = 0;
		}
	}

	public void ShowBoss ()
	{
		uiLog_Kill.ShowBoss ();
	}

	void UpdateText ()
	{
		textKill.text = "" + numberKill;
		textAlive.text = "" + (numberEnemy + 1);
	}

	public int GetKill {
		get {
			return numberKill;
		}
	}
}
