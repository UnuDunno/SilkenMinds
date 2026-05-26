using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private List<EnemyData> enemiesDefeated;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        enemiesDefeated = new List<EnemyData>();
    }

    public void ClearData()
    {
        enemiesDefeated.Clear();
    }

    public void AddEnemie(EnemyData enemy)
    {
        enemiesDefeated.Add(enemy);
    }

    public List<EnemyData> GetDefeatedEnemies()
    {
        return enemiesDefeated;
    }
}
