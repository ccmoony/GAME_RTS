using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Card_manager : MonoBehaviour
{
    public TextAsset cardData;
    public GameObject cardPrefab;
    private List<Card> cardList_level_0 = new();
    private List<Card> cardList_level_1 = new();
    private List<Card> cardList_level_2 = new();
    public void Start() {
        LoadCardData();
        GameObject init_card1=Instantiate(cardPrefab);
        GameObject init_card2=Instantiate(cardPrefab);
        GameObject init_card3=Instantiate(cardPrefab);
        init_card1.GetComponent<RectTransform>().SetParent(transform);
        init_card2.GetComponent<RectTransform>().SetParent(transform);
        init_card3.GetComponent<RectTransform>().SetParent(transform);
        init_card1.GetComponent<RectTransform>().anchoredPosition=new Vector2(0,140);
        init_card2.GetComponent<RectTransform>().anchoredPosition=new Vector2(0,74);
        init_card3.GetComponent<RectTransform>().anchoredPosition=new Vector2(0,8);

        init_card1.GetComponent<Card_button>().card_info=GetNewCard();
        init_card2.GetComponent<Card_button>().card_info=GetNewCard();
        init_card3.GetComponent<Card_button>().card_info=GetNewCard();

        var info1=init_card1.GetComponent<Card_button>().card_info;
        var info2=init_card2.GetComponent<Card_button>().card_info;
        var info3=init_card3.GetComponent<Card_button>().card_info;
        if (info1.cardName!="House"&&info2.cardName!="House"&&info3.cardName!="House")//防止无民房
        {
            init_card1.GetComponent<Card_button>().card_info=cardList_level_0[0];
        }
        
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
                
                ATK_building_Card aTK_Building_Card=
                new(id,cardCode,cardName,maximum_HP,cost_gold,level,ATK,cycle);
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
        }
    }

    public Card GetNewCard(int level=-1)
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
    public void Create_New_Card(Vector3 old_position)
    {
        
        GameObject new_card=Instantiate(cardPrefab);
        new_card.GetComponent<RectTransform>().SetParent(transform);
        new_card.GetComponent<RectTransform>().position=old_position;
        new_card.GetComponent<Card_button>().card_info=GetNewCard();
    }
}
