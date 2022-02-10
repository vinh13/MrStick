public class CharacterData
{
	public static int GetBoot (BootType bootType)
	{
		return TaskUtil.GetInt ("Boot_" + bootType);
	}

	public static void SetBoot (BootType bootType, int newValue)
	{
		TaskUtil.SetInt ("Boot_" + bootType, newValue);
	}

	public static void AddBoot (BootType bootType, int newValue)
	{
		int late = GetBoot (bootType);
		late += newValue;
		SetBoot (bootType, late);
	}

	public static string ObjectStart {
		get {
			return TaskUtil.GetString ("ObjectStart");
		}
		set {
			TaskUtil.SetString ("ObjectStart", value);
		}
	}

	public static int GetCountVideoBoot (string bootType)
	{
		return TaskUtil.GetInt ("CountVideo" + bootType.ToString ());
	}

	public static void SetCountVideoBoot (string bootType, int newCount)
	{
		TaskUtil.SetInt ("CountVideo" + bootType, newCount);
	}

	public static bool bFirtTimeBoot {
		get {
			bool b = TaskUtil.GetInt ("bFirtTimeBoot_") == 0 ? true : false;
			if (b) {
				TaskUtil.SetInt ("bFirtTimeBoot_", 1);
			}
			return b;
		}
	}
}
