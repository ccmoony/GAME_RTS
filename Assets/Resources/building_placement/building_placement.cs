using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class building_placement : MonoBehaviour
{
    public float radius=5f;

    public float terrain_height_offset=-0.9f;

    public float max_x=300f;
    private float min_x=-300f;
    public float max_z=300f;
    private float min_z=-300f;
    public float noise_scale=1f;
    public float island_size=0.8f;
    private float z_step;
    private float x_step;
    private float x_offset;
    private int z_num;
    private int x_num;


    public GameObject hexPrefab;
    public List<GameObject> treePrefab;

    public List<GameObject> landprefabs;
    public List<float> land_lower_bounds;
    private Dictionary<Vector2,int> positionList=new ();
    //0水面
    //1可建造地面
    //2树木，暂不可建造地面

    private List<GameObject> HexRings=new();//标记坐标占用情况

    public List<GameObject> Buildings=new();//记录所有建筑

    // Start is called before the first frame update
    void Awake()
    {
        // float terrain_y=terrain.transform.position.y;
        // float x_length = terrain.terrainData.size.x;
        // float z_length = terrain.terrainData.size.z;

        // float max_x=x_length+terrain.transform.position.x;
        // float min_x=terrain.transform.position.x;
        // float max_z=z_length+terrain.transform.position.z;
        // float min_z=terrain.transform.position.z;
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
        //初始化坐标，并生成六边形预制件
        GameObject tmpobj;

        bool isoffset=false;
        
        for (float tmp_z=min_z+z_step/2;tmp_z+z_step/2<max_z;tmp_z+=z_step)
        {
            
            for(float tmp_x=min_x+x_offset ;tmp_x+x_offset<max_x;tmp_x+=x_step)
            {
                
                if(isoffset)
                {
                    
                    positionList.Add(new Vector2(tmp_x+x_offset,tmp_z),0);
                    tmpobj=Instantiate(hexPrefab,new Vector3(tmp_x+x_offset,0.2f,tmp_z),hexPrefab.transform.rotation);
                    tmpobj.SetActive(false);
                    
                    HexRings.Add(tmpobj);
                }
                else
                {
                    positionList.Add(new Vector2(tmp_x,tmp_z),0);
                    tmpobj=Instantiate(hexPrefab,new Vector3(tmp_x,0.2f,tmp_z),hexPrefab.transform.rotation);
                    tmpobj.SetActive(false);
                    
                    HexRings.Add(tmpobj);
                }
                
            }
            isoffset=!isoffset;
        }

        Debug.Log("x_num:"+x_num);
        Debug.Log("坐标初始化完成");
        //地形生成
        int land_types=landprefabs.Count;
        int tree_types=treePrefab.Count;    
        float rand_x=UnityEngine.Random.Range(0f,100f);
        float rand_y=UnityEngine.Random.Range(0f,100f);
        //Debug.Log("rand_x:"+rand_x+" \nrand_y:"+rand_y);
        land_lower_bounds.Add(100f);

        //此处代码可能存在问题

        
        
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
                    positionList[positionList.ElementAt(j).Key]=land_index==0?0:1;
                    tmpobj.transform.parent=transform;
                    tmpobj.transform.name="land";
                }
                else{//种树种草
                    bool is_tree=false;
                    for(int i=0;i<treePrefab.Count;i++)
                    {
                        float noise_=Mathf.PerlinNoise(tmp_x*noise_scale*3+rand_x+i*100,tmp_z*noise_scale*3+rand_y+i*100);
                        if (noise_>0.75)
                        {
                            tmpobj=Instantiate(treePrefab[i],new Vector3(tmp_x,terrain_height_offset,tmp_z)+treePrefab[i].transform.position,treePrefab[i].transform.rotation);
                            positionList[positionList.ElementAt(j).Key]=2;
                            tmpobj.transform.parent=transform;
                            tmpobj.transform.name="land";
                            is_tree=true;
                            break;
                    }
                    if (!is_tree)
                    {
                        tmpobj=Instantiate(landprefabs[land_index],new Vector3(tmp_x,terrain_height_offset,tmp_z)+landprefabs[land_index].transform.position,landprefabs[land_index].transform.rotation);
                        positionList[positionList.ElementAt(j).Key]=1;
                        tmpobj.transform.parent=transform;
                        tmpobj.transform.name="land";

                    }

                    }
                
                }
            }
        }


        // plant_tree();

    }
    void Start()
    {
        GameObject HexParent=GameObject.Find("HexRings");
        foreach(GameObject obj in HexRings)
        {
            obj.transform.parent=HexParent.transform;
        }
        

    }
    // Update is called once per frame
    float perlin_falloff(float x,float z)
    {
        float x_falloff=MathF.Max(1f-Mathf.Abs(x/(max_x*island_size)),0);
        float y_falloff=MathF.Max(1f-Mathf.Abs(z/(max_z*island_size)),0);
        float falloff=MathF.Pow(x_falloff * y_falloff, 0.5f);
        
        return falloff;
    }
    public Dictionary<Vector2,int> GetPositionList()
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
                    if(item.Value==1)
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
            if(positionList.ElementAt(index).Value!=1){return nearest_position;}//不可建造

            nearest_position=positionList.ElementAt(index).Key;        
        }
        return nearest_position;
    }
    public GameObject Place_New_Building(GameObject new_building,Vector3 place,Quaternion rotation)
    {

        GameObject tmpobj=Instantiate(new_building,place,rotation);
        tmpobj.transform.parent=transform;
        positionList[new Vector2(place.x, place.z)]=0;
        Buildings.Add(tmpobj);

        return tmpobj;
    }

    public void Destroy_Building(int ID)
    {
        foreach(GameObject obj in Buildings)
        {
            if (obj.GetInstanceID()==ID)
            {
                Vector2 position=new Vector2(obj.transform.position.x,obj.transform.position.z);
                positionList[position]=1;
                Buildings.Remove(obj);
                Destroy(obj);
                break;
            }
        }
    }
}
