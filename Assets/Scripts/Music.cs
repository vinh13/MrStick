using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Music
{
	public string mName = "";
	public AudioClip clip = null;
	[Range (0, 1F)]
	public float fVolume = 1F;
	[Range (0, 1F)]
	public float fPitch = 1F;
	public bool loop = false;
}
