using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Base_Building_Behavior : MonoBehaviour
{

    private Currency_Manager currency_Manager;
    public Resource_building_Card card_info;
    private building_placement manager;
    

    private float health;
    private float maxHealth;
    public Image healthBarFill;
    private float healthBarWidth;

    private IEnumerator GenerateCurrency()
    {
        while (true)
        {
            yield return new WaitForSeconds(card_info.cycle);
            
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
            Destroy(gameObject);
            Debug.Log("Game Over");
        }
    }
    public void SetHealth(float Health)
    {
        health = Mathf.Clamp(Health, 0, maxHealth);
        float healthPercentage = health / maxHealth; 

        healthBarFill.fillAmount = health / maxHealth;

        healthBarFill.rectTransform.sizeDelta = new Vector2(healthBarWidth*healthPercentage, 0.3f);
    }

}