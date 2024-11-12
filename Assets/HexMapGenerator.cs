using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMapGenerator : MonoBehaviour
{
    public GameObject[] hexPrefabs; // 包含两种基础六边形的数组
    public GameObject[] forestPrefab; // 森林的预制件
    public GameObject[] lakePrefab; // 湖泊的预制件
    public GameObject mountainPrefab; // 山的预制件
    
    public GameObject hex_map; // 六边形地图的父对象
    public int width = 20; // 地图宽度
    public int height = 20; // 地图高度

    private GameObject[,] hexMap; // 存储生成的六边形

    void Start()
    {
        hexMap = new GameObject[width+2, height+2];
        GenerateMap();
        for (int i = 0; i < 5; i++)
        {
            CreateEnvironment();
        }
        // CreateEnvironment();
        // createAroundMountian();
    }

    void GenerateMap()
    {
        for (int x = 1; x <=width; x++)
        {
            for (int y = 1; y <=height; y++)
            {
                float yPos = y * 4.5f; // 每个六边形的宽度
                float xPos = x * Mathf.Sqrt(3)*3f; // 每个六边形的高度

                // 偶数行偏移
                if ((y-1) % 2 == 1)
                {
                    xPos += Mathf.Sqrt(3)/2*3f;                 
                }
                GameObject hexPrefab;
                if (Random.Range(0, 100) < 20)
                {
                    hexPrefab = hexPrefabs[Random.Range(0, hexPrefabs.Length)];
                }
                else
                {
                    hexPrefab = hexPrefabs[0];
                }
                // hexPrefab = hexPrefabs[1]; 
                hexPrefab.transform.localScale = new Vector3(5.196f, 5.196f, 5.196f); // 重置缩放
                GameObject hex = Instantiate(hexPrefab, new Vector3(xPos, 0, yPos), Quaternion.identity);
                hex.transform.SetParent(hex_map.transform); // 设置父对象
                hexMap[x, y] = hex; // 存储生成的六边形
            }
        }
    }

    void CreateEnvironment()
    {
        // 随机选择开始位置和环境类型
        int startX = Random.Range(1, width+1);
        int startY = Random.Range(1, height+1);
        bool isForest = Random.value > 0.5f; // 50%概率选择森林或湖泊

        if (isForest)
        {
            CreateContinuousArea(startX, startY, forestPrefab);
        }
        else
        {
            CreateContinuousArea(startX, startY, lakePrefab);
        }
    }

    void CreateContinuousArea(int startX, int startY, GameObject[] prefab_list)
    {
        GameObject prefab = prefab_list[Random.Range(0, prefab_list.Length)];
        prefab.transform.localScale = new Vector3(5.196f, 5.196f, 5.196f); // 重置缩放
        // float random_threshold = 0.2f; // 随机阈值
        float growth_probability = 0.6f; // 生长概率
        int max_growth = 5; // 最大生长次数
        int growth = 1; // 当前生长次数
        Destroy(hexMap[startX, startY]); // 删除起始位置的六边形
        hexMap[startX, startY] = Instantiate(prefab, hexMap[startX, startY].transform.position, Quaternion.identity); // 生成环境
        hexMap[startX, startY].transform.SetParent(hex_map.transform); // 设置父对象
        while (growth < max_growth)
        {
            if (Random.value < growth_probability)
            {
                List<Vector2Int> neighbors = GetNeighbors(startX, startY);
                Vector2Int randomNeighbor = neighbors[Random.Range(0, neighbors.Count)];
                startX = randomNeighbor.x;
                startY = randomNeighbor.y;
         
                Destroy(hexMap[startX, startY]); // 删除起始位置的六边形
                hexMap[startX, startY] = Instantiate(prefab, hexMap[startX, startY].transform.position, Quaternion.identity); // 生成环境
                hexMap[startX, startY].transform.SetParent(hex_map.transform); // 设置父对象
                growth++;

            }
            else
            {
                growth++;
            }
            
        }
    }

    List<Vector2Int> GetNeighbors(int x, int y)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        if (IsInBounds(x - 1, y))
        {
            neighbors.Add(new Vector2Int(x - 1, y));
        }
        if (IsInBounds(x + 1, y))
        {
            neighbors.Add(new Vector2Int(x + 1, y));
        }
        if (IsInBounds(x, y - 1))
        {
            neighbors.Add(new Vector2Int(x, y - 1));
        }
        if (IsInBounds(x, y + 1))
        {
            neighbors.Add(new Vector2Int(x, y + 1));
        }
        if (y % 2 == 0)
        {
            if (IsInBounds(x - 1, y - 1))
            {
                neighbors.Add(new Vector2Int(x - 1, y - 1));
            }
            if (IsInBounds(x - 1, y + 1))
            {
                neighbors.Add(new Vector2Int(x - 1, y + 1));
            }
        }
        else
        {
            if (IsInBounds(x + 1, y - 1))
            {
                neighbors.Add(new Vector2Int(x + 1, y - 1));
            }
            if (IsInBounds(x + 1, y + 1))
            {
                neighbors.Add(new Vector2Int(x + 1, y + 1));
            }
        }
        return neighbors;
    }


    bool IsInBounds(int x, int y)
    {
        return x >= 1 && x <=width && y >= 1 && y <=height;
    }

    void createAroundMountian(){
        // Debug.Log("createAroundMountian");
        for (int x = 1; x <=width; x++)
        {
            int y=0;
            float yPos = y * 4.5f; // 每个六边形的宽度
            float xPos = x * Mathf.Sqrt(3)*3f; // 每个六边形的高度
            GameObject hexPrefab = mountainPrefab;
            hexPrefab.transform.localScale = new Vector3(5.196f, 5.196f, 5.196f); // 重置缩放
            GameObject hex = Instantiate(hexPrefab, new Vector3(xPos, 0, yPos), Quaternion.identity);

        }   
        for (int x = 1; x <=width; x++)
        {
            int y=height+1;
            float yPos = y * 4.5f; // 每个六边形的宽度
            float xPos = x * Mathf.Sqrt(3)*3f; // 每个六边形的高度
            GameObject hexPrefab = mountainPrefab;
            hexPrefab.transform.localScale = new Vector3(5.196f, 5.196f, 5.196f); // 重置缩放
            GameObject hex = Instantiate(hexPrefab, new Vector3(xPos, 0, yPos), Quaternion.identity);

        }
        for (int y = 1; y <=height; y++)
        {
            int x=0;
            float yPos = y * 4.5f; // 每个六边形的宽度
            float xPos = x * Mathf.Sqrt(3)*3f; // 每个六边形的高度
            GameObject hexPrefab = mountainPrefab;
            hexPrefab.transform.localScale = new Vector3(5.196f, 5.196f, 5.196f); // 重置缩放
            GameObject hex = Instantiate(hexPrefab, new Vector3(xPos, 0, yPos), Quaternion.identity);

        }
        for (int y = 1; y <=height; y++)
        {
            int x=width+1;
            float yPos = y * 4.5f; // 每个六边形的宽度
            float xPos = x * Mathf.Sqrt(3)*3f; // 每个六边形的高度
            GameObject hexPrefab = mountainPrefab;
            hexPrefab.transform.localScale = new Vector3(5.196f, 5.196f, 5.196f); // 重置缩放
            GameObject hex = Instantiate(hexPrefab, new Vector3(xPos, 0, yPos), Quaternion.identity);

        }
    }
}



