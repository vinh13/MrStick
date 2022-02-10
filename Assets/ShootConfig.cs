using UnityEngine;

public class ShootConfig
{
	public static Vector2 Throw (Vector2 throwPoint, Vector2 target, float timeTillHit)
	{
		float xdistance = target.x - throwPoint.x;
		float ydistance = target.y - throwPoint.y;
		float throwAngle = Mathf.Atan ((ydistance + 4.905f * (timeTillHit * timeTillHit)) / xdistance);
		float totalVelo = xdistance / (Mathf.Cos (throwAngle) * timeTillHit);
		float xVelo = totalVelo * Mathf.Cos (throwAngle);
		float yVelo = totalVelo * Mathf.Sin (throwAngle);
		return new Vector2 (xVelo, yVelo);
	}
}
