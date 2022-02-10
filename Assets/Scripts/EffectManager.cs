using UnityEngine;
using System.Collections;

public class EffectManager : MonoBehaviour
{
	public static EffectManager Instance = null;
	[SerializeField]Transform posEffectWin = null;

	public void PlayEffectWin ()
	{
		GameObject go = Instantiate (Resources.Load<GameObject> ("Effect/EffectWin"), posEffectWin);
		go.transform.localPosition = Vector2.zero;
	}

	void Awake ()
	{
		if (Instance == null) {
			Instance = this;
		}
		textDame.CreatePool (3);
	}

	[SerializeField]PoolEffectControl BloodA = null;

	public void Play_BloodA (Vector3 pos, Vector2 dir, Color color)
	{
		dir.Normalize ();
		float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
		angle -= 90;
		EffectControl ef = BloodA.Current;
		ef.transform.position = pos;
		ef.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
		ef.SetColor (color);
		ef.Active (true);
		SFXManager.Instance.Play (TypeSFX.blood);
	}

	[SerializeField]ObjectPool textDame = null;
	[SerializeField]ObjectPool textHit = null;
	[SerializeField]ObjectPool textHead = null;

	public void TextHead (Vector3 pos)
	{
		textHead.Current.GetComponent<TextMeshScript> ().SexTextNormal (pos, "HEADSHOT", Color.green);
	}

	public void TextDame (Vector3 pos, float dame, bool b, bool critical)
	{
		if (dame != 0) {
			if (!critical) {
				textDame.Current.GetComponent<TextMeshScript> ().SetText (pos, "-" + dame, b, false);
			} else {
				textHit.Current.GetComponent<TextMeshScript> ().SetText (pos, "Critical -" + dame, b, true);
			}
		} else {
			textDame.Current.GetComponent<TextMeshScript> ().SetText (pos, "Miss", b, false);
		}
	}
	//
	//	[SerializeField]CreateObjectPooling HitGround = null;
	//
	//	public void Play_HitGround (Vector3 pos)
	//	{
	//		HitGround._ChooseEffect (pos);
	//	}
}

//	#region effect Break
//
//	public CreateObjectPooling BreakWheel;
//
//	public void Play_BreakWheel (Vector3 pos)
//	{
//		BreakWheel._ChooseEffect (pos);
//	}
//
//
//	#endregion


//	#region effect BulletDestroy
//
//	public CreateObjectPooling BulletDestroy;
//
//	public void Play_BulletDestroy (Vector3 pos)
//	{
//		BulletDestroy._ChooseEffect (pos);
//	}
//
//	#endregion


//	#region effect Bullet_Hit_WP
//
//	public CreateObjectPooling Bullet_Hit_WP;
//
//	public void Play_Bullet_Hit_WP (Vector3 pos)
//	{
//		Bullet_Hit_WP._ChooseEffect (pos);
//	}
//
//	#endregion

//	#region effect Vo dan
//
//	public CreateObjectPooling EffectVodan = null;
//
//	public void Play_Effect_Vodan (Vector3 pos)
//	{
//		EffectVodan._ChooseEffect (pos);
//	}
//
//	public CreateObjectPooling MuzzleFlash = null;
//
//	public void Play_MuzzleFlash (Vector3 pos)
//	{
//		MuzzleFlash._ChooseEffect (pos);
//	}
//
//	#endregion
//}
