using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public class Daily
{
	public TypeReward typeReward = TypeReward.None;
	public int ValueReward = 0;
	public Sprite spr = null;
	public EquipType equipType = EquipType.None;
	public EquipConfig equiConfig = null;
}
public class DailyConfig : ScriptableObject
{
	public Daily[] listDaily = new Daily[7];
}
