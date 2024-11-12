using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 3; 

    public Transform target; 
    private UnityEngine.AI.NavMeshAgent agent; 
    private building_placement manager;
    void Start()
    {
        manager = FindObjectOfType<building_placement>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>(); 
        if (target == null)
        {
            Debug.LogWarning("Target not set for the enemy!");
        }
    }

    void Update()
    {
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
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject); // 销毁敌人
    }
}
