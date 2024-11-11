using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Currency_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    public int start_currency=10;
    private int currency;

    public TextMeshProUGUI money_show;
    void Start()
    {
        currency=start_currency>=0?start_currency:0;
    }

    // Update is called once per frame
    void Update()
    {
        money_show.text=currency.ToString();
    }

    public bool Change_money(int change)
    {
        if (currency+change<0)
        {
            return false;
        }
        else
        {
            currency+=change;
            return true;
        }
    }
    public int Get_money()
    {
        var copy_money=currency;
        return copy_money;
    }
}
