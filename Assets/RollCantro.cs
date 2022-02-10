using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class ConfigRoll
{
	public Vector3 localPos = Vector3.zero;
	public Vector3 localS = Vector3.zero;
}

public class RollCantro : MonoBehaviour
{
	[SerializeField]Transform panelS;
	[SerializeField]ConfigRoll[] configRolls;
	[SerializeField]UIButton btnGif = null;
	[SerializeField]RollScript[] rollS;
	[SerializeField]float fSpeed = 0, fSpeedScale = 0.25F;
	[SerializeField]AnimatorPopUpScript animPop = null;
	int _IDTarget = 0;
	float speedM = 0;
	bool bCheck = false;
	Action<object> _cbRolldone = null;

	void OnValidate ()
	{
		rollS = panelS.GetComponentsInChildren<RollScript> ();
		for (int i = 0; i < configRolls.Length; i++) {
			rollS [i].ID = i;
			configRolls [i].localPos = rollS [i].transform.localPosition;
			configRolls [i].localS = rollS [i].transform.localScale;
		}
	}

	public void Show (bool b)
	{
		if (b) {
			animPop.show (null);
		} else {
			animPop.hide (null);
		}
	}

	void Start ()
	{
		speedM = fSpeed;
		btnGif.Register (Clickback);
		for (int i = 0; i < configRolls.Length; i++) {
			rollS [i].ID = i;
		}
	}

	public void Clickback ()
	{
		GifManager.Instance.OffRoll ();
	}

	public void BlockExit (bool b)
	{
		btnGif.Block (b);
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
			speedM -= 800;
			if (speedM <= 800) {
				if (!bRecheck) {
					bRecheck = true;
					StartCoroutine (_recheck ());
				}
			}
		}
		if (speedM == 0)
			speedM = 800;
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
			if (rollS [3].ID == _IDTarget) {
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
