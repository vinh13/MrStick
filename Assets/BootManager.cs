using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BootType
{
	None = 0,
	Health = 1,
	Shield = 2,
}

public class BootManager : MonoBehaviour
{
	[SerializeField]ButtonTdz[] btnsBoot;
	[SerializeField]float[] durationsCd;
	[SerializeField]float durationShield = 0;
	[SerializeField]float ratioRestoreHealth = 0.3F;

	void Awake ()
	{
		if (CharacterData.bFirtTimeBoot) {
			CharacterData.SetBoot (BootType.Health, 1);
			CharacterData.SetBoot (BootType.Shield, 1);
		}
	}

	void Start ()
	{
		ButtonItem btH = btnsBoot [0] as ButtonItem;
		btH.Setup (BootType.Health, ActiveBoot, durationsCd [0]);
		ButtonItem btS = btnsBoot [1] as ButtonItem;
		btS.Setup (BootType.Shield, ActiveBoot, durationsCd [1]);
		bool bH = false, bS = false;
		if (!TutorialData.bTutorialStart) {
			bH = CharacterData.GetBoot (BootType.Health) >= 1;
			bS = CharacterData.GetBoot (BootType.Shield) >= 1;
		} else {
			bH = false;
			bS = false;
		}
		PlayerControl.Instance.UpdateTutorial (bH, bS);
	}

	void ActiveBoot (BootType t)
	{
		switch (t) {
		case BootType.Shield:
			if (!Logic.bShield) {
				PlayerControl.Instance.ShowShield (true);
				TaskUtil.ScheduleWithTimeScale (this, this.ResetShield, durationShield);
			} else {
				if (PlayerControl.Instance.bShieldRestore) {
					PlayerControl.Instance.ShowShield (true);
					TaskUtil.ScheduleWithTimeScale (this, this.ResetShield, durationShield);
				}
			}
			break;
		case BootType.Health:
			PlayerControl.Instance.RestoreHealth (ratioRestoreHealth);
			break;
		}
	}

	void ResetShield ()
	{
		PlayerControl.Instance.ShowShield (false);
	}
}
