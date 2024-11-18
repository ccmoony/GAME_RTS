using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public Image healthBarFill;  // 这是血条填充部分
    private float healthBarWidth; // 血条宽度

    public float attackRange = 2f; // 攻击范围
    public float moveSpeed = 2f; // 移动速度
    public float attackInterval = 1f; // 攻击间隔
    private float attackTimer; // 攻击计时器
    private Tower targetTower; // 目标防御塔
    public float damage = 50f; // 每次攻击造成的伤害

    void Start()
    {
        currentHealth = maxHealth;
        healthBarWidth = healthBarFill.rectTransform.sizeDelta.x;    //获取血条的宽度
        
    }
    

    
    void Update()
    {
        UpdateHealthBarPosition();
        targetTower = FindClosestTower();
        
        if (targetTower != null)
        {
            
            // 检查是否在攻击范围内
            if (Vector3.Distance(transform.position, targetTower.transform.position) <= attackRange)
            {
                attackTimer += Time.deltaTime; // 更新计时器
                if (attackTimer >= attackInterval)
                {
                    Attack();
                    attackTimer = 0; // 重置计时器
                }
            }
            else
            {
                MoveTowardsTower();
            }
        }
    }
    void UpdateHealthBarPosition()
    {
        // 使血条在敌人头顶正确显示
        if (healthBarFill != null)
        {
            Canvas canvas = healthBarFill.GetComponentInParent<Canvas>();
            canvas.transform.position = transform.position + Vector3.up *0.25f;
        }
    }
    Tower FindClosestTower()
    {
        Tower[] towers = FindObjectsOfType<Tower>();
        Tower closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Tower tower in towers)
        {
            float distance = Vector3.Distance(transform.position, tower.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = tower;
            }
        }

        return closest;
    }

    void MoveTowardsTower()
    {
        if (targetTower != null)
        {
            Vector3 direction = (targetTower.transform.position - transform.position).normalized;
            direction.y = 0; // 确保只在水平面上移动
            transform.position += 0.1f*direction * moveSpeed * Time.deltaTime;
        }
    }

    void Attack()
    {
        // 实现攻击逻辑，例如减少防御塔血量
        targetTower.TakeDamage(damage); // 假设每次攻击造成5点伤害
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Destroy(gameObject); // 销毁敌人
        }
        SetHealth(currentHealth);
    }
    public void SetHealth(float health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);
        float healthPercentage = currentHealth / maxHealth; 
        // 通过比例设置血条
        // Debug.Log("Health Percentage: " + healthPercentage);
        healthBarFill.fillAmount = currentHealth / maxHealth;
        //设置rectTransform的宽度
        healthBarFill.rectTransform.sizeDelta = new Vector2(healthBarWidth*healthPercentage, 0.1f);
    }
}