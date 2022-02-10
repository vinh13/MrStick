using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TungDz;

public class ButtonEquipObject : MonoBehaviour
{
	[SerializeField]UIButton btn = null, btnSave = null;
	[SerializeField]Transform rectActive = null;
	[SerializeField]Image imgPreview = null;
	[SerializeField]Scaler scaler = null;
	bool bAvailable = false;
	ObjectType currentObject = ObjectType.None;
	Action<object> _StartRace = null;
	float timer = 0;
	[SerializeField]Image imgAva = null, imgFill = null;
	bool bSave = false;

	void Start ()
	{
		string ob = CharacterData.ObjectStart;
		if (ob != "") {
			ObjectType type = ObjectType.None;
			foreach (var e in Enum.GetValues(typeof(ObjectType))) {
				if (e.ToString () == ob) {
					type = (ObjectType)e;
					break;
				}
			}
			currentObject = type;
			CharacterData.ObjectStart = ObjectType.None.ToString ();
		}
		if (currentObject == ObjectType.None) {
			PlayerControl.Instance.ActiveTutorialWeapon (false);
			bAvailable = false;
		} else {
			imgPreview.sprite = UIObjectManager.Instance.sprObject (currentObject);
			bAvailable = true;
			if (TutorialData.bUseWeapon && !TutorialData.bTutorialStart) {
				PlayerControl.Instance.ActiveTutorialWeapon (true);
				_StartRace = (param) => StartRace ();
				EventDispatcher.Instance.RegisterListener (EventID.StartRace, _StartRace);
			} else {
				PlayerControl.Instance.ActiveTutorialWeapon (false);
			}
		}
		Active (bAvailable);
		btn.Register (ClickEquip);
		btnSave.Register (ClickSave);
		ActiveSave (false);
	}

	void Active (bool b)
	{
		rectActive.gameObject.SetActive (b);
		btn.Block (!b);
	}

	void ClickEquip ()
	{
		PlayerControl.Instance.EquipObject (currentObject, true, ObjectData.time_Saw + 5F);
		bAvailable = false;
		Active (bAvailable);
	}

	void StartRace ()
	{
		PlayTutorial.Instance.ShowTutorial (TutorialID.UseWeapon);
		EventDispatcher.Instance.RemoveListener (EventID.StartRace, _StartRace);
	}

	void OnDestroy ()
	{
		EventDispatcher.Instance.RemoveListener (EventID.StartRace, _StartRace);
	}

	public void ShowObject (ObjectType _type, float time)
	{
		if (bAvailable)
			return;
		currentObject = _type;
		StopAllCoroutines ();
		ActiveSave (true);
		PlayEffect ();
		durationWait = time;
		timer = time;
		imgFill.fillAmount = 1F;
		imgAva.sprite = UIObjectManager.Instance.sprObject (_type);
		StartCoroutine (_ShowObject ());
	}

	float durationWait = 0;

	public void ActiveSave ()
	{
		if (bSave) {
			if (!bClickSave) {
				AutoSave ();
			}
		}
		bClickSave = false;
	}
	bool bClickSave = false;
	void ClickSave ()
	{
		bClickSave = true;
		StopAllCoroutines ();
		PlayerControl.Instance.EquipObject (currentObject, false, timer);
		ActiveSave (false);
	}

	void AutoSave ()
	{
		StopAllCoroutines ();
		PlayerControl.Instance.EquipObject (currentObject, false, timer);
		ActiveSave (false);
	}

	void ActiveSave (bool b)
	{
		bSave = b;
		btnSave.gameObject.SetActive (b);
	}

	IEnumerator _ShowObject ()
	{
		yield return new WaitForSeconds (1F);
		timer -= 1F;
		timer = Mathf.Clamp (timer, 0, durationWait);
		float ratio = timer / durationWait;
		imgFill.fillAmount = ratio;
		if (timer == durationWait) {
			ActiveSave (false);
		} else {
			StartCoroutine (_ShowObject ());
		}
	}

	void PlayEffect ()
	{
		scaler.StopAllCoroutines ();
		scaler.gameObject.SetActive (true);
		scaler.transform.localScale = new Vector3 (1.5F, 1.5F, 1.5F);
		scaler.Scale (1F, 0.5F, Done);
	}

	void Done ()
	{
		scaler.gameObject.SetActive (false);
		scaler.transform.localScale = new Vector3 (1.5F, 1.5F, 1.5F);
	}
}
