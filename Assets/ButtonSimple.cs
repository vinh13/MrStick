using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSimple : MonoBehaviour,IPointerDownHandler
{
	[SerializeField]TypeControl typeControl = TypeControl.None;
	[SerializeField]Image imgClick = null;
	[SerializeField]Image imgFill = null;
	float duration = 0;
	float time = 0;

	public void OnPointerDown (PointerEventData eventData)
	{
		switch (typeControl) {
		case TypeControl.Slide:
			duration = PlayerControl.Instance.Slide (UnLock);
			duration += 0.5F;
			time = duration;
			Block (true);
			StartCoroutine (waitUnlock ());
			break;
		}	
	}

	void UnLock ()
	{
		
	}

	void Block (bool b)
	{
		imgClick.raycastTarget = !b;
	}

	IEnumerator waitUnlock ()
	{
		bool done = false;
		while (!done) {
			duration -= Time.deltaTime;
			float ratio = duration / time;
			imgFill.fillAmount = ratio;
			if (duration < 0)
				done = true;
			yield return null;
		}
		Block (false);
	}
}
