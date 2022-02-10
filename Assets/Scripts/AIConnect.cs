using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIConnect : MonoBehaviour,AIInterface
{
	public int ID {
		get { 
			return _ID;
		}
		set {
			if (_ID == -100) {
				GetComponent<VehicleStatus> ().ID = value;
			}
			_ID = value;
		}
	}

	private int _ID = -100;
	AICall aiCall = null;
	RankConnect rankConnect = null;
	[HideInInspector]public float damage = 0;
	public void Init (EnemyData data = null)
	{
		damage = data.damageBase;
		GameObject AI = Instantiate (Resources.Load<GameObject> ("AIBase"), transform);
		AI.transform.localPosition = Vector3.zero;
		VehicleControl temp = this.gameObject.GetComponent<VehicleControl> ();
		temp.RegisterOnEnd (RemoveIndexDistance);
		temp.SetSkin (ColorManager.Instance.colors [data.aiLevel.GetHashCode () - 1].colors, data.aiLevel.GetHashCode () - 1);
		aiCall = AI.GetComponent<AICall> ();
		aiCall.Register (temp, data);
	}

	public void Active (bool b, Vector2 pos)
	{
		gameObject.SetActive (b);
		if (b)
			aiCall.Init ();
		transform.position = pos;
	}

	public void StartVehicle ()
	{
		aiCall.StartVehicle ();
		aiCall.UpSpeed (3F);
	}

	public RankConnect GetRankConnect ()
	{
		rankConnect = gameObject.AddComponent<RankConnect> ();
		rankConnect.ID = ID;
		return rankConnect;
	}

	public void StartRace ()
	{
		aiCall.StartRace ();
	}

	int IndexDistance = 0;

	public void RegisterIndexDistance (int index)
	{
		IndexDistance = index;
		TakeDistance ();
	}

	void RemoveIndexDistance ()
	{
		StopAllCoroutines ();
		AIEnemyManager.Instance.RemoveIndexChange (IndexDistance);
	}

	void TakeDistance ()
	{
		StartCoroutine (_TakeDistance ());
	}

	IEnumerator _TakeDistance ()
	{
		yield return new WaitForSeconds (GameConfig.durationUpdateDistance);
		TakeDistance ();
		AIEnemyManager.Instance.ChangeDistance (rankConnect.GetPosX, IndexDistance);
	}

}
