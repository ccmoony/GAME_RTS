using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_enemy : MonoBehaviour
{
    public float speed = 10f;        
    public float damage = 1f;       
    private Transform target;        

    public void SetTarget(Transform enemy)
    {
        target = enemy;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        float distanceThisFrame = speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, target.position) <= distanceThisFrame)
        {
            HitTarget();
            Destroy(gameObject);
        }
        Quaternion lookRotation = Quaternion.LookRotation(direction); 

        transform.rotation = lookRotation * Quaternion.Euler(90f, 0f, 0f); 

        transform.Translate(direction * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        if (target != null)
        {
            // 对目标造成伤害
            var buildingComponent = target.GetComponent<ATK_Building_Behavior>();
            if (buildingComponent != null)
            {
                buildingComponent.TakeDamage((int)damage);
            }
            else
            {
                var buildingComponent2 = target.GetComponent<Resource_Building_Behavior>();
                if (buildingComponent2 != null)
                {
                    buildingComponent2.TakeDamage((int)damage);
                }
            }

            //Debug.Log($"Attacked {targetBuilding.name} for {attackDamage} damage!");
        }
    }
}
