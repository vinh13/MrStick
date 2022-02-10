using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipConfig : ScriptableObject
{
	public EquipType equipType = EquipType.None;
	public EquipLevel level = EquipLevel.None;
	public int price = 0;
	public Sprite spr = null;
	public int skinId = 0;
	public int ID = 0;
}
