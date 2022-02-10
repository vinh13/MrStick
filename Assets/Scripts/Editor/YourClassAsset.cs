using UnityEngine;
using UnityEditor;

public class YourClassAsset
{
	//	[MenuItem ("Create/GameAsset/AudioData")]
	//	public static void CreateAudioData ()
	//	{
	//		ScriptableObjectUtility.CreateAsset<AudioData> ();
	//	}
	[MenuItem ("Create/GameAsset/EnemyData")]
	public static void CreateSpawnData ()
	{
		ScriptableObjectUtility.CreateAsset<SpawnData> ();
	}

	[MenuItem ("Create/GameAsset/AudioData")]
	public static void CreateAudioData ()
	{
		ScriptableObjectUtility.CreateAsset<AudioData> ();
	}

	[MenuItem ("Create/GameAsset/MusicData")]
	public static void CreateMusicData ()
	{
		ScriptableObjectUtility.CreateAsset<MusicData> ();
	}

	[MenuItem ("Create/GameAsset/Character")]
	public static void CreateCharacter ()
	{
		ScriptableObjectUtility.CreateAsset<Character> ();
	}

	[MenuItem ("Create/GameAsset/Skin")]
	public static void CreateSkin ()
	{
		ScriptableObjectUtility.CreateAsset<Skin> ();
	}

	[MenuItem ("Create/GameAsset/DailyReward")]
	public static void CreateDailyReward ()
	{
		ScriptableObjectUtility.CreateAsset<DailyConfig> ();
	}

	[MenuItem ("Create/GameAsset/SkinData")]
	public static void CreateDailyRewardSkinData ()
	{
		ScriptableObjectUtility.CreateAsset<EquipConfig> ();
	}

	[MenuItem ("Create/GameAsset/RollData")]
	public static void RollData ()
	{
		ScriptableObjectUtility.CreateAsset<RollData> ();
	}

	[MenuItem ("Create/GameAsset/PartSkinData")]
	public static void _SkinDataPart ()
	{
		ScriptableObjectUtility.CreateAsset<PartSkinData> ();
	}

	[MenuItem ("Create/GameAsset/Achie")]
	public static void Achie ()
	{
		ScriptableObjectUtility.CreateAsset<AchievementConfig> ();
	}

	[MenuItem ("Create/GameAsset/GiftData")]
	public static void Gifdata ()
	{
		ScriptableObjectUtility.CreateAsset<GiftCode> ();
	}

	[MenuItem ("Create/GameAsset/Nitro")]
	public static void Nitro ()
	{
		ScriptableObjectUtility.CreateAsset<NitroData> ();
	}

	[MenuItem ("Create/GameAsset/SkinLevel")]
	public static void DataLevel ()
	{
		ScriptableObjectUtility.CreateAsset<SkinDataLevel> ();
	}
}