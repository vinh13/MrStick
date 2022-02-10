using UnityEngine;

public abstract class Rank : MonoBehaviour
{
	public abstract void SetRank (int rank, int lap, Color color);

	public abstract void Init (Sprite sp, int rank, int lap);

	public abstract void Active (bool b);

	public abstract void ToDeath ();
}
