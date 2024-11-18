using UnityEngine;
using UnityEngine.UI;

public class test_bar : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    // 引用UI对象
    public Image healthBarFill;  // 这是血条填充部分

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        // 更新血条位置
        // UpdateHealthBarPosition();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }
    }

    void UpdateHealthBarPosition()
    {
        // 使血条在敌人头顶正确显示
        if (healthBarFill != null)
        {
            Canvas canvas = healthBarFill.GetComponentInParent<Canvas>();
            canvas.transform.position = transform.position + Vector3.up * 2f;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Destroy(gameObject); // 敌人死亡后销毁对象
        }
    }

    public void SetHealth(float health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);
        float healthPercentage = currentHealth / maxHealth; 
        // 通过比例设置血条
         Debug.Log("Health Percentage: " + healthPercentage);
        healthBarFill.fillAmount = currentHealth / maxHealth;
        //设置rectTransform的宽度
        healthBarFill.rectTransform.sizeDelta = new Vector2(healthPercentage, 0.1f);
    }
}
