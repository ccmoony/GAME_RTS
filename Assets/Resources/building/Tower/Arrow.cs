using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
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
        Enemy_melle enemy = target.GetComponent<Enemy_melle>();
        if (enemy != null)
        {
            enemy.TakeDamage((int)damage);
        }
        else
        {
            Enemy_ranged enemy2 =  target.GetComponent<Enemy_ranged>();
            if (enemy2 != null)
            {
                enemy2.TakeDamage((int)damage);
            }
        }
        //Debug.Log("Hit " + target.name + " for " + damage + " damage!");

        Destroy(gameObject);
    }
}
