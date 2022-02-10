using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

[System.Serializable]
public enum TypeSFX
{
	None = 0,
	hurt = 1,
	dead = 2,
	blood = 3,
	gunmachine = 4,
	bow = 5,
	rocket1 = 6,
	pickwp = 7,
	rocket2 = 8,
}

public class SFXManager : MonoBehaviour
{
	AudioData audioData = null;
	private static SFXManager instance;
	int lateHurt = 0;
	int lateDead = 0;
	int lateBlood = 0;
	int latePick = 0;
	int lateBreak = 0;
	bool bBlockHurt = false;
	bool bBlockBlood = false;
	bool bBlockObjectHit = false;
	bool bBlockBreak = false;

	public static SFXManager Instance {
		get { 

			if (instance == null) {

				GameObject singletonObject = new GameObject ();
				instance = singletonObject.AddComponent<SFXManager> ();
				singletonObject.name = "Singleton - SFXManager";
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
			instance = this as SFXManager;
			DontDestroyOnLoad (gameObject);
		}

		audioData = Resources.Load<AudioData> ("AudioData/SFX");
		foreach (var s in audioData.Sounds) {
			s.audioSource = gameObject.AddComponent<AudioSource> ();
			s.audioSource.clip = s.clip;
			s.audioSource.volume = s.fVolume;
			s.audioSource.pitch = s.fPitch;
			s.audioSource.loop = s.loop;
		}
	}

	public void Play (TypeSFX typeSFx)
	{
		string s = typeSFx.ToString ();
		int index = 0;
		if (typeSFx == TypeSFX.hurt) {
			if (!bBlockHurt) {
				index = Index (5, ref lateHurt);
				Play (s + "" + index);
				bBlockHurt = true;
				TaskUtil.Schedule (this, this.UnlockHurt, 0.5F);
			}
		} else if (typeSFx == TypeSFX.dead) {
			index = Index (6, ref lateDead);
			//Play (s + "" + index);
		} else if (typeSFx == TypeSFX.blood) {
			if (!bBlockBlood) {
				bBlockBlood = true;
				index = Index (3, ref lateBlood);
				Play (s + "" + index);
				TaskUtil.Schedule (this, this.UnlockBlood, 0.5F);
			}
		} else if (typeSFx == TypeSFX.gunmachine ||
		           typeSFx == TypeSFX.bow ||
		           typeSFx == TypeSFX.rocket1 ||
		           typeSFx == TypeSFX.rocket2) {
			Play (s);

		} else if (typeSFx == TypeSFX.pickwp) {
			index = Index (4, ref latePick);
			Play (s + "" + index);
		}
	}

	public void ObjectHit (ObjectType obType)
	{
		if (!bBlockObjectHit) {
			switch (obType) {
			case ObjectType.Saw:
				Play ("saw1");
				TaskUtil.Schedule (this, this._ObjectHit, 0.5F);
				bBlockObjectHit = true;
				break;
			}
		}
	}

	void _ObjectHit ()
	{
		bBlockObjectHit = false;
	}

	void UnlockHurt ()
	{
		bBlockHurt = false;
	}

	void UnlockBlood ()
	{
		bBlockBlood = false;
	}

	int Index (int range, ref int id)
	{
		int index = Random.Range (1, range);
		if (index != id) {
			id = index;
			return index;
		} else {
			return Index (range, ref id);
		}
	}

	public void Play (string name)
	{
		Sound s = Array.Find (audioData.Sounds, sound => sound.sName == name);
		if (s == null)
			return;
		s.audioSource.Play ();
	}

	public void PlayNitro (bool b)
	{
		Sound s = audioData.Sounds [48];
		if (b) {
			s.audioSource.Play ();
		} else {
			s.audioSource.Stop ();
		}
	}

	public void Stop (string name)
	{
		Sound s = Array.Find (audioData.Sounds, sound => sound.sName == name);
		if (s == null)
			return;
		s.audioSource.Stop ();
	}

	public void PlayBreak ()
	{
		if (!bBlockBreak) {
			int index = Index (3, ref lateBreak);
			Play ("bonebreak" + "" + index);
			bBlockBreak = true;
			TaskUtil.Schedule (this, this.UnlockBreak, 0.5F);
		}
	}

	void UnlockBreak ()
	{
		bBlockBreak = false;
	}
}
