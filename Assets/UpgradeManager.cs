using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
	public static UpgradeManager Instance = null;
	[SerializeField]IUpgrade[] iUpgrades;
	Character characterData = null;
	public float[] prices = new float[10];
	[SerializeField]Color32[] colorBars = new Color32[3];
	[SerializeField]string[] paths;
	[SerializeField]Transform posEf = null;
	ParticleSystem[] parsS;
	[SerializeField]TextMeshScript textMesh = null;
	[SerializeField]Transform posText = null;
	[SerializeField]UIUpgradeHero uiUpgradeHero = null;

	void Awake ()
	{
		if (Instance == null)
			Instance = this;
		characterData = Resources.Load<Character> ("Character/Character1");
		parsS = new ParticleSystem[paths.Length];
	}

	public void ShowUpgrade (bool b)
	{
		uiUpgradeHero.Show (b);
	}

	public void PlayText (UpgradeType t, string text)
	{
//		switch (t) {
//		case UpgradeType.Health:
//			textMesh.SexTextNormal (posText.position, text + " HP", colorBars [0]);
//			break;
//		case UpgradeType.Speed:
//			textMesh.SexTextNormal (posText.position, text + " SPD", colorBars [1]);
//			break;
//		case UpgradeType.ATK:
//			textMesh.SexTextNormal (posText.position, text + " ATK", colorBars [2]);
//			break;
//		}
	}

	public void Play (int id)
	{
		if (parsS [id] == null) {
			parsS [id] = Effect (id);
		}
		parsS [id].Play ();
	}

	ParticleSystem Effect (int id)
	{
		return  Instantiate (Resources.Load<GameObject> (paths [id]), posEf).
			GetComponent<ParticleSystem> ();

	}

	public float GetConfig (UpgradeType t, int curLevel)
	{
		CharacterConfig config = characterData.config [curLevel - 1];
		if (t == UpgradeType.Health)
			return config.HP;
		else if (t == UpgradeType.Speed)
			return config.SPD;
		else {
			return config.ATK;
		}
	}

	public float GetPrice (UpgradeType t, int curLevel)
	{
		return prices [curLevel - 1];
	}

	public void ClearPreview ()
	{
		for (int i = 0; i < iUpgrades.Length; i++) {
			iUpgrades [i].ClearPreview ();
		}
	}

	public void UpdateBonus (UpgradeType t, int level)
	{
		switch (t) {
		case UpgradeType.Health:
			iUpgrades [0].UpdateBonus (level);
			break;
		case UpgradeType.Speed:
			iUpgrades [1].UpdateBonus (level);
			break;
		case UpgradeType.ATK:
			iUpgrades [2].UpdateBonus (level);
			break;
		}
	}

	public void Preview (UpgradeType t, int level, bool bShow)
	{
		switch (t) {
		case UpgradeType.Health:
			if (bShow) {
				iUpgrades [0].Preview ((float)level / 20F, true);
			} else {
				iUpgrades [0].Preview (0, false);
			}
			break;
		case UpgradeType.Speed:
			if (bShow) {
				iUpgrades [1].Preview ((float)level / 20F, true);
			} else {
				iUpgrades [1].Preview (0, false);
			}
			break;
		case UpgradeType.ATK:
			if (bShow) {
				iUpgrades [2].Preview ((float)level / 20F, true);
			} else {
				iUpgrades [2].Preview (0, false);
			}
			break;
		}

	}
}
