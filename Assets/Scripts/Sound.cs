using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
	public string sName = "";
	public AudioClip clip = null;
	[Range (0, 1F)]
	public float fVolume = 1F;
	[Range (0, 2F)]
	public float fPitch = 1F;
	public bool loop = false;
	[HideInInspector]
	public AudioSource audioSource = null;
}
