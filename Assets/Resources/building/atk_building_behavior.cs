using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;

public class ATK_Building_Behavior : MonoBehaviour
{


    public ATK_building_Card card_info;
    private building_placement manager;

    public GameObject arrowPrefab;        
    private Transform arrowSpawnPoint;   
    public LayerMask enemyLayer;          
    private float lastAttackTime = -Mathf.Infinity;
    void Start()
    {
        manager = FindObjectOfType<building_placement>();
        arrowSpawnPoint = transform;
    }
    void Update()
    {
        Debug.Log(transform.position);
        Debug.Log(card_info.ATK_range);
        Debug.Log(enemyLayer);
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, card_info.ATK_range, enemyLayer);

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

        if (card_info.maximum_HP <= 0){
            manager.Destroy_Building(gameObject.GetInstanceID());
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
        card_info.maximum_HP -= attackDamage;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, card_info.ATK_range);
    }
}