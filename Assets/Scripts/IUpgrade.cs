using UnityEngine;

public abstract class IUpgrade : MonoBehaviour
{
	public abstract void UpdateData ();

	public abstract void Preview (float ratio, bool bShow);

	public abstract void UpdateBonus (int level);

	public abstract void SetColor (Color32 color);

	public abstract void ClearPreview();
}
