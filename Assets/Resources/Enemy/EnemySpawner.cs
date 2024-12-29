using System.Collections;
using UnityEngine;
using TMPro;
public class EnemySpawner : MonoBehaviour
{
    public bool startSpawn=false;
    public GameObject enemyPrefab_melle;

    public GameObject enemyPrefab_ranged; 

    public GameObject enemyPrefab_boss;
    public float spawnInterval = 1.0f; 
    public int maxEnemies = 5; 

    public float roundInterval = 30.0f;

    private int currentEnemyCount_melle = 0; 

    private int currentEnemyCount_ranged = 0;

    private Coroutine spawnCoroutine_melle;
    private Coroutine spawnCoroutine_ranged;
    public MainMenuController mainMenuController;
    public GameObject enemy; 

    private bool is_enemy=false;
    void Start()
    {
        spawnCoroutine_melle = StartCoroutine(SpawnEnemies_melle());
        spawnCoroutine_ranged = StartCoroutine(SpawnEnemies_ranged());
        mainMenuController=FindObjectOfType<MainMenuController>();
    }

    void Update()
    {
        enemy=GameObject.Find("Enemy(Clone)");
        if (is_enemy){
            if (enemy==null){
                enemy=GameObject.Find("Enemy_ranged(Clone)");
                if (enemy==null){
                    enemy=GameObject.Find("Boss(Clone)");
                    if (enemy==null){
                        mainMenuController.Gamewin();
                    }
                }
            }
        }
    }


    IEnumerator SpawnEnemies_melle()
    {
        currentEnemyCount_melle = 0;
        if (startSpawn)
        {   
            yield return new WaitForSeconds(roundInterval);
            while (currentEnemyCount_melle < maxEnemies)
            {
                SpawnEnemy_melle();
                currentEnemyCount_melle++;
                yield return new WaitForSeconds(spawnInterval);
            }
            yield return new WaitForSeconds(roundInterval-maxEnemies*spawnInterval);
            currentEnemyCount_melle = 0;

            while (currentEnemyCount_melle < maxEnemies)
            {
                SpawnEnemy_melle();
                currentEnemyCount_melle++;

                yield return new WaitForSeconds(spawnInterval);
            }            

            yield return new WaitForSeconds(roundInterval-maxEnemies*spawnInterval);
            SpawnEnemy_boss();

            is_enemy=true;
            spawnCoroutine_melle = null;

        }
    }

    IEnumerator SpawnEnemies_ranged()
    {
        currentEnemyCount_ranged = 0;
        if (startSpawn)
        {
            yield return new WaitForSeconds(roundInterval);
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
                SpawnEnemy_ranged();
                currentEnemyCount_ranged++;

                yield return new WaitForSeconds(spawnInterval);
            }      
            spawnCoroutine_ranged = null;
        }
    }

    void SpawnEnemy_melle()
    {
        Instantiate(enemyPrefab_melle, transform.position-new Vector3(0f,0.2f,0f), enemyPrefab_melle.transform.rotation);
    }

    void SpawnEnemy_ranged()
    {
        Instantiate(enemyPrefab_ranged, transform.position-new Vector3(0f,0.2f,0f), enemyPrefab_ranged.transform.rotation);
    }
    void SpawnEnemy_boss()
    {
        Instantiate(enemyPrefab_boss, transform.position-new Vector3(0f,0.2f,0f), enemyPrefab_boss.transform.rotation);
    }
}