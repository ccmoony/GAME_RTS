using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refresh_button : MonoBehaviour
{
    // Start is called before the first frame update
    Currency_Manager currency_Manager;
    void Start()
    {
        currency_Manager=GameObject.Find("Cardbox").GetComponent<Currency_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currency_Manager.Get_money()>=1)
        {
            GetComponent<UnityEngine.UI.Button>().interactable=true;
        }
        else
        {
            GetComponent<UnityEngine.UI.Button>().interactable=false;
        }
    }
}
