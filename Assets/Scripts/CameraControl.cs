using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
	public static CameraControl Instance = null;
	[SerializeField]FollowCamera follow = null;
	[SerializeField]Camera _camera = null;
	[SerializeField]float zoomRange = 5F;
	[SerializeField]float originalSize = 20;
	[SerializeField]float speedZoom = 0.1F;
	float sizeTemp = 0;
	[SerializeField]bool bFollowX = false;
	[SerializeField]bool bFollowY = false;
	[SerializeField]Vector2 _offset = Vector2.zero;
	float[] xRange = new float[2];
	float[] xRangeOriginal = new float[2];
	float xW = 0;
	float aspect = 0;
	[Space][SerializeField]UIDistance uiDistance = null;
	[SerializeField]ScreenTdz screenTdz = null;
	float originalYpos = 0;
	bool bGetYPos = false;
	bool bBlockEnd = false;

	public float GetAspect {
		get { 
			return _camera.aspect;
		}
	}

	public float GetX {
		get { 
			return xW;
		}
	}

	public float GetSizeOut {
		get { 
			return originalSize + zoomRange;
		}
	}

	void Awake ()
	{
		if (Instance == null)
			Instance = this;
		bBlock = true;
		originalSize = _camera.orthographicSize;
	}

	void Start ()
	{
		BackgroundManager.Instance.SetRatioY (originalSize);
		bBlock = false;
	}

	public void TakeLimit (Vector2 s, Vector2 e)
	{
		xRangeOriginal [0] = s.x;
		xRangeOriginal [1] = e.x;
		float size = _camera.orthographicSize;
		aspect = _camera.aspect;
		xW = size * aspect;
		xRange [0] = xRangeOriginal [0] + xW;
		xRange [1] = xRangeOriginal [1] - xW;
		screenTdz.SetLimit (aspect);
	}

	void SyncLimit (float size)
	{
		xW = size * aspect;
		xRange [0] = xRangeOriginal [0] + xW;
		xRange [1] = xRangeOriginal [1] - xW;
	}

	[SerializeField]Transform target = null;
	[SerializeField]Transform _cam = null;
	[SerializeField]float smoothness = 0;
	public bool bBlock = false;

	public void SyncCameraBG (float rrangeY)
	{
		BackgroundManager.Instance.SyncY (rrangeY);
	}

	void LateUpdate ()
	{
		if (bBlock)
			return;
		Vector3 pos = target.position;
		if (!bFollowX) {
			pos.x = _cam.position.x;
		}
		if (!bFollowY) {
			pos.y = _cam.position.y;
		}
		pos.x += _offset.x;
		pos.y += _offset.y;
		pos.z = -10;
		if (!bGetYPos) {
			originalYpos = pos.y;
			bGetYPos = true;
		}
		pos.x = Mathf.Clamp (pos.x, xRange [0], xRange [1]);
		_cam.position = Vector3.Slerp (_cam.position, pos, smoothness * Time.deltaTime);

		float ratio = MapManager.Intance.GetRatioDistance (_cam.position.x);
		uiDistance.Change (ratio);
		float rr = pos.y - originalYpos;
		SyncCameraBG (rr);
		BackgroundManager.Instance.Set (ratio);
		//Follow
		follow.Follow (pos);
	}

	public void ShowPlayer ()
	{

	}

	float timer = 0;

	public void ShowTarget (Transform _target, float time, Transform newP)
	{
		target = newP;
		bBlockEnd = true;
		timer = time;
		bBlock = true;
		float t = originalSize - 2F;
		sizeTemp = _camera.orthographicSize;
		BackgroundManager.Instance.SyncSize (sizeTemp);
		StopAllCoroutines ();
		StartCoroutine (ZoomIn (t, true));
		StartCoroutine (_ShowTarget (_target));
	}

	bool bSlow = false;

	IEnumerator _ShowTarget (Transform _target)
	{
		bool done = false;
		while (!done) {
			timer -= Time.unscaledDeltaTime;
			if (_target == null) {
				done = true;
			} else {
				Vector3 pos = _target.position;
				pos.x += _offset.x;
				pos.y += _offset.y;
				pos.z = -10;
				pos.x = Mathf.Clamp (pos.x, xRange [0], xRange [1]);
				_cam.position = Vector3.MoveTowards (_cam.position, pos, (smoothness) * Time.unscaledDeltaTime);


				float ratio = MapManager.Intance.GetRatioDistance (_cam.position.x);
				uiDistance.Change (ratio);
				float rr = pos.y - originalYpos;
				SyncCameraBG (rr);
				BackgroundManager.Instance.Set (ratio);
				//Follow
				follow.Follow (pos);
				if (Vector3.Distance (_cam.position, pos) < 0.2F) {
					if (!bSlow) {
						bSlow = true;
						SlowMotionManager.Instance.Slow (true);
						StartCoroutine (_SlowMotion ());
					}
				}
				if (timer <= 0) {
					done = true;
				}
			}
			yield return null;
		}
		bEnd = false;
		PlayerControl.Instance.EndRace (true);
		OnWin ();
	}

	void OnWin ()
	{
		if (target == null)
			return;
		Vector3 pos = target.position;
		pos.x += _offset.x;
		pos.y += _offset.y;
		pos.z = -10;
		pos.x = Mathf.Clamp (pos.x, xRange [0], xRange [1]);
		_cam.position = pos;
		float t = originalSize;
		sizeTemp = _camera.orthographicSize;
		BackgroundManager.Instance.SyncSize (sizeTemp);
		StartCoroutine (ZoomOut (t, true));
		StartCoroutine (_ShowPlayer ());
	}

	IEnumerator _SlowMotion ()
	{
		yield return new WaitForSecondsRealtime (1F);
		SlowMotionManager.Instance.Slow (false);
	}

	bool bEnd = false;

	IEnumerator _ShowPlayer ()
	{
		bool done = false;
		float timer = 0;
		while (!done) {
			if (target != null) {
				Vector3 pos = target.position;
				pos.x += _offset.x;
				pos.y += _offset.y;
				pos.z = -10;
				pos.x = Mathf.Clamp (pos.x, xRange [0], xRange [1]);
				_cam.position = Vector3.MoveTowards (_cam.position, pos, smoothness * Time.unscaledDeltaTime);
				float ratio = MapManager.Intance.GetRatioDistance (_cam.position.x);
				uiDistance.Change (ratio);
				float rr = pos.y - originalYpos;
				SyncCameraBG (rr);
				BackgroundManager.Instance.Set (ratio);
				//Follow
				follow.Follow (pos);
				if (!bEnd) {
					timer += Time.unscaledDeltaTime;
					if (timer > 1F) {
						bEnd = true;
					} else {
						if (Vector2.Distance (_cam.position, pos) < 0.2F) {
							bEnd = true;
						}
					}
				}
			}
			yield return null;
		}
	}
	void OnDisable ()
	{
		StopAllCoroutines ();
	}
	public void Jump (float ratio)
	{
		if (bBlockEnd)
			return;
		float t = originalSize + (zoomRange * ratio);
		sizeTemp = originalSize;
		StopAllCoroutines ();
		StartCoroutine (ZoomOut (t, true));
	}

	IEnumerator ZoomOut (float t, bool b)
	{
		bool done = false;
		while (!done) {
			if (sizeTemp < t) {
				sizeTemp += speedZoom;
			}
			sizeTemp = Mathf.Clamp (sizeTemp, originalSize, t);
			_camera.orthographicSize = sizeTemp;
			if (b)
				BackgroundManager.Instance.SyncSize (sizeTemp - originalSize);
			SyncLimit (sizeTemp);
			yield return null;
		}
	}

	public void EndJump ()
	{
		if (bBlockEnd)
			return;
		float t = originalSize;
		sizeTemp = _camera.orthographicSize;
		BackgroundManager.Instance.SyncSize (sizeTemp);
		StopAllCoroutines ();
		StartCoroutine (ZoomIn (t, true));
	}

	IEnumerator ZoomIn (float t, bool b)
	{
		bool done = false;
		while (!done) {
			if (sizeTemp > t) {
				sizeTemp -= speedZoom;
			}
			sizeTemp = Mathf.Clamp (sizeTemp, t, sizeTemp);
			_camera.orthographicSize = sizeTemp;
			if (b)
				BackgroundManager.Instance.SyncSize (sizeTemp - originalSize);
			SyncLimit (sizeTemp);
			yield return null;
		}
	}

	public bool CheckObjectInViewport (Vector3 pos)
	{
		Vector2 view = _camera.WorldToViewportPoint (pos);
		if (view.x > -.1F && view.y > 0 && view.x <= 1 && view.y <= 1)
			return true;
		else {
			return  false;
		}
	}
}
