public class TutorialData
{
	public static bool bTutorialStart {
		get {
			return TaskUtil.GetInt ("TutorialStart") == 0;
		}
		set {
			TaskUtil.SetInt ("TutorialStart", value ? 0 : 1);
		}
	}

	public static bool bMenuTutorial {
		get {
			return TaskUtil.GetInt ("bMenuTutorial") == 0;
		}
		set { 
			TaskUtil.SetInt ("bMenuTutorial", value ? 0 : 1);
		}
	}

	public static bool bLevelTutorial {
		get {
			return TaskUtil.GetInt ("bLevelTutorial") == 0;
		}
		set { 
			TaskUtil.SetInt ("bLevelTutorial", value ? 0 : 1);
		}
	}

	public static bool bBeforePlay {
		get {
			return TaskUtil.GetInt ("bBeforePlay") == 0;
		}
		set { 
			TaskUtil.SetInt ("bBeforePlay", value ? 0 : 1);
		}
	}

	public static bool bUpgradeATKDirec {
		get {
			return TaskUtil.GetInt ("bUpgradeATKDirec") == 0;
		}
		set { 
			TaskUtil.SetInt ("bUpgradeATKDirec", value ? 0 : 1);
		}
	}

	public static bool bTutorialUpgradeATK {
		get {
			return TaskUtil.GetInt ("bTutorialUpgradeATK") == 0;
		}
		set { 
			TaskUtil.SetInt ("bTutorialUpgradeATK", value ? 0 : 1);
		}
	}

	public static bool bDonePlay {
		get {
			return TaskUtil.GetInt ("bDonePlay_Tutorial") == 0;
		}
		set { 
			TaskUtil.SetInt ("bDonePlay_Tutorial", value ? 0 : 1);
		}
	}

	public static bool bUseHealth {
		get {
			return TaskUtil.GetInt ("Tutorial_bUseHealth") == 0;
		}
		set { 
			TaskUtil.SetInt ("Tutorial_bUseHealth", value ? 0 : 1);
		}
	}

	public static bool bUseShield {
		get {
			return TaskUtil.GetInt ("Tutorial_bUseShield") == 0;
		}
		set { 
			TaskUtil.SetInt ("Tutorial_bUseShield", value ? 0 : 1);
		}
	}

	public static bool bUseWeapon {
		get {
			return TaskUtil.GetInt ("Tutorial_bUseWeapon") == 0;
		}
		set { 
			TaskUtil.SetInt ("Tutorial_bUseWeapon", value ? 0 : 1);
		}
	}

	public static bool bAttack_2Hand {
		get {
			bool b = LevelData.GetUnlock ("Map1_level_7");
			if (b) {
				return true;
			} else {
				return TaskUtil.GetInt ("Tutorial_bAttack_2Hand") == 0;
			}
		}
		set { 
			TaskUtil.SetInt ("Tutorial_bAttack_2Hand", value ? 0 : 1);
		}
	}

	public static bool bToATK = false;
}
