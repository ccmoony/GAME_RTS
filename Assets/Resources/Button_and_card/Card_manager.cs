using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_manager : MonoBehaviour
{
    public TextAsset cardData;
    public GameObject cardPrefab;
    public List<GameObject> buttonList = new();

    private Currency_Manager currency_Manager;
    
    private List<Card> cardList_level_0 = new();
    private List<Card> cardList_level_1 = new();
    private List<Card> cardList_level_2 = new();
    public void Start() {
        LoadCardData();
        GameObject init_card1=Instantiate(cardPrefab);
        GameObject init_card2=Instantiate(cardPrefab);
        GameObject init_card3=Instantiate(cardPrefab);
        GameObject init_card4=Instantiate(cardPrefab);
        GameObject init_card5=Instantiate(cardPrefab);

        buttonList.Add(init_card1);
        buttonList.Add(init_card2);
        buttonList.Add(init_card3);
        buttonList.Add(init_card4);
        buttonList.Add(init_card5);

        // GameObject init_card6=Instantiate(cardPrefab);
        init_card1.GetComponent<RectTransform>().SetParent(transform);
        init_card2.GetComponent<RectTransform>().SetParent(transform);
        init_card3.GetComponent<RectTransform>().SetParent(transform);
        init_card4.GetComponent<RectTransform>().SetParent(transform);
        init_card5.GetComponent<RectTransform>().SetParent(transform);
        // init_card6.GetComponent<RectTransform>().SetParent(transform);
        init_card1.GetComponent<RectTransform>().anchoredPosition=new Vector2(0,140);
        init_card2.GetComponent<RectTransform>().anchoredPosition=new Vector2(0,74);
        init_card3.GetComponent<RectTransform>().anchoredPosition=new Vector2(0,8);
        init_card4.GetComponent<RectTransform>().anchoredPosition=new Vector2(0,-58);
        init_card5.GetComponent<RectTransform>().anchoredPosition=new Vector2(0,-124);
        // init_card6.GetComponent<RectTransform>().anchoredPosition=new Vector2(0,-190);

        init_card1.GetComponent<Card_button>().card_info=GetNewCard();
        init_card2.GetComponent<Card_button>().card_info=GetNewCard();
        init_card3.GetComponent<Card_button>().card_info=GetNewCard();
        init_card4.GetComponent<Card_button>().card_info=GetNewCard();
        init_card5.GetComponent<Card_button>().card_info=GetNewCard();
        // init_card6.GetComponent<Card_button>().card_info=GetNewCard();


        var button1=init_card1.GetComponent<Card_button>();
        var button2=init_card2.GetComponent<Card_button>();
        var button3=init_card3.GetComponent<Card_button>();
        var button4=init_card4.GetComponent<Card_button>();
        var button5=init_card5.GetComponent<Card_button>();
        // var button6=init_card6.GetComponent<Card_button>();

        button1.button_id=1;
        button2.button_id=2;
        button3.button_id=3;
        button4.button_id=4;
        button5.button_id=5;
        // button6.button_id=6;

        if (button1.card_info.cardName!="House"&&button2.card_info.cardName!="House"&&button3.card_info.cardName!="House")//防止无民房
        {
            init_card1.GetComponent<Card_button>().card_info=cardList_level_0[0];
        }
        currency_Manager=gameObject.GetComponent<Currency_Manager>();
        
    }

    public void Update() {
        
    }
    public void LoadCardData()
    {
        string[] dataRow = cardData.text.Split('\n');
        int id;
        string cardCode;
        string cardName;
        int maximum_HP;
        int cost_gold;
        int level;
        int output_gold;
        int ATK;
        int cycle;
        float ATK_range;
        float spawnInterval;
        int maxSoldiers;

        foreach (var row in dataRow)
        {
            string[] rowArray=row.Split(',');
            if (rowArray[0] == "#")
            {
                continue;
            }
            else if (rowArray[0] == "Resource_building_Card")
            {
                id=int.Parse(rowArray[1]);
                cardCode=rowArray[2];
                cardName=rowArray[3];
                maximum_HP=int.Parse(rowArray[4]);
                cost_gold=int.Parse(rowArray[5]);
                level=int.Parse(rowArray[6]);
                output_gold=int.Parse(rowArray[7]);
                cycle=int.Parse(rowArray[8]);

                Resource_building_Card resource_Building_Card=
                new(id,cardCode,cardName,maximum_HP,cost_gold,level,output_gold,cycle);
                switch (level)
                {
                    case 0:
                        cardList_level_0.Add(resource_Building_Card);
                        break;
                    case 1:
                        cardList_level_1.Add(resource_Building_Card);
                        break;
                    case 2:
                        cardList_level_2.Add(resource_Building_Card);
                        break;
                    default:
                        Debug.Log("Error! No such a LEVEL!");
                        break;
                }

                
            }
            else if (rowArray[0] == "ATK_building_Card")
            {
                id=int.Parse(rowArray[1]);
                cardCode=rowArray[2];
                cardName=rowArray[3];
                maximum_HP=int.Parse(rowArray[4]);
                cost_gold=int.Parse(rowArray[5]);
                level=int.Parse(rowArray[6]);
                ATK=int.Parse(rowArray[7]);
                cycle=int.Parse(rowArray[8]);
                ATK_range=float.Parse(rowArray[9]);
                
                ATK_building_Card aTK_Building_Card=
                new(id,cardCode,cardName,maximum_HP,cost_gold,level,ATK,cycle,ATK_range);
                switch (level)
                {
                    case 0:
                        cardList_level_0.Add(aTK_Building_Card);
                        break;
                    case 1:
                        cardList_level_1.Add(aTK_Building_Card);
                        break;
                    case 2:
                        cardList_level_2.Add(aTK_Building_Card);
                        break;
                    default:
                        Debug.Log("Error! No such a LEVEL!");
                        break;
                }
            }
            else if (rowArray[0] == "Camp_building_Card")
            {
                id=int.Parse(rowArray[1]);
                cardCode=rowArray[2];
                cardName=rowArray[3];
                maximum_HP=int.Parse(rowArray[4]);
                cost_gold=int.Parse(rowArray[5]);
                level=int.Parse(rowArray[6]);
                maxSoldiers=int.Parse(rowArray[7]);
                spawnInterval=int.Parse(rowArray[8]);
                
                
                Camp_building_Card camp_Building_Card=
                new(id,cardCode,cardName,maximum_HP,cost_gold,level,maxSoldiers,spawnInterval);
                switch (level)
                {
                    case 0:
                        cardList_level_0.Add(camp_Building_Card);
                        break;
                    case 1:
                        cardList_level_1.Add(camp_Building_Card);
                        break;
                    case 2:
                        cardList_level_2.Add(camp_Building_Card);
                        break;
                    default:
                        Debug.Log("Error! No such a LEVEL!");
                        break;
                }
            }
        }
    }

    public Card GetNewCard(int level=-1,int button_id=-1)
    {

        if(level==0)//0级
        {
            return cardList_level_0[Random.Range(0,cardList_level_0.Count)];
        }
        else if(level==1)
        {
            return cardList_level_1[Random.Range(0,cardList_level_1.Count)];
        }
        else if(level==2)
        {
            return cardList_level_2[Random.Range(0,cardList_level_2.Count)];
        }
        else //无等级要求
        {
            int index=Random.Range(0,cardList_level_0.Count+
                                    cardList_level_1.Count+
                                    cardList_level_2.Count);
            if(index>=cardList_level_0.Count)
            {
                index-=cardList_level_0.Count;
                if (index>=cardList_level_1.Count)
                {
                    index-=cardList_level_1.Count;
                    return cardList_level_2[index];
                }
                else
                {
                    return cardList_level_1[index];
                }
            }
            else
            {
                return cardList_level_0[index];
            }
        }
    }
    public GameObject Create_New_Card(Vector3 old_position,int button_id)
    {
        
        GameObject new_card=Instantiate(cardPrefab);
        new_card.GetComponent<RectTransform>().SetParent(transform);
        new_card.GetComponent<RectTransform>().position=old_position;
        new_card.GetComponent<Card_button>().card_info=GetNewCard(-1,button_id);
        new_card.GetComponent<Card_button>().button_id=button_id;
        return new_card;
    }
    public void refresh_All()
    {
        if (currency_Manager.Get_money()<1)
        {
            return;
        }
        for( int i=0;i<=4;i++)
        {
            var newCard=buttonList[i].GetComponent<Card_button>().remove_card(false);
        }
        currency_Manager.Change_money(-1);
    }
}
