using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RagdollConnect : MonoBehaviour
{
	[SerializeField]TypeJoint[] hjConnects;
	public void Death ()
	{
		for (int i = 0; i < hjConnects.Length; i++) {
			hjConnects [i].Active (false);
		}
	}
}
