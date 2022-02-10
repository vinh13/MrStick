using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class HoldStatus
{
	public float durationRevive = 0;
	public float durationRevive_Nitro = 0;
	public float durationHold = 0;
	public float durationNitro = 0;
	[Range (0, 1F)]public float ratio = 0;
}

public class ButtonHold : MonoBehaviour,IPointerUpHandler,IPointerEnterHandler,IPointerDownHandler,IPointerExitHandler
{
	[SerializeField]Image imgClick = null, imgFill = null, imgFillEffect = null;
	[SerializeField]Color32[] colors;
	bool bSelected = false;
	HoldStatus status = null;
	float ratioHold = 0;
	bool bBlock = false;
	float holdRatioPerMSeconds = 0, holdRatioPerMSeconds_Nitro = 0;
	float reviveRatioPerSeconds = 0, reviveRatioPerSeconds_Nitro = 0;
	bool activeNitro = false;
	bool bCanNitro = false;
	float timerSelect = 0;
	bool bStartNitro = false;
	bool bWaitDisable = false;

	#region Setting

	void Start ()
	{
		activeNitro = false;
		PlayerControl.Instance.RegisterUpPower (UpPower);
		status = PlayerControl.Instance.GetHold;
		ratioHold = status.ratio;
		holdRatioPerMSeconds = 0.1F / status.durationHold;
		holdRatioPerMSeconds_Nitro = 0.1F / status.durationNitro;
		reviveRatioPerSeconds = 1 / (status.durationRevive * 2F);
		reviveRatioPerSeconds_Nitro = 1 / (status.durationRevive_Nitro * 2F);
		Check ();
		Fill (ratioHold);
		if (ratioHold < 1F)
			Revive ();

	}

	void Check ()
	{
		bBlock = ratioHold < holdRatioPerMSeconds;
		Block (bBlock);
	}

	void Block (bool b)
	{
		imgClick.raycastTarget = !b;
	}

	void Fill (float ratio)
	{
		imgFill.fillAmount = ratio;
		int iC = ratio < 0.5F ? 0 : 1;
		imgFill.color = colors [iC];
		if (!bCanNitro)
			bCanNitro = (iC == 1);
		PlayerControl.Instance.ratioPower = ratio;
		PlayerControl.Instance.PowerUpdate (ratio, colors [iC]);
	}

	#endregion

	#region Pointer

	public void OnPointerEnter (PointerEventData eventData)
	{
		Active ();	
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		Active ();	
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		if (!bSelected)
			return;
		if (!bStartNitro) {
			StopAllCoroutines ();
			Disable ();
		} else {
			if (!bWaitDisable) {
				DisableWait ();
				bWaitDisable = true;
			} else {
				StopAllCoroutines ();
				Disable ();
			}
		}
	}

	public void OnPointerUp (PointerEventData eventData)
	{
		if (!bSelected)
			return;
		if (!bStartNitro) {
			StopAllCoroutines ();
			Disable ();
		} else {
			if (!bWaitDisable) {
				DisableWait ();
				bWaitDisable = true;
			} else {
				StopAllCoroutines ();
				Disable ();
			}
		}
	}

	IEnumerator DisableWait ()
	{
		bool done = false;
		while (!done) {
			if (ratioHold < 0.3F) {
				done = true;
			} else {
				ratioHold -= 0.1F;
			}
			yield return null;
		}
		StopAllCoroutines ();
		Disable ();
	}

	#endregion

	#region Revive

	void Active ()
	{
		if (bBlock)
			return;
		if (bSelected)
			return;
		if (ratioHold <= 0.1F)
			return;
		timerSelect = 0;
		bSelected = true;
		bStartNitro = true;
		bWaitDisable = false;
		imgFillEffect.enabled = false;
		PlayerControl.Instance.UpSpeed ();
		if (bCanNitro) {
			PlayerControl.Instance.ActiveNitro (true);
		}
		StopAllCoroutines ();
		StartCoroutine (_OnHold ());
	}

	void Disable ()
	{
		bSelected = false;
		PlayerControl.Instance.DownSpeed ();
		if (bCanNitro) {
			PlayerControl.Instance.ActiveNitro (false);
			bCanNitro = false;
		}
		StartCoroutine (_Disable ());
	}

	IEnumerator _Disable ()
	{
		yield return new WaitForSeconds (1F);
		Revive ();
	}

	IEnumerator _OnHold ()
	{
		yield return new WaitForSeconds (0.1F);
		timerSelect += 0.1F;
		if (timerSelect >= 0.2F) {
			if (bStartNitro)
				bStartNitro = false;
		}
		if (bCanNitro) {
			ratioHold -= holdRatioPerMSeconds_Nitro;
		} else {
			ratioHold -= holdRatioPerMSeconds;
		}
		ratioHold = Mathf.Clamp (ratioHold, 0, 1F);
		if (bSelected) {
			if (ratioHold > 0) {
				Fill (ratioHold);
				StartCoroutine (_OnHold ());
			} else {
				Fill (0);
				Disable ();
				bWaitDisable = true;
			}
		}
	}

	void Revive ()
	{
		StartCoroutine (_Revive ());
	}

	IEnumerator _Revive ()
	{
		float timer = 0;
		bool done = false;
		while (!done) {
			timer += Time.deltaTime;
			if (timer >= 0.5F) {
				timer = 0;
				if (ratioHold <= 0.4F) {
					ratioHold += reviveRatioPerSeconds;
				} else {
					ratioHold += reviveRatioPerSeconds_Nitro;
				}
				ratioHold = Mathf.Clamp (ratioHold, 0, 1);
				Fill (ratioHold);
				Check ();
				if (ratioHold == 1) {
					done = true;
				}
			}
			yield return null;
		}
	}

	#endregion

	#region UpPower

	void UpPower (float ratio)
	{
		if (ratioHold == 1)
			return;
	
		ratioHold += ratio;
		ratioHold = Mathf.Clamp (ratioHold, 0, 1F);
		imgFillEffect.fillAmount = ratioHold;
		imgFillEffect.enabled = true;
		Check ();
		Fill (ratioHold);
		TaskUtil.Schedule (this, EffectUpPower, 0.1F);
	}

	void EffectUpPower ()
	{
		imgFillEffect.enabled = false;
	}

	#endregion
}
