public class VideoRewardData
{
	public static int CountVideo {
		get {
			return TaskUtil.GetInt ("VideoRewardData_CountVideo");
		}
		set {
			TaskUtil.SetInt ("VideoRewardData_CountVideo", value);
		}
	}

	public static void ResetVideo ()
	{
		TaskUtil.SetInt ("VideoRewardData_CountVideo", 0);
	}

	public static int GetValueReward (float min, float max)
	{
		float temp = min + ((max - min) / 100F) * (float)GameData.PlayerLevel;
		return (int)temp;
	}
}
