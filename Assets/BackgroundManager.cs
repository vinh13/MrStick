using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BackgroundManager : MonoBehaviour
{
	public static BackgroundManager Instance = null;
	AutoMove[] autoMove;
	[SerializeField]Camera camBg = null;
	[SerializeField]float rangeMoveY = 0;
	[SerializeField]Transform[] tPos;
	float ratioSize = 0;
	float originalPosY = 0;
	float originalSize = 0;
	float ratioZoom = 0;
	[SerializeField]bool bTest = false;
	[SerializeField]int indexMap = 0;

	void Awake ()
	{
		if (Instance == null)
			Instance = this;
		CreateBackground ();
		Setup ();
	}

	void CreateBackground ()
	{
		int indexLevel = LevelData.mapID.GetHashCode ();
		if (indexLevel == 0)
			indexLevel = 1;
		if (bTest)
			indexLevel = indexMap;
		CreateBG (indexLevel);
	}

	void CreateBG (int index)
	{
		GameObject ef = Instantiate (Resources.Load<GameObject> ("Maps/Background/Effect" + index), tPos [0]);
		GameObject bg = Instantiate (Resources.Load<GameObject> ("Maps/Background/BG" + index), tPos [1]);
		GameObject theme = Instantiate (Resources.Load<GameObject> ("Maps/Background/Theme" + index), transform);
		GameObject water = Instantiate (Resources.Load<GameObject> ("Maps/Background/water" + index), tPos [2]);
		autoMove = theme.GetComponentsInChildren<AutoMove> ();
	}

	public void Setup ()
	{
		originalPosY = camBg.transform.position.y;
		originalSize = camBg.orthographicSize;
		float aspect = camBg.aspect;
		float size = camBg.orthographicSize;
		float xW = size * aspect;
		for (int i = 0; i < autoMove.Length; i++) {
			autoMove [i].TakeLimit (xW);
		}
	}

	public void SetRatioY (float sizeMain)
	{
		ratioSize = camBg.orthographicSize / sizeMain;
		ratioZoom = ratioSize * 0.25F;
	}

	public void SyncY (float rr)
	{
		float y = rr * ratioSize;
		Vector3 pos = camBg.transform.position;
		pos.y = originalPosY + y;
		camBg.transform.position = pos;
	}

	public void SyncSize (float size)
	{
		float s = size * ratioZoom;
		camBg.orthographicSize = originalSize + s;
	}

	public void Set (float ratio)
	{
		for (int i = 0; i < autoMove.Length; i++) {
			autoMove [i].SetRatio (ratio);
		}
	}
}
