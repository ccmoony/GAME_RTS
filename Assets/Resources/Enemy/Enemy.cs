using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float health; 
    public float maxHealth=3f;
    public Transform target; 
    private UnityEngine.AI.NavMeshAgent agent; 
    private building_placement manager;
    public Image healthBarFill;  // 这是血条填充部分
    private float healthBarWidth; // 血条宽度
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
        float min_distance=100f;
        foreach (var building in manager.Buildings)
        {
            float distance = Vector3.Distance(transform.position, building.transform.position);
            if (distance < min_distance)
            {
                target = building.transform;
                min_distance = distance;
            }
        }

        if (target != null)
        {
            agent.SetDestination(target.position);
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
