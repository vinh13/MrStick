using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface  HPBar
{
	void Change (float ratio = 0);

	void Disable ();

	float Ratio ();
}
