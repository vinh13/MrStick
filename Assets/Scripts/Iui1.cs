using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface Iui1
{
	void Show ();

	void Hide ();

	void Register (Action<object> a);
}
