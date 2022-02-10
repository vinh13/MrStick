public class ObjectData
{
	public const float time_Saw = 13;

	public static ObTypeAttack GetTypeAttack (ObjectType type)
	{
		if (type == ObjectType.GunSimple ||
		    type == ObjectType.Bow ||
		    type == ObjectType.BowII ||
		    type == ObjectType.BowIII ||
		    type == ObjectType.GunBlue ||
		    type == ObjectType.GunGreen ||
		    type == ObjectType.GunRed ||
		    type == ObjectType.GunYellow)
			return ObTypeAttack.HoldShoot;
		else if (type == ObjectType.GunRocket ||
		         type == ObjectType.GunBomb)
			return ObTypeAttack.ClickShoot;
		else
			return ObTypeAttack.None;
	}
}
