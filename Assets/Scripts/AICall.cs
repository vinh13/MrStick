public interface AICall
{
	void UpSpeed (float time = 0);

	void StartVehicle ();

	void Init ();

	void Register (VehicleControl vc,EnemyData data);

	void StartRace ();
}
