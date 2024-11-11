using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card_button : MonoBehaviour
{
    
    public GameObject blueprint;

    public TextMeshProUGUI card_name;
    public TextMeshProUGUI HP;
    public TextMeshProUGUI Resource;
    public TextMeshProUGUI Output;
    public Image building_photo;
    public Image level_color;
    public Image color_mask;
    
    private GameObject placement_obj;
    private building_placement placement_class;
    private GameObject card_box;
    private Card_manager card_manager;
    private bool selected=false;
    private RectTransform rectTransform;

    private Currency_Manager currency_Manager;
    
    public Card card_info;



    
    public void Start()
    {
        placement_obj=GameObject.Find("Building_placement");
        placement_class=placement_obj.GetComponent<building_placement>();

        card_box=GameObject.Find("Cardbox");
        card_manager=card_box.GetComponent<Card_manager>();

        currency_Manager=card_box.GetComponent<Currency_Manager>();
        rectTransform=GetComponent<RectTransform>();
        //load card info

        //load prefab 
        Debug.Log("loading"+"building/"+card_info.cardCode+"/"+card_info.cardCode+"_blueprint");
        blueprint=Resources.Load<GameObject>("building/"+card_info.cardCode+"/"+card_info.cardCode+"_blueprint");

        //print info

        card_name.text=card_info.cardName;
        HP.text = "HP: "+card_info.maximum_HP.ToString();
        Resource.text="消耗:\n "+card_info.cost_gold.ToString()+"金";
        level_color.color=Card.CARD_LEVEL_COLORS[card_info.level];

        if (card_info is Resource_building_Card)
        {
            var resource_building= card_info as Resource_building_Card;
            Output.text="产出:\n "+resource_building.output_gold.ToString()+"金/"+resource_building.cycle.ToString()+"秒";
        }
        else if (card_info is ATK_building_Card)
        {
            var ATK_building= card_info as ATK_building_Card;
            Output.text="伤害:\n "+ATK_building.ATK.ToString()+"/"+ATK_building.cycle.ToString()+"秒";
        }
    }
    public void Update()
    {
        if(card_info.cost_gold>currency_Manager.Get_money())//钱不够
        {
            
            color_mask.color=new Color(66f/255f,66f/255f,66f/255f,225f/255f);
        }
        else
        {
            color_mask.color=new Color(66f/255f,66f/255f,66f/255f,0);
        }
    }
    public bool isSelected()
    {
        if(selected){return true;}
        else{return false;}
    }
    public void SetSelected()
    {
        selected=true;
        rectTransform.position-=new Vector3(40,0,0);

    }
    public void SetDeselected()
    {
        selected=false;
        rectTransform.position+=new Vector3(40,0,0);
    }
    public void Spawn_blueprint()
    {
        if(selected)
        {
            return;
        }
        if(card_info.cost_gold>currency_Manager.Get_money())
        {
            return;
        }
        
        
        GameObject tmp_obj=Instantiate(blueprint);
        SetSelected();//卡片被选择
        var blueprint_class=tmp_obj.GetComponent<blueprint_behavior>();

        blueprint_class.init(gameObject,gameObject.GetComponent<Card_button>());

        placement_class.SetHexRingStatus(true);
        
        
    }
    public void remove_card()
    {
        card_manager.Create_New_Card(gameObject.GetComponent<RectTransform>().position);
        currency_Manager.Change_money(-card_info.cost_gold);

        Destroy(gameObject);
    }
}
