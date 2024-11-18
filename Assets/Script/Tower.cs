using System.Collections;
using System.Collections.Generic;
// using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public float attackRange = 5f; // 攻击范围
    public float attackInterval = 1f; // 攻击间隔
    public GameObject projectilePrefab; // 投射物预设
    public int projectilesPerAttack = 1; // 每次攻击的子弹数量
    public float maxHealth = 300f; // 防御塔生命值    
    private float currentHealth; // 防御塔当前生命值

    // private float currentHealth;
    public Transform projectileSpawnPoint; // 子弹生成点
    private float attackTimer;

    public Image healthBarFill;  // 这是血条填充部分
    private float healthBarWidth; // 血条宽度


    void Start()
    {
        currentHealth = maxHealth;
        healthBarWidth = healthBarFill.rectTransform.sizeDelta.x;    //获取血条的宽度
    }
    void Update()
    {
        
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval)
        {
            Attack();
            attackTimer = 0; // 重置计时器
        }
    }

    void Attack()
    {
        // 找到所有敌人
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        // 将可攻击的敌人存入列表
        List<Enemy> targets = new List<Enemy>();

        foreach (Enemy enemy in enemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) <= attackRange)
            {
                targets.Add(enemy);
            }
        }

        // 确保至少有两个目标
        for (int i = 0; i < projectilesPerAttack; i++)
        {
            if (targets.Count > 0)
            {
                // 随机选择一个目标
                int targetIndex = Random.Range(0, targets.Count);
                Enemy target = targets[targetIndex];
                if (projectileSpawnPoint){
                // 实例化投射物并设置目标
                    GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
                    projectile.GetComponent<Projectile>().SetTarget(target);
                }
                else{
                    GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                    projectile.GetComponent<Projectile>().SetTarget(target);
                }

                // 从目标列表中移除已攻击的敌人
                targets.RemoveAt(targetIndex);
            }
        }
    }
    public void Upgrade()
    {
        projectilesPerAttack = 2; // 升级后每次发射两个子弹
        // projectilePrefab.GetComponent<Projectile>().damage = 200; // 升级后子弹伤害为20
        // 可以在这里添加其他升级逻辑（如增加攻击力等）
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject); // 销毁防御塔
        }
        SetHealth(currentHealth);
    }

    public void SetHealth(float health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);
        float healthPercentage = currentHealth / maxHealth; 
        // 通过比例设置血条
        Debug.Log("tower Health Percentage: " + healthPercentage);
        healthBarFill.fillAmount = currentHealth / maxHealth;
        //设置rectTransform的宽度
        healthBarFill.rectTransform.sizeDelta = new Vector2(healthPercentage, 0.1f);
    }
}
