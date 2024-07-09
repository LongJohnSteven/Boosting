using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private List<Enemy> enemyList = new List<Enemy>();

    // Start is called before the first frame update
    void Start()
    {
        Enemy[] enemyArray = this.GetComponents<Enemy>();
        foreach (Enemy enemy in enemyArray)
        {
            enemyList.Add(enemy);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
