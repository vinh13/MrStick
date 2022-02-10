using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChestCantro : MonoBehaviour
{
	[SerializeField]Transform panelS;
	[SerializeField]ConfigRoll[] configRolls;
	[SerializeField]RollScript[] rollS;
	[SerializeField]float fSpeed = 0, fSpeedScale = 0.25F;
	int _IDTarget = 0;
	float speedM = 0;
	bool bCheck = false;
	Action<object> _cbRolldone = null;
	public void _OnValidate ()
	{
		rollS = panelS.GetComponentsInChildren<RollScript> ();
		configRolls = new ConfigRoll[rollS.Length];
		for (int i = 0; i < configRolls.Length; i++) {
			rollS [i].ID = i;
			configRolls [i] = new ConfigRoll ();
			configRolls [i].localPos = rollS [i].transform.localPosition;
			configRolls [i].localS = rollS [i].transform.localScale;
		}
	}

	public void ResetRoll ()
	{
		RollScript temp = new RollScript ();
		for (int i = 0; i < rollS.Length; i++) {
			for (int j = i + 1; j < rollS.Length; j++) {
				if (rollS [j].ID < rollS [i].ID) {
					temp = rollS [i];
					rollS [i] = rollS [j];
					rollS [j] = temp;
				}
			}
		}
		ResetPos ();
	}

	void ResetPos ()
	{
		for (int i = 0; i < rollS.Length; i++) {
			rollS [i].transform.localPosition = configRolls [i].localPos;
			rollS [i].transform.localScale = configRolls [i].localS;
		}
	}

	void Start ()
	{
		speedM = fSpeed;
		for (int i = 0; i < configRolls.Length; i++) {
			rollS [i].ID = i;
		}
	}

	public void Roll (int idTarget, Action<object> a)
	{
		bCheck = false;
		_IDTarget = idTarget;
		speedM = fSpeed;
		_cbRolldone = a;
		bRecheck = false;
		_Roll ();	
	}

	void _Roll ()
	{
		int number = rollS.Length;
		for (int i = 0; i < number; i++) {
			if (i == 0) {
				rollS [i].Move (configRolls [i].localPos, speedM, MoveNow);
			} else if (i == 1) {
				rollS [i].Move (configRolls [i].localPos, speedM, null);
			} else {
				rollS [i].Move (configRolls [i].localPos, speedM, null);
			}
			rollS [i]._Scale (configRolls [i].localS, fSpeedScale);
		}
	}

	void MoveNow ()
	{
		rollS [0].MoveNow (configRolls [rollS.Length - 1].localPos);
		if (speedM > 0) {
			speedM -= 1000;
			if (speedM <= 1000) {
				if (!bRecheck) {
					bRecheck = true;
					StartCoroutine (_recheck ());
				}
			}
		}
		if (speedM == 0)
			speedM = 1000;
		RollDone ();
	}

	IEnumerator _recheck ()
	{
		yield return new WaitForSecondsRealtime (0.5F);
		bCheck = true;
	}

	bool bRecheck = false;

	void RollDone ()
	{
		if (!CheckDone ()) {
			Switch ();
			_Roll ();
		} else {
			Switch ();
			StartCoroutine (delayDone ());
		}
	}

	IEnumerator delayDone ()
	{
		yield return new WaitForSecondsRealtime (0.8F);
		_cbRolldone.Invoke (_IDTarget);

	}

	bool CheckDone ()
	{
		if (bCheck) {
			if (rollS [2].ID == _IDTarget) {
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}

	void Switch ()
	{
		for (int i = 0; i <= rollS.Length - 2; i++) {
			RollScript temp = rollS [i];
			rollS [i] = rollS [i + 1];
			rollS [i + 1] = temp;
		}
	}
}
