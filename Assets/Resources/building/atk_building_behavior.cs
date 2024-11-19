using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ATK_Building_Behavior : MonoBehaviour
{


    public ATK_building_Card card_info;
    private building_placement manager;

    public float health;
    public float maxHealth;
    public Image healthBarFill;
    private float healthBarWidth;

    public GameObject arrowPrefab;        
    private Transform arrowSpawnPoint;   
    public LayerMask enemyLayer;          
    private float lastAttackTime = -Mathf.Infinity;
    void Start()
    {
        manager = FindObjectOfType<building_placement>();
        arrowSpawnPoint = transform;
        health = card_info.maximum_HP;

        maxHealth = card_info.maximum_HP;
        healthBarWidth = healthBarFill.rectTransform.sizeDelta.x;

    }
    void Update()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, card_info.ATK_range, enemyLayer);
        SetHealth(health);
        if (enemiesInRange.Length > 0)
        {
            Transform closestEnemy = FindClosestEnemy(enemiesInRange);

            if (Time.time >= lastAttackTime + card_info.cycle)
            {
                Attack(closestEnemy);
                lastAttackTime = Time.time;
            }
        }
        else
        {
            Debug.Log("No enemies in range");
        }

        if (health <= 0){
            manager.Destroy_Building_from_List(gameObject.GetInstanceID());
            Destroy(gameObject);
        }
    }

    Transform FindClosestEnemy(Collider[] enemies)
    {
        Transform closestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                closestEnemy = enemy.transform;
            }
        }

        return closestEnemy;
    }

    void Attack(Transform enemy)
    {
        if (arrowPrefab == null)
        {
            Debug.LogError("Arrow prefab is not assigned");
        }
        if (arrowPrefab != null && arrowSpawnPoint != null)
        {

            Debug.Log("Attacking enemy");
            GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);
            Arrow arrowScript = arrow.GetComponent<Arrow>();
            if (arrowScript != null)
            {
                arrowScript.SetTarget(enemy); 
            }
        }
    }
    public void TakeDamage(int attackDamage)
    {
        health -= attackDamage;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, card_info.ATK_range);
    }
    public void SetHealth(float Health)
    {
        health = Mathf.Clamp(Health, 0, maxHealth);
        float healthPercentage = health / maxHealth; 
        // 通过比例设置血条
        // Debug.Log("Health Percentage: " + healthPercentage);
        healthBarFill.fillAmount = health / maxHealth;
        //设置rectTransform的宽度
        healthBarFill.rectTransform.sizeDelta = new Vector2(healthBarWidth*healthPercentage, 0.2f);
        // Debug.Log("Health: " + health);
    }
}