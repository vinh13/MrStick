using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterConfig
{
	public float HP = 0;
	public float ATK = 0;
	public float SPD = 0;
}

public class Character : ScriptableObject
{
	public CharacterConfig[] config = new CharacterConfig[10];
}
