using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NitroConfig
{
	public float display = 0;
	public float time = 0;
	public float damage = 0;
}

public class NitroData : ScriptableObject
{
	public NitroConfig[] nitroConfigs;
}
