public class UpgradeData
{
	public static int GetUpgradeLevel (UpgradeType type)
	{
		return TaskUtil.GetInt ("UpgradeLevel" + type.ToString ());
	}

	public static void SetUpgradeLevel (UpgradeType type, int newLevel)
	{
		TaskUtil.SetInt ("UpgradeLevel" + type.ToString (), newLevel);
	}

	public static int GetUpgradeLevelBonus (UpgradeType type)
	{
		return TaskUtil.GetInt ("UpgradeLevelBonus_" + type.ToString ());
	}

	public static void SetUpgradeLevelBonus (UpgradeType type, int newLevel)
	{
		TaskUtil.SetInt ("UpgradeLevelBonus_" + type.ToString (), newLevel);
	}

	public static int GetLevelEquipTypeBonus (EquipType type)
	{
		return TaskUtil.GetInt ("LevelEquipTypeBonus" + type.ToString ());
	}

	public static void SetLevelEquipTypeBonus (EquipType type, int newLevel)
	{
		TaskUtil.SetInt ("LevelEquipTypeBonus" + type.ToString (), newLevel);
	}

}
