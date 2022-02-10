using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIGame : MonoBehaviour
{
	[SerializeField]Transform[] rects;
	public Transform GetRect (int index)
	{
		return rects [index];
	}
}
