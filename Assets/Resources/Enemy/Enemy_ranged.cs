using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_ranged : MonoBehaviour
{

    public float health; 
    public float maxHealth=3f;
    public int attackDamage = 1; 
    public float attackInterval = 1f; 

    public float attackRange = 10f;
    public Transform target; 
    private GameObject targetBuilding;

    public GameObject arrowPrefab;  
    private Transform arrowSpawnPoint; 
    private UnityEngine.AI.NavMeshAgent agent; 
    private building_placement manager;
    public Image healthBarFill;  
    private float healthBarWidth; 
    private float attackCooldown; 
    void Start()
    {
        manager = FindObjectOfType<building_placement>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>(); 
        healthBarWidth = healthBarFill.rectTransform.sizeDelta.x;
        health = maxHealth;
        arrowSpawnPoint = transform;

    }

    void Update()
    {
        SetHealth(health);
        FindClosestTarget();

        if (target != null)
        {
            agent.SetDestination(target.position);
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= attackRange)
            {
                Attack();
            }
        }
    }

    void FindClosestTarget()
    {
        float minDistance = 100f;
        targetBuilding = null;

        foreach (var building in manager.Buildings)
        {
            if(building.name=="ENEMY_BASE"||building.name=="ENEMY_BASE(Clone)")
            {
                continue;
            }
            float distance = Vector3.Distance(transform.position, building.transform.position);
            if (distance < minDistance)
            {
                target = building.transform; // 设置导航目标
                targetBuilding = building; // 设置攻击目标
                minDistance = distance;
            }
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    void Attack()
    {
        // 攻击冷却判断
        //Debug.Log("Attacking building");
        if (attackCooldown <= 0f)
        {
 
            if (arrowPrefab != null && arrowSpawnPoint != null)
            {

                GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);
                Arrow_enemy arrowScript = arrow.GetComponent<Arrow_enemy>();
                if (arrowScript != null)
                {
                    arrowScript.SetTarget(targetBuilding.transform); 
                }
            }

            // 重置冷却时间
            attackCooldown = attackInterval;
        }

        // 减少冷却计时
        attackCooldown -= Time.deltaTime;
    }
    public void SetHealth(float Health)
    {
        health = Mathf.Clamp(Health, 0, maxHealth);
        float healthPercentage = health / maxHealth; 
        // 通过比例设置血条
        // Debug.Log("Health Percentage: " + healthPercentage);
        healthBarFill.fillAmount = health / maxHealth;
        //设置rectTransform的宽度
        healthBarFill.rectTransform.sizeDelta = new Vector2(healthBarWidth*healthPercentage, 0.1f);
    }

}
