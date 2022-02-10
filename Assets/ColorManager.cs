using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorStickman
{
	public Color[] colors = new Color[2];
}

public class ColorManager : MonoBehaviour
{
	int indexLayer = 0;
	int maxLayer = 9;
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
	private bool bChangeed = false;
	public bool bChangeJoint{
		get{
			bChangeed = !bChangeed;
			return bChangeed;
		}
	}

	public JointLimit GetJointDealth (int index)
	{
		switch (index) {
		case 0:
			return jointSpine;
		case 1:
			return jointHead;
		case 2:
			return jointSpineB;
		case 3:
			return jointLegL;
		case 4:
			return jointLegLB;
		case 5:
			return jointFootL;
		case 6:
			return jointLegR;
		case 7:
			return jointLegRB;
		case 8:
			return jointFootR;
		case 9:
			return jointArmL;
		case 10:
			return jointHandL;
		case 11:
			return jointArmR;
		case 12:
			return jointHandR;
		default :
			return jointSpine;
		}
	}

	public static ColorManager Instance {
		get {
			if (_instance == null) {
				_instance = GameObject.Find ("ColorManager").GetComponent<ColorManager> ();
			}
			return _instance;
		}
	}

	private static ColorManager _instance;
	[Space]
	public ColorStickman[] colors = new ColorStickman[4];
	public Color[] colorsHit = new Color[4];

	public int GetIndex {
		get { 
			indexLayer++;
			if (indexLayer == maxLayer) {
				indexLayer = 1;
			}
			return indexLayer;
		}
	}
}
