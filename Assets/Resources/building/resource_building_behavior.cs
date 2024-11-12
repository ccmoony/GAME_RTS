using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Resource_Building_Behavior : MonoBehaviour
{

    private Currency_Manager currency_Manager;
    public Resource_building_Card card_info;

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
        currency_Manager=GameObject.Find("Cardbox").GetComponent<Currency_Manager>();
        StartCoroutine(GenerateCurrency());
    }

}