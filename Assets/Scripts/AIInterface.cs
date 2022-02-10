using UnityEngine;

public interface AIInterface
{
	void Active (bool b, Vector2 pos);

	void StartVehicle ();

	void Init (EnemyData data = null);

	void StartRace ();

	void RegisterIndexDistance (int index);
}
