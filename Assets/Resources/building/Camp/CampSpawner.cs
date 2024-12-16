using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class CampSpawner : MonoBehaviour
{
    public GameObject soldierPrefab; // 小兵预制体
    public float spawnInterval = 5.0f; // 生成小兵的间隔时间
    public int maxSoldiers = 10; // 最大小兵数量
    private int currentSoldierCount = 0; // 当前小兵数量

    private Coroutine spawnCoroutine; // 用于生成小兵的协程

    private Currency_Manager currency_Manager;
    public Camp_building_Card card_info;
    private building_placement manager;

    public float health;
    public float maxHealth;
    public Image healthBarFill;
    private float healthBarWidth;

    void Start()
    {
        // 只在兵营存活时开始生成小兵
        manager = FindObjectOfType<building_placement>();
        currency_Manager=GameObject.Find("Cardbox").GetComponent<Currency_Manager>();
        
        health = card_info.maximum_HP;

        maxHealth = card_info.maximum_HP;
        healthBarWidth = healthBarFill.rectTransform.sizeDelta.x;
        spawnCoroutine = StartCoroutine(SpawnSoldiers());
    }

    IEnumerator SpawnSoldiers()
    {
        while (currentSoldierCount < maxSoldiers)
        {
            SpawnSoldier();
            currentSoldierCount++;
            yield return new WaitForSeconds(spawnInterval); // 等待一定时间后再生成下一个小兵
        }
    }

    void SpawnSoldier()
    {
        // 在兵营位置生成小兵
        Instantiate(soldierPrefab, transform.position + new Vector3(0f, 0.5f, 0f), Quaternion.identity);
    }

    // 这个方法可以用来在兵营被摧毁时停止生成小兵
    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }
    public void TakeDamage(int attackDamage)
    {
        health -= attackDamage;
    }


    void Update(){
        
        SetHealth(health);
        if (health <= 0){
            manager.Destroy_Building_from_List(gameObject.GetInstanceID());
            StopSpawning();
            Destroy(gameObject);
        }
    }
    public void SetHealth(float Health)
    {
        health = Mathf.Clamp(Health, 0, maxHealth);
        float healthPercentage = health / maxHealth; 
        // 通过比例设置血条
        // Debug.Log("Health Percentage: " + healthPercentage);
        healthBarFill.fillAmount = health / maxHealth;
        //设置rectTransform的宽度
        healthBarFill.rectTransform.sizeDelta = new Vector2(healthBarWidth*healthPercentage, 0.3f);
    }
}
