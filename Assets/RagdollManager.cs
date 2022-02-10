using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public enum PartType
{
	None = 0,
	Head = 1,
	Spine = 2,
	SpineB = 3,
	Vehicle = 4,
	Leg = 5,
	LegB = 6,
	Arm = 7,
}

[System.Serializable]
public class CollisionLog
{
	public PartType partType = PartType.None;
	public int collLayer = 0;
	public TypeHit typeHit = TypeHit.None;
	public HitObject hitObject = HitObject.None;
	public Vector3 posHit = Vector3.zero;
	public float damage = 10;
}

public class RagdollManager : MonoBehaviour
{
	[SerializeField]TypeCharacter typeChar = TypeCharacter.None;
	[SerializeField]PartBody[] partBodies;
	[SerializeField]Color[] colors = new Color[2];

	public Color[] getColor {
		get { 
			return colors;
		}
	}

	[SerializeField]string slayerName = "";
	[SerializeField]RagdollConnect ragdollConnect = null;
	[SerializeField]JointLimit jointSpine = new JointLimit ();
	[SerializeField]JointLimit jointHead = new JointLimit ();
	[SerializeField]JointLimit jointSpineB = new JointLimit ();
	[SerializeField]JointLimit jointLegL = new JointLimit ();
	[SerializeField]JointLimit jointLegLB = new JointLimit ();
	[SerializeField]JointLimit jointFootL = new JointLimit ();
	[SerializeField]JointLimit jointLegR = new JointLimit ();
	[SerializeField]JointLimit jointLegRB = new JointLimit ();
	[SerializeField]JointLimit jointFootR = new JointLimit ();
	[SerializeField]JointLimit jointArmL = new JointLimit ();
	[SerializeField]JointLimit jointHandL = new JointLimit ();
	[SerializeField]JointLimit jointArmR = new JointLimit ();
	[SerializeField]JointLimit jointHandR = new JointLimit ();
	[SerializeField]EffectControl[] effectsControl;
	bool bOnOffPhy = false;

	public void ActiveEffect (bool b)
	{
		for (int i = 0; i < effectsControl.Length; i++) {
			effectsControl [i].Active (b);
		}
	}

	Action vehicleEnd = null;
	Action<object> vehicleHit = null;
	HealthScript health = null;
	RagdollStatus status = null;
	bool isP = false;
	bool bStarted = false;

	#region Editor

	void OnValidate ()
	{
		//partBodies = GetComponentsInChildren<PartBody> ();
		//Spine
		SetPart (0, 0, 10, jointSpine);
		//Head
		SetPart (1, 0, 20, jointHead);
		//SpineB
		SetPart (2, 0, 0, jointSpineB);
		//Leg L
		SetPart (3, 1, -50, jointLegL);
		//Leg L 2
		SetPart (4, 1, -60, jointLegLB);
		//Foot L
		SetPart (5, 1, -70, jointFootL);
		//Leg R
		SetPart (6, 0, 50, jointLegR);
		//Leg R 2
		SetPart (7, 0, 60, jointLegRB);
		//Foot R
		SetPart (8, 0, 70, jointFootR);
		//Arm L
		SetPart (9, 1, -30, jointArmL);
		//Hand L
		SetPart (10, 1, -40, jointHandL);
		//Arm R
		SetPart (11, 0, 110, jointArmR);
		//Hand R
		SetPart (12, 0, 100, jointHandR);
	}

	public string Init ()
	{
		indexSkin = ColorManager.Instance.GetIndex;
		string sName = "E" + (indexSkin);
		for (int i = 0; i < partBodies.Length; i++) {
			partBodies [i].Setup (sName);
			partBodies [i].RefreshSkin ();
		}
		slayerName = sName;
		this.GetComponent<SkinManager> ().ChangeLayer (sName);
		return sName;
	}

	public void CreateSkin (int idSkin)
	{
		this.GetComponent<SkinManager> ().CreateSkin (idSkin);
	}

	#endregion

	public void OnOffPhy (bool b)
	{
		if (b) {
			if (!bOnOffPhy) {
				bOnOffPhy = true;
				for (int i = 0; i < partBodies.Length; i++) {
					partBodies [i].OnOffPhy (true);
				}
			}
		} else {
			if (bOnOffPhy) {
				bOnOffPhy = false;
				for (int i = 0; i < partBodies.Length; i++) {
					partBodies [i].OnOffPhy (false);
				}
			}
		}

	}

	public void RegisterEnd (Action a, TypeCharacter _typeChar, Action<object> hit, EnemyType enemyType, Transform pHp)
	{
		vehicleHit = hit;
		vehicleEnd = a;
		typeChar = _typeChar;
		health = transform.GetComponentInParent<HealthScript> ();
		if (typeChar == TypeCharacter.Enemy) {
			if (enemyType == EnemyType.None) {
				GameObject go = Instantiate (Resources.Load<GameObject> ("HPBar"), pHp);
				health.Init (go.GetComponent<HPBar> ());
			}
			isP = false;
		} else {
			isP = true;
			health.Init (PlayerControl.Instance.ChangeHP);
			health.RegisterChange (PlayerControl.Instance.ChangeHealth);
		}
		this._Start ();
	}

	public int indexSkin = 0;
	public int indexColor = 0;
	bool bPlayer = false;

	public void PlayerSetSkin ()
	{
		
	}

