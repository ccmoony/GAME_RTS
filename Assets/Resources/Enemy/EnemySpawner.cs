using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool startSpawn=false;
    public GameObject enemyPrefab; 
    public float spawnInterval = 3.0f; 
    public int maxEnemies = 10; 
    private int currentEnemyCount = 0; 

    private Coroutine spawnCoroutine; 

    void Start()
    {
        spawnCoroutine = StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        currentEnemyCount = 0;
        if (startSpawn)
        {
            while (currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
                currentEnemyCount++;

                yield return new WaitForSeconds(spawnInterval);
            }

            spawnCoroutine = null;
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position-new Vector3(0f,0.2f,0f), Quaternion.identity);
    }
}
