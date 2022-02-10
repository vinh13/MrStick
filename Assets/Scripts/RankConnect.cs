using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankConnect : MonoBehaviour
{
	public bool bDeath = false;
	public int ID = 0;
	public AILevel aiLevel = AILevel.None;

	public float GetPosX {
		get {
			return transform.position.x;
		}
	}
	public int iLap = 1;
	public int iRank = 0;
}
