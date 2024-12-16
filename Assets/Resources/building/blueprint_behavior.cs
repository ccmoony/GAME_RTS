using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class blueprint_behavior : MonoBehaviour
{
    RaycastHit hit;
    public GameObject real_building;
    [HideInInspector]
    public GameObject placement_obj;
    [HideInInspector]
    public GraphicRaycaster graphicRaycaster;
    private GameObject father_button;

    

    private building_placement placement_class;

    private Card_button card_Button;



    
    
    
    // Start is called before the first frame update
    void Start()
    {
        placement_obj=GameObject.Find("Building_placement");

        graphicRaycaster=GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();

        placement_class=placement_obj.GetComponent<building_placement>();
        
        


        Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray,out hit))
        {   

            Vector2 nearest_position= placement_class.Find_nearest_build_position(hit,transform.position);//获取最近建造位置
            
            transform.position=new Vector3(nearest_position.x,0.15f,nearest_position.y);

        }
        
        
    }
    // Update is called once per frame
    void Update()
    {
        Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);



        
        if(Physics.Raycast(ray,out hit))
        {   
            Vector2 nearest_position= placement_class.Find_nearest_build_position(hit,transform.position);//获取最近建造位置
            
            transform.position=new Vector3(nearest_position.x,0.15f,nearest_position.y);//+0.1f防止模型底部高亮和地形发生穿模

            if (Input.GetMouseButton(0) && hit.transform.name=="Terrain")//进行建造
            {
                if (!EventSystem.current.IsPointerOverGameObject())//没有位于UI
                {
                    if(transform.position.x==-1000f || transform.position.y==-1000f)
                    {
                        Destroy(gameObject);
                    }
                    else//放置实体
                    {
                        placement_class.SetHexRingStatus(false);

                        

                        GameObject instance_real_building=
                        placement_class.Place_New_Building(real_building,transform.position,real_building.transform.rotation);//真实建筑物体

                        //卡片归位
                        card_Button.SetDeselected();
                        card_Button.remove_card();//移除当前使用的卡片


                        if (card_Button.card_info is Resource_building_Card)
                        {
                            var building_behavior=instance_real_building.GetComponent<Resource_Building_Behavior>();

                            building_behavior.card_info=card_Button.card_info as Resource_building_Card;

                        }
                        else if (card_Button.card_info is ATK_building_Card)
                        {
                            var building_behavior=instance_real_building.GetComponent<ATK_Building_Behavior>();

                            building_behavior.card_info=card_Button.card_info as ATK_building_Card;
                        }
                        else if (card_Button.card_info is Enemy_base_Card)
                        {
                        }
                        else{}


                        Destroy(gameObject);

                    }
                }
                else//位于UI
                {
                    //投射UI光线
                    List<RaycastResult> results = new List<RaycastResult>();
                    PointerEventData pointerData = new PointerEventData(EventSystem.current)
                    {
                        position = Input.mousePosition
                    };
                    graphicRaycaster.Raycast(pointerData, results);
                    foreach( var result in results)
                    {
                        Debug.Log(result.gameObject.name);
                    }

                    placement_class.SetHexRingStatus(false);
                    //卡片归位
                    card_Button.SetDeselected();
                    Destroy(gameObject);
                }
            }
            else if(Input.GetMouseButton(1) || (Input.GetMouseButton(0) && hit.transform.name!="Terrain"))
            {
                placement_class.SetHexRingStatus(false);
                //卡片归位
                card_Button.SetDeselected();
                Destroy(gameObject);
            }
        }
        else
        {
            if(Input.GetMouseButton(1))
            {
                placement_class.SetHexRingStatus(false);
                //卡片归位
                card_Button.SetDeselected();
                Destroy(gameObject);

            }
        }

    }
    public void init(GameObject _father_button,Card_button _card_Button)
    {
        father_button=_father_button;
        card_Button=_card_Button;
    }
}
