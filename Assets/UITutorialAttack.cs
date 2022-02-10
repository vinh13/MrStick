using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UITutorialAttack : MonoBehaviour
{
	[SerializeField]UIButton btnSkip = null;
	void Start ()
	{
		btnSkip.Register (Click);
	}

	void Click ()
	{
		TutorialAttack.Instance.HideTutorial (TutorialID.Attack);
	}
}
