using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContainer : MonoBehaviour
{
	List<AIInterface> listEnemies = new List<AIInterface> ();
	[SerializeField]string path = "";
	int countIndex = 0;
	int _numberE = 0;

	public void CreateEnemies (int numberEnemy, EnemyData[] enemiesData)
	{
		_numberE = numberEnemy;
		for (int i = 0; i < numberEnemy; i++) {
			listEnemies.Add (Instantiate (Resources.Load<GameObject> (path)).GetComponent<AIInterface> ());
			listEnemies [i].Init (enemiesData [i]);
			listEnemies [i].Active (false, new Vector2 (-1000, 0));
			AIConnect ai = (AIConnect)listEnemies [i];
			ai.ID = i;
			RankConnect rank = ai.GetRankConnect ();
			rank.aiLevel = enemiesData [i].aiLevel;
			LeaderboardManager.Instance.RegisterLB (rank, i == numberEnemy - 1);
		}
	}

	public AIInterface GetAIInterface {
		get {
			AIInterface ai = listEnemies [countIndex];
			countIndex++;
			return ai;
		}
	}

	public bool CheckEnemy ()
	{
		return countIndex == _numberE;
	}
}
