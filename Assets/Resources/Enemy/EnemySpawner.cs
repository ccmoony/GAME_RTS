using System.Collections;
using UnityEngine;
using TMPro;
public class EnemySpawner : MonoBehaviour
{
    public bool startSpawn=false;
    public GameObject enemyPrefab_melle;

    public GameObject enemyPrefab_ranged; 
    public float spawnInterval = 1.0f; 
    public int maxEnemies = 2; 

    public float roundInterval = 5.0f;

    private int currentEnemyCount_melle = 0; 

    private int currentEnemyCount_ranged = 0;

    private Coroutine spawnCoroutine_melle;
    private Coroutine spawnCoroutine_ranged;
    public MainMenuController mainMenuController;
    public GameObject enemy; 
    void Start()
    {
        spawnCoroutine_melle = StartCoroutine(SpawnEnemies_melle());
        spawnCoroutine_ranged = StartCoroutine(SpawnEnemies_ranged());
        mainMenuController=FindObjectOfType<MainMenuController>();
    }

    void Update()
    {
        enemy=GameObject.Find("Enemy(Clone)");
        if (enemy==null){
            enemy=GameObject.Find("Enemy_ranged(Clone)");
            if (enemy==null){
                mainMenuController.Gamewin();
            }
        }

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
            yield return new WaitForSeconds(roundInterval);
            currentEnemyCount_melle = 0;

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

            yield return new WaitForSeconds(roundInterval);
            
            currentEnemyCount_ranged = 0;
            while (currentEnemyCount_ranged < maxEnemies)
            {
                SpawnEnemy_melle();
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