	public void SetLayerNameAndColor (Color[] cls, int idColor)
	{
		indexColor = idColor;
		colors = cls;
		int index = 0;
		for (int i = 0; i < partBodies.Length; i++) {
			if (i == 3 || i == 4 || i == 5 || i == 9 || i == 10) {
				index = 1;
			} else {
				index = 0;
			}
			partBodies [i].Setup (cls [index]);
		}
		for (int i = 0; i < effectsControl.Length; i++) {
			effectsControl [i].SetColor (colors [i]);
		}
	}

	public Transform getPosSlow ()
	{
		return partBodies [1].transform;
	}

	public void _Start ()
	{
		if (bStarted)
			return;
		
		bStarted = true;
		status = gameObject.AddComponent<RagdollStatus> ();

		bOnOffPhy = true;

		partBodies [2].AddFixedJoint2D (1);
		partBodies [1].AddFixedJoint2D (3);
		partBodies [4].AddFixedJoint2D (3);
		partBodies [7].AddFixedJoint2D (3);
		partBodies [9].AddFixedJoint2D (2);
		//partBodies [11].AddFixedJoint2D (2);
		//partBodies [2].SetMass (5F);

//		vehicleCollision.Setup (PartType.Vehicle, Collision, typeChar);

		Color colorHit;

		if (typeChar == TypeCharacter.Enemy) {
			colorHit = ColorManager.Instance.colorsHit [indexColor];
		} else {
			colorHit = colors [0];
		}

		partBodies [0].AddCheckCollision (PartType.Spine, Collision, typeChar, status, false, colorHit);
		//Head
		partBodies [1].AddCheckCollision (PartType.Head, Collision, typeChar, status, true, colorHit);

		partBodies [2].AddCheckCollision (PartType.SpineB, Collision, typeChar, status, false, colorHit);

		partBodies [3].AddCheckCollision (PartType.Leg, Collision, typeChar, status, true, colorHit);
		partBodies [6].AddCheckCollision (PartType.Leg, Collision, typeChar, status, false, colorHit);


		partBodies [4].AddCheckCollision (PartType.LegB, Collision, typeChar, status, false, colorHit);
		partBodies [7].AddCheckCollision (PartType.LegB, Collision, typeChar, status, false, colorHit);


		partBodies [9].AddCheckCollision (PartType.Arm, Collision, typeChar, status, false, colorHit);
		partBodies [11].AddCheckCollision (PartType.Arm, Collision, typeChar, status, true, colorHit);


		if (isP) {
			health.SetHealth (PlayerControl.Instance.GetHealth, null);
		}

	}

	void  SetPart (int index, int i, int id, JointLimit limit)
	{
		partBodies [index].Setup (index, colors [i], slayerName, id);
		partBodies [index].Setup (limit);
	}

	public void StopAll ()
	{
		for (int i = 0; i < partBodies.Length; i++) {
			partBodies [i].Stop ();
		}
	}


	public void ToAnim ()
	{
		ragdollConnect.Death ();
		for (int i = 0; i < partBodies.Length; i++) {
			partBodies [i].ToAnim ();
		}
	}

	public void Reset ()
	{
		for (int i = 0; i < partBodies.Length; i++) {
			partBodies [i].Reset ();
		}
	}

	void Collision (CollisionLog collLog)
	{
		if (status.bDeath) {
			return;
		}
		if (isP && Logic.bShield)
			return;
		float damage = 0;
		if (collLog.typeHit == TypeHit.Trapp) {
			float maxHP = health.GetMaxHP;
			damage = (collLog.damage / 100F) * maxHP;
			damage = Mathf.Clamp (damage, TrappConfig.dameSaw, Mathf.Infinity);
		} else {
			damage = collLog.damage;
		}
		status.bDeath = health.ChangeHealth (damage);
		EffectManager.Instance.TextDame (transform.position, (float)((int)damage), isP, false);
		if (!isP) {
			if (collLog.hitObject == HitObject.Bullet) {
				if (collLog.partType == PartType.Head) {
					EffectManager.Instance.TextHead (transform.position);
				}
			}
		}
		if (status.bDeath) {
			OnDeath ();
			if (!isP) {
				EnemyManager.Instance.AddDamage (damage);
			}
		} else {
			HitDir hitdir = HitDir.None;
			if (collLog.typeHit == TypeHit.Object) {
				hitdir = collLog.posHit.y > transform.position.y ? HitDir.Top : HitDir.Bottom;
			}
			vehicleHit.Invoke (hitdir);
			if (!isP) {
				if (bOnOffPhy)
					SFXManager.Instance.Play (TypeSFX.hurt);
				EnemyManager.Instance.AddDamage (damage);
			}
		}
	}

	void OnDeath ()
	{
		if (bOnOffPhy)
			SFXManager.Instance.Play (TypeSFX.dead);
		transform.SetParent (null);
		ragdollConnect.Death ();
		vehicleEnd.Invoke ();
		StartCoroutine (OD ());
	}

	IEnumerator OD ()
	{
		yield return new WaitForEndOfFrame ();
		int l = isP ? 12 : 15;
		partBodies [0].ToDeath (l);
		partBodies [1].ToDeath (l);
		partBodies [2].ToDeath (l);
		if (!isP) {
			for (int i = 3; i < partBodies.Length; i++) {
				partBodies [i].ToDeath (18);
			}
			TaskUtil.Schedule (this, _OnDeath, 2F);
		} else {
			TaskUtil.Schedule (this, _PlayerDeath, 1F);
		}
	}

	void _OnDeath ()
	{
		for (int i = 0; i < 3; i++) {
			if (partBodies [i] != null)
				partBodies [i].ChangeLayer (18);
		}
		Destroy (gameObject);
	}

	void _PlayerDeath ()
	{
		AIEnemyManager.Instance.GetAllEnemy ();
	}
}
