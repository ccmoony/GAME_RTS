using UnityEngine;
using UnityEngine.UI;


public class Card{
    public static Color[] CARD_LEVEL_COLORS={new Color(88f/255f,202f/255f,53f/255f),new Color(35f/255f,198f/255f,255f/255f),new Color(160f/255f,0f,170f/255f)};
    public int id;
    public string cardCode;
    public string cardName;
    public int maximum_HP;
    //costs
    public int cost_gold;

    public int level;
    public Card(int _id,string _cardCode,string _cardName,int _HP,int _gold,int _level)
    {
        this.id=_id;
        this.cardCode=_cardCode;
        this.cardName=_cardName;
        this.maximum_HP=_HP;
        this.cost_gold=_gold;
        this.level=_level;
    }

}
public class Resource_building_Card : Card
{
    public int output_gold;
    public int cycle;
    public Resource_building_Card(int _id,string _cardCode,string _cardName,int _HP,int _gold,int _level,
                    int _o_gold,int _cycle):base(_id,_cardCode,_cardName,_HP,_gold,_level)
    {
        this.output_gold=_o_gold;
        this.cycle=_cycle;
    }

}
public class ATK_building_Card : Card
{
    public int ATK;
    public int cycle;
    public ATK_building_Card(int _id,string _cardCode,string _cardName,int _HP,int _gold,int _level,
                    int _ATK,int _cycle):base(_id,_cardCode,_cardName,_HP,_gold,_level)
    {
        this.ATK=_ATK;
        this.cycle=_cycle;
    }
}