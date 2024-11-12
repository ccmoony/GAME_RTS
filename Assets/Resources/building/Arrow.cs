using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;        // 箭的飞行速度
    public float damage = 1f;       // 箭的伤害
    private Transform target;        // 目标敌人

    // 设置箭的目标
    public void SetTarget(Transform enemy)
    {
        target = enemy;
    }

    void Update()
    {
        if (target == null)
        {
             // 如果目标已经消失，则销毁箭
            Destroy(gameObject);
            return;
        }

        // 让箭朝向目标移动
        Vector3 direction = (target.position - transform.position).normalized;
        float distanceThisFrame = speed * Time.deltaTime;

        // 检查箭是否到达了目标
        if (Vector3.Distance(transform.position, target.position) <= distanceThisFrame)
        {
            HitTarget();
            return;
        }
        Quaternion lookRotation = Quaternion.LookRotation(direction); 

        // 3. 进行额外的旋转修正，使Y轴指向目标（预制件箭头指向y轴）
        transform.rotation = lookRotation * Quaternion.Euler(90f, 0f, 0f); 

        transform.Translate(direction * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage((int)damage);
        }
        Debug.Log("Hit " + target.name + " for " + damage + " damage!");

        Destroy(gameObject);
    }
}
