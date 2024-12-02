using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using System.IO;

[Serializable]
public class save_map_data
{
    public float radius=5f;
    public float terrain_height_offset=-0.9f;

    public float max_x=300f;
    public float max_z=300f;
    public float noise_scale=1f;
    public float island_size=0.8f;

    public float z_step;
    public float x_step;
    public float x_offset;
    public int z_num;
    public int x_num;
    public List<float> land_lower_bounds;
    public List<Vector2> saveList1=new();
    public List<int> saveList2=new();
    public List<bool> saveList3=new();
    public int base_location_index=new();
    public List<int> enemy_spawn_location_indices=new();
    //记录建造占用情况(0/1)
    //0水面
    //1沙地
    //2草地
    //3树木1
    //4树木2

    public save_map_data(float _radius,float _terrain_height_offset,float _max_x,float _max_z,float _noise_scale,float _island_size,
        float _z_step,float _x_step,float _x_offset,int _z_num,int _x_num,List<float> _land_lower_bounds,Dictionary<Vector2,Tuple<int,bool>> _positionList,
        int _base_location_index, List<int> _enemy_spawn_location_indices)
    {
        radius=_radius;
        terrain_height_offset=_terrain_height_offset;
        max_x=_max_x;
        max_z=_max_z;
        noise_scale=_noise_scale;
        island_size=_island_size;
        z_step=_z_step;
        x_step=_x_step;
        x_offset=_x_offset;
        z_num=_z_num;
        x_num=_x_num;
        land_lower_bounds=_land_lower_bounds;
        base_location_index=_base_location_index;
        enemy_spawn_location_indices=_enemy_spawn_location_indices;
        foreach(var item in _positionList)
        {
            saveList1.Add(item.Key);
            saveList2.Add(item.Value.Item1);
            saveList3.Add(item.Value.Item2);
        }
    }
    public save_map_data()
    {

    }
    public void save_map()
    {
        string json = JsonUtility.ToJson(this);
        string filePath=Application.dataPath+"/Data/Levels/level_1"+".json";
        File.WriteAllText(filePath, json);
        Debug.Log("Save Level 1");
    }
   
}

public class building_placement : MonoBehaviour
{
    
    public bool ifrandom =false;
    public bool random_base=false;
    public TextAsset cardData;
    public float radius=5f;
    public float terrain_height_offset=-0.9f;
    public float max_x=300f;
    private float min_x=-300f;
    public float max_z=300f;
    private float min_z=-300f;
    public float noise_scale=1f;
    public float island_size=0.8f;

    [HideInInspector]
    public float z_step;
    [HideInInspector]
    public float x_step;
    [HideInInspector]
    public float x_offset;
    [HideInInspector]
    public int z_num;
    [HideInInspector]
    public int x_num;


    public GameObject hexPrefab;
    public List<GameObject> treePrefab;

    public List<GameObject> landprefabs;
    public List<float> land_lower_bounds;
    public GameObject base_prefab;
    private Dictionary<Vector2,Tuple<int,bool>> positionList=new ();
    //记录建造占用情况(0/1)
    //0水面
    //1沙地
    //2草地
    //3树木1
    //4树木2
    public int base_location_index;
    public List<int> enemy_spawn_location_indices=new();
    
    [SerializeField]
    [HideInInspector]
    private List<GameObject> HexRings=new();
    [HideInInspector]   
    public List<GameObject> Buildings=new();//记录所有建筑


