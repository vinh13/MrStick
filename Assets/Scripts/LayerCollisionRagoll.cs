public enum TypeHit
{
	None = 0,
	Weapon = 1,
	Object = 2,
	Trapp = 3,
}
public enum HitObject
{
	None = 0,
	Bullet = 1,
}
public class LayerCollisionRagoll
{
	public static int[] layerColl_P = { 16, 11 };
	public static int[] layerColl_E = { 17, 11 };
	public static int layerGround = 10;

	public static bool checkHit (int layerCheck, TypeCharacter _type)
	{
		int[] layerColl = _type == TypeCharacter.Player ? layerColl_P : layerColl_E;
		bool temp = false;
		for (int i = 0; i < layerColl.Length; i++) {
			if (layerColl [i] == layerCheck) {
				temp = true;
				break;
			}
		}
		return temp;
	}

	public static bool CheckGround (int layerCheck)
	{
		return layerCheck == layerGround;
	}
}
