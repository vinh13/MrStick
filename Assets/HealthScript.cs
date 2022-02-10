using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthScript : MonoBehaviour
{
	public float HP = 100;
	float maxHP = 0;
	bool bDeathed = false;
	HPBar hpBar = null;
	List<Action<float>> listChange = new List<Action<float>> ();

	public float GetMaxHP {
		get { 
			return HP;
		}
	}

	public void RegisterChange (Action<float> a)
	{
		listChange.Add (a);
	}

	public void Init (HPBar _hpbar)
	{
		hpBar = _hpbar;
		hpBar.Change (1);
	}

	public void SetHealth (float hp, HPBar _bar = null)
	{
		HP = hp;
		maxHP = HP;
	}

	public bool ChangeHealth (float dame)
	{
		if (bDeathed)
			return true;
		HP -= dame;
		HP = Mathf.Clamp (HP, 0, maxHP);
		bDeathed = HP == 0;
		float ratio = HP / maxHP;
		for (int i = 0; i < listChange.Count; i++) {
			listChange [i].Invoke (ratio);
		}
		hpBar.Change (ratio);
		if (bDeathed)
			hpBar.Disable ();
		return bDeathed;
	}

	public void RestoreHealth (float r)
	{
		float reH = maxHP * r;
		HP += reH;
		HP = Mathf.Clamp (HP, 0, maxHP);
		float ratio = HP / maxHP;
		hpBar.Change (ratio);
	}

}
