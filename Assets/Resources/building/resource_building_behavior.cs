using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Resource_Building_Behavior : MonoBehaviour
{

    private Currency_Manager currency_Manager;
    public Resource_building_Card card_info;
    private building_placement manager;

    public float health;
    public float maxHealth;
    public Image healthBarFill;
    private float healthBarWidth;

    private IEnumerator GenerateCurrency()
    {
        while (true)
        {
            // 等待指定的时间
            yield return new WaitForSeconds(card_info.cycle);
            
            // 生成货币
            currency_Manager.Change_money(card_info.output_gold);
        }
    }

    public void Start()
    {
        manager = FindObjectOfType<building_placement>();
        currency_Manager=GameObject.Find("Cardbox").GetComponent<Currency_Manager>();
        StartCoroutine(GenerateCurrency());
        health = card_info.maximum_HP;

        maxHealth = card_info.maximum_HP;
        healthBarWidth = healthBarFill.rectTransform.sizeDelta.x;

    }
    public void TakeDamage(int attackDamage)
    {
        health -= attackDamage;
    }


    void Update(){

        SetHealth(health);
        if (health <= 0){
            manager.Destroy_Building(gameObject.GetInstanceID());
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
        healthBarFill.rectTransform.sizeDelta = new Vector2(healthBarWidth*healthPercentage, 0.3f);
    }

}