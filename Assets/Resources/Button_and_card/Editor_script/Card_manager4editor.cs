using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_manager4editor : MonoBehaviour
{
    public TextAsset cardData;
    public GameObject cardPrefab;
    //private List<Card> cardList_level_0 = new();
    //private List<Card> cardList_level_1 = new();
    //private List<Card> cardList_level_2 = new();
    private Resource_building_Card base_info;
    private Enemy_base_Card enemy_base_info;
    public void Start() {
        LoadCardData();
        GameObject init_card1=Instantiate(cardPrefab);
        GameObject init_card2=Instantiate(cardPrefab);
        //GameObject init_card3=Instantiate(cardPrefab);
        init_card1.GetComponent<RectTransform>().SetParent(transform);
        init_card2.GetComponent<RectTransform>().SetParent(transform);
        //init_card3.GetComponent<RectTransform>().SetParent(transform);
        init_card1.GetComponent<RectTransform>().anchoredPosition=new Vector2(0,140);
        init_card2.GetComponent<RectTransform>().anchoredPosition=new Vector2(0,74);
        //init_card3.GetComponent<RectTransform>().anchoredPosition=new Vector2(0,8);

        init_card1.GetComponent<Card_button>().card_info=GetNewCard(1);
        init_card2.GetComponent<Card_button>().card_info=GetNewCard(2);
        //init_card3.GetComponent<Card_button>().card_info=GetNewCard();

        var button1=init_card1.GetComponent<Card_button>();
        var button2=init_card2.GetComponent<Card_button>();
        //var button3=init_card3.GetComponent<Card_button>();
        button1.button_id=1;
        button1.edit_mode=true;
        button2.button_id=2;
        button2.edit_mode=true;
        //button3.button_id=3;
        // if (button1.card_info.cardName!="House"&&button2.card_info.cardName!="House"&&button3.card_info.cardName!="House")//防止无民房
        // {
        //     init_card1.GetComponent<Card_button>().card_info=cardList_level_0[0];
        // }
        
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
        //int ATK;
        int cycle;
        //float ATK_range;

        foreach (var row in dataRow)
        {
            string[] rowArray=row.Split(',');
            if (rowArray[0] == "#")
            {
                continue;
            }
            // else if (rowArray[0] == "Resource_building_Card")
            // {
            //     id=int.Parse(rowArray[1]);
            //     cardCode=rowArray[2];
            //     cardName=rowArray[3];
            //     maximum_HP=int.Parse(rowArray[4]);
            //     cost_gold=int.Parse(rowArray[5]);
            //     level=int.Parse(rowArray[6]);
            //     output_gold=int.Parse(rowArray[7]);
            //     cycle=int.Parse(rowArray[8]);

            //     Resource_building_Card resource_Building_Card=
            //     new(id,cardCode,cardName,maximum_HP,cost_gold,level,output_gold,cycle);
            //     switch (level)
            //     {
            //         case 0:
            //             cardList_level_0.Add(resource_Building_Card);
            //             break;
            //         case 1:
            //             cardList_level_1.Add(resource_Building_Card);
            //             break;
            //         case 2:
            //             cardList_level_2.Add(resource_Building_Card);
            //             break;
            //         default:
            //             Debug.Log("Error! No such a LEVEL!");
            //             break;
            //     }

                
            // }
            // else if (rowArray[0] == "ATK_building_Card")
            // {
            //     id=int.Parse(rowArray[1]);
            //     cardCode=rowArray[2];
            //     cardName=rowArray[3];
            //     maximum_HP=int.Parse(rowArray[4]);
            //     cost_gold=int.Parse(rowArray[5]);
            //     level=int.Parse(rowArray[6]);
            //     ATK=int.Parse(rowArray[7]);
            //     cycle=int.Parse(rowArray[8]);
            //     ATK_range=float.Parse(rowArray[9]);
                
            //     ATK_building_Card aTK_Building_Card=
            //     new(id,cardCode,cardName,maximum_HP,cost_gold,level,ATK,cycle,ATK_range);
            //     switch (level)
            //     {
            //         case 0:
            //             cardList_level_0.Add(aTK_Building_Card);
            //             break;
            //         case 1:
            //             cardList_level_1.Add(aTK_Building_Card);
            //             break;
            //         case 2:
            //             cardList_level_2.Add(aTK_Building_Card);
            //             break;
            //         default:
            //             Debug.Log("Error! No such a LEVEL!");
            //             break;
            //     }
            // }
            else if(rowArray[0]=="BASE")
                {
                    id=int.Parse(rowArray[1]);
                    cardCode=rowArray[2];
                    cardName=rowArray[3];
                    maximum_HP=int.Parse(rowArray[4]);
                    cost_gold=int.Parse(rowArray[5]);
                    level=int.Parse(rowArray[6]);
                    output_gold=int.Parse(rowArray[7]);
                    cycle=int.Parse(rowArray[8]);
                    base_info=new(id,cardCode,cardName,maximum_HP,cost_gold,level,output_gold,cycle);
                }
            else if(rowArray[0]=="ENEMY_BASE")
            {
                id=int.Parse(rowArray[1]);
                cardCode=rowArray[2];
                cardName=rowArray[3];
                maximum_HP=int.Parse(rowArray[4]);
                cost_gold=int.Parse(rowArray[5]);
                level=int.Parse(rowArray[6]);
                float spawnInterval=float.Parse(rowArray[7]);
                int maxEnemies=int.Parse(rowArray[8]);
                enemy_base_info=new(id,cardCode,cardName,maximum_HP,cost_gold,level,spawnInterval,maxEnemies);
            }
        }
    }

    public Card GetNewCard(int button_id=-1)
    {
        if (button_id==1)
        {
            return base_info;
        }
        else
        {
            return enemy_base_info;
        }
    }
    public void Create_New_Card(Vector3 old_position,int button_id)
    {
        
        GameObject new_card=Instantiate(cardPrefab);
        new_card.GetComponent<Card_button>().edit_mode=true;
        new_card.GetComponent<RectTransform>().SetParent(transform);
        new_card.GetComponent<RectTransform>().position=old_position;
        new_card.GetComponent<Card_button>().card_info=GetNewCard(button_id);
    }
}
