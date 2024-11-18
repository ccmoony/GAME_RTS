using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 10f;
    private Enemy target;

    void Update()
    {
        if (target != null)
        {
            // 移动到目标
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // 检查与目标的距离
            if (Vector3.Distance(transform.position, target.transform.position) < 0.5f)
            {
                // 对目标造成伤害
                target.TakeDamage(damage);
                Destroy(gameObject); // 销毁投射物
            }
        }
    }

    public void SetTarget(Enemy enemy)
    {
        target = enemy;
    }
}