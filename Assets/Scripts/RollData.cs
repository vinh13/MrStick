using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RollConfig
{
	public TypeReward typeReward = TypeReward.None;
	public string _data = "";
}
public class RollData : ScriptableObject
{
	public RollConfig[] rollsConfig;
}
