using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // 敌人Prefab
    public float spawnInterval = 3.0f; // 每次生成敌人的时间间隔
    public int maxEnemies = 10; // 最大生成数量
    private int currentEnemyCount = 0; // 当前生成的敌人数量

    private Coroutine spawnCoroutine; // 用于跟踪生成协程

    void Start()
    {
        // 直接启动生成敌人的协程
        spawnCoroutine = StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        // 重置当前敌人计数
        currentEnemyCount = 0;

        while (currentEnemyCount < maxEnemies)
        {
            // 生成敌人
            SpawnEnemy();
            currentEnemyCount++;

            // 等待指定时间间隔
            yield return new WaitForSeconds(spawnInterval);
        }

        // 协程完成后，重置协程引用
        spawnCoroutine = null;
    }

    void SpawnEnemy()
    {
        // 在出怪口的位置生成敌人
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
}
