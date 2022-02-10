using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GunScript : ObjectAttack
{
	Action<float> AT = null;
	public TypeCharacter typeCharacter = TypeCharacter.None;
	float damage = 0;

	public override void Attack ()
	{
		AT.Invoke (damage);
	}

	public override void RegisterAttack (Action<float> a = null)
	{
		AT = a;
	}

	public override void Setup (TypeCharacter typeChar, float dame = 0)
	{
		typeCharacter = typeChar;
		damage = dame;
	}
}
