using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAnimation : MonoBehaviour
{
	[SerializeField]Animator anim = null;
	[SerializeField]Transform iks = null;
	[SerializeField]float rangeMove = 6F, rangeGround = 3F;
	[SerializeField]float fSpeed = 0;
	LayerMask maskGround;
	Vector2 posGround = Vector2.zero;

	void Start ()
	{
		maskGround = LayerMask.NameToLayer ("Ground");
		anim.updateMode = AnimatorUpdateMode.UnscaledTime;
	}

	public bool OnWin ()
	{
		return CheckGround ();
	}

	void PlayAim ()
	{
		iks.gameObject.SetActive (true);
		transform.SetParent (null);
		GetComponent<RagdollManager> ().ToAnim ();
		anim.enabled = true;
		anim.Rebind ();
		anim.SetTrigger ("win");
	}

	bool CheckGround ()
	{
		RaycastHit2D hit = Physics2D.Raycast (this.transform.position, Vector2.down, 100F, 1 << maskGround);
		if (hit.collider != null) {
			if (hit.collider.attachedRigidbody == null) {
				PlayAim ();
				Vector2 p = hit.point;
				p.x -= 2F;
				Vector2 target = p;
				target.y += rangeMove;
				posGround = p;
				posGround.y += rangeGround;
				StartCoroutine (MoveSimulation (target));
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}

	IEnumerator MoveSimulation (Vector2 target)
	{	
		bool done = false;
		while (!done) {
			Vector2 pos = transform.position;
			pos = Vector2.MoveTowards (pos, target, fSpeed * Time.unscaledDeltaTime);
			transform.position = pos;
			if (pos == target) {
				done = true;
			}
			yield return null;
		}
		Debug.Log ("Move simutation Done!!!");
	}

	public void JumpUp ()
	{
		StopAllCoroutines ();
		anim.SetTrigger ("win1");
		StartCoroutine (MoveSimulation (posGround));
	}

	public void PlayWin ()
	{
		UIManager.Instance.Win (true);
	}
}
