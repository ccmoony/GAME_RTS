using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class building_placement : MonoBehaviour
{
    public float radius=5f;

    public float max_x=200f;
    private float min_x=-200f;
    public float max_z=200f;
    private float min_z=-200f;
    public float noise_scale=1f;
    public float island_size=0.8f;

    public GameObject hexPrefab;
    public List<GameObject> landprefabs;
    public List<float> land_lower_bounds;
    
    private Dictionary<Vector2,bool> positionList=new ();

    private List<GameObject> HexRings=new();//标记坐标占用情况

    private List<GameObject> Buildings=new();//记录所有建筑

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

        float z_step=1.5f*radius;
        float x_step=Mathf.Sqrt(3f)*radius;
        float x_offset=0.5f*x_step;
        max_x-=x_step;
        min_x+=x_step;
        max_z-=z_step;
        min_z+=z_step;

        //初始化坐标，并生成六边形预制件
        GameObject tmpobj;

        bool isoffset=false;
        for (float tmp_z=min_z;tmp_z<max_z;tmp_z+=z_step)
        {
            for(float tmp_x=min_x;tmp_x<max_x;tmp_x+=x_step)
            {
                
                if(isoffset)
                {
                    
                    positionList.Add(new Vector2(tmp_x+x_offset,tmp_z),true);
                    tmpobj=Instantiate(hexPrefab,new Vector3(tmp_x+x_offset,0.07f,tmp_z),hexPrefab.transform.rotation);
                    tmpobj.SetActive(false);
                    
                    HexRings.Add(tmpobj);
                }
                else
                {
                    positionList.Add(new Vector2(tmp_x,tmp_z),true);
                    tmpobj=Instantiate(hexPrefab,new Vector3(tmp_x,0.07f,tmp_z),hexPrefab.transform.rotation);
                    tmpobj.SetActive(false);
                    
                    HexRings.Add(tmpobj);
                }
                
            }
            isoffset=!isoffset;
        }
        Debug.Log("坐标初始化完成");
        //地形生成
        int land_types=landprefabs.Count;
        float rand_x=UnityEngine.Random.Range(0f,100f);
        float rand_y=UnityEngine.Random.Range(0f,100f);
        Debug.Log("rand_x:"+rand_x+" \nrand_y:"+rand_y);
        land_lower_bounds.Add(100f);

        //此处代码可能存在问题

        


        //
        foreach(var pos in positionList)
        {
            if (true)
            {
                float tmp_x=pos.Key.x;
                float tmp_z=pos.Key.y;
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
                
                tmpobj=Instantiate(landprefabs[land_index],new Vector3(tmp_x,-0.89f,tmp_z)+landprefabs[land_index].transform.position,landprefabs[land_index].transform.rotation);
                tmpobj.transform.parent=transform;
                
            }
        }
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
    public Dictionary<Vector2,bool> GetPositionList()
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
    public Vector2 Find_nearest_build_position(RaycastHit hit)//用于移动蓝色半透预制件，获取最近建造位置
    {
        Vector2 nearest_position= new(-1000f,-1000f);
        float min_distance=radius*2;
        if (hit.transform.name=="Terrain")
            {
                foreach(var item in positionList)
                {
                    if(!item.Value){continue;}
                    if (Mathf.Abs(item.Key.x-hit.point.x)>min_distance||Mathf.Abs(item.Key.y-hit.point.z)>min_distance){continue;}
                    float tmp_dist=Vector2.Distance(new Vector2(hit.point.x,hit.point.z),item.Key);
                    if (tmp_dist<min_distance)
                    {
                        nearest_position=item.Key;
                        min_distance=tmp_dist;
                    }
                }
            }
        return nearest_position;
    }
    public GameObject Place_New_Building(GameObject new_building,Vector3 place,Quaternion rotation)
    {

        GameObject tmpobj=Instantiate(new_building,place,rotation);
        tmpobj.transform.parent=transform;
        positionList[new Vector2(place.x, place.z)]=false;
        Buildings.Add(tmpobj);

        return tmpobj;
    }
}