    // Start is called before the first frame update
    void Start()
    {
        init_positionList();
        if(ifrandom)
        {
            Generate_Random_Terrain();
        }
        else
        {
            string filePath=Application.dataPath+"/Data/Levels/level_1.json";
            if (File.Exists(filePath))
            {
                string json=File.ReadAllText(filePath);
                Load_Terrain(json);
            }
        }
        Load_Hex();
        
    }
    void init_positionList()
    {
        if(island_size>1f){island_size=1f;}
        min_x=-max_x;
        min_z=-max_z;

        z_step=1.5f*radius;
        x_step=Mathf.Sqrt(3f)*radius;
        x_offset=0.5f*x_step;

        max_x-=x_step;
        min_x+=x_step;
        max_z-=z_step;
        min_z+=z_step;

        z_num=(int)((max_z-min_z)/z_step);
        x_num=(int)((max_x-min_x)/x_step);
        //初始化坐标，
        

        bool isoffset=false;
        
        for (float tmp_z=min_z+z_step/2;tmp_z+z_step/2<max_z;tmp_z+=z_step)
        {
            
            for(float tmp_x=min_x+x_offset ;tmp_x+x_offset<max_x;tmp_x+=x_step)
            {
                
                if(isoffset)
                {
                    
                    positionList.Add(new Vector2(tmp_x+x_offset,tmp_z),Tuple.Create(0,false));
                }
                else
                {
                    positionList.Add(new Vector2(tmp_x,tmp_z),Tuple.Create(0,false));
                }
                
            }
            isoffset=!isoffset;
        }
    }
    public void Generate_Random_Terrain()
    {
        // float terrain_y=terrain.transform.position.y;
        // float x_length = terrain.terrainData.size.x;
        // float z_length = terrain.terrainData.size.z;

        // float max_x=x_length+terrain.transform.position.x;
        // float min_x=terrain.transform.position.x;
        // float max_z=z_length+terrain.transform.position.z;
        // float min_z=terrain.transform.position.z;

        GameObject tmpobj;




        //地形生成
        int land_types=landprefabs.Count;
        int tree_types=treePrefab.Count;    
        float rand_x=UnityEngine.Random.Range(0f,100f);
        float rand_y=UnityEngine.Random.Range(0f,100f);
        //Debug.Log("rand_x:"+rand_x+" \nrand_y:"+rand_y);
        land_lower_bounds.Add(100f);
        //
        for(int j=0;j<positionList.Keys.Count;j++)
        {
            if (true)
            {
                float tmp_x=positionList.ElementAt(j).Key.x; 
                float tmp_z=positionList.ElementAt(j).Key.y;
                float noise= Mathf.PerlinNoise(tmp_x*noise_scale+rand_x,tmp_z*noise_scale+rand_y)*perlin_falloff(tmp_x,tmp_z);

                int land_index=0;
                for(int i=0;i<land_types;i++)
                {
                    if (noise>land_lower_bounds[i]&&noise<land_lower_bounds[i+1])
                    {
                        land_index=i;
                        break;
                    }
                }
                if (land_index==0||land_index==1){
                    tmpobj=Instantiate(landprefabs[land_index],new Vector3(tmp_x,terrain_height_offset,tmp_z)+landprefabs[land_index].transform.position,landprefabs[land_index].transform.rotation);
                    positionList[positionList.ElementAt(j).Key]=land_index==0?Tuple.Create(0,false):Tuple.Create(1,true);
                    tmpobj.transform.parent=transform;
                    tmpobj.transform.name="land";
                }
                else{//种树种草
                    bool is_tree=false;
                    for(int i=0;i<treePrefab.Count;i++)
                    {
                        float noise_=Mathf.PerlinNoise(tmp_x*noise_scale*3+rand_x+i*100,tmp_z*noise_scale*3+rand_y+i*100);
                        if (noise_>0.75)//树木
                        {
                            tmpobj=Instantiate(treePrefab[i],new Vector3(tmp_x,terrain_height_offset,tmp_z)+treePrefab[i].transform.position,treePrefab[i].transform.rotation);
                            positionList[positionList.ElementAt(j).Key]=Tuple.Create(3+i,false);
                            tmpobj.transform.parent=transform;
                            tmpobj.transform.name="land";
                            is_tree=true;
                            break;
                    }
                    if (!is_tree)//草地
                    {
                        tmpobj=Instantiate(landprefabs[land_index],new Vector3(tmp_x,terrain_height_offset,tmp_z)+landprefabs[land_index].transform.position,landprefabs[land_index].transform.rotation);
                        positionList[positionList.ElementAt(j).Key]=Tuple.Create(2,true);
                        tmpobj.transform.parent=transform;
                        tmpobj.transform.name="land";

                    }

                    }
                
                }
            }
        }
        //读取卡片数据
        string[] dataRow = cardData.text.Split('\n');
        //加载基地
        if(random_base)
        {
            int tmp_count=(int)z_num/2*x_num+(int)x_num/2;
            bool exist_land=false;
            while(true)
            {
                int tmp_rand=UnityEngine.Random.Range(0,3);
                switch(tmp_rand)
                {
                    case 0:
                        tmp_count++;
                        break;
                    case 1:
                        tmp_count--;
                        break;
                    case 2:
                        tmp_count+=x_num;
                        break;
                    case 3:
                        tmp_count-=x_num;
                        break;
                }
                if (tmp_count<0||tmp_count>=positionList.Count)
                {
                    for(int i=0;i<positionList.Count;i++)
                    {
                        if (positionList.ElementAt(i).Value.Item1==1||positionList.ElementAt(i).Value.Item1==2)
                        {
                            tmp_count=i;
                            exist_land=true;
                            break;
                        }
                    }
                    break;
                
                }
                if(positionList.ElementAt(tmp_count).Value.Item1==1
                ||positionList.ElementAt(tmp_count).Value.Item1==2)
                {
                    exist_land=true;
                    break;
                }
            }
            if (!exist_land)
            {
                Debug.Log("找不到可供放置基地的地块");
            }
            else//放置基地
            {
                base_location_index=tmp_count;
                Place_Base(dataRow,positionList.ElementAt(tmp_count).Key);
            }
        }
    }
    public void Load_Terrain(string json)
    {
        GameObject tmpobj;
        save_map_data save_data=new save_map_data();
        JsonUtility.FromJsonOverwrite(json,save_data);
        radius=save_data.radius;
        terrain_height_offset=save_data.terrain_height_offset;
        max_x=save_data.max_x;
        max_z=save_data.max_z;
        noise_scale=save_data.noise_scale;
        island_size=save_data.island_size;
        z_step=save_data.z_step;
        x_step=save_data.x_step;
        x_offset=save_data.x_offset;
        z_num=save_data.z_num;
        x_num=save_data.x_num;
        land_lower_bounds=save_data.land_lower_bounds;
        List<Vector2> saveList1=save_data.saveList1;
        List<int> saveList2=save_data.saveList2;
        List<bool> saveList3=save_data.saveList3;
        base_location_index=save_data.base_location_index;
        enemy_spawn_location_indices=save_data.enemy_spawn_location_indices;
        

        for(int i=0;i<saveList1.Count;i++)
        {
            positionList[saveList1[i]]=Tuple.Create(saveList2[i],saveList3[i]);
        }

        foreach(var item in positionList)//重新生成地形
        {
            if(item.Value.Item1<=2)//非树
            {
                
                Debug.Log(landprefabs.Count);
                tmpobj=Instantiate(landprefabs[item.Value.Item1],
                    new Vector3(item.Key.x,terrain_height_offset,item.Key.y)+landprefabs[item.Value.Item1].transform.position,
                    landprefabs[item.Value.Item1].transform.rotation);
                tmpobj.transform.parent=transform;
            }
            else//树
            {
                tmpobj=Instantiate(treePrefab[item.Value.Item1-3],
                    new Vector3(item.Key.x,terrain_height_offset,item.Key.y)+treePrefab[item.Value.Item1-3].transform.position,
                    treePrefab[item.Value.Item1-3].transform.rotation);
                tmpobj.transform.parent=transform;
            }
        }
        //加载基地
        string[] dataRow = cardData.text.Split('\n');
        Place_Base(dataRow,positionList.ElementAt(base_location_index).Key);

    }
    void Place_Base(string[] dataRow,Vector2 location2d)//放置基地
    {
        Resource_building_Card resource_Building_Card = null;
            foreach (var row in dataRow)
            {
                string[] rowArray=row.Split(',');
                if (rowArray[0]=="BASE")
                {
                    int id=int.Parse(rowArray[1]);
                    string cardCode=rowArray[2];
                    string cardName=rowArray[3];
                    int maximum_HP=int.Parse(rowArray[4]);
                    int cost_gold=int.Parse(rowArray[5]);
                    int level=int.Parse(rowArray[6]);
                    int output_gold=int.Parse(rowArray[7]);
                    int cycle=int.Parse(rowArray[8]);

                resource_Building_Card=
                new(id,cardCode,cardName,maximum_HP,cost_gold,level,output_gold,cycle);
                break;
                }
            }
            GameObject real_base=Place_New_Building(base_prefab,
                                                new Vector3(location2d.x,0,location2d.y),
                                                base_prefab.transform.rotation);
            var building_behavior=real_base.GetComponent<Resource_Building_Behavior>();
            building_behavior.card_info=resource_Building_Card;
    }
    void Load_Hex()
    {
        foreach(var item in positionList)
        {
            if (item.Value.Item2==true)
            {
                GameObject tmpobj=Instantiate(hexPrefab,new Vector3(item.Key.x,0.2f,item.Key.y),hexPrefab.transform.rotation);
                tmpobj.SetActive(false);
                HexRings.Add(tmpobj);
            }
        }
        GameObject HexParent=GameObject.Find("HexRings");
        foreach(GameObject obj in HexRings)
        {
            obj.transform.parent=HexParent.transform;
        }
    }
    float perlin_falloff(float x,float z)
    {
        float x_falloff=MathF.Max(1f-Mathf.Abs(x/(max_x*island_size)),0);
        float y_falloff=MathF.Max(1f-Mathf.Abs(z/(max_z*island_size)),0);
        float falloff=MathF.Pow(x_falloff * y_falloff, 0.5f);
        
        return falloff;
    }
    public Dictionary<Vector2,Tuple<int,bool>> GetPositionList()
    {
        return positionList;
    }
    public void SetHexRingStatus(bool visable=true)
    {
        foreach(GameObject obj in HexRings)
        {
                obj.SetActive(visable);
        }
    }
    public Vector2 Find_nearest_build_position_old(RaycastHit hit)//用于移动蓝色半透预制件，获取最近建造位置
    {
        Vector2 nearest_position= new(-1000f,-1000f);
        float min_distance=radius*2;
        if (hit.transform.name=="Terrain")
            {
                foreach(var item in positionList)
                {
                    if(item.Value.Item2==true)
                    {
                        if (Mathf.Abs(item.Key.x-hit.point.x)>min_distance||Mathf.Abs(item.Key.y-hit.point.z)>min_distance){continue;}
                        float tmp_dist=Vector2.Distance(new Vector2(hit.point.x,hit.point.z),item.Key);
                        if (tmp_dist<min_distance)
                        {
                            nearest_position=item.Key;
                            min_distance=tmp_dist;
                        }
                    }
                }
            }
        return nearest_position;
    }
    public Vector2 Find_nearest_build_position(RaycastHit hit, Vector3 blue_position)//用于移动蓝色半透预制件，获取最近建造位置
    {
        Vector2 nearest_position= new(blue_position.x,blue_position.z);
        
        if (hit.transform.name=="Terrain")
        {
            float hitx=hit.point.x;
            float hitz=hit.point.z;
            int index=-1;
            int z_index=(int)((hitz-min_z)/z_step);
            int z_offset=z_index%2;

            if(z_index<0||z_index>=z_num-1){return nearest_position;}//超出边界
            
            index+=z_index*x_num;
            if (z_offset==0)//No offset
            {
                index+=(int)((hitx-min_x)/x_step)+1;
            }
            else//Offset
            {
                index+=(int)((hitx-min_x-x_offset)/x_step)+1;
            }
            if(positionList.ElementAt(index).Value.Item2==false){return nearest_position;}//不可建造

            nearest_position=positionList.ElementAt(index).Key;        
        }
        return nearest_position;
    }
    public GameObject Place_New_Building(GameObject new_building,Vector3 place,Quaternion rotation)
    {
        Debug.Log(new_building.transform.name);
        if(new_building.transform.name=="BASE")
        {
            base_location_index=positionList.Keys.ToList().IndexOf(new Vector2(place.x,place.z));
        }
        GameObject tmpobj=Instantiate(new_building,place,rotation);
        tmpobj.transform.parent=transform;
        positionList[new Vector2(place.x, place.z)]=Tuple.Create(positionList[new Vector2(place.x, place.z)].Item1,false);
        Buildings.Add(tmpobj);

        return tmpobj;
    }
    public void Destroy_Building_from_List(int ID)
    {
        int i=0;
        bool found=false;
        for(;i<Buildings.Count;i++)
        {
            if (Buildings[i].GetInstanceID()==ID)
            {
                Vector2 position=new Vector2(Buildings[i].transform.position.x,Buildings[i].transform.position.z);
                positionList[position]=Tuple.Create(positionList[position].Item1,true);//可建造
                found=true;
                break;
            }
        }
        if (found)
        {
            Buildings.RemoveAt(i);
        }
    }
    public void Save_map()
    {
        save_map_data save_data=new save_map_data(radius,
                                                terrain_height_offset,
                                                max_x,
                                                max_z,
                                                noise_scale,
                                                island_size,
                                                z_step,
                                                x_step,
                                                x_offset,
                                                z_num,
                                                x_num,
                                                land_lower_bounds,
                                                positionList,
                                                base_location_index,
                                                enemy_spawn_location_indices);
        save_data.save_map();
    }
    
}
