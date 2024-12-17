using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Soldier : MonoBehaviour
{
    public float health = 10f; // 小兵的生命值
    public float maxHealth = 10f;
    public int attackDamage = 1; // 攻击伤害
    public float attackInterval = 1f; // 攻击间隔
    public float attackRange = 5f; // 攻击范围
    private Transform target; // 当前目标
    private float attackCooldown; // 攻击冷却时间
    private UnityEngine.AI.NavMeshAgent agent; // 用于导航
    public Image healthBarFill;  
    private float healthBarWidth; 

    public void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = FindClosestEnemy(); // 查找最近的敌人
        healthBarWidth = healthBarFill.rectTransform.sizeDelta.x;
        health = maxHealth;
    }

    void Update()
    {   
        SetHealth(health);
        if (target != null)
        {
            agent.SetDestination(target.position); // 导航到敌人
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            // 如果接近目标，进行攻击
            if (distanceToTarget <= attackRange)
            {
                Attack();
            }
        }
        else
        {
            target = FindClosestEnemy(); // 没有目标时重新查找
        }
    }

    Transform FindClosestEnemy()
    {
        float minDistance = float.MaxValue;
        Transform closestEnemy = null;
        
        // 查找场景中的所有敌人
        Enemy_melle[] meleeEnemies = FindObjectsOfType<Enemy_melle>();
        Enemy_ranged[] rangedEnemies = FindObjectsOfType<Enemy_ranged>();

        // 寻找最近的敌人（近战和远程）
        foreach (var enemy in meleeEnemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        foreach (var enemy in rangedEnemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        return closestEnemy;
    }

    void Attack()
    {
        if (attackCooldown <= 0f)
        {
            // 攻击目标
            if (target != null)
            {
                // 处理近战敌人
                Enemy_melle meleeEnemy = target.GetComponent<Enemy_melle>();
                if (meleeEnemy != null)
                {
                    meleeEnemy.TakeDamage(attackDamage);
                }

                // 处理远程敌人
                Enemy_ranged rangedEnemy = target.GetComponent<Enemy_ranged>();
                if (rangedEnemy != null)
                {
                    rangedEnemy.TakeDamage(attackDamage);
                }
            }

            // 重置攻击冷却时间
            attackCooldown = attackInterval;
        }

        // 减少冷却计时
        attackCooldown -= Time.deltaTime;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject); // 小兵死亡
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
        healthBarFill.rectTransform.sizeDelta = new Vector2(healthBarWidth*healthPercentage, 0.1f);
    }
}
