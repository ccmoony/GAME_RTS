using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float health; 
    public float maxHealth=3f;
    public int attackDamage = 1; // 攻击伤害
    public float attackInterval = 1f; // 攻击间隔时间

    public float attackRange = 6f;
    public Transform target; 
    private GameObject targetBuilding;
    private UnityEngine.AI.NavMeshAgent agent; 
    private building_placement manager;
    public Image healthBarFill;  // 这是血条填充部分
    private float healthBarWidth; // 血条宽度
    private float attackCooldown; // 攻击冷却计时
    void Start()
    {
        manager = FindObjectOfType<building_placement>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>(); 
        healthBarWidth = healthBarFill.rectTransform.sizeDelta.x;
        health = maxHealth;
        if (target == null)
        {
            Debug.LogWarning("Target not set for the enemy!");
        }
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
        Debug.Log("Attacking building");
        if (attackCooldown <= 0f)
        {
            if (targetBuilding != null)
            {
                // 对目标造成伤害
                var buildingComponent = targetBuilding.GetComponent<ATK_Building_Behavior>();
                if (buildingComponent != null)
                {
                    buildingComponent.TakeDamage(attackDamage);
                }
                else
                {
                    var buildingComponent2 = targetBuilding.GetComponent<Resource_Building_Behavior>();
                    if (buildingComponent2 != null)
                    {
                        buildingComponent2.TakeDamage(attackDamage);
                    }
                }

                Debug.Log($"Attacked {targetBuilding.name} for {attackDamage} damage!");
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
