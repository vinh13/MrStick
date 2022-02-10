using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MusicManager : MonoBehaviour
{
	[SerializeField]AudioSource audioSource = null;
	private static MusicManager instance = null;
	MusicData musicData = null;

	public static MusicManager Instance {
		get { 

			if (instance == null) {

				GameObject singletonObject = new GameObject ();
				instance = singletonObject.AddComponent<MusicManager> ();
				singletonObject.name = "Singleton - MusicManager";
			}
			return instance;
		}
	}

	public static bool HasInstance ()
	{
		return instance != null;
	}

	void Awake ()
	{
		if (instance != null && instance.GetInstanceID () != this.GetInstanceID ()) {
			Destroy (gameObject);
			return;
		} else {
			instance = this as MusicManager;
			DontDestroyOnLoad (gameObject);
		}
		musicData = Resources.Load<MusicData> ("AudioData/MFX");
		audioSource = gameObject.AddComponent<AudioSource> ();
	}

	public void PlayMusic ()
	{
		//Play ("music1");
	}

	public void Play (string name)
	{
		audioSource.Stop ();
		Music m = Array.Find (musicData.Musics, music => music.mName == name);
		if (m == null)
			return;
		audioSource.volume = m.fVolume;
		audioSource.pitch = m.fPitch;
		audioSource.clip = m.clip;
		audioSource.loop = m.loop;
		audioSource.Play ();
	}

	public void Stop (string name)
	{
		audioSource.Stop ();
	}

}
