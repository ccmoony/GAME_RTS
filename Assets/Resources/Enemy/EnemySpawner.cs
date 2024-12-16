using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool startSpawn=false;
    public GameObject enemyPrefab_melle;

    public GameObject enemyPrefab_ranged; 
    public float spawnInterval = 3.0f; 
    public int maxEnemies = 10; 
    private int currentEnemyCount_melle = 0; 

    private int currentEnemyCount_ranged = 0;

    private Coroutine spawnCoroutine_melle;
    private Coroutine spawnCoroutine_ranged;

    void Start()
    {
        spawnCoroutine_melle = StartCoroutine(SpawnEnemies_melle());
        spawnCoroutine_ranged = StartCoroutine(SpawnEnemies_ranged());
    }

    IEnumerator SpawnEnemies_melle()
    {
        currentEnemyCount_melle = 0;
        if (startSpawn)
        {
            while (currentEnemyCount_melle < maxEnemies)
            {
                SpawnEnemy_melle();
                currentEnemyCount_melle++;

                yield return new WaitForSeconds(spawnInterval);
            }

            spawnCoroutine_melle = null;
        }
    }

    IEnumerator SpawnEnemies_ranged()
    {
        currentEnemyCount_ranged = 0;
        if (startSpawn)
        {
            while (currentEnemyCount_ranged < maxEnemies)
            {
                SpawnEnemy_ranged();
                currentEnemyCount_ranged++;

                yield return new WaitForSeconds(spawnInterval);
            }

            spawnCoroutine_ranged = null;
        }
    }

    void SpawnEnemy_melle()
    {
        Instantiate(enemyPrefab_melle, transform.position-new Vector3(0f,0.2f,0f), Quaternion.identity);
    }

    void SpawnEnemy_ranged()
    {
        Instantiate(enemyPrefab_ranged, transform.position-new Vector3(0f,0.2f,0f), Quaternion.identity);
    }
}
