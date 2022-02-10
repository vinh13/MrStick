using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : MonoBehaviour
{
	public Sprite[] sprs;

	public Sprite GetSprite (ObjectType type)
	{
		int index = 0;
		switch (type) {
		case ObjectType.Bow:
			index = 0;
			break;
		case ObjectType.GunBomb:
			index = 1;
			break;
		case ObjectType.GunRocket:
			index = 2;
			break;
		case ObjectType.GunSimple:
			index = 3;
			break;
		case ObjectType.Saw:
			index = 4;
			break;
		case ObjectType.SawI:
			index = 5;
			break;
		case ObjectType.BowII:
			index = 6;
			break;
		case ObjectType.BowIII:
			index = 7;
			break;
		case ObjectType.GunBlue:
			index = 8;
			break;
		case ObjectType.GunGreen:
			index = 9;
			break;
		case ObjectType.GunRed:
			index = 10;
			break;
		case ObjectType.GunYellow:
			index = 11;
			break;

		}
		return sprs [index];
	}
}
