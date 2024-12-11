using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    public GameObject bladeEffectPrefab; // 刀光预制体
    public float effectDuration = 0.5f;  // 刀光持续时间
    public float spawnOffset = 0.1f;     // 刀光相对于敌人位置的偏移距离

    // 方法：在敌人附近生成刀光
    public void GenerateBladeEffectNearEnemy(Transform enemy, Transform target)
    {
        if (bladeEffectPrefab != null && enemy != null && target != null)
        {
            // 计算刀光的生成位置
            //Debug.Log("Attack effect generated");

            Vector3 spawnPosition = enemy.position + spawnOffset * (target.position - enemy.position);

            // 实例化刀光
            GameObject bladeEffect = Instantiate(bladeEffectPrefab, spawnPosition, Quaternion.identity);

            // 设置刀光方向
            bladeEffect.transform.LookAt(enemy.position); // 刀光面向敌人

            // 自动销毁刀光
            Destroy(bladeEffect, effectDuration);
        }
    }
}
